/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

const Daily = {
    template: "#daily",
    created: function () {
        this.fetchData();
    },
    data: function () {
        let curentDate = new Date();
        return {
            start: new Date(curentDate.getFullYear() - 3, 0, 1),
            end: new Date(),
            dailys: []
        }
    },
    methods: {
        fetchData: function () {
            this.dailys = [];
            this.$http.get("http://localhost:1500/api/dailys", {
                params: {
                    start: this.start.format("yyyy-MM-dd"),
                    end: this.end.format("yyyy-MM-dd")
                }
            })
                .then(response => this.dailys = response.body)
                .catch(response => this.$alert(response.body.Message, "日消费清单", {type:"error"}));
        }
    }
}