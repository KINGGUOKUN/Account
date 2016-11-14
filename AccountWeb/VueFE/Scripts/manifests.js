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
        return {
            start: new Date(currentDate.getFullYear(), currentDate.getMonth() - 3, 1),
            end: new Date(),
            manifests: [],
            title: "",
            manifest: {},
            showOperateManifest: false,
            isAdd: false
        }
    },
    methods: {
        fetchData: function () {
            this.manifests = [];
            this.$http.get("http://localhost:1500/api/Manifests", {
                params: {
                    start: this.start.format("yyyy-MM-dd"),
                    end: this.end.format("yyyy-MM-dd")
                }
            })
                      .then(response => this.manifests = response.body)
                      .catch(response => this.$alert(response.body.Message, "日消费明细", { type: "error" }));
        },
        add: function () {
            this.title = "添加消费明细";
            this.manifest = {
                ID: Guid.NewGuid().ToString("D"),
                Date: new Date(),
                Cost: "",
                Remark: ""
            };
            this.isAdd = true;
            this.showOperateManifest = true;
        },
        save: function () {
            this.manifest.Date = this.manifest.Date.format("yyyy-MM-dd");
            if (this.isAdd) {
                this.$http.post("http://localhost:1500/api/Manifests", this.manifest)
                .then(() => {
                    this.manifests.push(this.manifest);
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
                this.$http.put("http://localhost:1500/api/Manifests", this.manifest)
                .then(response => {
                    let updatedManifest = this.manifests.find(x => x.ID == this.manifest.ID);
                    updatedManifest.Date = this.manifest.Date;
                    updatedManifest.Cost = this.manifest.Cost;
                    updatedManifest.Remark = this.manifest.Remark;
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
        },
        cancel: function () {
            this.manifest = {};
            this.showOperateManifest = false;
        },
        edit: function (ID) {
            let currentManifest = this.manifests.find(x => x.ID == ID);
            this.manifest = JSON.parse(JSON.stringify(currentManifest));
            this.manifest.Date = new Date(this.manifest.Date);
            this.title = "编辑消费明细";
            this.isAdd = false;
            this.showOperateManifest = true;
        },
        del: function (ID) {
            this.$http.delete("http://localhost:1500/api/Manifests/" + ID)
            .then(response => {
                let index = this.manifests.findIndex(x => x.ID == ID);
                this.manifests.splice(index, 1);
                bus.$emit("manifestChanged");
                this.$message({
                    message: "删除成功",
                    type: "success"
                });
            })
            .catch(err => {
                //this.$alert(err.body.Message, "删除消费明细", { type: "error" });
                console.log(err);
            });
        }
    }
}