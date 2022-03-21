
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
    $scope.isMsg = false;
    $scope.capacityLeft = 0;
    $scope.lpShow = false;

    $scope.getInitialData = function () {
        $http({
            method: "Post",
            url: "/LoadPlan/getInitialData",
            data: $scope.payload,
        }).then(function ({ data, status }) {
            console.log("Weight Data", data);
            $scope.weight = data.weight;
            $scope.unplaned = data.priorities;
            $scope.loadPlan = data.loadplan;
            $scope.remainingCapacity = data.remaining;
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
    $scope.loadtruck = function () {
        if ($scope.payload.CfaId == "") {
            toastr.error("Select Cfa");
            return;
        }
        $scope.lpShow = true;
    }
    $scope.getPlan = function () {
        if ($scope.payload.CfaId == "") {
            toastr.error("Please Select Cfa");
            return;
        }
        if ($scope.payload.Plandate == undefined) {
            toastr.error("Please Enter Date");
            return;
        }
        $http({
            method: "Post",
            url: "/LoadPlan/getPlan",
            data: $scope.payload
        }).then(function ({ data, status }) {
            console.log(data)
            $scope.planed = data.planed;
            $scope.loadPlan = data.loadplan;
            $scope.remainingCapacity = data.remaining;
            $scope.planed.map((val) => {
                val.PlanDate = getDate(val.PlanDate);
            });
            $scope.loadPlan.map((val) => {
                val.PlanDate = getDate(val.PlanDate);
            });
            let weight = 0.00;
            let cap = 0.00;
            $scope.planed.map((m) => {
                weight = (parseFloat( weight )+ parseFloat(m.ToLoad)).toFixed(1);
            })
            $scope.trucks.map((c) => {
                if (c.TruckId == $scope.payload.TruckId) {
                    cap = parseFloat(c.Limit).toFixed(1);
                }
            });
            if (cap - weight > 50 && $scope.planed.length > 0) {
                $scope.isMsg = true;
                $scope.capacityLeft = cap - weight;
               // swal("ok");
            }
        }, function ({ data, status }) {
            console.warn(data);
        });
    }
    $scope.confirm = function () {
        let confirm = { payload: {}, priorities: [] };
        confirm.payload = $scope.payload;
        confirm.priorities = $scope.planed;
        $http({
            method: "Post",
            url: "/LoadPlan/Confirm",
            data: { confirm: confirm }
        }).then(function ({ data, status }) {
            console.log("confirm", data);
            $scope.loadPlan = data.loadplan;
            $scope.remainingCapacity = data.remaining;
            $scope.planed = [];
            $scope.payload.TruckId = "";
            $scope.lpShow = false;
            $scope.loadPlan.map((val) => {
                val.PlanDate = getDate(val.PlanDate);
            });
        }, function ({ data, status }) {
            console.warn(status, data);
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
    $scope.getplanFromPr=function(){
        let date = formatdate($scope.payload.Plandate);
        let cap = 0;
        let weight = 0;
        $scope.trucks.map((c) => {
            if (c.TruckId == $scope.payload.TruckId) {
                cap = parseFloat(c.Limit).toFixed(1);
            }
        });
        $scope.planed.map((m) => {
            weight = (parseFloat(weight) + parseFloat(m.ToLoad)).toFixed(1);
        })
        $http({
            method: "Get",
            url: `/LoadPlan/getplanFromPr?d=${date}&cap=${$scope.capacityLeft}&CfaId=${$scope.payload.CfaId}`,
        }).then(function ({ data, status }) {
            console.log("prPriorities", data);
            data.planed.map((c) => {
                c.PlanDate = getDate(c.PlanDate);
                $scope.planed.push(c);
            });
            let weightLeft = 0;
            data.planed.map((c) => {
                weightLeft = weightLeft + c.PendingQty;
            });
            $scope.capacityLeft -= weightLeft;
            if ($scope.capacityLeft == 0) {
                $scope.isMsg = false;
            }
        }, function ({ data, status }) {
            console.log("prPrioritieserrror", data);
        });
    }

});


function getDate(d) {
    return new Date(parseInt(d.replace("/Date(", "").replace(")/", "")));
}
function formatdate(d) {
    return `${d.getFullYear()}-${d.getMonth() + 1}-${d.getDate()}`;
}