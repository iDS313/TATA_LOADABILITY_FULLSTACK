var truckObj = { TruckId: "", Name: "", Capacity: "", Limit: "" };
var PrioritiesObj = { PriorityId: "", SkuId: "", CfaId: "", Rank: "", InPr: "", InStock: "", QtyFromPr: "", FinalQty: "", IsPlaned: "", PlanDate: "" }
var LoadPlanVm = { truck: truckObj, priorities: [PrioritiesObj] };

var app = angular.module("LoadPlan", []);
app.controller("LoadPlanController", function ($scope, $http) {
    $scope.confirmed = [];
    $scope.loadPlan = [];
    $scope.priorities = [];
    $scope.weight = 0;
    $scope.cfa = [];
    $scope.trucks = [];
    $scope.planed = [];
    $scope.unplaned = [];
    $scope.noOfTrucks = 0;
    $scope.payload = { CfaId: "", TruckId: "", Plandate: new Date() };

    $scope.selectTruck = function (x) {
        // console.log(x.truck);
        let truck = {}
        $scope.trucks.map((val) => {
            if (val.TruckId == x.truck.TruckId) {
                truck = JSON.parse(JSON.stringify(val));
            }
        });
        x.truck = truck;
        console.log(x);

        let payl = { CfaId: $scope.payload.CfaId, TruckId: x.truck.TruckId, Plandate: $scope.payload.Plandate };

        $http({
            method: "post",
            url: "/LoadPlan/getLoadPlan",
            data:payl,
        }).then(function ({ data, status }) {
            x.priorities = data.priority;
            x.loadPlan = data.loadplan;
            console.log(x);
        }, function ({ data, status }) { })
        
    }

    $scope.getweight = function () {
        if ($scope.payload.CfaId == "") {
            return;
        }
        if ($scope.payload.Plandate == undefined) {
            toastr.error("Please Select Load Plan date");
            return;
        }

        $http({
            method: "post",
            url: "/LoadPlan/getWeight",
            data: $scope.payload
        }).then(function ({ data, status }) {
            console.log(data)
            $scope.weight = data.weight;
            $scope.priorities = data.unplaned;
        }, function ({ data, status }) { })
    }
    $scope.priorityForm = function () {

        while ($scope.loadPlan.length <= $scope.noOfTrucks) {
            $scope.loadPlan.push(JSON.parse(JSON.stringify(LoadPlanVm)));
        }
    }
    $scope.getPlan = function () {
        if ($scope.payload.CfaId == "") {
            return;
        }
        if ($scope.payload.TruckId == "") {
            return;
        }
        if ($scope.payload.Plandate == undefined) {
            toastr.error("Select Date");
            return;
        }
        $http({
            method: "Post",
            url: "/LoadPlan/getLoadPlan",
            data: $scope.payload
        }).then(function ({ data, status }) {
            $scope.planed = data.planed;
            $scope.unplaned = data.unplaned;
            console.log('plandata', data);
        }, function ({ data, status }) {
            console.error("error loading plan",data);
        })

    }

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
            url: "/Trucks"
        }).then(function (res) {
            console.log(res);
            $scope.trucks = res.data;
        }, function (res) { });
    }

});