/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

const Yearly = {
    template: "#yearly",
    created: function () {
        this.fetchData();
        bus.$on("manifestChanged", () => this.fetchData());
    },
    data: function () {
        let currentYear = new Date().getFullYear();
        return {
            start: currentYear - 3,
            end: currentYear,
            yearly: [],
            pageIndex: 0,
            pageSize: 10,
            total: 0,
            pageSizes: [1,2,5,10, 20, 50, 100]
        }
    },
    methods: {
        fetchData: function () {
            this.yearlys = [];
            this.$http.get("http://localhost:1500/api/yearly/paged", {
                params: {
                    start: this.start,
                    end: this.end,
                    pageIndex: this.pageIndex,
                    pageSize: this.pageSize
                }
            })
                .then(response => {
                    this.total = response.body.count;
                    this.yearly = response.body.data;
                })
                .catch(response => this.$alert(response.body.Message, "年消费清单", { type: "error" }));
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