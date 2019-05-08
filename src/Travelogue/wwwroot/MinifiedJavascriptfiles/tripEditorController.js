!function(){"use strict";function t(t,n,a){var i=this,s=document.getElementById("name");new google.maps.places.Autocomplete(s);i.tripName=t.tripName,i.stops=[],i.errorMessage="",i.isBusy=!0,i.newStop={},o();var r="/api/trips/"+i.tripName+"/stops";n.get(r).then(function(t){console.log(i.stops),angular.copy(t.data,i.stops),e(i.stops)},function(t){i.errorMessage="failied to load stops"}).finally(function(){i.isBusy=!1}),i.addStop=function(){i.isBusy=!0;var t=$("#name").val();i.newStop.name=t,n.post(r,i.newStop).then(function(t){i.stops.push(t.data),e(i.stops),show_confirm_message({message:"Do you want to create a post for "+i.newStop.name,executeYes:function(){var t="/Posts/Create#/withMap/"+i.tripName+"/"+i.stops[i.stops.length-1].name;window.location=t},executeNo:function(){return!1}})},function(){i.errorMessage="Failed to add stop..."}).finally(function(){i.newStop={},i.isBusy=!1})},$("#viewMap").click(function(){$("html, body").animate({scrollTop:$("#map").offset().top},2e3)})}function e(t){if(t&&t.length>0){var e=_.map(t,function(t){return{lat:t.latitude,long:t.longitude,info:t.name}});travelMap.createMap({stops:e,selector:"#map",currentStop:1,initialZoom:3})}}function o(){travelMap.createMap({stops:!1,selector:"#map",currentStop:1,initialZoom:1.5})}angular.module("app-trips").controller("tripEditorController",t).directive("datepicker",function(){return{restrict:"A",require:"ngModel",link:function(t,e,o,n){var a=function(e){t.$apply(function(){n.$setViewValue(e)})},i={dateFormat:"dd/mm/yy",onSelect:function(t){a(t)}};e.datepicker(i)}}})}();