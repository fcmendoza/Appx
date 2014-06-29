
(function () {
    var app = angular.module('trackin', []);

    app.controller('MoneyController', function ($scope) {
        this.transaction = tran;
        this.transactions = trans;
        this.canAddTransaction = function () {
            return this.transaction.description && this.transaction.description != ""
                    && this.transaction.amount && this.transaction.amount != 0
                    && this.transaction.tag && this.transaction.tag != "";
        };

        this.addTransaction = function (form) {
            this.transactions.push(this.transaction);
            this.transaction = {};
            if (form) {
                form.$setPristine();
            }
        };
    });

    var tran = {
        description: "",
        amount: 0,
        tag: ""
    };

    var trans = [
        {
            description: "Movies - Maleficent",
            amount: 10.15,
            tag: "leisure"
        },
        {
            description: "Netflix June",
            amount: 100.50,
            tag: "services"
        }
    ];

})();