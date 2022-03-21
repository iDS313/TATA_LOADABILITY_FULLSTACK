var app = angular.module("LoadPlan", []);
app.controller("LoadPlanController", function ($scope, $http) {
    $scope.priorities = [];
    $scope.payload = { CfaId: "", TruckId: "", Plandate: new Date() };

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
        $http({
            method: "get",
            url: "/Trucks"
        }).then(function (res) {
            console.log(res);
            $scope.trucks = res.data;
        }, function (res) { });

    }

    $scope.getInitialData = function () {
        $http({
            method: "Post",
            url: "/LoadPlan/getInitialData",
            data: $scope.payload,
        }).then(function ({ data, status }) {
            console.log("Weight Data", data);

            $scope.priorities = data.priorities;
            
            $scope.unplaned.map((val) => {
                val.PlanDate = getDate(val.PlanDate);
            });
            $scope.loadPlan.map((val) => {
                val.PlanDate = getDate(val.PlanDate);
            });
        }, function ({ data, status }) {
            console.log(data);
        });
    }
});