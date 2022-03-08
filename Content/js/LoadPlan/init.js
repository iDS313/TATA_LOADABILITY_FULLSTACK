
var app = angular.module("LoadPlan", []);

app.controller("LoadplanController", function ($scope, $http) {
    $scope.cfa = [];
    $scope.sku = [];
    $scope.LoadPlan = { SkuId: "", CfaId: "", PriorityQty: "", QtyInTransit: "", PlanDate: new Date() };

    $scope.init = function () {
        $http({
            method: "get",
            url: "/Cfa"
        }).then(function (res) {
            console.log(res);
            $scope.cfa = res.data.cfa;
        }, function (res) { });
        $http({
            method: "get",
            url: "/Sku"
        }).then(function (res) {
            console.log(res);
            $scope.sku = res.data.sku;
        }, function (res) { });
    }

    $scope.dateChange = function () {
        let d = new Date().getTime();
        let d2 = $scope.LoadPlan.PlanDate.getTime();
        if (d2 < d) {
            toastr.warning("Please Select Todays Date or Future Date");
        }
    }
});
