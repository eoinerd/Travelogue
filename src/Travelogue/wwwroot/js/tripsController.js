// tripsComtroller.js

(function () {

    "use strict";

    // Getting the existing module
    angular.module("app-trips")
         .controller("tripsController", tripsController);
     
    function tripsController($http) {

        var vm = this;

        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;
        
        _showEmptyMap();

        $http.get("/api/trips")
        .then(function (response) {
            // Success
            angular.copy(response.data, vm.trips);
        }, function (error) {
            // Failure
            vm.errorMessage = "Failed to load data: " + errorMessage;
        })
        .finally(function () {
            vm.isBusy = false;
        });

        vm.addTrip = function () {
            
            vm.isBusy = true;
            vm.errorMessage = "";
            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                // Success
                vm.trips.push(response.data);
                vm.newTrip = {};
            }, function () {
                vm.errorMessage = "Failed to save new trip"
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };

        function _showEmptyMap() {
            // Show Empty Map
            travelMap.createMap({
                stops: false,
                selector: "#map",
                currentStop: 1,
                initialZoom: 1.5
            });
        }
    }
})();