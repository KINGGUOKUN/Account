/// <reference path="vue.js" />
/// <reference path="vue-resource.js" />

const Monthly = {
    template: "#monthly",
    created: function () {
        this.fetchData();
        bus.$on("manifestChanged", () => this.fetchData());
    },
    data: function () {
        let currentDate = new Date();
        return {
            start: new Date(currentDate.getFullYear(), currentDate.getMonth() - 3),
            end: new Date(),
            monthly: [],
            pageIndex: 1,
            pageSize: 10,
            total: 0,
            pageSizes: [10, 20, 50, 100]
        }
    },
    methods: {
        fetchData: function () {
            this.monthly = [];
            this.$http.get(SERVER_URL + "/monthly/paged", {
                params: {
                    start: this.start.format("yyyy-MM"),
                    end: this.end.format("yyyy-MM"),
                    pageIndex: this.pageIndex,
                    pageSize: this.pageSize
                }
            })
                .then(response => {
                    this.total = response.body.count;
                    this.monthly = response.body.data;
                })
                .catch(response => this.$alert(response.body.Message, "月消费清单", { type: "error" }));
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