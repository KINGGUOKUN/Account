/// <reference path="../util.js" />
/// <reference path="../knockout-3.3.0.js" />
/// <reference path="../moment.min.js" />

//日消费清单
function DailyViewModel() {
    var self = this;
    this.startDate = ko.observable(moment().add(-1, "months").format("YYYY-MM-DD"));
    this.endDate = ko.observable(moment().format("YYYY-MM-DD"));
    this.dailys = ko.observableArray();
    this.count = ko.computed(function () {
        return self.dailys().length;
    });
    this.getDailys = function () {
        sendAjaxRequest("GET", "/api/Dailys",
            {
                start: self.startDate(),
                end: self.endDate()
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