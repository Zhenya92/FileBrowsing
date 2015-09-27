myApp.controller("values", function ($scope, $http, DirectoryService) {
    getDirectoryModel("...");
    function getDirectoryModel(url) {
        $('.loader').show();
        $http({ method: 'GET', url: "api/values/GetDirectoryData", headers: { 'Authorization': 'Basic login:password' }, params: { 'path': url } })
            .success(function (directoryData) {

                if (directoryData.currendDirInfo.ParentFolder.FullName == null)
                    $('.backTo p:first').hide();
                else
                    $('.backTo p:first').show();

                if (directoryData.currendDirInfo.Files == null)
                    $('.files_block').hide();
                else
                    $('.files_block').show();
                $scope.directoryModel = directoryData;
                $('.loader').hide();
                //return angular.toJson(directoryData);
                /*
                if (book.Id >= 0) {
                    alert('success');
                    var modelBlock = angular.element(document.querySelector('.bookModel'));
                    modelBlock.find('.item').eq(0).text(book.Id);
                    modelBlock.find('.item').eq(1).text(book.Name);
                    modelBlock.css('display', 'block');
                }
                else
                    alert('failue');*/
            })
            .error(function (data, status) {
                $('.loader').hide();
            });
    }
    $scope.openDirectory = function (url) {
        getDirectoryModel(url);
    }
});