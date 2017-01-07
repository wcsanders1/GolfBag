(function () {
    "use strict";

    //getting existing module
    angular.module("app-enter-score")
        .controller("enterScoreController", enterScoreController);

    function enterScoreController() {
        var vm = this;

        vm.name = "Bill";

        console.log("Bill");
    }
})();