webpackJsonp([94],{353:function(module,exports,__webpack_require__){try{(function(){"use strict";Object.defineProperty(exports,"__esModule",{value:!0}),exports.closeDialog=exports.openDialog=void 0;var _constants=__webpack_require__(38);exports.openDialog=function(_ref){var headerTitle=_ref.headerTitle,isCloseButton=_ref.isCloseButton,body=_ref.body,footer=_ref.footer;return function(dispatch){dispatch({type:_constants.DIALOG_OPEN,payload:{headerTitle:headerTitle,isCloseButton:isCloseButton,body:body,footer:footer}})}},exports.closeDialog=function(){return function(dispatch){return dispatch({type:_constants.DIALOG_CLOSE})}}}).call(this)}finally{}}});