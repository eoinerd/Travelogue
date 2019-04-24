!function(){"use strict";function t(t,n,a){var s=this,i=document.getElementById("name");new google.maps.places.Autocomplete(i);s.tripName=t.tripName,s.stops=[],s.errorMessage="",s.isBusy=!0,s.newStop={},o();var r="/api/trips/"+s.tripName+"/stops";n.get(r).then(function(t){console.log(s.stops),angular.copy(t.data,s.stops),e(s.stops)},function(t){s.errorMessage="failied to load stops"}).finally(function(){s.isBusy=!1}),s.addStop=function(){s.isBusy=!0;var t=$("#name").val();s.newStop.name=t,n.post(r,s.newStop).then(function(t){s.stops.push(t.data),e(s.stops),show_confirm_message({message:"Do you want to create a post for "+s.newStop.name,executeYes:function(){var t="/Posts/Create#/withMap/"+s.tripName+"/"+s.stops[s.stops.length-1].name;window.location=t},executeNo:function(){return!1}})},function(){s.errorMessage="Failed to add stop..."}).finally(function(){s.newStop={},s.isBusy=!1})}}function e(t){if(t&&t.length>0){var e=_.map(t,function(t){return{lat:t.latitude,long:t.longitude,info:t.name}});travelMap.createMap({stops:e,selector:"#map",currentStop:1,initialZoom:3})}}function o(){travelMap.createMap({stops:!1,selector:"#map",currentStop:1,initialZoom:1.5})}angular.module("app-trips").controller("tripEditorController",t).directive("datepicker",function(){return{restrict:"A",require:"ngModel",link:function(t,e,o,n){var a=function(e){t.$apply(function(){n.$setViewValue(e)})},s={dateFormat:"dd/mm/yy",onSelect:function(t){a(t)}};e.datepicker(s)}}})}();