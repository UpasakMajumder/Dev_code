webpackJsonp([24],{355:function(module,exports,__webpack_require__){try{(function(){"use strict";function _classCallCheck(instance,Constructor){if(!(instance instanceof Constructor))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(exports,"__esModule",{value:!0});var Dialog=function Dialog(clicker){var _this=this;_classCallCheck(this,Dialog),this.clicker=clicker,this.activeClass="active",this.html=document.querySelector("html");var dialogSelector=clicker.dataset.dialog;this.dialog=document.querySelector(dialogSelector),this.closerNodes=this.dialog.querySelectorAll(".dialog__closer"),this.clicker.addEventListener("click",function(){!_this.dialog.classList.contains(_this.activeClass)&&_this.dialog.classList.add(_this.activeClass),_this.html.classList.add("css-overflow-hidden")}),Array.from(this.closerNodes).forEach(function(closer){closer.addEventListener("click",function(){_this.dialog.classList.contains(_this.activeClass)&&_this.dialog.classList.remove(_this.activeClass),_this.html.classList.remove("css-overflow-hidden")})})};exports.default=Dialog}).call(this)}finally{}}});