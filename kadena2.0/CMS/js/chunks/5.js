webpackJsonp([5],{381:function(t,o,i){"use strict";function e(t,o){if(!(t instanceof o))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(o,"__esModule",{value:!0});var r=function(){function t(t,o){for(var i=0;i<o.length;i++){var e=o[i];e.enumerable=e.enumerable||!1,e.configurable=!0,"value"in e&&(e.writable=!0),Object.defineProperty(t,e.key,e)}}return function(o,i,e){return i&&t(o.prototype,i),e&&t(o,e),o}}(),s=i(106),n=i(69),a=function(){function t(o){var i=this;e(this,t);var r=s.SPOTFIRE.serverUrl,a=s.SPOTFIRE.url,u=s.SPOTFIRE.customerId;try{this.customisation=new window.spotfire.webPlayer.Customization}catch(t){console.error(t),n.toastr.error(s.NOTIFICATION.spotfireError.title,s.NOTIFICATION.spotfireError.text)}var c=new window.spotfire.webPlayer.Application(r,this.customisation,a,"",!1),l=window.spotfire.webPlayer.filteringOperation.REPLACE,f={filteringSchemeName:"Filtering scheme",dataTableName:"CDH_Inventory_Extract_VW_IL",dataColumnName:"Client_ID",filteringOperation:l,filterSettings:{values:[u]}};if(o.dataset.report){var h=o.id;this.initCustomization(!0),c.openDocument(h,0,this.customisation)}else{var m=Array.from(o.querySelectorAll(".js-spotfire-tab"));this.initCustomization(!1),m.forEach(function(t){var o=t.id,e=t.dataset,r=e.doc;c.openDocument(o,r,i.customisation)})}c.analysisDocument.filtering.setFilter(f,l),c.onError(function(t,o){console.error(t,o),n.toastr.error(s.NOTIFICATION.spotfireError.title,s.NOTIFICATION.spotfireError.text)})}return r(t,[{key:"initCustomization",value:function(t){this.customisation.showClose=!1,this.customisation.showUndoRedo=!0,this.customisation.showToolBar=!1,this.customisation.showDodPanel=!1,this.customisation.showStatusBar=!1,this.customisation.showExportFile=!1,this.customisation.showFilterPanel=!1,this.customisation.showAnalysisInfo=!0,this.customisation.showPageNavigation=t,this.customisation.showExportVisualization=!1}}]),t}();o.default=a}});