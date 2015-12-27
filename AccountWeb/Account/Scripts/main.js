/// <reference path="knockout-3.3.0.js" />
/// <reference path="knockout.validation.min.js" />
/// <reference path="viewModels/manifestViewModel.js" />
/// <reference path="viewModels/dailyViewModel.js" />
/// <reference path="viewModels/monthlyViewModel.js" />
/// <reference path="viewModels/yearlyViewModel.js" />

var manifestViewModel = null,
    dailyViewModel = null,
    monthlyViewModel = null,
    yearlyViewModel = null;

window.customEvent = new CustomEvent();

$(function () {
    $("a[data-toggle='tab']").on("show.bs.tab", function (e) {
        var tab = e.target.name;
        switch (tab) {
            case "manifest":
                if(manifestViewModel == null){
                    manifestViewModel = new ManifestViewModel();
                    //ko.validation.configure({
                    //    registerExtenders: true,
                    //    messagesOnModified: true,
                    //    insertMessages: true,
                    //    parseInputAttributes: true,
                    //    messageTemplate: null
                    //});
                    manifestViewModel.errors = ko.validation.group(manifestViewModel);
                    ko.applyBindings(manifestViewModel, document.getElementById("manifests"));
                }
            case "daily":
                if(dailyViewModel == null){
                    dailyViewModel = new DailyViewModel();
                    ko.applyBindings(dailyViewModel, document.getElementById("daily"));
                }
                break;
            case "monthly":
                if(monthlyViewModel == null){
                    monthlyViewModel = new MonthlyViewModel();
                    ko.applyBindings(monthlyViewModel, document.getElementById("monthly"));
                }
                break;
            case "yearly":
                if(yearlyViewModel == null){
                    yearlyViewModel = new YearlyViewModel();
                    ko.applyBindings(yearlyViewModel, document.getElementById("yearly"));
                }
                break;
            default:
                break;
        }
    });

    $("a[data-toggle='tab'][name='manifest']").tab("show");
});