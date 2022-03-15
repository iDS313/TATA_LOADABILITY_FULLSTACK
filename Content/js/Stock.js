var app = angular.module("StockDetails", []);

app.controller("StockDetailsController", function ($scope, $http) {
    
    $scope.sku = [];
    $scope.payload = { SkuId: "", RecordedAt: new Date(), StartQty: 0 };

    $scope.getstock = function () {
        if ($scope.payload.RecordedAt == undefined) {
            toastr.error(" please Select Date");
            return;
        }
        if ($scope.payload.SkuId == "") {
           // toastr.error(" please Select Date");
            return;
        }
        $http({
            method: "Post",
            url: "/Stock/getStock",
            data: $scope.payload
        }).then(
            function ({ data, status }) {
                
                if (data.SkuId == 0) {
                    $scope.payload.SkuId = "";
                } else {
                    $scope.payload.SkuId = `${data.SkuId}`;
                }
                let d = parseInt(data.RecordedAt.replace("/Date(", "").replace(")/", ""));
                $scope.payload.StartQty = data.StartQty;
                $scope.payload.RecordedAt = new Date(d);
            },
            function ({ data, status }) {
                toastr.error("Record for selected SKU not found");
            });
    }
    $scope.save = function () {
        if ($scope.payload.SkuId == "") {
            toastr.error("select SKU ");
            return;
        }
        if ($scope.payload.RecordedAt == undefined) {
            toastr.error("Enter Date ");
            return;
        }
        if (isNaN($scope.payload.StartQty)) {
            $scope.payload.StartQty = 0;
            toastr.error("Please Enter valid Qty"); return;
        }
        $http({
            method: "Post",
            url: "/Stock/Save",
            data: $scope.payload
        }).then(function ({ data, status }) {
            $scope.payload.StartQty = data.StartQty;
            toastr.success("Stock Details Saved Successfully");
        }, function ({ data, status }) {
            toastr.error("Failed to save data");
            console.error("http save error", data);
        })
    }
    $scope.init = function () {
       
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
            toastr.error("Please Select Stock Details  file (excel) to upload ");
            return
        }
        let fileName = file.name.split(".");
        if (fileName[1] == "xlsx" || fileName[1] == "xls") {
            let fd = new FormData();
            let date = $scope.payload.RecordedAt;
            fd.append("excel", file, "StockDetails.xlsx");
            fd.append("SaveDate", `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`);

            $.ajax({
                url: '/Stock/UploadExcel',
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