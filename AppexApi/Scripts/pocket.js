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

        return {
            Init: init,
            Sum: sum,
            SayHello: sayHello
        };

    };
};

var returned = (thefunction);

returned(jQuery, "moni");