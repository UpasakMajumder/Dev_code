webpackJsonp([68,89],{377:function(module,exports,__webpack_require__){try{(function(){"use strict";Object.defineProperty(exports,"__esModule",{value:!0});exports.cutWords=function(text,number){var array=text.split(" "),filteredArray=array.filter(function(word,i){return i<number-1}),string=filteredArray.join(" ");return array.length>number?string+"...":string},exports.bla=1}).call(this)}finally{}},396:function(module,exports,__webpack_require__){try{(function(){"use strict";function _interopRequireDefault(obj){return obj&&obj.__esModule?obj:{default:obj}}function _classCallCheck(instance,Constructor){if(!(instance instanceof Constructor))throw new TypeError("Cannot call a class as a function")}function _possibleConstructorReturn(self,call){if(!self)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return!call||"object"!=typeof call&&"function"!=typeof call?self:call}function _inherits(subClass,superClass){if("function"!=typeof superClass&&null!==superClass)throw new TypeError("Super expression must either be null or a function, not "+typeof superClass);subClass.prototype=Object.create(superClass&&superClass.prototype,{constructor:{value:subClass,enumerable:!1,writable:!0,configurable:!0}}),superClass&&(Object.setPrototypeOf?Object.setPrototypeOf(subClass,superClass):subClass.__proto__=superClass)}Object.defineProperty(exports,"__esModule",{value:!0});var _createClass=function(){function defineProperties(target,props){for(var i=0;i<props.length;i++){var descriptor=props[i];descriptor.enumerable=descriptor.enumerable||!1,descriptor.configurable=!0,"value"in descriptor&&(descriptor.writable=!0),Object.defineProperty(target,descriptor.key,descriptor)}}return function(Constructor,protoProps,staticProps){return protoProps&&defineProperties(Constructor.prototype,protoProps),staticProps&&defineProperties(Constructor,staticProps),Constructor}}(),_react=__webpack_require__(21),_react2=_interopRequireDefault(_react),_propTypes=__webpack_require__(36),_propTypes2=_interopRequireDefault(_propTypes),_string=__webpack_require__(377),Page=function(_Component){function Page(){return _classCallCheck(this,Page),_possibleConstructorReturn(this,(Page.__proto__||Object.getPrototypeOf(Page)).apply(this,arguments))}return _inherits(Page,_Component),_createClass(Page,[{key:"render",value:function(){var _props=this.props,url=_props.url,title=_props.title,text=_props.text,descriptionText=text?_react2.default.createElement("p",null,(0,_string.cutWords)(text,24)):null;return _react2.default.createElement("div",{className:"search-result__page"},_react2.default.createElement("h3",null,_react2.default.createElement("a",{href:url},title)),descriptionText)}}]),Page}(_react.Component);exports.default=Page,Page.propTypes={url:_propTypes2.default.string.isRequired,title:_propTypes2.default.string.isRequired,text:_propTypes2.default.string}}).call(this)}finally{}}});