//jQuery(function () {
//    $("#moni-placeholder").text("Hola Moni");
//});


var thenamespace = function (name, separator, container) {
    var ns = name.split(separator || '.'),
    o = container || window,
    i,
    len;
    for (i = 0, len = ns.length; i < len; i++) {
        o = o[ns[i]] = o[ns[i]] || {};
    }
    return o;
};

thenamespace("Appx.Pocket"); // this adds an Appx object to the window object and a Pocket object to the Appx object.

function thefunction($, undefined) {
    Appx.Pocket.Main = function () { // Appx.Pocket.Main = function () {} // adds the Main object to the Pocket object.

        function init() {
            console.log("Initializing Pocket Main script... " + undefined);
        }

        function sum(a, b) {
            console.log(a + b);
        }

        function sayHello(name) {
            name = name || "Moni";
            $("#moni-placeholder").text("Hola " + name);
        }

        function displayLinks() {
            $.ajax({
                url: "/api/links",
                data: {},
                success: function (links) {
                    render(links);
                },
                type: "GET",
                dataType: 'json'
            });
        }

        function render(data) {
            data = data || [];

            //var dasArray = ko.observableArray(data);
            var linksArray = new links(data);

            ko.applyBindings(linksArray);

            var content = "";
            for (var i = 0; i < data.length; i++) {
                content += "<label>{0}</label>".format(data[i].title);
            }
            $("#links-main-container").append(content);
        }

        var links = function (data) {
            this.items = ko.observableArray(data);
        };

        return {
            Init: init,
            Sum: sum,
            SayHello: sayHello,
            DisplayLinks: displayLinks
        };

    };
};

var returned = (thefunction);

returned(jQuery, "moni");