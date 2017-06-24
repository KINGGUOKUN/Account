/// <reference path="index.js" />
/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />
/// <reference path="util.js" />

const Manifests = {
    template: "#manifests",
    created: function () {
        this.fetchData();
    },
    data: function () {
        let currentDate = new Date();
        let costValidator = (rule, value, callback) => {
            if (!/^[0-9]+(.[0-9]{2})?$/.test(value)) {
                callback(new Error("请输入合法金额"));
            }
            else {
                callback();
            }
        };
        return {
            start: new Date(currentDate.getFullYear(), currentDate.getMonth() - 3, 1),
            end: new Date(),
            manifests: [],
            title: "",
            manifest: {},
            showOperateManifest: false,
            isAdd: false,
            rules: {
                date: [
                    { type: "date", required: true, message: "请选择消费日期", trigger: "change" }
                ],
                cost: [
                    { required: true, message: "请填写消费金额", trigger: "blur" },
                    { validator: costValidator, trigger: "change" }
                ],
                remark: [
                    { required: true, message: "请填写消费明细", trigger: "blur" }
                ]
            },
            pageIndex: 1,
            pageSize: 10,
            total: 0,
            pageSizes: [10, 20, 50, 100]
        }
    },
    methods: {
        fetchData: function () {
            this.manifests = [];
            this.$http.get(SERVER_URL + "/Manifest/paged", {
                params: {
                    start: this.start.format("yyyy-MM-dd"),
                    end: this.end.format("yyyy-MM-dd"),
                    pageIndex: this.pageIndex,
                    pageSize: this.pageSize
                }
            })
                .then(response => {
                    this.total = response.body.count;
                    this.manifests = response.body.items;
                })
                .catch(response => this.$alert(response.body.Message, "日消费明细", { type: "error" }));
        },
        add: function () {
            this.title = "添加消费明细";
            this.manifest = {
                id: Guid.NewGuid().ToString("D"),
                date: new Date(),
                cost: "",
                remark: ""
            };
            this.isAdd = true;
            this.showOperateManifest = true;
        },
        save: function () {
            this.$refs.formManifest.validate(valid => {
                if (valid) {
                    let operateManifest = JSON.parse(JSON.stringify(this.manifest));
                    operateManifest.date = this.manifest.date.format("yyyy-MM-dd hh:mm:ss");
                    if (this.isAdd) {
                        this.$http.post(SERVER_URL + "/Manifest", operateManifest)
                            .then(() => {
                                this.manifests.push(operateManifest);
                                this.showOperateManifest = false;
                                bus.$emit("manifestChanged");
                                this.$message({
                                    message: "添加成功",
                                    type: "success"
                                });
                            })
                            .catch(err => {
                                //console.log(err);
                                this.$alert(err.body.Message, "添加日消费明细", { type: "error" });
                            });
                    }
                    else {
                        this.$http.put(SERVER_URL + "/Manifest", operateManifest)
                            .then(response => {
                                let updatedManifest = this.manifests.find(x => x.id == this.manifest.id);
                                updatedManifest.date = operateManifest.date;
                                updatedManifest.cost = operateManifest.cost;
                                updatedManifest.remark = operateManifest.remark;
                                this.showOperateManifest = false;
                                bus.$emit("manifestChanged");
                                this.$message({
                                    message: "修改成功",
                                    type: "success"
                                });
                            })
                            .catch(err => {
                                //console.log(err);
                                this.$alert(err.body.Message, "修改消费明细", { type: "error" });
                            });
                    }
                }
                else {
                    return false;
                }
            });
        },
        cancel: function () {
            this.manifest = {};
            this.showOperateManifest = false;
        },
        edit: function (id) {
            let currentManifest = this.manifests.find(x => x.id == id);
            this.manifest = JSON.parse(JSON.stringify(currentManifest));
            this.manifest.date = new Date(this.manifest.date);
            this.title = "编辑消费明细";
            this.isAdd = false;
            this.showOperateManifest = true;
        },
        del: function (id) {
            this.$confirm("是否删除？", "警告", { type: "warning" })
                .then(() => {
                    this.$http.delete(SERVER_URL + "/Manifest/" + id)
                        .then(response => {
                            let index = this.manifests.findIndex(x => x.id == id);
                            this.manifests.splice(index, 1);
                            bus.$emit("manifestChanged");
                            this.$message({
                                message: "删除成功",
                                type: "success"
                            });
                        })
                        .catch(err => {
                            this.$alert(err.body.Message, "删除消费明细", { type: "error" });
                            //console.log(err);
                        });
                })
                .catch(err => console.log(err));
        },
        dialogClosed: function () {
            this.$refs.formManifest.resetFields();
        },
        sizeChange: function (pageSize) {
            this.pageSize = pageSize;
            this.fetchData();
        },
        pageIndexChange: function (pageIndex) {
            this.pageIndex = pageIndex;
            this.fetchData();
        }
    }
}