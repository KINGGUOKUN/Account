/// <reference path="index.js" />
/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

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
            manifests: []
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
        }
    }
}