webpackJsonp([19],{361:function(module,exports,__webpack_require__){try{(function(){"use strict";function _classCallCheck(instance,Constructor){if(!(instance instanceof Constructor))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(exports,"__esModule",{value:!0});var _createClass=function(){function defineProperties(target,props){for(var i=0;i<props.length;i++){var descriptor=props[i];descriptor.enumerable=descriptor.enumerable||!1,descriptor.configurable=!0,"value"in descriptor&&(descriptor.writable=!0),Object.defineProperty(target,descriptor.key,descriptor)}}return function(Constructor,protoProps,staticProps){return protoProps&&defineProperties(Constructor.prototype,protoProps),staticProps&&defineProperties(Constructor,staticProps),Constructor}}(),Spotfire=function(){function Spotfire(container){var _this=this;_classCallCheck(this,Spotfire);var dataset=container.dataset,containerId=container.id;this.customization=new spotfire.webPlayer.Customization,this.initCustomization(),window.addEventListener("load",function(){var app=new spotfire.webPlayer.Application("https://spotfire.cloud.tibco.com/spotfire/wp/",_this.customization,"/Gallery/Mashup","",!1),spotfireDoc=dataset.spotfireDoc;dataset.spotfireFull&&_this.initFullScreenCustomization(),app.openDocument(containerId,spotfireDoc,_this.customization)})}return _createClass(Spotfire,[{key:"initCustomization",value:function(){this.customization.showClose=!1,this.customization.showUndoRedo=!0,this.customization.showToolBar=!1,this.customization.showDodPanel=!1,this.customization.showStatusBar=!1,this.customization.showExportFile=!1,this.customization.showFilterPanel=!1,this.customization.showAnalysisInfo=!0,this.customization.showPageNavigation=!1,this.customization.showExportVisualization=!1}},{key:"initFullScreenCustomization",value:function(){this.customization.showClose=!0,this.customization.showToolBar=!0,this.customization.showDodPanel=!0,this.customization.showStatusBar=!0,this.customization.showExportFile=!0,this.customization.showFilterPanel=!0,this.customization.showExportVisualization=!0}}]),Spotfire}();exports.default=Spotfire}).call(this)}finally{}}});