/// <reference path="../util.js" />
/// <reference path="../knockout-3.3.0.js" />
/// <reference path="../moment.min.js" />

function MonthlyViewModel() {
    var self = this;
    $("#rangeMonthly input").datepicker({
        format: "yyyy-mm",
        minViewMode: 1,
        language: "zh-CN",
        autoclose: true
    });
    this.start = ko.observable(moment().add(-12, "months").format("YYYY-MM"));
    this.end = ko.observable(moment().format("YYYY-MM"));
    $("#rangeMonthly input:first").datepicker("setDate", this.start());
    $("#rangeMonthly input:last").datepicker("setDate", this.end());
    this.monthlys = ko.observableArray();
    this.count = ko.pureComputed(function () {
        return self.monthlys().length;
    });
    this.getMonthlys = function () {
        sendAjaxRequest("GET", "/api/Monthly",
            {
                start: this.start(),
                end: this.end()
            },
            function (err, res) {
                if (err) {
                    alert("获取月消费清单出错：" + err);
                }
                else {
                    self.monthlys(res);
                }
            });
    };
    self.getMonthlys();
    window.customEvent.addHandler("refresh", function (event) {
        self.getMonthlys();
    });
}