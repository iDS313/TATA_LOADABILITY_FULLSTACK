/*/*const { data } = require("jquery");*/

var app = angular.module("LoadPlan", []);

app.controller("LoadplanController", function ($scope, $http) {
    $scope.cfa = [];
    $scope.sku = [];
    $scope.LoadPlan = { SkuId: "", CfaId: "", SIT: "", QtyInTransit: "", PlanDate: new Date() };

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
        let d = new Date(new Date().toDateString()).getTime();
        if ($scope.LoadPlan.PlanDate == undefined) {
            toastr.warning("Please Enter Date");
            return;
        }
        let d2 =$scope.LoadPlan.PlanDate.getTime();
        if (d2 < d) {
            toastr.warning("Please Select Todays Date or Future Date");
        }
    }
    $scope.save = function () {
        
        if ($scope.LoadPlan.SkuId == "") {
          //  console.log($scope.LoadPlan);
            toastr.warning("Please Select Sku");
            return;
        }
        if ($scope.LoadPlan.CfaId == "") {
            toastr.warning("Please Select Cfa");
            return;
        }
        if ($scope.LoadPlan.PriorityQty == "") {
            toastr.warning("Please Enter PriorityQty");
            return;
        }
        if ($scope.LoadPlan.QtyInTransit == "") {
            toastr.warning("Please Enter QtyInTransit");
            return;
        }
        $http({
            method: "Post",
            url: "/DailyPlan/Save",
            data: $scope.LoadPlan
        }).then(function (res) {
            res.data.PlanDate = new Date(parseInt(res.data.PlanDate.replace("/Date(", "").replace(")/", "")));
            res.data.CfaId = "" + res.data.CfaId;
            res.data.SkuId = "" + res.data.SkuId;
            $scope.LoadPlan = res.data;
            toastr.success("Load Plan Saved Successfully");
        }, function (res) {
            console.error(res.data);
            toastr.error("Something went wrong, Failed to Save Load Plan ")
        })

    }
    $scope.delete = function () {
        console.log( "date", $scope.LoadPlan.PlanDate);
        if ($scope.LoadPlan.SkuId == "") {
            //  console.log($scope.LoadPlan);
            toastr.warning("Please Select Sku");
            return;
        }
        if ($scope.LoadPlan.CfaId == "") {
            toastr.warning("Please Select Cfa");
            return;
        }
        if ($scope.LoadPlan.PlanDate == undefined) {
            toastr.error("Please Enter valid Plan Date");
            return;
        }
    }
    $scope.getPlan = function () {
        if ($scope.LoadPlan.SkuId == "") {
            return;
        }
        if ($scope.LoadPlan.CfaId == "") {
            return;
        }
        if ($scope.LoadPlan.PlanDate == undefined) {
            return;
        }
        $http({
            method: "post",
            url: "/DailyPlan/getPlan",
            data: $scope.LoadPlan
        }).then(function (res) {
            res.data.SkuId = "" + res.data.SkuId;
            res.data.CfaId = "" + res.data.CfaId;
            res.data.PlanDate = new Date(parseInt(res.data.PlanDate.replace("/Date(", "").replace(")/", "")));
            $scope.LoadPlan = res.data;
            console.log(res.data);
        }, function ({data,status }) {
            console.log(status);
            if (status == 404) {
                toastr.error("Load Plan does not Exist");
            }
           
        })
    }

    $scope.upload = function () {
        var file = document.getElementById("filesheet").files[0];
        if (file == undefined) {
            toastr.error("Please Select Daily plan  file (excel) to upload ");
            return
        }
        let fileName = file.name.split(".");
        if (fileName[1] == "xlsx" || fileName[1] == "xls") {
            let fd = new FormData();
            let date = $scope.LoadPlan.PlanDate;
            fd.append("excel", file, "DailyPlan.xlsx");
            fd.append("SaveDate",`${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()}` );

            $.ajax({
                url: '/DailyPlan/UploadExcel',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fd,
                success: function ({ data, status }) {
                    console.log("succ", data);
                    console.log("succ", status);
                },
                error: function ({ data, status }) {
                    console.log("Fail", data);
                    console.log("fail", status);
                }
            });

        } else {
            toastr.error("Please Select excel file to upload ");
        }
        
        //$http({
        //    method: "Post",
        //    url: "/DailyPlan/UploadExcel",
        //    data: fd,
        //    headers: {
        //        "content-type": false,
        //        "process-data": false,
        //    }
        //}).then(function ({ data, status }) {
        //    console.log("succ", data);
        //    console.log("succ", status);
        //}, function ({ data, status }) {
        //    console.log("Fail", data);
        //    console.log("fail", status);
        //})
    }

});
