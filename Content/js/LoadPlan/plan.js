
var app = angular.module("LoadPlan", []);
app.controller("LoadPlanController", function ($scope, $http) {
   
    $scope.loadPlan = [];
    $scope.priorities = [];
    $scope.weight = 0;
    $scope.remainingCapacity = 0;
    $scope.cfa = [];
    $scope.trucks = [];
    $scope.planed = [];
    $scope.unplaned = [];
    $scope.noOfTrucks = 0;
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

});