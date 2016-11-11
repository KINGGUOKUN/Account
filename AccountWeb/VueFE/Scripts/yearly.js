/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

const Yearly = {
    template: "#yearly",
    created: function () {
        this.fetchData();
    },
    data: function () {
        let currentYear = new Date().getFullYear();
        return {
            start: currentYear - 3,
            end: currentYear,
            yearly: []
        }
    },
    methods: {
        fetchData: function () {
            this.yearlys = [];
            this.$http.get("http://localhost:1500/api/yearly", {
                params: {
                    start: this.start,
                    end: this.end
                }
            })
                .then(response => this.yearly = response.body)
                .catch(response => this.$alert(response.body.Message, "年消费清单", {type:"error"}));
        }
    }
}