!function(){"use strict";angular.module("app-trips",[]).directive("datepicker",function(){return{restrict:"A",require:"ngModel",link:function(e,t,n,i){var r=function(t){e.$apply(function(){i.$setViewValue(t)})},c={dateFormat:"dd/mm/yy",onSelect:function(e){r(e)}};t.datepicker(c)}}})}();