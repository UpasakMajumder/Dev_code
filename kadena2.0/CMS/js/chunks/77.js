webpackJsonp([77],{438:function(module,exports,__webpack_require__){try{(function(){"use strict";function _classCallCheck(instance,Constructor){if(!(instance instanceof Constructor))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(exports,"__esModule",{value:!0});var _createClass=function(){function defineProperties(target,props){for(var i=0;i<props.length;i++){var descriptor=props[i];descriptor.enumerable=descriptor.enumerable||!1,descriptor.configurable=!0,"value"in descriptor&&(descriptor.writable=!0),Object.defineProperty(target,descriptor.key,descriptor)}}return function(Constructor,protoProps,staticProps){return protoProps&&defineProperties(Constructor.prototype,protoProps),staticProps&&defineProperties(Constructor,staticProps),Constructor}}(),Sidebar=function(){function Sidebar(container){var _this=this;_classCallCheck(this,Sidebar),this.container=container,this.animatedClass="isFixed",this.containerOffsetTop=container.parentNode.offsetTop,this.containerHeight=container.offsetHeight,this.screenVisibleHeight=window.innerHeight-this.containerOffsetTop,window.addEventListener("scroll",function(){_this.scroll()}),this.scroll(),this.resize()}return _createClass(Sidebar,[{key:"scroll",value:function(){window.pageYOffset<this.containerOffsetTop||this.containerHeight>this.screenVisibleHeight?this.container.classList.remove(this.animatedClass):this.container.classList.add(this.animatedClass)}},{key:"resize",value:function(){var _this2=this;window.addEventListener("resize",function(){_this2.screenVisibleHeight=window.innerHeight-_this2.containerOffsetTop})}}]),Sidebar}();exports.default=Sidebar}).call(this)}finally{}}});