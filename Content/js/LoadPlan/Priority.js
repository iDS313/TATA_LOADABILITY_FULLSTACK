var app = angular.module("Priority", []);
app.controller("PriorityController", function ($scope ,$http) {
    $scope.priorities = [];
    $scope.payload = { CfaId: "", PlanDate: new Date() };
    $scope.cfa = [];
    $scope.sku = [];


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

    $scope.getPlan = function () {
        if ($scope.payload.PlanDate == undefined) {
            toastr.error("Please Select Plan Date");
            return;
        }
        if ($scope.payload.CfaId == "") {
            toastr.error("Please Select Cfa");
            return;
        }
        $http({
            method: "Post",
            url: "/Priority/getPriority",
            data: $scope.payload,
        })
        .then(
            function ({ data, status }) {
                $scope.priorities = data;
            },
            function ({ data, status }) {
                toastr.error("Data Not Found")
            });
    }
})