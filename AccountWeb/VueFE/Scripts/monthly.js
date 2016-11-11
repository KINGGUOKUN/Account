/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

const Monthly = {
    template: "#monthly",
    created: function () {
        this.fetchData();
    },
    data: function () {
        let currentDate = new Date();
        return {
            start: new Date(currentDate.getFullYear(), currentDate.getMonth() - 3),
            end: new Date(),
            monthly: []
        }
    },
    methods: {
        fetchData: function () {
            this.monthly = [];
            this.$http.get("http://localhost:1500/api/monthly", {
                params: {
                    start: this.start.format("yyyy-MM"),
                    end: this.end.format("yyyy-MM")
                }
            })
                .then(response => this.monthly = response.body)
                .catch(err => console.log(err.body.Message));
        }
    }
}