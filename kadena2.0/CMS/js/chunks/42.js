webpackJsonp([42],{391:function(module,exports,__webpack_require__){try{(function(){"use strict";function _classCallCheck(instance,Constructor){if(!(instance instanceof Constructor))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(exports,"__esModule",{value:!0});var _createClass=function(){function defineProperties(target,props){for(var i=0;i<props.length;i++){var descriptor=props[i];descriptor.enumerable=descriptor.enumerable||!1,descriptor.configurable=!0,"value"in descriptor&&(descriptor.writable=!0),Object.defineProperty(target,descriptor.key,descriptor)}}return function(Constructor,protoProps,staticProps){return protoProps&&defineProperties(Constructor.prototype,protoProps),staticProps&&defineProperties(Constructor,staticProps),Constructor}}(),ReplaceValue=function(){function ReplaceValue(button){_classCallCheck(this,ReplaceValue),this.fieldFromClass="js-replace-value-from",this.fieldToClass="js-replace-value-to";var replaceValueId=button.dataset.replaceValueId;this.fieldsFrom=document.querySelectorAll("."+this.fieldFromClass+'[data-replace-value-id="'+replaceValueId+'"]'),button.addEventListener("click",this.replace.bind(this))}return _createClass(ReplaceValue,[{key:"replace",value:function(){var _this=this;Array.from(this.fieldsFrom).forEach(function(fieldFrom){var value=fieldFrom.innerHTML,replaceId=fieldFrom.dataset.replaceId,fieldsTo=document.querySelectorAll("."+_this.fieldToClass+'[data-replace-id="'+replaceId+'"]');Array.from(fieldsTo).forEach(function(fieldTo){fieldTo.value=value})})}}]),ReplaceValue}();exports.default=ReplaceValue}).call(this)}finally{}}});