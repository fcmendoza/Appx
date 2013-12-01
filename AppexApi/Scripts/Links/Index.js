namespace("Appx.Links");

(function ($) {
    Appx.Links.Index = function () {
        var DATA_URL = "/api/links/";
        function init() {
            console.log("Initializing Links page...");
            console.log("Retrieving links data...");

            $.ajax({
                url: "/api/links",
                data: { },
                success: function (links) {
                    render(links);
                },
                type: "GET",
                dataType: 'json'
            });
        }

        function render(data) {
            data = data || [];
            var vm = new links(data);
            ko.applyBindings(vm);
        }

        var links = function(data) { // js view model
            this.items = ko.observableArray(data);
        };

        return {
            Init: init
        };
    };
})(jQuery);


 

namespace("Appx.Links.ViewModels"); // maybe it should be "renamed" to Appx.ViewModels

(function ($, undefined) {
    Appx.Links.ViewModels.Link = function (o) {
        var vm = {};
        o = o || {};

        vm.id = ko.observable(o.id || 0);
        vm.title = ko.observable(o.title || '');
        vm.url = ko.observable(o.url || '');
        vm.description = ko.observable(o.description || '');

        return vm;
    };
})(jQuery);