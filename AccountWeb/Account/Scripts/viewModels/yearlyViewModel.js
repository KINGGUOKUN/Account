/// <reference path="../util.js" />
/// <reference path="../knockout-3.3.0.js" />
/// <reference path="../moment.min.js" />

function YearlyViewModel() {
    var self = this;
    $("#rangeYearly input").datepicker({
        format: "yyyy",
        minViewMode: 2,
        language: "zh-CN",
        autoclose: true
    });
    this.start = ko.observable(moment().add(-3, "years").year());
    this.end = ko.observable(moment().year());
    $("#rangeYearly input:first").datepicker("setDate", this.start().toString());
    $("#rangeYearly input:last").datepicker("setDate", this.end().toString());
    this.yearlys = ko.observableArray();
    this.count = ko.pureComputed(function () {
        return self.yearlys().length;
    });
    this.getYearlys = function () {
        sendAjaxRequest("GET", "/api/Yearly",
            {
                start: this.start(),
                end: this.end()
            },
            function (err, res) {
                if (err) {
                    alert("获取年消费清单出错：" + err);
                }
                else {
                    self.yearlys(res);
                }
            });
    };
    self.getYearlys();
    window.customEvent.addHandler("refresh", function (event) {
        self.getYearlys();
    });
}