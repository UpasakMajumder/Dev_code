webpackJsonp([4],{331:function(e,r,a){"use strict";function t(e,r){if(!(e instanceof r))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(r,"__esModule",{value:!0});var n=function(){function e(e,r){for(var a=0;a<r.length;a++){var t=r[a];t.enumerable=t.enumerable||!1,t.configurable=!0,"value"in t&&(t.writable=!0),Object.defineProperty(e,t.key,t)}}return function(r,a,t){return a&&e(r.prototype,a),t&&e(r,t),r}}(),l=function(){function e(r){t(this,e),this.fieldFromClass="js-replace-value-from",this.fieldToClass="js-replace-value-to";var a=r.dataset.replaceValueId;this.fieldsFrom=document.querySelectorAll("."+this.fieldFromClass+'[data-replace-value-id="'+a+'"]'),r.addEventListener("click",this.replace.bind(this))}return n(e,[{key:"replace",value:function(){var e=this;Array.from(this.fieldsFrom).forEach(function(r){var a=r.innerHTML,t=r.dataset.replaceId,n=document.querySelectorAll("."+e.fieldToClass+'[data-replace-id="'+t+'"]');Array.from(n).forEach(function(e){e.value=a})})}}]),e}();r.default=l}});