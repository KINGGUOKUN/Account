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
            start: new Date(currentDate.getFullYear() - 3, 0, 1),
            end: new Date(),
            manifests: [],
            title:"",
            manifest:{},
            showOperateManifest:false
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
                      .catch(response => this.$alert(response.body.Message, "日消费明细", {type:"error"}));
        },
        add:function(){
            this.title = "添加消费明细";
            this.manifest = {
                ID:Guid.NewGuid().ToString("N"),
                Date:new Date(),
                Cost:"",
                Remark:""
            };
            this.showOperateManifest = true;
        },
        save:function(){           
            this.$http.post("http://localhost:1500/api/Manifests", this.manifest)
            .then(() => {
                this.manifests.push(this.manifest);
            this.showOperateManifest = false;
            this.$message({
                message:"添加成功",
                type:"success"
            });
        })
            .catch(err => this.$alert(err.body.Message, "添加日消费明细", {type:"error"}));
}
}
}