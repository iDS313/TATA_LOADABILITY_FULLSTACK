var app = angular.module("PrDetails", []);

app.controller("PrController", function ($scope, $http) {
    $scope.cfa = [];
    $scope.sku = [];

    $scope.payload = { CfaId: "", SkuId: "", PrQty: 0, PrDate: new Date() };

    $scope.getpr = function () {
        if ($scope.payload.CfaId == "") {
            toastr.error("Please Select Cfa");
            return;
        }
        if ($scope.payload.SkuId == "") {
            toastr.error("Please Select Sku");
            return;
        }
        if ($scope.payload.PrDate == undefined) {
            toastr.error("please provide PR Date");
        }
        $http({
            method: "Post",
            url: "/PrDetails/getPr",
            data: $scope.payload,
        }).then(function ({ data, status }) {
            $scope.payload.PrQty = data.PrQty;
        },
            function ({ data, status }) {
                console.error(data);
                $scope.payload.PrQty = 0;
        });
    }
    $scope.save = function () {
        if ($scope.payload.CfaId == "") {
            toastr.error("Please Select Cfa");
            return;
        }
        if ($scope.payload.SkuId == "") {
            toastr.error("Please Select Sku");
            return;
        }
        if ($scope.payload.PrDate == undefined) {
            toastr.error("please provide PR Date");
        }
        $http({
            method: "Post",
            url: "/PrDetails/save",
            data: $scope.payload,
        }).then(function ({ data, status }) {
            $scope.payload.PrQty = data.PrQty;
        },
            function ({ data, status }) {
                console.error(data);
            });
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
    }

    $scope.upload = function () {
        var file = document.getElementById("filesheet").files[0];
        if (file == undefined) {
            toastr.error("Please Select PR Details  file (excel) to upload ");
            return
        }
        let fileName = file.name.split(".");
        if (fileName[1] == "xlsx" || fileName[1] == "xls") {
            let fd = new FormData();
            let date = $scope.payload.PrDate;
            fd.append("excel", file, "PrDetails.xlsx");
            fd.append("SaveDate", `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`);

            $.ajax({
                url: '/PrDetails/UploadExcel',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fd,
                success: function ({ data, status }) {
                    toastr.success("File Uploaded Successfully");
                    console.log("succ", status);
                },
                error: function ({ data, status }) {
                    toastr.error("Failed to Upload File");
                    console.log("Fail", data);
                    console.log("fail", status);
                }
            });

        } else {
            toastr.error("Please Select excel file to upload ");
        }

        
    }
});