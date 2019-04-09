(function () {
    "use strict";

    angular.module("app-trips").controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http, $location) {
        var vm = this;

        var input = document.getElementById('name');
        var autocomplete = new google.maps.places.Autocomplete(input);

        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        var url = "/api/trips/" + vm.tripName + "/stops";

        $http.get(url)
            .then(function (response) {
                // success
                console.log(vm.stops);
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            }, function (err) {
                vm.errorMessage = "failied to load stops";
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {

            vm.isBusy = true;
            var stopName = $("#name").val();
            vm.newStop.name = stopName;

            $http.post(url, vm.newStop)
                .then(function (response) {
                    // success
                    vm.stops.push(response.data);
                    _showMap(vm.stops);
                   
                    show_confirm_message({
                        message: "Do you want to create a post for " + vm.newStop.name + "",
                        executeYes: function () {
                            window.location = "/Posts/Create";
                        },
                        executeNo: function () {
                            return false;
                        }
                    });

                }, function () {
                    //  failure
                    vm.errorMessage = "Failed to add stop..."
                })
            .finally(function () {
                vm.newStop = {};
                vm.isBusy = false;
            });
        };
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

    function createPost(message) {
        show_confirm_message({
            message: message,
            executeYes: function () {
                //window.location("~/Posts/Create");
                $location.path('~/Posts/Create');
            },
            executeNo: function () {
                return false;
            }
        });
    }

})();