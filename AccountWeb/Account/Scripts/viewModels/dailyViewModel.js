/// <reference path="../util.js" />
/// <reference path="../knockout-3.3.0.js" />
/// <reference path="../moment.min.js" />

//日消费清单
function DailyViewModel() {
    var self = this;
    $("#rangeDaily input").datepicker({
        format: "yyyy-mm-dd",
        minViewMode: 0,
        language: "zh-CN",
        autoclose: true
    });
    this.start = ko.observable(moment().add(-1, "months").format("YYYY-MM-DD"));
    this.end = ko.observable(moment().format("YYYY-MM-DD"));
    $($("#rangeDaily").children("input").get(0)).datepicker("setDate", this.start());
    $($("#rangeDaily").children("input").get(1)).datepicker("setDate", this.end());
    this.dailys = ko.observableArray();
    this.count = ko.computed(function () {
        return self.dailys().length;
    });
    this.getDailys = function () {
        sendAjaxRequest("GET", "/api/Dailys",
            {
                start: self.start(),
                end: self.end()
            },
            function (err, res) {
                if(err){
                    alert("获取日消费清单失败：" + err);
                }
                else {
                    self.dailys(res);
                }
            });
    }
    this.getDailys();
    window.customEvent.addHandler("refresh", function (event) {
        self.getDailys();
    });
}