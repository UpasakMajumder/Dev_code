webpackJsonp([48,72],{337:function(module,exports,__webpack_require__){try{(function(){"use strict";function removeProps(obj,props){var objRemovedProps=Object.assign({},obj);return props.forEach(function(prop){delete objRemovedProps[prop]}),objRemovedProps}Object.defineProperty(exports,"__esModule",{value:!0}),exports.default=removeProps}).call(this)}finally{}},346:function(module,exports,__webpack_require__){try{(function(){"use strict";function _interopRequireDefault(obj){return obj&&obj.__esModule?obj:{default:obj}}function TextInput(props){var label=props.label,error=props.error,disabled=props.disabled,inputProps=(0,_object2.default)(props,["label","error"]),labelElement=label?_react2.default.createElement("span",{className:"input__label"},label):null,className=disabled?"input__wrapper input__wrapper--disabled":"input__wrapper",errorElement=error?_react2.default.createElement("span",{className:"input__error input__error--noborder"},error):null,errorClass=error?"input--error":"";return _react2.default.createElement("div",{className:className},labelElement,_react2.default.createElement("input",_extends({type:"text",className:"input__text "+errorClass},inputProps)),errorElement)}Object.defineProperty(exports,"__esModule",{value:!0});var _extends=Object.assign||function(target){for(var i=1;i<arguments.length;i++){var source=arguments[i];for(var key in source)Object.prototype.hasOwnProperty.call(source,key)&&(target[key]=source[key])}return target};exports.default=TextInput;var _react=__webpack_require__(21),_react2=_interopRequireDefault(_react),_object=__webpack_require__(337),_object2=_interopRequireDefault(_object);TextInput.propTypes={label:_react.PropTypes.string,placeholder:_react.PropTypes.string,error:_react.PropTypes.string,disabled:_react.PropTypes.bool}}).call(this)}finally{}}});