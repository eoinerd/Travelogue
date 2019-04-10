(function () {
    "use strict";

    angular.module("app-trips").controller("createPostWithMapController", createPostWithMapController);

    function createPostWithMapController($routeParams, $http, $location) {
        var vm = this;

        var input = document.getElementById('name');
        var autocomplete = new google.maps.places.Autocomplete(input);
        vm.stopForCreatePost = [];

        vm.tripName = $routeParams.tripName;
        vm.stopName = $routeParams.stopName;

        var url = "/api/stop/" + vm.tripName + "/" + vm.stopName;

        $http.get(url)
            .then(function (response) {
                // success
                angular.copy(response.data, vm.stopForCreatePost);
                _showMap(vm.stopForCreatePost);
            }, function (err) {
                vm.errorMessage = "failied to load stops";
            })
            .finally(function () {
                vm.isBusy = false;
            });
    }

    function _showMap(stops) {
        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });

            // Show Map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3
            });
        }
    }

})();