/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./Scripts/Common/ContextMenu/ContextMenu.tsx");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Scripts/Common/ContextMenu/ContextMenu.tsx":
/*!****************************************************!*\
  !*** ./Scripts/Common/ContextMenu/ContextMenu.tsx ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var kendo_react_buttons_1 = __webpack_require__(/*! @progress/kendo-react-buttons */ "./node_modules/@progress/kendo-react-buttons/dist/es/main.js");

var ContextMenu = function (_React$Component) {
    _inherits(ContextMenu, _React$Component);

    function ContextMenu(props) {
        _classCallCheck(this, ContextMenu);

        var _this = _possibleConstructorReturn(this, (ContextMenu.__proto__ || Object.getPrototypeOf(ContextMenu)).call(this, props));

        _this.onMouseOut = _this.onMouseOut.bind(_this);
        _this.onClick = _this.onClick.bind(_this);
        return _this;
    }

    _createClass(ContextMenu, [{
        key: "onClick",
        value: function onClick(e) {
            var b = e.target;
            var index = e.target.getAttribute("itemid");
            if (this.props.menus && this.props.menus[index] && this.props.menus[index].action) this.props.menus[index].action(e, this.props.item);
        }
    }, {
        key: "onMouseOut",
        value: function onMouseOut(e) {
            this.props.sender.onContextMenuMouseOut(e, this.props.item);
        }
    }, {
        key: "renderMenu",
        value: function renderMenu() {
            var _this2 = this;

            var menus = [];
            this.props.menus.map(function (item, index) {
                menus.push(React.createElement(kendo_react_buttons_1.Button, { className: "col-sm-12 context-menu-item", style: { textAlign: 'left', display: 'block' }, icon: item.icon, key: index, itemID: index + "", onClick: _this2.onClick }, item.text));
            });
            return menus;
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement("div", { id: "context_menu_container", className: "row context-menu", style: this.props.style, onMouseLeave: this.onMouseOut }, this.renderMenu());
        }
    }]);

    return ContextMenu;
}(React.Component);

exports.ContextMenu = ContextMenu;

/***/ }),

/***/ "./node_modules/@babel/runtime/helpers/interopRequireDefault.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@babel/runtime/helpers/interopRequireDefault.js ***!
  \**********************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function _interopRequireDefault(obj) {
  return obj && obj.__esModule ? obj : {
    default: obj
  };
}

module.exports = _interopRequireDefault;

/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/add-scroll.js":
/*!*************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/add-scroll.js ***!
  \*************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return addScroll; });
function addScroll(rect, scroll) {
    return {
        top: rect.top + scroll.y,
        left: rect.left + scroll.x,
        height: rect.height,
        width: rect.width
    };
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/align-point.js":
/*!**************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/align-point.js ***!
  \**************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ({
    "bottom": "bottom",
    "center": "center",
    "middle": "middle",
    "left": "left",
    "right": "right",
    "top": "top"
});


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/align.js":
/*!********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/align.js ***!
  \********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _align_point__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./align-point */ "./node_modules/@progress/kendo-popup-common/dist/es/align-point.js");


var align = function (options) {
    var anchorRect = options.anchorRect;
    var anchorAlign = options.anchorAlign;
    var elementRect = options.elementRect;
    var elementAlign = options.elementAlign;
    var margin = options.margin; if ( margin === void 0 ) margin = {};
    var anchorHorizontal = anchorAlign.horizontal;
    var anchorVertical = anchorAlign.vertical;
    var elementHorizontal = elementAlign.horizontal;
    var elementVertical = elementAlign.vertical;

    var horizontalMargin = margin.horizontal || 0;
    var verticalMargin = margin.vertical || 0;

    var top = anchorRect.top;
    var left = anchorRect.left;

    if (anchorVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].bottom) {
        top += anchorRect.height;
    }

    if (anchorVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || anchorVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle) {
        top += Math.round(anchorRect.height / 2);
    }

    if (elementVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].bottom) {
        top -= elementRect.height;
        verticalMargin *= -1;
    }

    if (elementVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || elementVertical === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle) {
        top -= Math.round(elementRect.height / 2);
        verticalMargin *= -1;
    }

    if (anchorHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].right) {
        left += anchorRect.width;
    }

    if (anchorHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || anchorHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle) {
        left += Math.round(anchorRect.width / 2);
    }

    if (elementHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].right) {
        left -= elementRect.width;
        horizontalMargin *= -1;
    }

    if (elementHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || elementHorizontal === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle) {
        left -= Math.round(elementRect.width / 2);
        horizontalMargin *= -1;
    }

    return {
        top: top + verticalMargin,
        left: left + horizontalMargin
    };
};

/* harmony default export */ __webpack_exports__["default"] = (align);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/apply-location-offset.js":
/*!************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/apply-location-offset.js ***!
  \************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return applyLocationOffset; });
function applyLocationOffset(rect, location, isOffsetBody) {
    var top = rect.top;
    var left = rect.left;

    if (isOffsetBody) {
        left = 0;
        top = 0;
    }

    return {
        top: top + location.top,
        left: left + location.left,
        height: rect.height,
        width: rect.width
    };
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/bounding-offset.js":
/*!******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/bounding-offset.js ***!
  \******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _window_viewport__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./window-viewport */ "./node_modules/@progress/kendo-popup-common/dist/es/window-viewport.js");


var boundingOffset = function (element) {
    if (!element.getBoundingClientRect) {
        var viewport = Object(_window_viewport__WEBPACK_IMPORTED_MODULE_0__["default"])(element);
        return {
            bottom: viewport.height,
            left: 0,
            right: viewport.width,
            top: 0
        };
    }

    var ref = element.getBoundingClientRect();
    var bottom = ref.bottom;
    var left = ref.left;
    var right = ref.right;
    var top = ref.top;

    return {
        bottom: bottom,
        left: left,
        right: right,
        top: top
    };
};

/* harmony default export */ __webpack_exports__["default"] = (boundingOffset);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/collision.js":
/*!************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/collision.js ***!
  \************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ({
    "fit": "fit",
    "flip": "flip"
});


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/document.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/document.js ***!
  \***********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _owner_document__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./owner-document */ "./node_modules/@progress/kendo-popup-common/dist/es/owner-document.js");


var getDocument = function (element) { return Object(_owner_document__WEBPACK_IMPORTED_MODULE_0__["default"])(element).documentElement; };

/* harmony default export */ __webpack_exports__["default"] = (getDocument);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/element-scroll-position.js":
/*!**************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/element-scroll-position.js ***!
  \**************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _scroll_position__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/scroll-position.js");


/* harmony default export */ __webpack_exports__["default"] = (function (element) {
    if (element === (element.ownerDocument || {}).body) {
        return Object(_scroll_position__WEBPACK_IMPORTED_MODULE_0__["default"])(element);
    }

    return {
        x: element.scrollLeft,
        y: element.scrollTop
    };
});;


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/is-body-offset.js":
/*!*****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/is-body-offset.js ***!
  \*****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _offset_parent__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./offset-parent */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js");


var isBodyOffset = function (element) { return (Object(_offset_parent__WEBPACK_IMPORTED_MODULE_0__["default"])(element) === element.ownerDocument.body); };

/* harmony default export */ __webpack_exports__["default"] = (isBodyOffset);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/main.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/main.js ***!
  \*******************************************************************/
/*! exports provided: align, addScroll, applyLocationOffset, boundingOffset, isBodyOffset, offsetParent, offset, parents, parentScrollPosition, position, positionWithScroll, removeScroll, restrictToView, scrollPosition, siblingContainer, siblings, getDocumentElement, getWindow, getWindowViewPort, AlignPoint, Collision */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _align__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./align */ "./node_modules/@progress/kendo-popup-common/dist/es/align.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "align", function() { return _align__WEBPACK_IMPORTED_MODULE_0__["default"]; });

/* harmony import */ var _add_scroll__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./add-scroll */ "./node_modules/@progress/kendo-popup-common/dist/es/add-scroll.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "addScroll", function() { return _add_scroll__WEBPACK_IMPORTED_MODULE_1__["default"]; });

/* harmony import */ var _apply_location_offset__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./apply-location-offset */ "./node_modules/@progress/kendo-popup-common/dist/es/apply-location-offset.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "applyLocationOffset", function() { return _apply_location_offset__WEBPACK_IMPORTED_MODULE_2__["default"]; });

/* harmony import */ var _bounding_offset__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./bounding-offset */ "./node_modules/@progress/kendo-popup-common/dist/es/bounding-offset.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "boundingOffset", function() { return _bounding_offset__WEBPACK_IMPORTED_MODULE_3__["default"]; });

/* harmony import */ var _is_body_offset__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./is-body-offset */ "./node_modules/@progress/kendo-popup-common/dist/es/is-body-offset.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "isBodyOffset", function() { return _is_body_offset__WEBPACK_IMPORTED_MODULE_4__["default"]; });

/* harmony import */ var _offset_parent__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./offset-parent */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "offsetParent", function() { return _offset_parent__WEBPACK_IMPORTED_MODULE_5__["default"]; });

/* harmony import */ var _offset__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./offset */ "./node_modules/@progress/kendo-popup-common/dist/es/offset.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "offset", function() { return _offset__WEBPACK_IMPORTED_MODULE_6__["default"]; });

/* harmony import */ var _parents__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./parents */ "./node_modules/@progress/kendo-popup-common/dist/es/parents.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "parents", function() { return _parents__WEBPACK_IMPORTED_MODULE_7__["default"]; });

/* harmony import */ var _parent_scroll_position__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./parent-scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/parent-scroll-position.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "parentScrollPosition", function() { return _parent_scroll_position__WEBPACK_IMPORTED_MODULE_8__["default"]; });

/* harmony import */ var _position__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./position */ "./node_modules/@progress/kendo-popup-common/dist/es/position.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "position", function() { return _position__WEBPACK_IMPORTED_MODULE_9__["default"]; });

/* harmony import */ var _position_with_scroll__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./position-with-scroll */ "./node_modules/@progress/kendo-popup-common/dist/es/position-with-scroll.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "positionWithScroll", function() { return _position_with_scroll__WEBPACK_IMPORTED_MODULE_10__["default"]; });

/* harmony import */ var _remove_scroll__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./remove-scroll */ "./node_modules/@progress/kendo-popup-common/dist/es/remove-scroll.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "removeScroll", function() { return _remove_scroll__WEBPACK_IMPORTED_MODULE_11__["default"]; });

/* harmony import */ var _restrict_to_view__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./restrict-to-view */ "./node_modules/@progress/kendo-popup-common/dist/es/restrict-to-view.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "restrictToView", function() { return _restrict_to_view__WEBPACK_IMPORTED_MODULE_12__["default"]; });

/* harmony import */ var _scroll_position__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ./scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/scroll-position.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "scrollPosition", function() { return _scroll_position__WEBPACK_IMPORTED_MODULE_13__["default"]; });

/* harmony import */ var _sibling_container__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ./sibling-container */ "./node_modules/@progress/kendo-popup-common/dist/es/sibling-container.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "siblingContainer", function() { return _sibling_container__WEBPACK_IMPORTED_MODULE_14__["default"]; });

/* harmony import */ var _siblings__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ./siblings */ "./node_modules/@progress/kendo-popup-common/dist/es/siblings.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "siblings", function() { return _siblings__WEBPACK_IMPORTED_MODULE_15__["default"]; });

/* harmony import */ var _document__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ./document */ "./node_modules/@progress/kendo-popup-common/dist/es/document.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "getDocumentElement", function() { return _document__WEBPACK_IMPORTED_MODULE_16__["default"]; });

/* harmony import */ var _window__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ./window */ "./node_modules/@progress/kendo-popup-common/dist/es/window.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "getWindow", function() { return _window__WEBPACK_IMPORTED_MODULE_17__["default"]; });

/* harmony import */ var _window_viewport__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ./window-viewport */ "./node_modules/@progress/kendo-popup-common/dist/es/window-viewport.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "getWindowViewPort", function() { return _window_viewport__WEBPACK_IMPORTED_MODULE_18__["default"]; });

/* harmony import */ var _align_point__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ./align-point */ "./node_modules/@progress/kendo-popup-common/dist/es/align-point.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AlignPoint", function() { return _align_point__WEBPACK_IMPORTED_MODULE_19__["default"]; });

/* harmony import */ var _collision__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(/*! ./collision */ "./node_modules/@progress/kendo-popup-common/dist/es/collision.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Collision", function() { return _collision__WEBPACK_IMPORTED_MODULE_20__["default"]; });


























/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent-scroll-position.js":
/*!********************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/offset-parent-scroll-position.js ***!
  \********************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _element_scroll_position__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./element-scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/element-scroll-position.js");
/* harmony import */ var _parent_scroll_position__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./parent-scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/parent-scroll-position.js");



/* harmony default export */ __webpack_exports__["default"] = (function (offsetParentElement, element) { return ( // eslint-disable-line no-arrow-condition
    offsetParentElement ? Object(_element_scroll_position__WEBPACK_IMPORTED_MODULE_0__["default"])(offsetParentElement) : Object(_parent_scroll_position__WEBPACK_IMPORTED_MODULE_1__["default"])(element)
); });;


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js":
/*!****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js ***!
  \****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _document__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./document */ "./node_modules/@progress/kendo-popup-common/dist/es/document.js");


var offsetParent = function (element) {
    var offsetParent = element.offsetParent;

    while (offsetParent && offsetParent.style.position === "static") {
        offsetParent = offsetParent.offsetParent;
    }

    return offsetParent || Object(_document__WEBPACK_IMPORTED_MODULE_0__["default"])(element);
};

/* harmony default export */ __webpack_exports__["default"] = (offsetParent);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/offset.js":
/*!*********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/offset.js ***!
  \*********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
var rectOfHiddenElement = function (element) {
    var ref = element.style;
    var display = ref.display;
    var left = ref.left;
    var position = ref.position;

    element.style.display = '';
    element.style.left = '-10000px';
    element.style.position = 'absolute';

    var rect = element.getBoundingClientRect();

    element.style.display = display;
    element.style.left = left;
    element.style.position = position;

    return rect;
};

var offset = function (element) {
    var rect = element.getBoundingClientRect();
    var left = rect.left;
    var top = rect.top;

    if (!rect.height && !rect.width) {
        rect = rectOfHiddenElement(element);
    }

    return {
        top: top,
        left: left,
        height: rect.height,
        width: rect.width
    };
};

/* harmony default export */ __webpack_exports__["default"] = (offset);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/owner-document.js":
/*!*****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/owner-document.js ***!
  \*****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return ownerDocument; });
function ownerDocument(element) {
    return element.ownerDocument || element.document || element;
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/parent-scroll-position.js":
/*!*************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/parent-scroll-position.js ***!
  \*************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return parentScrollPosition; });
/* harmony import */ var _offset_parent__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./offset-parent */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js");
/* harmony import */ var _element_scroll_position__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./element-scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/element-scroll-position.js");



function parentScrollPosition(element) {
    var parent = Object(_offset_parent__WEBPACK_IMPORTED_MODULE_0__["default"])(element);

    return parent ? Object(_element_scroll_position__WEBPACK_IMPORTED_MODULE_1__["default"])(parent) : { x: 0, y: 0 };
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/parents.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/parents.js ***!
  \**********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = (function (element, until) {
    var result = [];
    var next = element.parentNode;

    while (next) {
        result.push(next);

        if (next === until) { break; }

        next = next.parentNode;
    }

    return result;
});;


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/position-with-scroll.js":
/*!***********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/position-with-scroll.js ***!
  \***********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _offset_parent_scroll_position__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./offset-parent-scroll-position */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent-scroll-position.js");
/* harmony import */ var _offset_parent__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./offset-parent */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js");
/* harmony import */ var _position__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./position */ "./node_modules/@progress/kendo-popup-common/dist/es/position.js");




/* harmony default export */ __webpack_exports__["default"] = (function (element, parent, scale) {
    if ( scale === void 0 ) scale = 1;

    var offsetParentElement = parent ? Object(_offset_parent__WEBPACK_IMPORTED_MODULE_1__["default"])(parent) : null;
    var ref = Object(_position__WEBPACK_IMPORTED_MODULE_2__["default"])(element, offsetParentElement);
    var top = ref.top;
    var left = ref.left;
    var height = ref.height;
    var width = ref.width;
    var ref$1 = Object(_offset_parent_scroll_position__WEBPACK_IMPORTED_MODULE_0__["default"])(offsetParentElement, element);
    var x = ref$1.x;
    var y = ref$1.y;
    var ownerDocument = element.ownerDocument;
    var positionScale = offsetParentElement === ownerDocument.body || offsetParentElement === ownerDocument.documentElement ? 1 : scale;

    return {
        top: top + y * positionScale,
        left: left + x * positionScale,
        height: height,
        width: width
    };
});;


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/position.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/position.js ***!
  \***********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _offset_parent__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./offset-parent */ "./node_modules/@progress/kendo-popup-common/dist/es/offset-parent.js");
/* harmony import */ var _offset__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./offset */ "./node_modules/@progress/kendo-popup-common/dist/es/offset.js");
/* harmony import */ var _window__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./window */ "./node_modules/@progress/kendo-popup-common/dist/es/window.js");




var position = function (element, parent) {
    var win = Object(_window__WEBPACK_IMPORTED_MODULE_2__["default"])(element);
    var elementStyles = win.getComputedStyle(element);
    var offset = Object(_offset__WEBPACK_IMPORTED_MODULE_1__["default"])(element);
    var parentElement = parent || Object(_offset_parent__WEBPACK_IMPORTED_MODULE_0__["default"])(element);

    var ownerDocument = element.ownerDocument;
    var useRelative = parentElement !== ownerDocument.body && parentElement !== ownerDocument.documentElement;

    var parentOffset = { top: 0, left: 0 };

    if (elementStyles.position !== "fixed" && useRelative) {
        var parentStyles = win.getComputedStyle(parentElement);

        parentOffset = Object(_offset__WEBPACK_IMPORTED_MODULE_1__["default"])(parentElement);
        parentOffset.top += parseInt(parentStyles.borderTopWidth, 10);
        parentOffset.left += parseInt(parentStyles.borderLeftWidth, 10);
    }

    return {
        top: offset.top - parentOffset.top,
        left: offset.left - parentOffset.left,
        height: offset.height,
        width: offset.width
    };
};

/* harmony default export */ __webpack_exports__["default"] = (position);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/remove-scroll.js":
/*!****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/remove-scroll.js ***!
  \****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return removeScroll; });
function removeScroll(rect, scroll) {
    return {
        top: rect.top - scroll.y,
        left: rect.left - scroll.x,
        height: rect.height,
        width: rect.width
    };
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/restrict-to-view.js":
/*!*******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/restrict-to-view.js ***!
  \*******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _align_point__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./align-point */ "./node_modules/@progress/kendo-popup-common/dist/es/align-point.js");
/* harmony import */ var _collision__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./collision */ "./node_modules/@progress/kendo-popup-common/dist/es/collision.js");



var fit = function(position, size, viewPortSize) {
    var output = 0;

    if (position + size > viewPortSize) {
        output = viewPortSize - (position + size);
    }

    if (position < 0) {
        output = -position;
    }

    return output;
};

var flip = function(ref) {
    var offset = ref.offset;
    var size = ref.size;
    var anchorSize = ref.anchorSize;
    var viewPortSize = ref.viewPortSize;
    var anchorAlignPoint = ref.anchorAlignPoint;
    var elementAlignPoint = ref.elementAlignPoint;
    var margin = ref.margin;

    var output = 0;

    var isPositionCentered = elementAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || elementAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle;
    var isOriginCentered = anchorAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].center || anchorAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].middle;
    var marginToAdd = 2 * margin; //2x to keep margin after flip

    if (elementAlignPoint !== anchorAlignPoint && !isPositionCentered && !isOriginCentered) {
        var isBeforeAnchor = anchorAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].top || anchorAlignPoint === _align_point__WEBPACK_IMPORTED_MODULE_0__["default"].left;
        if (offset < 0 && isBeforeAnchor) {
            output = size + anchorSize + marginToAdd;
            if (offset + output + size > viewPortSize) {
                output = 0; //skip flip
            }
        } else if (offset >= 0 && !isBeforeAnchor) {
            if (offset + size > viewPortSize) {
                output += -(anchorSize + size + marginToAdd);
            }

            if (offset + output < 0) {
                output = 0; //skip flip
            }
        }
    }

    return output;
};

var restrictToView = function (options) {
    var anchorRect = options.anchorRect;
    var anchorAlign = options.anchorAlign;
    var elementRect = options.elementRect;
    var elementAlign = options.elementAlign;
    var collisions = options.collisions;
    var viewPort = options.viewPort;
    var margin = options.margin; if ( margin === void 0 ) margin = {};
    var elementTop = elementRect.top;
    var elementLeft = elementRect.left;
    var elementHeight = elementRect.height;
    var elementWidth = elementRect.width;
    var viewPortHeight = viewPort.height;
    var viewPortWidth = viewPort.width;
    var horizontalMargin = margin.horizontal || 0;
    var verticalMargin = margin.vertical || 0;

    var left = 0;
    var top = 0;

    var isHorizontalFlip = collisions.horizontal === _collision__WEBPACK_IMPORTED_MODULE_1__["default"].flip;
    var isVerticalFlip = collisions.vertical === _collision__WEBPACK_IMPORTED_MODULE_1__["default"].flip;

    if (collisions.vertical === _collision__WEBPACK_IMPORTED_MODULE_1__["default"].fit) {
        top += fit(elementTop, elementHeight, viewPortHeight);
    }

    if (collisions.horizontal === _collision__WEBPACK_IMPORTED_MODULE_1__["default"].fit) {
        left += fit(elementLeft, elementWidth, viewPortWidth);
    }

    if (isVerticalFlip) {
        top += flip({
            margin: verticalMargin,
            offset: elementTop,
            size: elementHeight,
            anchorSize: anchorRect.height,
            viewPortSize: viewPortHeight,
            anchorAlignPoint: anchorAlign.vertical,
            elementAlignPoint: elementAlign.vertical
        });
    }

    if (isHorizontalFlip) {
        left += flip({
            margin: horizontalMargin,
            offset: elementLeft,
            size: elementWidth,
            anchorSize: anchorRect.width,
            viewPortSize: viewPortWidth,
            anchorAlignPoint: anchorAlign.horizontal,
            elementAlignPoint: elementAlign.horizontal
        });
    }
    var flippedHorizontal = isHorizontalFlip && left !== 0;
    var flippedVertical = isVerticalFlip && top !== 0;

    return {
        flipped: flippedHorizontal || flippedVertical,
        flip: {
            horizontal: flippedHorizontal,
            vertical: flippedVertical
        },
        offset: {
            left: left,
            top: top
        }
    };
};

/* harmony default export */ __webpack_exports__["default"] = (restrictToView);


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/scroll-position.js":
/*!******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/scroll-position.js ***!
  \******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return scrollPosition; });
/* harmony import */ var _document__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./document */ "./node_modules/@progress/kendo-popup-common/dist/es/document.js");
/* harmony import */ var _window__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./window */ "./node_modules/@progress/kendo-popup-common/dist/es/window.js");



function scrollPosition(element) {
    var documentElement = Object(_document__WEBPACK_IMPORTED_MODULE_0__["default"])(element);
    var win = Object(_window__WEBPACK_IMPORTED_MODULE_1__["default"])(element);

    return {
        x: win.pageXOffset || documentElement.scrollLeft || 0,
        y: win.pageYOffset || documentElement.scrollTop || 0
    };
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/scrollbar-width.js":
/*!******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/scrollbar-width.js ***!
  \******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return scrollbarWidth; });
var cachedWidth = 0;

function scrollbarWidth() {
    if (!cachedWidth && typeof document !== 'undefined') {
        var div = document.createElement("div");

        div.style.cssText = "overflow:scroll;overflow-x:hidden;zoom:1;clear:both;display:block";
        div.innerHTML = "&nbsp;";
        document.body.appendChild(div);

        cachedWidth = div.offsetWidth - div.scrollWidth;

        document.body.removeChild(div);
    }

    return cachedWidth;
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/sibling-container.js":
/*!********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/sibling-container.js ***!
  \********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _parents__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./parents */ "./node_modules/@progress/kendo-popup-common/dist/es/parents.js");
/* harmony import */ var _siblings__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./siblings */ "./node_modules/@progress/kendo-popup-common/dist/es/siblings.js");
/* eslint-disable no-loop-func */




/* harmony default export */ __webpack_exports__["default"] = (function (anchor, container) {
    var parentElements = Object(_parents__WEBPACK_IMPORTED_MODULE_0__["default"])(anchor);
    var containerElement = container;
    var siblingElements;
    var result;

    while (containerElement) {
        siblingElements = Object(_siblings__WEBPACK_IMPORTED_MODULE_1__["default"])(containerElement);

        result = parentElements.reduce(
            function (list, p) { return list.concat(siblingElements.filter(function (s) { return s === p; })); },
            []
        )[0];

        if (result) { break; }

        containerElement = containerElement.parentElement;
    }

    return result;
});;



/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/siblings.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/siblings.js ***!
  \***********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = (function (element) {
    var result = [];

    var sibling = element.parentNode.firstElementChild;

    while (sibling) {
        if (sibling !== element) {
            result.push(sibling);
        }

        sibling = sibling.nextElementSibling;
    }
    return result;
});;


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/window-viewport.js":
/*!******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/window-viewport.js ***!
  \******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return windowViewport; });
/* harmony import */ var _window__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./window */ "./node_modules/@progress/kendo-popup-common/dist/es/window.js");
/* harmony import */ var _document__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./document */ "./node_modules/@progress/kendo-popup-common/dist/es/document.js");
/* harmony import */ var _scrollbar_width__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./scrollbar-width */ "./node_modules/@progress/kendo-popup-common/dist/es/scrollbar-width.js");




function windowViewport(element) {
    var win = Object(_window__WEBPACK_IMPORTED_MODULE_0__["default"])(element);
    var document = Object(_document__WEBPACK_IMPORTED_MODULE_1__["default"])(element);
    var result = {
        height: win.innerHeight,
        width: win.innerWidth
    };

    if (document.scrollHeight - document.clientHeight > 0) {
        result.width -= Object(_scrollbar_width__WEBPACK_IMPORTED_MODULE_2__["default"])();
    }

    return result;
}


/***/ }),

/***/ "./node_modules/@progress/kendo-popup-common/dist/es/window.js":
/*!*********************************************************************!*\
  !*** ./node_modules/@progress/kendo-popup-common/dist/es/window.js ***!
  \*********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _owner_document__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./owner-document */ "./node_modules/@progress/kendo-popup-common/dist/es/owner-document.js");


var getWindow = function (element) { return Object(_owner_document__WEBPACK_IMPORTED_MODULE_0__["default"])(element).defaultView; };

/* harmony default export */ __webpack_exports__["default"] = (getWindow);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js":
/*!***************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Animation.js ***!
  \***************************************************************************/
/*! exports provided: Animation */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Animation", function() { return Animation; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _AnimationChild__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./AnimationChild */ "./node_modules/@progress/kendo-react-animation/dist/es/AnimationChild.js");
/* harmony import */ var react_transition_group__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! react-transition-group */ "./node_modules/react-transition-group/index.js");
/* harmony import */ var react_transition_group__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(react_transition_group__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-animation/dist/es/util.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};






var styles = _util__WEBPACK_IMPORTED_MODULE_5__["default"].styles;
// tslint:enable:max-line-length
var Animation = /** @class */ (function (_super) {
    __extends(Animation, _super);
    function Animation() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Animation.prototype.render = function () {
        var _a = this.props, id = _a.id, style = _a.style, children = _a.children, component = _a.component, className = _a.className, childFactory = _a.childFactory, stackChildren = _a.stackChildren, componentChildStyle = _a.componentChildStyle, componentChildClassName = _a.componentChildClassName, other = __rest(_a, ["id", "style", "children", "component", "className", "childFactory", "stackChildren", "componentChildStyle", "componentChildClassName"]);
        var transitionProps = {
            id: id,
            style: style,
            component: component,
            childFactory: childFactory,
            className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])(styles['animation-container'], styles['animation-container-relative'], className)
        };
        var content = react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(children || null, function (child) { return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_AnimationChild__WEBPACK_IMPORTED_MODULE_3__["AnimationChild"], __assign({}, other, { style: componentChildStyle, className: componentChildClassName }), child)); });
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](react_transition_group__WEBPACK_IMPORTED_MODULE_4__["TransitionGroup"], __assign({}, transitionProps), content));
    };
    /**
     * @hidden
     */
    Animation.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        transitionName: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"].isRequired,
        appear: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"].isRequired,
        enter: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"].isRequired,
        exit: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"].isRequired,
        transitionEnterDuration: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"].isRequired,
        transitionExitDuration: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"].isRequired
    };
    /**
     * @hidden
     */
    Animation.defaultProps = {
        component: 'div'
    };
    return Animation;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/AnimationChild.js":
/*!********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/AnimationChild.js ***!
  \********************************************************************************/
/*! exports provided: AnimationChild */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AnimationChild", function() { return AnimationChild; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var react_transition_group__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-transition-group */ "./node_modules/react-transition-group/index.js");
/* harmony import */ var react_transition_group__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(react_transition_group__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-animation/dist/es/util.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};





var styles = _util__WEBPACK_IMPORTED_MODULE_4__["default"].styles;
var AnimationChild = /** @class */ (function (_super) {
    __extends(AnimationChild, _super);
    function AnimationChild() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(AnimationChild.prototype, "element", {
        /**
         * The element that is being animated.
         */
        get: function () {
            return this._element;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @hidden
     */
    AnimationChild.prototype.render = function () {
        var _this = this;
        var _a = this.props, children = _a.children, style = _a.style, appear = _a.appear, enter = _a.enter, exit = _a.exit, transitionName = _a.transitionName, transitionEnterDuration = _a.transitionEnterDuration, transitionExitDuration = _a.transitionExitDuration, className = _a.className, onEnter = _a.onEnter, onEntering = _a.onEntering, onEntered = _a.onEntered, onExit = _a.onExit, onExiting = _a.onExiting, onExited = _a.onExited, mountOnEnter = _a.mountOnEnter, unmountOnExit = _a.unmountOnExit, animationEnteringStyle = _a.animationEnteringStyle, animationEnteredStyle = _a.animationEnteredStyle, animationExitingStyle = _a.animationExitingStyle, animationExitedStyle = _a.animationExitedStyle, other = __rest(_a, ["children", "style", "appear", "enter", "exit", "transitionName", "transitionEnterDuration", "transitionExitDuration", "className", "onEnter", "onEntering", "onEntered", "onExit", "onExiting", "onExited", "mountOnEnter", "unmountOnExit", "animationEnteringStyle", "animationEnteredStyle", "animationExitingStyle", "animationExitedStyle"]);
        var childAnimationContainerClassNames = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])(className, styles['child-animation-container']);
        var defaultStyle = __assign({ transitionDelay: '0ms' }, style);
        var animationStyle = {
            entering: __assign({ transitionDuration: transitionEnterDuration + "ms" }, animationEnteringStyle),
            entered: __assign({}, animationEnteredStyle),
            exiting: __assign({ transitionDuration: transitionExitDuration + "ms" }, animationExitingStyle),
            exited: __assign({}, animationExitedStyle)
        };
        var transitionProps = {
            in: this.props.in,
            appear: appear,
            enter: enter,
            exit: exit,
            mountOnEnter: mountOnEnter,
            unmountOnExit: unmountOnExit,
            timeout: {
                enter: transitionEnterDuration,
                exit: transitionExitDuration
            },
            onEnter: (function (e) {
                if (onEnter) {
                    onEnter.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            onEntering: (function (e) {
                if (onEntering) {
                    onEntering.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            onEntered: (function (e) {
                if (onEntered) {
                    onEntered.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            onExit: (function (e) {
                if (onExit) {
                    onExit.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            onExiting: (function (e) {
                if (onExiting) {
                    onExiting.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            onExited: (function (e) {
                if (onExited) {
                    onExited.call(undefined, { animatedElement: e, target: _this });
                }
            }),
            classNames: {
                appear: styles[transitionName + "-appear"] || transitionName + "-appear",
                appearActive: styles[transitionName + "-appear-active"] || transitionName + "-appear-active",
                enter: styles[transitionName + "-enter"] || transitionName + "-enter",
                enterActive: styles[transitionName + "-enter-active"] || transitionName + "-enter-active",
                exit: styles[transitionName + "-exit"] || transitionName + "-exit",
                exitActive: styles[transitionName + "-exit-active"] || transitionName + "-exit-active"
            }
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](react_transition_group__WEBPACK_IMPORTED_MODULE_3__["CSSTransition"], __assign({}, transitionProps, other), function (status) {
            return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", { style: __assign({}, defaultStyle, animationStyle[status]), className: childAnimationContainerClassNames, ref: function (div) { _this._element = div; } }, children));
        }));
    };
    /**
     * @hidden
     */
    AnimationChild.propTypes = {
        in: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        transitionName: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"].isRequired,
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        appear: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        enter: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        exit: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        transitionEnterDuration: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"].isRequired,
        transitionExitDuration: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"].isRequired,
        mountOnEnter: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        unmountOnExit: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        animationEnteringStyle: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        animationEnteredStyle: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        animationExitingStyle: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        animationExitedStyle: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"]
    };
    /**
     * @hidden
     */
    AnimationChild.defaultProps = {
        mountOnEnter: true,
        unmountOnExit: false,
        onEnter: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        onEntering: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        onEntered: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        onExit: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        onExiting: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        onExited: _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["noop"],
        animationEnteringStyle: {},
        animationEnteredStyle: {},
        animationExitingStyle: {},
        animationExitedStyle: {}
    };
    return AnimationChild;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Expand.js":
/*!************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Expand.js ***!
  \************************************************************************/
/*! exports provided: Expand */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Expand", function() { return Expand; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};



// tslint:enable:max-line-length
var Expand = /** @class */ (function (_super) {
    __extends(Expand, _super);
    function Expand() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Expand.prototype.render = function () {
        var _a = this.props, direction = _a.direction, children = _a.children, other = __rest(_a, ["direction", "children"]);
        var animationProps = {
            transitionName: "expand-" + direction
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, animationProps, other), children));
    };
    /**
     * @hidden
     */
    Expand.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        direction: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"]([
            'horizontal',
            'vertical'
        ]),
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    /**
     * @hidden
     */
    Expand.defaultProps = {
        appear: false,
        enter: true,
        exit: true,
        transitionEnterDuration: 300,
        transitionExitDuration: 300,
        direction: 'vertical'
    };
    return Expand;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Fade.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Fade.js ***!
  \**********************************************************************/
/*! exports provided: Fade */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Fade", function() { return Fade; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};



// tslint:enable:max-line-length
var Fade = /** @class */ (function (_super) {
    __extends(Fade, _super);
    function Fade() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Fade.prototype.render = function () {
        var _a = this.props, children = _a.children, other = __rest(_a, ["children"]);
        var animationProps = {
            transitionName: "fade"
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, animationProps, other), children));
    };
    /**
     * @hidden
     */
    Fade.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    /**
     * @hidden
     */
    Fade.defaultProps = {
        appear: false,
        enter: true,
        exit: false,
        transitionEnterDuration: 500,
        transitionExitDuration: 500
    };
    return Fade;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Push.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Push.js ***!
  \**********************************************************************/
/*! exports provided: Push */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Push", function() { return Push; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};



var EXITING_ANIMATION_STYLE = { position: 'absolute', top: '0', left: '0' };
// tslint:enable:max-line-length
var Push = /** @class */ (function (_super) {
    __extends(Push, _super);
    function Push() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Push.prototype.render = function () {
        var _a = this.props, children = _a.children, direction = _a.direction, other = __rest(_a, ["children", "direction"]);
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, other, { transitionName: "push-" + direction, animationExitingStyle: this.props.stackChildren
                ? EXITING_ANIMATION_STYLE
                : undefined }), children));
    };
    /**
     * @hidden
     */
    Push.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        direction: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"]([
            'up',
            'down',
            'left',
            'right'
        ]),
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        stackChildren: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"]
    };
    /**
     * @hidden
     */
    Push.defaultProps = {
        appear: false,
        enter: true,
        exit: true,
        transitionEnterDuration: 300,
        transitionExitDuration: 300,
        direction: 'right',
        stackChildren: false
    };
    return Push;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Reveal.js":
/*!************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Reveal.js ***!
  \************************************************************************/
/*! exports provided: Reveal */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Reveal", function() { return Reveal; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-animation/dist/es/util.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};





var Reveal = /** @class */ (function (_super) {
    __extends(Reveal, _super);
    function Reveal() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /**
         * @hidden
         */
        _this.state = {
            maxHeight: undefined,
            maxWidth: undefined
        };
        _this.componentWillEnter = function (event) {
            var onEnter = _this.props.onEnter;
            _this.updateContainerDimensions(event.animatedElement, function () {
                if (onEnter) {
                    onEnter.call(undefined, event);
                }
            });
        };
        _this.componentIsEntering = function (event) {
            var onEntering = _this.props.onEntering;
            _this.updateContainerDimensions(event.animatedElement, function () {
                if (onEntering) {
                    onEntering.call(undefined, event);
                }
            });
        };
        _this.componentWillExit = function (event) {
            var onExit = _this.props.onExit;
            _this.updateContainerDimensions(event.animatedElement, function () {
                if (onExit) {
                    onExit.call(undefined, event);
                }
            });
        };
        _this.updateContainerDimensions = function (node, done) {
            if (done === void 0) { done = _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["noop"]; }
            var content = node.firstChild;
            if (content) {
                var newHeight = _util__WEBPACK_IMPORTED_MODULE_4__["default"].outerHeight(content);
                var newWidth = _util__WEBPACK_IMPORTED_MODULE_4__["default"].outerWidth(content);
                _this.setState({
                    maxHeight: newHeight,
                    maxWidth: newWidth
                }, done);
            }
        };
        return _this;
    }
    /**
     * @hidden
     */
    Reveal.prototype.render = function () {
        var _a = this.props, direction = _a.direction, children = _a.children, childFactory = _a.childFactory, other = __rest(_a, ["direction", "children", "childFactory"]);
        var _b = this.state, maxHeight = _b.maxHeight, maxWidth = _b.maxWidth;
        var maxOffset;
        if (direction === 'vertical') {
            maxOffset = { maxHeight: maxHeight ? maxHeight + "px" : null };
        }
        else {
            maxOffset = { maxWidth: maxWidth ? maxWidth + "px" : null };
        }
        var animationEnteringStyle = {
            maxHeight: maxOffset.maxHeight,
            maxWidth: maxOffset.maxWidth
        };
        var factory = function (child) {
            if (!child.props.in) {
                return react__WEBPACK_IMPORTED_MODULE_0__["cloneElement"](child, __assign({}, child.props, { style: __assign({}, child.props.style, { maxHeight: maxOffset.maxHeight, maxWidth: maxOffset.maxWidth }) }));
            }
            return child;
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, other, { childFactory: childFactory ? childFactory : factory, onEnter: this.componentWillEnter, onEntering: this.componentIsEntering, onExit: this.componentWillExit, animationEnteringStyle: animationEnteringStyle, transitionName: "reveal-" + direction }), children));
    };
    /**
     * @hidden
     */
    Reveal.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        direction: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"]([
            'horizontal',
            'vertical'
        ]),
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    /**
     * @hidden
     */
    Reveal.defaultProps = {
        appear: false,
        enter: true,
        exit: true,
        transitionEnterDuration: 300,
        transitionExitDuration: 300,
        direction: 'vertical'
    };
    return Reveal;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Slide.js":
/*!***********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Slide.js ***!
  \***********************************************************************/
/*! exports provided: Slide */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Slide", function() { return Slide; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};



// tslint:enable:max-line-length
var Slide = /** @class */ (function (_super) {
    __extends(Slide, _super);
    function Slide() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Slide.prototype.render = function () {
        var _a = this.props, direction = _a.direction, children = _a.children, other = __rest(_a, ["direction", "children"]);
        var animationProps = {
            transitionName: "slide-" + direction
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, animationProps, other), children));
    };
    /**
     * @hidden
     */
    Slide.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        direction: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"]([
            'up',
            'down',
            'left',
            'right'
        ]),
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    /**
     * @hidden
     */
    Slide.defaultProps = {
        appear: false,
        enter: true,
        exit: true,
        transitionEnterDuration: 300,
        transitionExitDuration: 300,
        direction: 'down'
    };
    return Slide;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/Zoom.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/Zoom.js ***!
  \**********************************************************************/
/*! exports provided: Zoom */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Zoom", function() { return Zoom; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};



var EXITING_ANIMATION_STYLE = { position: 'absolute', top: '0', left: '0' };
// tslint:enable:max-line-length
var Zoom = /** @class */ (function (_super) {
    __extends(Zoom, _super);
    function Zoom() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    Zoom.prototype.render = function () {
        var _a = this.props, children = _a.children, direction = _a.direction, other = __rest(_a, ["children", "direction"]);
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_Animation__WEBPACK_IMPORTED_MODULE_2__["Animation"], __assign({}, other, { transitionName: "zoom-" + direction, animationExitingStyle: this.props.stackChildren
                ? EXITING_ANIMATION_STYLE
                : undefined }), children));
    };
    /**
     * @hidden
     */
    Zoom.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]),
            prop_types__WEBPACK_IMPORTED_MODULE_1__["node"]
        ]),
        childFactory: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        direction: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"]([
            'in',
            'out'
        ]),
        component: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        stackChildren: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"]
    };
    /**
     * @hidden
     */
    Zoom.defaultProps = {
        appear: false,
        enter: true,
        exit: true,
        transitionEnterDuration: 300,
        transitionExitDuration: 300,
        direction: 'out',
        stackChildren: false
    };
    return Zoom;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/main.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/main.js ***!
  \**********************************************************************/
/*! exports provided: Animation, AnimationChild, Fade, Expand, Push, Slide, Zoom, Reveal */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _Animation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Animation */ "./node_modules/@progress/kendo-react-animation/dist/es/Animation.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Animation", function() { return _Animation__WEBPACK_IMPORTED_MODULE_0__["Animation"]; });

/* harmony import */ var _AnimationChild__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./AnimationChild */ "./node_modules/@progress/kendo-react-animation/dist/es/AnimationChild.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "AnimationChild", function() { return _AnimationChild__WEBPACK_IMPORTED_MODULE_1__["AnimationChild"]; });

/* harmony import */ var _Fade__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Fade */ "./node_modules/@progress/kendo-react-animation/dist/es/Fade.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Fade", function() { return _Fade__WEBPACK_IMPORTED_MODULE_2__["Fade"]; });

/* harmony import */ var _Expand__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./Expand */ "./node_modules/@progress/kendo-react-animation/dist/es/Expand.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Expand", function() { return _Expand__WEBPACK_IMPORTED_MODULE_3__["Expand"]; });

/* harmony import */ var _Push__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./Push */ "./node_modules/@progress/kendo-react-animation/dist/es/Push.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Push", function() { return _Push__WEBPACK_IMPORTED_MODULE_4__["Push"]; });

/* harmony import */ var _Slide__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./Slide */ "./node_modules/@progress/kendo-react-animation/dist/es/Slide.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Slide", function() { return _Slide__WEBPACK_IMPORTED_MODULE_5__["Slide"]; });

/* harmony import */ var _Zoom__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./Zoom */ "./node_modules/@progress/kendo-react-animation/dist/es/Zoom.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Zoom", function() { return _Zoom__WEBPACK_IMPORTED_MODULE_6__["Zoom"]; });

/* harmony import */ var _Reveal__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./Reveal */ "./node_modules/@progress/kendo-react-animation/dist/es/Reveal.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Reveal", function() { return _Reveal__WEBPACK_IMPORTED_MODULE_7__["Reveal"]; });












/***/ }),

/***/ "./node_modules/@progress/kendo-react-animation/dist/es/util.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-animation/dist/es/util.js ***!
  \**********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/**
 * @hidden
 */
var outerHeight = function (element) {
    if (!element) {
        return 0;
    }
    var wnd = element.ownerDocument.defaultView;
    var computedStyles = wnd.getComputedStyle(element);
    var marginTop = parseFloat(computedStyles.marginTop);
    var marginBottom = parseFloat(computedStyles.marginBottom);
    return element.offsetHeight + marginTop + marginBottom;
};
/**
 * @hidden
 */
var outerWidth = function (element) {
    if (!element) {
        return 0;
    }
    var wnd = element.ownerDocument.defaultView;
    var computedStyles = wnd.getComputedStyle(element);
    var marginLeft = parseFloat(computedStyles.marginLeft);
    var marginRight = parseFloat(computedStyles.marginRight);
    return element.offsetWidth + marginLeft + marginRight;
};
/**
 * @hidden
 */
var styles = {
    'animation-container': 'k-animation-container',
    'animation-container-relative': 'k-animation-container-relative',
    'animation-container-fixed': 'k-animation-container-fixed',
    'push-right-enter': 'k-push-right-enter',
    'push-right-appear': 'k-push-right-appear',
    'push-right-enter-active': 'k-push-right-enter-active',
    'push-right-appear-active': 'k-push-right-appear-active',
    'push-right-exit': 'k-push-right-exit',
    'push-right-exit-active': 'k-push-right-exit-active',
    'push-left-enter': 'k-push-left-enter',
    'push-left-appear': 'k-push-left-appear',
    'push-left-enter-active': 'k-push-left-enter-active',
    'push-left-appear-active': 'k-push-left-appear-active',
    'push-left-exit': 'k-push-left-exit',
    'push-left-exit-active': 'k-push-left-exit-active',
    'push-down-enter': 'k-push-down-enter',
    'push-down-appear': 'k-push-down-appear',
    'push-down-enter-active': 'k-push-down-enter-active',
    'push-down-appear-active': 'k-push-down-appear-active',
    'push-down-exit': 'k-push-down-exit',
    'push-down-exit-active': 'k-push-down-exit-active',
    'push-up-enter': 'k-push-up-enter',
    'push-up-appear': 'k-push-up-appear',
    'push-up-enter-active': 'k-push-up-enter-active',
    'push-up-appear-active': 'k-push-up-appear-active',
    'push-up-exit': 'k-push-up-exit',
    'push-up-exit-active': 'k-push-up-exit-active',
    'expand': 'k-expand',
    'expand-vertical-enter': 'k-expand-vertical-enter',
    'expand-vertical-appear': 'k-expand-vertical-appear',
    'expand-vertical-enter-active': 'k-expand-vertical-enter-active',
    'expand-vertical-appear-active': 'k-expand-vertical-appear-active',
    'expand-vertical-exit': 'k-expand-vertical-exit',
    'expand-vertical-exit-active': 'k-expand-vertical-exit-active',
    'expand-horizontal-enter': 'k-expand-horizontal-enter',
    'expand-horizontal-appear': 'k-expand-horizontal-appear',
    'expand-horizontal-enter-active': 'k-expand-horizontal-enter-active',
    'expand-horizontal-appear-active': 'k-expand-horizontal-appear-active',
    'expand-horizontal-exit': 'k-expand-horizontal-exit',
    'expand-horizontal-exit-active': 'k-expand-horizontal-exit-active',
    'child-animation-container': 'k-child-animation-container',
    'fade-enter': 'k-fade-enter',
    'fade-appear': 'k-fade-appear',
    'fade-enter-active': 'k-fade-enter-active',
    'fade-appear-active': 'k-fade-appear-active',
    'fade-exit': 'k-fade-exit',
    'fade-exit-active': 'k-fade-exit-active',
    'zoom-in-enter': 'k-zoom-in-enter',
    'zoom-in-appear': 'k-zoom-in-appear',
    'zoom-in-enter-active': 'k-zoom-in-enter-active',
    'zoom-in-appear-active': 'k-zoom-in-appear-active',
    'zoom-in-exit': 'k-zoom-in-exit',
    'zoom-in-exit-active': 'k-zoom-in-exit-active',
    'zoom-out-enter': 'k-zoom-out-enter',
    'zoom-out-appear': 'k-zoom-out-appear',
    'zoom-out-enter-active': 'k-zoom-out-enter-active',
    'zoom-out-appear-active': 'k-zoom-out-appear-active',
    'zoom-out-exit': 'k-zoom-out-exit',
    'zoom-out-exit-active': 'k-zoom-out-exit-active',
    'slide-in-appear': 'k-slide-in-appear',
    'centered': 'k-centered',
    'slide-in-appear-active': 'k-slide-in-appear-active',
    'slide-down-enter': 'k-slide-down-enter',
    'slide-down-appear': 'k-slide-down-appear',
    'slide-down-enter-active': 'k-slide-down-enter-active',
    'slide-down-appear-active': 'k-slide-down-appear-active',
    'slide-down-exit': 'k-slide-down-exit',
    'slide-down-exit-active': 'k-slide-down-exit-active',
    'slide-up-enter': 'k-slide-up-enter',
    'slide-up-appear': 'k-slide-up-appear',
    'slide-up-enter-active': 'k-slide-up-enter-active',
    'slide-up-appear-active': 'k-slide-up-appear-active',
    'slide-up-exit': 'k-slide-up-exit',
    'slide-up-exit-active': 'k-slide-up-exit-active',
    'slide-right-enter': 'k-slide-right-enter',
    'slide-right-appear': 'k-slide-right-appear',
    'slide-right-enter-active': 'k-slide-right-enter-active',
    'slide-right-appear-active': 'k-slide-right-appear-active',
    'slide-right-exit': 'k-slide-right-exit',
    'slide-right-exit-active': 'k-slide-right-exit-active',
    'slide-left-enter': 'k-slide-left-enter',
    'slide-left-appear': 'k-slide-left-appear',
    'slide-left-enter-active': 'k-slide-left-enter-active',
    'slide-left-appear-active': 'k-slide-left-appear-active',
    'slide-left-exit': 'k-slide-left-exit',
    'slide-left-exit-active': 'k-slide-left-exit-active',
    'reveal-vertical-enter': 'k-reveal-vertical-enter',
    'reveal-vertical-appear': 'k-reveal-vertical-appear',
    'reveal-vertical-enter-active': 'k-reveal-vertical-enter-active',
    'reveal-vertical-appear-active': 'k-reveal-vertical-appear-active',
    'reveal-vertical-exit': 'k-reveal-vertical-exit',
    'reveal-vertical-exit-active': 'k-reveal-vertical-exit-active',
    'reveal-horizontal-enter': 'k-reveal-horizontal-enter',
    'reveal-horizontal-appear': 'k-reveal-horizontal-appear',
    'reveal-horizontal-enter-active': 'k-reveal-horizontal-enter-active',
    'reveal-horizontal-appear-active': 'k-reveal-horizontal-appear-active',
    'reveal-horizontal-exit': 'k-reveal-horizontal-exit',
    'reveal-horizontal-exit-active': 'k-reveal-horizontal-exit-active'
};
/**
 * @hidden
 */
/* harmony default export */ __webpack_exports__["default"] = ({
    outerHeight: outerHeight,
    outerWidth: outerWidth,
    styles: styles
});


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/Button.js":
/*!**********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/Button.js ***!
  \**********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-buttons/dist/es/util.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var __rest = (undefined && undefined.__rest) || function (s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) if (e.indexOf(p[i]) < 0)
            t[p[i]] = s[p[i]];
    return t;
};




var styles = _util__WEBPACK_IMPORTED_MODULE_3__["default"].styles;
/**
 * @hidden
 */
function iconElement(_a) {
    var imageUrl = _a.imageUrl, icon = _a.icon, iconClass = _a.iconClass, imageAlt = _a.imageAlt;
    if (imageUrl) {
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("img", { role: "presentation", className: 'k-image', alt: imageAlt, src: imageUrl }));
    }
    else if (icon) {
        var iconClasses = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])('k-icon', 'k-i-' + icon);
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { role: "presentation", className: iconClasses }));
    }
    else if (iconClass) {
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { role: "presentation", className: iconClass }));
    }
    return null;
}
var Button = /** @class */ (function (_super) {
    __extends(Button, _super);
    function Button(props) {
        var _this = _super.call(this, props) || this;
        _this._element = null;
        _this.handleClick = function (event) {
            _this.toggleIfApplicable();
            if (_this.props.onClick) {
                _this.props.onClick.call(undefined, event);
            }
        };
        _this.state = { active: (_this.props.selected === true && _this.props.togglable === true) };
        return _this;
    }
    Object.defineProperty(Button.prototype, "element", {
        /**
         * Gets the DOM element of the Button component.
         */
        get: function () {
            return this._element;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @hidden
     */
    Button.prototype.componentWillReceiveProps = function (props) {
        this.setState({ active: props.selected === true && this.props.togglable === true });
    };
    /**
     * @hidden
     */
    Button.prototype.render = function () {
        var _this = this;
        var _a;
        var _b = this.props, children = _b.children, look = _b.look, primary = _b.primary, selected = _b.selected, togglable = _b.togglable, icon = _b.icon, iconClass = _b.iconClass, imageUrl = _b.imageUrl, imageAlt = _b.imageAlt, className = _b.className, onClick = _b.onClick, htmlAttributes = __rest(_b, ["children", "look", "primary", "selected", "togglable", "icon", "iconClass", "imageUrl", "imageAlt", "className", "onClick"]);
        var hasIcon = (icon !== undefined || iconClass !== undefined || imageUrl !== undefined);
        var hasChildren = children !== undefined;
        var buttonClasses = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])([styles.button], (_a = {},
            _a[styles["" + look]] = look !== 'default',
            _a[styles.primary] = primary,
            _a[styles['state-disabled']] = this.props.disabled,
            _a[styles['state-active']] = this.state.active,
            _a[styles['button-icon']] = !hasChildren && hasIcon,
            _a[styles['button-icontext']] = hasChildren && hasIcon,
            _a), [styles["" + this.props.dir]], className);
        var buttonProps = {
            className: buttonClasses,
            onClick: this.handleClick,
            // Accessibility properties
            role: togglable ? 'checkbox' : undefined,
            'aria-disabled': this.props.disabled || undefined,
            'aria-checked': togglable ? this.state.active : undefined
        };
        var iconEl = iconElement({
            icon: icon,
            iconClass: iconClass,
            imageUrl: imageUrl,
            imageAlt: imageAlt
        });
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("button", __assign({}, buttonProps, htmlAttributes, { ref: function (button) { return _this._element = button; } }),
            iconEl,
            children));
    };
    Button.prototype.toggleIfApplicable = function () {
        if (this.props.togglable) {
            this.setState({ active: !this.state.active });
        }
    };
    /**
     * @hidden
     */
    Button.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["node"],
        look: prop_types__WEBPACK_IMPORTED_MODULE_1__["oneOf"](['default', 'bare', 'flat', 'outline']),
        primary: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        selected: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        togglable: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        icon: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        iconClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageUrl: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageAlt: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"]
    };
    /**
     * @hidden
     */
    Button.defaultProps = {
        look: 'default',
        primary: false,
        selected: false,
        togglable: false
    };
    return Button;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (Button);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ButtonGroup.js":
/*!***************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ButtonGroup.js ***!
  \***************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-buttons/dist/es/util.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};




var styles = _util__WEBPACK_IMPORTED_MODULE_3__["default"].styles;
var ButtonGroup = /** @class */ (function (_super) {
    __extends(ButtonGroup, _super);
    function ButtonGroup() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this._element = null;
        return _this;
    }
    /**
     * @hidden
     */
    ButtonGroup.prototype.render = function () {
        var _this = this;
        var _a;
        var buttons = this.mapButtons(this.props.children);
        var groupClasses = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])([styles['button-group']], (_a = {},
            _a[styles['state-disabled']] = this.props.disabled,
            _a[styles['button-group-stretched']] = !!this.props.width,
            _a), this.props.className);
        var groupProps = {
            className: groupClasses,
            style: { 'width': "" + this.props.width },
            dir: this.props.dir,
            // Accessibility properties
            role: 'group',
            'aria-disabled': this.props.disabled,
            'aria-multiselectable': true
        };
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", __assign({ ref: function (div) { _this._element = div; } }, groupProps, { className: groupClasses }), buttons));
    };
    ButtonGroup.prototype.mapButtons = function (children) {
        var _this = this;
        var count = react__WEBPACK_IMPORTED_MODULE_0__["Children"].count(children);
        var rtl = this.props.dir !== undefined
            ? this.props.dir === 'rtl'
            : (this._element && (getComputedStyle(this._element).direction === 'rtl') || false);
        return react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(children, function (child, index) {
            if (react__WEBPACK_IMPORTED_MODULE_0__["isValidElement"](child)) {
                return _this.renderButton(child, index, (index === count - 1), rtl);
            }
            return child;
        });
    };
    ButtonGroup.prototype.renderButton = function (child, index, isLast, isRtl) {
        var _a;
        var buttonProps = __assign({}, child.props, { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_2__["classNames"])(child.props.className, (_a = {},
                _a[styles['group-start']] = isRtl ? isLast : index === 0,
                _a[styles['group-end']] = isRtl ? index === 0 : isLast,
                _a)), style: __assign({ width: this.props.width }, (child.props.style || {})), disabled: this.props.disabled || child.props.disabled });
        return react__WEBPACK_IMPORTED_MODULE_0__["cloneElement"](child, buttonProps, child.props.children);
    };
    /**
     * @hidden
     */
    ButtonGroup.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["element"]),
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        disabled: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        width: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        dir: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"]
    };
    return ButtonGroup;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (ButtonGroup);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/ButtonItem.js":
/*!*************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/ButtonItem.js ***!
  \*************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();


/**
 * @hidden
 */
var ButtonItem = /** @class */ (function (_super) {
    __extends(ButtonItem, _super);
    function ButtonItem() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.onClick = function (event) { return _this.props.onClick(event, _this.props.index); };
        return _this;
    }
    ButtonItem.prototype.render = function () {
        var _a = this.props, dataItem = _a.dataItem, focused = _a.focused, id = _a.id, onDown = _a.onDown;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("li", { id: id, className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_1__["classNames"])('k-item', {
                'k-state-focused': focused,
                'k-state-selected': dataItem.selected,
                'k-state-disabled': dataItem.disabled
            }), onClick: this.onClick, onMouseDown: onDown, onPointerDown: onDown, role: "menuItem", "aria-disabled": dataItem.disabled || undefined }, this.renderContent()));
    };
    ButtonItem.prototype.renderContent = function () {
        var _a = this.props, dataItem = _a.dataItem, textField = _a.textField, index = _a.index;
        var Render = this.props.dataItem.render || this.props.itemRender;
        var text = dataItem.text !== undefined ? dataItem.text :
            (textField ? dataItem[textField] : dataItem);
        var iconClass = dataItem.icon ? "k-icon k-i-" + dataItem.icon : dataItem.iconClass;
        return (Render && react__WEBPACK_IMPORTED_MODULE_0__["createElement"](Render, { item: dataItem, itemIndex: index })) || ([
            (iconClass && (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { className: iconClass, role: "presentation", key: "icon" }))),
            (dataItem.imageUrl && (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("img", { className: "k-image", alt: "", src: dataItem.imageUrl, role: "presentation", key: "image" }))),
            text
        ]);
    };
    return ButtonItem;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (ButtonItem);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButton.js":
/*!*****************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButton.js ***!
  \*****************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _main__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./../main */ "./node_modules/@progress/kendo-react-buttons/dist/es/main.js");
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _utils_navigation__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./utils/navigation */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/navigation.js");
/* harmony import */ var _DropDownButtonItem__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./DropDownButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButtonItem.js");
/* harmony import */ var _ButtonItem__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./ButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/ButtonItem.js");
/* harmony import */ var _progress_kendo_react_popup__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @progress/kendo-react-popup */ "./node_modules/@progress/kendo-react-popup/dist/es/main.js");
/* harmony import */ var _utils_popup__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./utils/popup */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/popup.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();










/**
 * Represents the [KendoReact DropDownButton component]({% slug overview_dropdownbutton %}).
 *
 * @example
 * ```jsx
 * class App extends React.Component {
 *    render() {
 *        return (
 *            <DropDownButton text="Act">
 *                <DropDownButtonItem text="Item1" />
 *                <DropDownButtonItem text="Item2" />
 *                <DropDownButtonItem text="Item3" />
 *            </DropDownButton>
 *        );
 *    }
 * }
 * ReactDOM.render(<App />, document.querySelector('my-app'));
 * ```
 */
var DropDownButton = /** @class */ (function (_super) {
    __extends(DropDownButton, _super);
    function DropDownButton() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /**
         * @hidden
         */
        _this.state = {
            opened: false,
            focused: false,
            focusedIndex: -1
        };
        _this.wrapper = null;
        _this.mainButton = null;
        _this.guid = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["guid"])();
        _this.buttonsData = [];
        _this.onKeyDown = function (event) {
            var _a = _this.state, opened = _a.opened, focusedIndex = _a.focusedIndex;
            if (event.altKey) {
                if (!opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].down) {
                    _this.dispatchPopupEvent(event, true);
                    _this.setState({ focusedIndex: 0, opened: true });
                }
                else if (opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].up) {
                    _this.dispatchPopupEvent(event, false);
                    _this.setState({ focusedIndex: -1, opened: false });
                }
                return;
            }
            var newState = undefined;
            if (event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].enter || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].space) {
                if (focusedIndex >= 0) {
                    _this.dispatchClickEvent(event, focusedIndex);
                }
                // Prevent default because otherwise when an item is selected
                // click on the default button gets emitted which opens the popup again.
                event.preventDefault();
                newState = {
                    focusedIndex: opened ? -1 : 0,
                    opened: !opened
                };
                _this.dispatchPopupEvent(event, newState.opened);
            }
            else if (opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].esc) {
                newState = {
                    focusedIndex: -1,
                    opened: false
                };
                _this.dispatchPopupEvent(event, newState.opened);
            }
            if (opened) {
                var newFocused = Object(_utils_navigation__WEBPACK_IMPORTED_MODULE_4__["default"])(focusedIndex, event.keyCode, event.altKey, _this.buttonsData.length);
                if (newFocused !== focusedIndex) {
                    newState = newState || {};
                    newState.focusedIndex = newFocused;
                }
                var arrowKey = event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].up || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].down ||
                    event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].left || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].right;
                if (!event.altKey && arrowKey) {
                    // Needed to notify the parent listeners that event is handled.
                    event.preventDefault();
                }
            }
            if (newState) {
                _this.setState(newState);
            }
        };
        _this.onFocus = function (event) {
            _this.setState({ focused: true, focusedIndex: _this.state.opened ? 0 : -1 });
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(_this.props.onFocus, event, _this, undefined);
        };
        _this.onBlur = function (event) {
            _this.setState({ focused: false, opened: false, focusedIndex: -1 });
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(_this.props.onBlur, event, _this, undefined);
            var fireCloseEvent = _this.state.opened;
            if (fireCloseEvent) {
                _this.dispatchPopupEvent(event, false);
            }
        };
        _this.onItemClick = function (event, clickedItemIndex) {
            _this.setState({ focusedIndex: -1, opened: false });
            _this.dispatchClickEvent(event, clickedItemIndex);
            _this.dispatchPopupEvent(event, false);
        };
        _this.onItemDown = function (event) {
            if (document.activeElement === _this.element) {
                event.preventDefault();
            }
        };
        _this.mouseDown = function (event) {
            event.preventDefault();
        };
        _this.onClickMainButton = function (event) {
            if (!_this.buttonsData.length) {
                return;
            }
            var opened = _this.state.opened;
            var toOpen = !opened;
            _this.setState({
                opened: toOpen,
                focused: true,
                focusedIndex: toOpen ? 0 : -1
            });
            _this.dispatchPopupEvent(event, toOpen);
        };
        _this.dispatchPopupEvent = function (dispatchedEvent, open) {
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(open ? _this.props.onOpen : _this.props.onClose, dispatchedEvent, _this, undefined);
        };
        return _this;
    }
    /**
     * @hidden
     */
    DropDownButton.prototype.render = function () {
        var _this = this;
        var rtl = this.isRtl();
        var dir = rtl ? 'rtl' : undefined;
        var _a = this.props, tabIndex = _a.tabIndex, disabled = _a.disabled;
        var focusedIndex = this.state.focusedIndex;
        this.buttonsData = this.props.items ||
            react__WEBPACK_IMPORTED_MODULE_0__["Children"].toArray(this.props.children)
                .filter(function (child) { return child && child.type === _DropDownButtonItem__WEBPACK_IMPORTED_MODULE_5__["default"]; })
                .map(function (child) { return child.props; });
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["classNames"])('k-widget', 'k-dropdown-button', this.props.className, {
                'k-state-focused': this.state.focused
            }), onKeyDown: this.onKeyDown, onFocus: this.onFocus, onBlur: this.onBlur, dir: dir, ref: function (el) { return _this.wrapper = el; } },
            react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_main__WEBPACK_IMPORTED_MODULE_2__["Button"], { onClick: this.onClickMainButton, onMouseDown: this.mouseDown, disabled: disabled || undefined, tabIndex: tabIndex, icon: this.props.icon, iconClass: this.props.iconClass, className: this.props.buttonClass, imageUrl: this.props.imageUrl, look: this.props.look, primary: this.props.primary, dir: dir, "aria-disabled": disabled, "aria-haspopup": true, "aria-expanded": this.state.opened, "aria-label": this.props.text + " dropdownbutton", ref: function (btn) { return _this.mainButton = btn && btn.element; }, "aria-owns": this.guid, "aria-activedescendant": focusedIndex >= 0 ? this.guid + "-" + focusedIndex : undefined }, this.props.text),
            this.renderPopup(rtl)));
    };
    /**
     * @hidden
     */
    DropDownButton.prototype.componentDidMount = function () {
        if (this.props.dir === undefined && this.isRtl()) {
            this.forceUpdate();
        }
    };
    /**
     * @hidden
     */
    DropDownButton.prototype.componentDidUpdate = function () {
        if (this.state.focused && this.element) {
            // firefox in mac does not focus on mouse-down, next line fixes this
            this.element.focus();
        }
    };
    Object.defineProperty(DropDownButton.prototype, "element", {
        /**
         * The DOM element of main button.
         */
        get: function () {
            return this.mainButton;
        },
        enumerable: true,
        configurable: true
    });
    DropDownButton.prototype.dispatchClickEvent = function (dispatchedEvent, index) {
        if (!this.isItemDisabled(index)) {
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(this.props.onItemClick, dispatchedEvent, this, {
                item: this.buttonsData[index],
                itemIndex: index
            });
        }
    };
    DropDownButton.prototype.renderPopup = function (rtl) {
        var _a = this.props.popupSettings, popupSettings = _a === void 0 ? {} : _a;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_progress_kendo_react_popup__WEBPACK_IMPORTED_MODULE_7__["Popup"], { anchor: this.wrapper || undefined, show: this.state.opened, animate: popupSettings.animate, popupClass: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["classNames"])('k-list-container k-reset k-group', popupSettings.popupClass), anchorAlign: popupSettings.anchorAlign || Object(_utils_popup__WEBPACK_IMPORTED_MODULE_8__["getAnchorAlign"])(rtl), popupAlign: popupSettings.popupAlign || Object(_utils_popup__WEBPACK_IMPORTED_MODULE_8__["getPopupAlign"])(rtl), style: rtl ? { direction: 'rtl' } : undefined },
            react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("ul", { className: "k-list k-reset", role: "menu", id: this.guid }, this.renderChildItems())));
    };
    DropDownButton.prototype.renderChildItems = function () {
        var _this = this;
        var _a = this.props, itemRender = _a.itemRender, textField = _a.textField;
        return this.buttonsData.length > 0 ? (this.buttonsData.map(function (item, index) {
            return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_ButtonItem__WEBPACK_IMPORTED_MODULE_6__["default"], { dataItem: item, textField: textField, focused: _this.state.focusedIndex === index, onClick: _this.onItemClick, onDown: _this.onItemDown, itemRender: itemRender, index: index, key: index, id: _this.guid + "-" + index }));
        })) : null;
    };
    DropDownButton.prototype.isItemDisabled = function (index) {
        return this.buttonsData[index] ? this.buttonsData[index].disabled : this.props.disabled;
    };
    DropDownButton.prototype.isRtl = function () {
        return this.props.dir !== undefined ? this.props.dir === 'rtl' :
            !!this.wrapper && getComputedStyle(this.wrapper).direction === 'rtl';
    };
    /**
     * @hidden
     */
    DropDownButton.propTypes = {
        primary: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        onFocus: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onBlur: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onItemClick: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onOpen: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onClose: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        text: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        items: prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]),
        textField: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"],
        disabled: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        icon: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        iconClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageUrl: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        popupSettings: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        itemRender: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        look: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        buttonClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        dir: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"]
    };
    return DropDownButton;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (DropDownButton);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButtonItem.js":
/*!*********************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButtonItem.js ***!
  \*********************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();


var DropDownButtonItem = /** @class */ (function (_super) {
    __extends(DropDownButtonItem, _super);
    function DropDownButtonItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    DropDownButtonItem.prototype.render = function () {
        return null;
    };
    /**
     * @hidden
     */
    DropDownButtonItem.propTypes = {
        text: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        icon: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        iconClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageUrl: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        selected: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        disabled: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        render: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    return DropDownButtonItem;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (DropDownButtonItem);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButton.js":
/*!**************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButton.js ***!
  \**************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _main__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./../main */ "./node_modules/@progress/kendo-react-buttons/dist/es/main.js");
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _ButtonItem__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./ButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/ButtonItem.js");
/* harmony import */ var _SplitButtonItem__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./SplitButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButtonItem.js");
/* harmony import */ var _utils_navigation__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./utils/navigation */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/navigation.js");
/* harmony import */ var _progress_kendo_react_popup__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @progress/kendo-react-popup */ "./node_modules/@progress/kendo-react-popup/dist/es/main.js");
/* harmony import */ var _utils_popup__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./utils/popup */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/popup.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();










/**
 * Represents the [KendoReact SplitButton component]({% slug overview_splitbutton %}).
 *
 * @example
 * ```jsx
 * class App extends React.Component {
 *    render() {
 *        return (
 *            <SplitButton text="Act">
 *                <SplitButtonItem text="Item1" />
 *                <SplitButtonItem text="Item2" />
 *                <SplitButtonItem text="Item3" />
 *            </SplitButton>
 *        );
 *    }
 * }
 * ReactDOM.render(<App />, document.querySelector('my-app'));
 * ```
 */
var SplitButton = /** @class */ (function (_super) {
    __extends(SplitButton, _super);
    function SplitButton() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /**
         * @hidden
         */
        _this.state = {
            focused: false,
            focusedIndex: -1,
            opened: false
        };
        _this.wrapper = null;
        _this.mainButton = null;
        _this.guid = Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["guid"])();
        _this.buttonsData = [];
        _this.onKeyDown = function (event) {
            var _a = _this.state, opened = _a.opened, focusedIndex = _a.focusedIndex;
            if (event.altKey) {
                if (!opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].down) {
                    _this.dispatchPopupEvent(event, true);
                    _this.setState({ focusedIndex: 0, opened: true });
                }
                else if (opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].up) {
                    _this.dispatchPopupEvent(event, false);
                    _this.setState({ focusedIndex: -1, opened: false });
                }
                return;
            }
            var newState = undefined;
            if (event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].enter || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].space) {
                // Prevent default because otherwise when an item is selected
                // click on the default button gets emitted which opens the popup again.
                event.preventDefault();
                _this.dispatchClickEvent(event, focusedIndex);
                if (focusedIndex >= 0) {
                    newState = {
                        focusedIndex: opened ? -1 : 0,
                        opened: !opened
                    };
                    _this.dispatchPopupEvent(event, newState.opened);
                }
            }
            else if (opened && event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].esc) {
                newState = {
                    focusedIndex: -1,
                    opened: false
                };
                _this.dispatchPopupEvent(event, newState.opened);
            }
            if (opened) {
                var newFocused = Object(_utils_navigation__WEBPACK_IMPORTED_MODULE_6__["default"])(focusedIndex, event.keyCode, event.altKey, _this.buttonsData.length);
                if (newFocused !== focusedIndex) {
                    newState = newState || {};
                    newState.focusedIndex = newFocused;
                }
                var arrowKey = event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].up || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].down ||
                    event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].left || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["Keys"].right;
                if (!event.altKey && arrowKey) {
                    // Needed to notify the parent listeners that event is handled.
                    event.preventDefault();
                }
            }
            if (newState) {
                _this.setState(newState);
            }
        };
        _this.onFocus = function (event) {
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(_this.props.onFocus, event, _this, undefined);
            _this.setState({ focused: true, focusedIndex: -1 });
        };
        _this.onItemClick = function (event, clickedItemIndex) {
            var opened = _this.state.opened;
            if (opened) {
                _this.setState({ focusedIndex: 0, opened: false });
            }
            _this.dispatchClickEvent(event, clickedItemIndex);
            if (opened) {
                _this.dispatchPopupEvent(event, false);
            }
        };
        _this.onBlur = function (event) {
            _this.setState({
                focused: false,
                focusedIndex: -1,
                opened: false
            });
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(_this.props.onBlur, event, _this, undefined);
            if (_this.state.opened) {
                _this.dispatchPopupEvent(event, false);
            }
        };
        _this.onSplitPartClick = function (event) {
            if (_this.buttonsData.length) {
                var opened = _this.state.opened;
                var toOpen = !opened;
                _this.dispatchPopupEvent(event, toOpen);
                _this.setState({
                    focusedIndex: toOpen ? 0 : -1,
                    opened: toOpen,
                    focused: true
                });
            }
        };
        _this.onDownSplitPart = function (event) {
            event.preventDefault();
            if (_this.element && document.activeElement !== _this.element) {
                _this.element.focus();
            }
        };
        _this.onItemDown = function (event) {
            if (document.activeElement === _this.element) {
                event.preventDefault();
            }
        };
        _this.dispatchPopupEvent = function (dispatchedEvent, open) {
            Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(open ? _this.props.onOpen : _this.props.onClose, dispatchedEvent, _this, undefined);
        };
        return _this;
    }
    /**
     * @hidden
     */
    SplitButton.prototype.render = function () {
        var _this = this;
        this.buttonsData = this.props.items ||
            react__WEBPACK_IMPORTED_MODULE_0__["Children"].toArray(this.props.children)
                .filter(function (child) { return child && child.type === _SplitButtonItem__WEBPACK_IMPORTED_MODULE_5__["default"]; })
                .map(function (child) { return child.props; });
        var rtl = this.isRtl();
        var dir = rtl ? 'rtl' : undefined;
        var _a = this.props, tabIndex = _a.tabIndex, disabled = _a.disabled;
        var focusedIndex = this.state.focusedIndex;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["classNames"])('k-widget', 'k-split-button', 'k-button-group', this.props.className, {
                'k-state-focused': this.state.focused
            }), onKeyDown: this.onKeyDown, onFocus: this.onFocus, onBlur: this.onBlur, dir: dir, ref: function (el) { return _this.wrapper = el; } },
            react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_main__WEBPACK_IMPORTED_MODULE_2__["Button"], { onClick: function (event) { return _this.onItemClick(event, -1); }, disabled: disabled || undefined, tabIndex: tabIndex, className: this.props.buttonClass, icon: this.props.icon, iconClass: this.props.iconClass, imageUrl: this.props.imageUrl, look: this.props.look, dir: dir, "aria-disabled": disabled, "aria-haspopup": true, "aria-expanded": this.state.opened, "aria-label": this.props.text + " splitbutton", ref: function (el) { return _this.mainButton = el && el.element; }, "aria-owns": this.guid, "aria-activedescendant": focusedIndex >= 0 ? this.guid + "-" + focusedIndex : undefined }, this.props.text),
            react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_main__WEBPACK_IMPORTED_MODULE_2__["Button"], { icon: "arrow-s", disabled: disabled || undefined, tabIndex: -1, look: this.props.look, onClick: this.onSplitPartClick, onMouseDown: this.onDownSplitPart, onPointerDown: this.onDownSplitPart, dir: dir, "aria-label": "menu toggling button" }),
            this.renderPopup(rtl)));
    };
    /**
     * @hidden
     */
    SplitButton.prototype.componentDidMount = function () {
        if (this.props.dir === undefined && this.isRtl()) {
            this.forceUpdate();
        }
    };
    Object.defineProperty(SplitButton.prototype, "element", {
        /**
         * The DOM element of main button.
         */
        get: function () {
            return this.mainButton;
        },
        enumerable: true,
        configurable: true
    });
    SplitButton.prototype.dispatchClickEvent = function (dispatchedEvent, clickedItemIndex) {
        if (!this.isItemDisabled(clickedItemIndex)) {
            if (clickedItemIndex === -1) {
                Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(this.props.onButtonClick, dispatchedEvent, this, undefined);
            }
            else {
                Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["dispatchEvent"])(this.props.onItemClick, dispatchedEvent, this, {
                    item: this.buttonsData[clickedItemIndex],
                    itemIndex: clickedItemIndex
                });
            }
        }
    };
    SplitButton.prototype.renderPopup = function (rtl) {
        var _a = this.props.popupSettings, popupSettings = _a === void 0 ? {} : _a;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_progress_kendo_react_popup__WEBPACK_IMPORTED_MODULE_7__["Popup"], { anchor: this.wrapper || undefined, show: this.state.opened, animate: popupSettings.animate, popupClass: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_3__["classNames"])('k-list-container k-reset k-group', popupSettings.popupClass), anchorAlign: popupSettings.anchorAlign || Object(_utils_popup__WEBPACK_IMPORTED_MODULE_8__["getAnchorAlign"])(rtl), popupAlign: popupSettings.popupAlign || Object(_utils_popup__WEBPACK_IMPORTED_MODULE_8__["getPopupAlign"])(rtl), style: rtl ? { direction: 'rtl' } : undefined },
            react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("ul", { className: "k-list k-reset", role: "menu", id: this.guid }, this.renderChildItems())));
    };
    SplitButton.prototype.renderChildItems = function () {
        var _this = this;
        var _a = this.props, itemRender = _a.itemRender, textField = _a.textField;
        return this.buttonsData.length > 0 ? (this.buttonsData.map(function (item, index) {
            return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_ButtonItem__WEBPACK_IMPORTED_MODULE_4__["default"], { dataItem: item, textField: textField, focused: _this.state.focusedIndex === index, onClick: _this.onItemClick, onDown: _this.onItemDown, itemRender: itemRender, key: index, index: index, id: _this.guid + "-" + index }));
        })) : null;
    };
    SplitButton.prototype.isItemDisabled = function (index) {
        return this.buttonsData[index] ? this.buttonsData[index].disabled : this.props.disabled;
    };
    SplitButton.prototype.isRtl = function () {
        return this.props.dir !== undefined ? this.props.dir === 'rtl' :
            !!this.wrapper && getComputedStyle(this.wrapper).direction === 'rtl';
    };
    /**
     * @hidden
     */
    SplitButton.propTypes = {
        onButtonClick: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onFocus: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onBlur: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onItemClick: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onOpen: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        onClose: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"],
        text: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        items: prop_types__WEBPACK_IMPORTED_MODULE_1__["arrayOf"](prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]),
        textField: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"],
        disabled: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        icon: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        iconClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageUrl: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        popupSettings: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        itemRender: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"],
        look: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        buttonClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        dir: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"]
    };
    return SplitButton;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (SplitButton);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButtonItem.js":
/*!******************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButtonItem.js ***!
  \******************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();


var SplitButtonItem = /** @class */ (function (_super) {
    __extends(SplitButtonItem, _super);
    function SplitButtonItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    SplitButtonItem.prototype.render = function () {
        return null;
    };
    /**
     * @hidden
     */
    SplitButtonItem.propTypes = {
        text: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        icon: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        iconClass: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        imageUrl: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        disabled: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"],
        render: prop_types__WEBPACK_IMPORTED_MODULE_1__["any"]
    };
    return SplitButtonItem;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (SplitButtonItem);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/navigation.js":
/*!*******************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/navigation.js ***!
  \*******************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");

/**
 * @hidden
 */
var navigate = function (focusedIndex, keyCode, altKey, total) {
    if (altKey) {
        return focusedIndex;
    }
    switch (keyCode) {
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].enter:
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].space:
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].esc:
            return -1;
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].up:
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].left:
            return Math.max(0, focusedIndex - 1);
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].down:
        case _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_0__["Keys"].right:
            return Math.min(total - 1, focusedIndex + 1);
        default:
            return focusedIndex;
    }
};
/* harmony default export */ __webpack_exports__["default"] = (navigate);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/popup.js":
/*!**************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/utils/popup.js ***!
  \**************************************************************************************/
/*! exports provided: getAnchorAlign, getPopupAlign */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getAnchorAlign", function() { return getAnchorAlign; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getPopupAlign", function() { return getPopupAlign; });
/**
 * @hidden
 */
function getAnchorAlign(isDirectionRightToLeft) {
    var align = { horizontal: 'left', vertical: 'bottom' };
    if (isDirectionRightToLeft) {
        align.horizontal = 'right';
    }
    return align;
}
/**
 * @hidden
 */
function getPopupAlign(isDirectionRightToLeft) {
    var align = { horizontal: 'left', vertical: 'top' };
    if (isDirectionRightToLeft) {
        align.horizontal = 'right';
    }
    return align;
}


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/main.js":
/*!********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/main.js ***!
  \********************************************************************/
/*! exports provided: Toolbar, ToolbarItem, ToolbarSeparator, Button, ButtonGroup, SplitButton, SplitButtonItem, DropDownButton, DropDownButtonItem */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _Button__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Button */ "./node_modules/@progress/kendo-react-buttons/dist/es/Button.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Button", function() { return _Button__WEBPACK_IMPORTED_MODULE_0__["default"]; });

/* harmony import */ var _ButtonGroup__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ButtonGroup */ "./node_modules/@progress/kendo-react-buttons/dist/es/ButtonGroup.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ButtonGroup", function() { return _ButtonGroup__WEBPACK_IMPORTED_MODULE_1__["default"]; });

/* harmony import */ var _ListButton_SplitButton__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ListButton/SplitButton */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButton.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "SplitButton", function() { return _ListButton_SplitButton__WEBPACK_IMPORTED_MODULE_2__["default"]; });

/* harmony import */ var _ListButton_SplitButtonItem__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./ListButton/SplitButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/SplitButtonItem.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "SplitButtonItem", function() { return _ListButton_SplitButtonItem__WEBPACK_IMPORTED_MODULE_3__["default"]; });

/* harmony import */ var _ListButton_DropDownButton__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./ListButton/DropDownButton */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButton.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropDownButton", function() { return _ListButton_DropDownButton__WEBPACK_IMPORTED_MODULE_4__["default"]; });

/* harmony import */ var _ListButton_DropDownButtonItem__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./ListButton/DropDownButtonItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/ListButton/DropDownButtonItem.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropDownButtonItem", function() { return _ListButton_DropDownButtonItem__WEBPACK_IMPORTED_MODULE_5__["default"]; });

/* harmony import */ var _toolbar_Toolbar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./toolbar/Toolbar */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/Toolbar.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Toolbar", function() { return _toolbar_Toolbar__WEBPACK_IMPORTED_MODULE_6__["default"]; });

/* harmony import */ var _toolbar_tools_ToolbarItem__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./toolbar/tools/ToolbarItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarItem.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ToolbarItem", function() { return _toolbar_tools_ToolbarItem__WEBPACK_IMPORTED_MODULE_7__["default"]; });

/* harmony import */ var _toolbar_tools_ToolbarSeparator__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./toolbar/tools/ToolbarSeparator */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarSeparator.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "ToolbarSeparator", function() { return _toolbar_tools_ToolbarSeparator__WEBPACK_IMPORTED_MODULE_8__["default"]; });













/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/Toolbar.js":
/*!*******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/Toolbar.js ***!
  \*******************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _tools_ToolbarItem__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./tools/ToolbarItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarItem.js");
/* harmony import */ var _tools_ToolbarButton__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./tools/ToolbarButton */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarButton.js");
/* harmony import */ var _ButtonGroup__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../ButtonGroup */ "./node_modules/@progress/kendo-react-buttons/dist/es/ButtonGroup.js");
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};






/**
 * Represents the [KendoReact Toolbar component]({% slug overview_toolbar %}).
 *
 * @example
 * ```jsx
 * class App extends React.Component {
 *    render() {
 *       return (
 *          <Toolbar>
 *              <ToolbarItem>
 *                  <ButtonGroup>
 *                      <Button togglable={true} icon="bold" />
 *                      <Button togglable={true} icon="italic" />
 *                      <Button togglable={true} icon="underline" />
 *                  </ButtonGroup>
 *              </ToolbarItem>
 *              <ToolbarItem>
 *                  <ButtonGroup>
 *                      <Button icon="hyperlink">Insert Link</Button>
 *                      <Button icon="image">Insert Image</Button>
 *                      <Button icon="table">Insert Table</Button>
 *                  </ButtonGroup>
 *              </ToolbarItem>
 *          </Toolbar>
 *       );
 *    }
 * }
 * ReactDOM.render(<App />, document.querySelector('my-app'));
 * ```
 */
var Toolbar = /** @class */ (function (_super) {
    __extends(Toolbar, _super);
    function Toolbar() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this._element = null;
        _this.buttons = [];
        _this.focusedIndex = 0;
        _this.counter = 0;
        _this.offsetHeight = 0;
        _this.offsetWidth = 0;
        _this.mapToolbarChild = function (toolbarItem) {
            if (toolbarItem && toolbarItem.type === _tools_ToolbarItem__WEBPACK_IMPORTED_MODULE_2__["default"]) {
                return react__WEBPACK_IMPORTED_MODULE_0__["cloneElement"](toolbarItem, toolbarItem.props, react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(toolbarItem.props.children, _this.mapItemChildren));
            }
            return toolbarItem;
        };
        _this.mapItemChildren = function (tool) {
            if (tool.type === _ButtonGroup__WEBPACK_IMPORTED_MODULE_4__["default"]) {
                return react__WEBPACK_IMPORTED_MODULE_0__["cloneElement"](tool, tool.props, react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(tool.props.children, _this.mapButton));
            }
            return _this.mapButton(tool);
        };
        _this.mapButton = function (button) {
            var index = _this.counter;
            _this.counter++;
            return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_tools_ToolbarButton__WEBPACK_IMPORTED_MODULE_3__["default"], { ref: function (b) { return _this.buttonRef(b, index); }, tabIndex: _this.focusedIndex === index ?
                    (_this.props.tabIndex || Toolbar.defaultProps.tabIndex) : -1, button: button }));
        };
        _this.buttonRef = function (button, index) {
            _this.buttons[index] = button;
            if (!button && !_this.buttons.find(function (b) { return b !== null; })) {
                _this.buttons.length = 0;
            }
        };
        _this.onKeyDown = function (event) {
            var target = event.target;
            var _a = _this, focusedIndex = _a.focusedIndex, buttons = _a.buttons;
            var arrowKey = event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["Keys"].left || event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["Keys"].right;
            if (!arrowKey || event.defaultPrevented || buttons.findIndex(function (b) { return b !== null && b.element === target; }) === -1) {
                return;
            }
            if (event.keyCode === _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["Keys"].left) {
                _this.focusButton(focusedIndex, focusedIndex - 1);
            }
            else {
                _this.focusButton(focusedIndex, focusedIndex + 1);
            }
        };
        _this.onWindowResize = function (event) {
            var element = _this.element;
            if (!element) {
                return;
            }
            var offsetWidth = element.offsetWidth;
            var offsetHeight = element.offsetHeight;
            if (_this.offsetWidth !== offsetWidth || _this.offsetHeight !== offsetHeight) {
                _this.offsetWidth = offsetWidth;
                _this.offsetHeight = offsetHeight;
                var newSizes = { offsetWidth: _this.offsetWidth, offsetHeight: _this.offsetHeight };
                if (_this.props.onResize) {
                    _this.props.onResize.call(undefined, __assign({ target: _this }, newSizes, { nativeEvent: event }));
                }
            }
        };
        return _this;
    }
    Object.defineProperty(Toolbar.prototype, "element", {
        /**
         * Returns the HTML element of the Toolbar component.
         */
        get: function () {
            return this._element;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @hidden
     */
    Toolbar.prototype.componentDidMount = function () {
        window.addEventListener('resize', this.onWindowResize);
        var element = this.element;
        if (element) {
            this.offsetWidth = element.offsetWidth;
            this.offsetHeight = element.offsetHeight;
        }
    };
    /**
     * @hidden
     */
    Toolbar.prototype.componentDidUpdate = function () {
        var _this = this;
        var element = this.element;
        if (!element) {
            return;
        }
        var focused = element.querySelector('button:focus');
        var prevFocusedIndex = this.focusedIndex;
        if (!focused) {
            this.focusedIndex = 0;
        }
        else {
            var focusedIndex = this.buttons.findIndex(function (b) { return b !== null && b.element === focused; });
            if (focusedIndex !== -1 && focusedIndex !== this.focusedIndex) {
                this.focusedIndex = focusedIndex;
            }
        }
        if (prevFocusedIndex !== this.focusedIndex) {
            var _a = this.props.tabIndex, tabIndex_1 = _a === void 0 ? Toolbar.defaultProps.tabIndex : _a;
            this.buttons.forEach(function (button, index) {
                if (button) {
                    button.tabIndex = (index === _this.focusedIndex && button.tabIndex === -1) ?
                        tabIndex_1 : -1;
                }
            });
        }
    };
    /**
     * @hidden
     */
    Toolbar.prototype.componentWillUnmount = function () {
        window.removeEventListener('resize', this.onWindowResize);
    };
    /**
     * @hidden
     */
    Toolbar.prototype.render = function () {
        var _this = this;
        this.counter = 0;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["classNames"])('k-widget k-toolbar', this.props.className), style: this.props.style, role: "toolbar", dir: this.props.dir, ref: function (element) { return _this._element = element; }, onKeyDown: this.onKeyDown }, react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(this.props.children, this.mapToolbarChild)));
    };
    Toolbar.prototype.focusButton = function (prevIndex, index) {
        var toolbarButton = this.buttons[index];
        if (toolbarButton) {
            var newFocused = toolbarButton.element;
            var currentFocused = this.element &&
                this.element.querySelector('button:focus');
            var _a = this.props.tabIndex, tabIndex = _a === void 0 ? Toolbar.defaultProps.tabIndex : _a;
            this.focusedIndex = index;
            if (currentFocused !== newFocused) {
                toolbarButton.tabIndex = tabIndex;
                toolbarButton.focus();
                var prevButton = this.buttons[prevIndex];
                if (prevButton) {
                    prevButton.tabIndex = -1;
                }
            }
        }
    };
    /**
     * @hidden
     */
    Toolbar.propTypes = {
        tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1__["number"],
        dir: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        style: prop_types__WEBPACK_IMPORTED_MODULE_1__["object"],
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        onResize: prop_types__WEBPACK_IMPORTED_MODULE_1__["func"]
    };
    /**
     * @hidden
     */
    Toolbar.defaultProps = {
        tabIndex: 0
    };
    return Toolbar;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (Toolbar);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarButton.js":
/*!*******************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarButton.js ***!
  \*******************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "react-dom");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};


/**
 * @hidden
 */
var ToolbarButton = /** @class */ (function (_super) {
    __extends(ToolbarButton, _super);
    function ToolbarButton(props) {
        var _this = _super.call(this, props) || this;
        _this._element = null;
        _this.state = {
            tabIndex: props.tabIndex
        };
        return _this;
    }
    Object.defineProperty(ToolbarButton.prototype, "button", {
        get: function () {
            var element = Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["findDOMNode"])(this);
            if (element && element.nodeName !== 'BUTTON') {
                return element.querySelector('button');
            }
            return element;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ToolbarButton.prototype, "tabIndex", {
        get: function () {
            return this.state.tabIndex;
        },
        set: function (value) {
            if (this.state.tabIndex !== value) {
                this.setState({
                    tabIndex: value
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    ToolbarButton.prototype.focus = function () {
        var button = this.element;
        if (button) {
            button.focus();
        }
    };
    Object.defineProperty(ToolbarButton.prototype, "element", {
        get: function () {
            return this._element;
        },
        enumerable: true,
        configurable: true
    });
    ToolbarButton.prototype.componentDidMount = function () {
        this._element = this.button;
        this.adjustTabIndex();
    };
    ToolbarButton.prototype.componentWillUnmount = function () {
        this._element = null;
    };
    ToolbarButton.prototype.componentDidUpdate = function () {
        this._element = this.button;
        this.adjustTabIndex();
    };
    ToolbarButton.prototype.render = function () {
        var button = this.props.button;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](button.type, __assign({}, button.props, { tabIndex: this.state.tabIndex }), button.props.children));
    };
    ToolbarButton.prototype.adjustTabIndex = function () {
        var button = this.element;
        var tabIndex = this.state.tabIndex;
        if (button && button.tabIndex !== tabIndex) {
            button.tabIndex = tabIndex;
        }
    };
    return ToolbarButton;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (ToolbarButton);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarItem.js":
/*!*****************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarItem.js ***!
  \*****************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();


/**
 * Represents the KendoReact ToolbarItem component.
 * To add a tool to the Toolbar, wrap it inside a ToolbarItem component
 * ([see example]({% slug content_toolbar %})).
 */
var ToolbarItem = /** @class */ (function (_super) {
    __extends(ToolbarItem, _super);
    function ToolbarItem() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this._element = null;
        return _this;
    }
    Object.defineProperty(ToolbarItem.prototype, "element", {
        /**
         * Returns the HTML element of the ToolbarItem component.
         */
        get: function () {
            return this._element;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @hidden
     */
    ToolbarItem.prototype.render = function () {
        var _this = this;
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { className: this.props.className, style: { display: 'inline-block' }, ref: function (e) { return _this._element = e; } }, this.props.children));
    };
    /**
     * @hidden
     */
    ToolbarItem.propTypes = {
        className: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"]
    };
    return ToolbarItem;
}(react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"]));
/* harmony default export */ __webpack_exports__["default"] = (ToolbarItem);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarSeparator.js":
/*!**********************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarSeparator.js ***!
  \**********************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _ToolbarItem__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./ToolbarItem */ "./node_modules/@progress/kendo-react-buttons/dist/es/toolbar/tools/ToolbarItem.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();


/**
 * Represents the KendoReact Toolbar Separator component.
 */
var ToolbarSeparator = /** @class */ (function (_super) {
    __extends(ToolbarSeparator, _super);
    function ToolbarSeparator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * @hidden
     */
    ToolbarSeparator.prototype.render = function () {
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_ToolbarItem__WEBPACK_IMPORTED_MODULE_1__["default"], { className: "k-separator" }));
    };
    return ToolbarSeparator;
}(react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"]));
/* harmony default export */ __webpack_exports__["default"] = (ToolbarSeparator);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-buttons/dist/es/util.js":
/*!********************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-buttons/dist/es/util.js ***!
  \********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/**
 * @hidden
 */
var styles = {
    button: 'k-button',
    'bare': 'k-bare',
    'flat': 'k-flat',
    'outline': 'k-outline',
    'primary': 'k-primary',
    'state-active': 'k-state-active',
    'button-icon': 'k-button-icon',
    'button-icontext': 'k-button-icontext',
    'state-disabled': 'k-state-disabled',
    'group-start': 'k-group-start',
    'group-end': 'k-group-end',
    'button-group': 'k-button-group',
    'button-group-stretched': 'k-button-group-stretched',
    'ltr': 'k-ltr',
    'rtl': 'k-rtl'
};
/**
 * @hidden
 */
/* harmony default export */ __webpack_exports__["default"] = ({
    styles: styles
});


/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/Draggable.js":
/*!************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/Draggable.js ***!
  \************************************************************************/
/*! exports provided: Draggable */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Draggable", function() { return Draggable; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _telerik_kendo_draggable__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @telerik/kendo-draggable */ "./node_modules/@telerik/kendo-draggable/dist/es/main.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();



/**
 * @hidden
 */
var Draggable = /** @class */ (function (_super) {
    __extends(Draggable, _super);
    function Draggable() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /**
         * @hidden
         */
        _this.element = null;
        _this.draggable = new _telerik_kendo_draggable__WEBPACK_IMPORTED_MODULE_2__["default"]({
            press: function (event) {
                if (_this.element && _this.props.onPress) {
                    _this.props.onPress.call(undefined, {
                        target: _this,
                        event: event,
                        element: _this.element
                    });
                }
            },
            drag: function (event) {
                if (_this.element && _this.props.onDrag) {
                    _this.props.onDrag.call(undefined, {
                        target: _this,
                        event: event,
                        element: _this.element
                    });
                }
            },
            release: function (event) {
                if (_this.element && _this.props.onRelease) {
                    _this.props.onRelease.call(undefined, {
                        target: _this,
                        event: event
                    });
                }
            }
        });
        _this.assingRef = function (element) {
            _this.element = element;
        };
        return _this;
    }
    Draggable.prototype.componentDidMount = function () {
        if (this.element) {
            this.draggable.bindTo(this.element);
        }
    };
    Draggable.prototype.componentWillUnmount = function () {
        this.draggable.destroy();
    };
    Draggable.prototype.render = function () {
        return (react__WEBPACK_IMPORTED_MODULE_0__["cloneElement"](react__WEBPACK_IMPORTED_MODULE_0__["Children"].only(this.props.children), {
            ref: this.assingRef
        }));
    };
    Draggable.propTypes = {
        children: prop_types__WEBPACK_IMPORTED_MODULE_1__["element"].isRequired
    };
    return Draggable;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));



/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/FloatingLabel.js":
/*!****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/FloatingLabel.js ***!
  \****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _classNames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./classNames */ "./node_modules/@progress/kendo-react-common/dist/es/classNames.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();



/**
 * @hidden
 */
var FloatingLabel = /** @class */ (function (_super) {
    __extends(FloatingLabel, _super);
    function FloatingLabel() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.state = {
            focused: false
        };
        _this.handleFocus = function (_) {
            _this.setState({ focused: true });
        };
        _this.handleBlur = function (_) {
            _this.setState({ focused: false });
        };
        return _this;
    }
    FloatingLabel.prototype.render = function () {
        var _a = this.props, label = _a.label, id = _a.id, className = _a.className, value = _a.value, placeholder = _a.placeholder, valid = _a.valid, style = _a.style;
        var childClassNames = Object(_classNames__WEBPACK_IMPORTED_MODULE_2__["classNames"])({
            'k-textbox-container': true,
            'k-state-focused': this.state.focused,
            'k-state-empty': !(value || placeholder),
            'k-state-invalid': !valid && valid !== undefined
        }, className);
        return (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { className: childClassNames, onFocus: this.handleFocus, onBlur: this.handleBlur, style: style },
            this.props.children,
            label
                ? id
                    ? react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("label", { htmlFor: id, className: "k-label" }, label)
                    : react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("span", { className: "k-label" }, label)
                : null));
    };
    FloatingLabel.propTypes = {
        label: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        value: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        placeholder: prop_types__WEBPACK_IMPORTED_MODULE_1__["string"],
        valid: prop_types__WEBPACK_IMPORTED_MODULE_1__["bool"]
    };
    return FloatingLabel;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (FloatingLabel);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/FormComponent.js":
/*!****************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/FormComponent.js ***!
  \****************************************************************************/
/*! exports provided: FormComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FormComponent", function() { return FormComponent; });
/**
 * @hidden
 */
var FormComponent = /** @class */ (function () {
    function FormComponent() {
    }
    return FormComponent;
}());



/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/classNames.js":
/*!*************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/classNames.js ***!
  \*************************************************************************/
/*! exports provided: classNames */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "classNames", function() { return classNames; });
/**
 * @hidden
 */
var classNames = function () {
    var args = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        args[_i] = arguments[_i];
    }
    return args
        .filter(function (arg) { return arg !== true && !!arg; })
        .map(function (arg) {
        return Array.isArray(arg)
            ? classNames.apply(void 0, arg) : typeof arg === 'object'
            ? Object
                .keys(arg)
                .map(function (key, idx) { return arg[idx] || (arg[key] && key) || null; })
                .filter(function (el) { return el !== null; })
                .join(' ')
            : arg;
    })
        .filter(function (arg) { return !!arg; })
        .join(' ');
};


/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/events/dispatchEvent.js":
/*!***********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/events/dispatchEvent.js ***!
  \***********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return dispatchEvent; });
/**
 * @hidden
 * Dispatches a new event bsed on an event that was already internally dispatched to KendoReact users.
 *
 * @param eventHandler - The public event handler that is assigned by the user.
 * When undefined, the method is not an option.
 * @param dispatchedEvent - The event that was already dispatched internally.
 * @param target - The target component of the new event.
 * @param eventData - The additional data that will be passed through the new event.
 * When the new event has no additional data
 *  other than the `BaseEvent` arguments, pass `undefined`.
 */
function dispatchEvent(eventHandler, dispatchedEvent, target, 
// TODO: Uncomment after switching to TS 3.
// eventData: Exclude<keyof E, keyof BaseEvent<React.Component>> extends never ?
//     undefined : Pick<E, Exclude<keyof E, keyof BaseEvent<React.Component>>>
eventData) {
    if (eventHandler) {
        var eventBaseData = {
            syntheticEvent: dispatchedEvent,
            nativeEvent: dispatchedEvent.nativeEvent,
            target: target
        };
        eventHandler.call(undefined, Object.assign(eventBaseData, eventData));
    }
}


/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/guid.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/guid.js ***!
  \*******************************************************************/
/*! exports provided: guid */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "guid", function() { return guid; });
/* tslint:disable:no-bitwise */
/**
 * @hidden
 */
var guid = function () {
    var id = '';
    var i;
    var random;
    for (i = 0; i < 32; i++) {
        random = Math.random() * 16 | 0;
        if (i === 8 || i === 12 || i === 16 || i === 20) {
            id += '-';
        }
        id += (i === 12 ? 4 : (i === 16 ? (random & 3 | 8) : random)).toString(16);
    }
    return id;
};



/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/keys.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/keys.js ***!
  \*******************************************************************/
/*! exports provided: Keys */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Keys", function() { return Keys; });
/**
 * @hidden
 */
var Keys = {
    backspace: 8,
    tab: 9,
    enter: 13,
    shift: 16,
    esc: 27,
    space: 32,
    pageUp: 33,
    pageDown: 34,
    end: 35,
    home: 36,
    left: 37,
    up: 38,
    right: 39,
    down: 40,
    delete: 46
};



/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/main.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/main.js ***!
  \*******************************************************************/
/*! exports provided: classNames, guid, Keys, noop, FloatingLabel, FormComponent, dispatchEvent, isServerRendering, Draggable */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _classNames__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./classNames */ "./node_modules/@progress/kendo-react-common/dist/es/classNames.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "classNames", function() { return _classNames__WEBPACK_IMPORTED_MODULE_0__["classNames"]; });

/* harmony import */ var _guid__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./guid */ "./node_modules/@progress/kendo-react-common/dist/es/guid.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "guid", function() { return _guid__WEBPACK_IMPORTED_MODULE_1__["guid"]; });

/* harmony import */ var _keys__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./keys */ "./node_modules/@progress/kendo-react-common/dist/es/keys.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Keys", function() { return _keys__WEBPACK_IMPORTED_MODULE_2__["Keys"]; });

/* harmony import */ var _noop__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./noop */ "./node_modules/@progress/kendo-react-common/dist/es/noop.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "noop", function() { return _noop__WEBPACK_IMPORTED_MODULE_3__["noop"]; });

/* harmony import */ var _FormComponent__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./FormComponent */ "./node_modules/@progress/kendo-react-common/dist/es/FormComponent.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "FormComponent", function() { return _FormComponent__WEBPACK_IMPORTED_MODULE_4__["FormComponent"]; });

/* harmony import */ var _FloatingLabel__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./FloatingLabel */ "./node_modules/@progress/kendo-react-common/dist/es/FloatingLabel.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "FloatingLabel", function() { return _FloatingLabel__WEBPACK_IMPORTED_MODULE_5__["default"]; });

/* harmony import */ var _events_dispatchEvent__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./events/dispatchEvent */ "./node_modules/@progress/kendo-react-common/dist/es/events/dispatchEvent.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "dispatchEvent", function() { return _events_dispatchEvent__WEBPACK_IMPORTED_MODULE_6__["default"]; });

/* harmony import */ var _serverRendering__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./serverRendering */ "./node_modules/@progress/kendo-react-common/dist/es/serverRendering.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "isServerRendering", function() { return _serverRendering__WEBPACK_IMPORTED_MODULE_7__["isServerRendering"]; });

/* harmony import */ var _Draggable__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./Draggable */ "./node_modules/@progress/kendo-react-common/dist/es/Draggable.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Draggable", function() { return _Draggable__WEBPACK_IMPORTED_MODULE_8__["Draggable"]; });













/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/noop.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/noop.js ***!
  \*******************************************************************/
/*! exports provided: noop */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "noop", function() { return noop; });
/**
 * @hidden
 */
var noop = function () { };



/***/ }),

/***/ "./node_modules/@progress/kendo-react-common/dist/es/serverRendering.js":
/*!******************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-common/dist/es/serverRendering.js ***!
  \******************************************************************************/
/*! exports provided: isServerRendering */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isServerRendering", function() { return isServerRendering; });
/**
 * @hidden
 */
var isServerRendering = function () {
    return typeof window === 'undefined';
};



/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/Popup.js":
/*!*******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/Popup.js ***!
  \*******************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "react-dom");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var react_dom_server__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-dom/server */ "./node_modules/react-dom/server.browser.js");
/* harmony import */ var react_dom_server__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(react_dom_server__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _progress_kendo_react_animation__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @progress/kendo-react-animation */ "./node_modules/@progress/kendo-react-animation/dist/es/main.js");
/* harmony import */ var _progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @progress/kendo-react-common */ "./node_modules/@progress/kendo-react-common/dist/es/main.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./util */ "./node_modules/@progress/kendo-react-popup/dist/es/util.js");
/* harmony import */ var _services_alignService__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./services/alignService */ "./node_modules/@progress/kendo-react-popup/dist/es/services/alignService.js");
/* harmony import */ var _services_domService__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./services/domService */ "./node_modules/@progress/kendo-react-popup/dist/es/services/domService.js");
/* harmony import */ var _services_positionService__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./services/positionService */ "./node_modules/@progress/kendo-react-popup/dist/es/services/positionService.js");
var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (undefined && undefined.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};










var DEFAULT_OFFSET = {
    left: -1000,
    top: 0
};
var ANIMATION_CONTAINER = 'k-animation-container';
var ANIMATION_CONTAINER_SHOWN = 'k-animation-container-shown';
var K_POPUP = 'k-popup';
var getHtmlElement = function (source) {
    return react_dom__WEBPACK_IMPORTED_MODULE_1__["findDOMNode"](source);
};
var Popup = /** @class */ (function (_super) {
    __extends(Popup, _super);
    function Popup(props) {
        var _this = _super.call(this, props) || this;
        _this._exitingAnimation = false;
        _this._prevShow = false;
        _this.onOpened = function () {
            var element = getHtmlElement(_this);
            if (_this.props.show) {
                element.classList.add(ANIMATION_CONTAINER_SHOWN);
            }
            _this.attachRepositionHandlers(element);
            if (_this.props.open) {
                _this.props.open.call(undefined, { target: _this });
            }
        };
        _this.onClosing = function () {
            if (!_this.props.show) {
                var element = getHtmlElement(_this);
                element.classList.remove(ANIMATION_CONTAINER_SHOWN);
            }
            _this.detachRepositionHandlers();
        };
        _this.onClosed = function () {
            if (_this._exitingAnimation) {
                _this._exitingAnimation = false;
                _this.forceUpdate();
            }
            if (_this.props.close) {
                _this.props.close.call(undefined, { target: _this });
            }
        };
        _this.position = function (settings, element, anchorElement) {
            var anchorAlign = settings.anchorAlign, popupAlign = settings.popupAlign, collision = settings.collision, offset = settings.offset;
            var alignedOffset = _this._alignService.alignElement({
                anchor: anchorElement,
                element: element,
                elementAlign: popupAlign,
                anchorAlign: anchorAlign,
                offset: offset
            });
            var result = _this._positionService.positionElement({
                anchor: anchorElement,
                anchorAlign: anchorAlign,
                collisions: collision,
                element: element,
                currentLocation: alignedOffset,
                elementAlign: popupAlign
            });
            return result;
        };
        _this._flipped = false;
        _this._offset = _this.props.offset;
        _this._prevShow = props.show;
        _this._domService = new _services_domService__WEBPACK_IMPORTED_MODULE_8__["DOMService"]();
        _this._alignService = new _services_alignService__WEBPACK_IMPORTED_MODULE_7__["AlignService"](_this._domService);
        _this._positionService = new _services_positionService__WEBPACK_IMPORTED_MODULE_9__["PositionService"](_this._domService);
        _this.onOpened = _this.onOpened.bind(_this);
        _this.onClosing = _this.onClosing.bind(_this);
        _this.reposition = Object(_util__WEBPACK_IMPORTED_MODULE_6__["throttle"])(_this.reposition.bind(_this), _util__WEBPACK_IMPORTED_MODULE_6__["FRAME_DURATION"]);
        return _this;
    }
    /**
     * @hidden
     */
    Popup.prototype.componentDidUpdate = function () {
        this._prevShow = this.props.show;
    };
    /**
     * @hidden
     */
    Popup.prototype.componentWillUnmount = function () {
        this.detachRepositionHandlers();
    };
    /**
     * @hidden
     */
    Popup.prototype.render = function () {
        var _a = this.props, children = _a.children, className = _a.className, popupClass = _a.popupClass, show = _a.show, id = _a.id, _b = _a.appendTo, appendTo = _b === void 0 ? Object(_util__WEBPACK_IMPORTED_MODULE_6__["isWindowAvailable"])() ? document.body : undefined : _b;
        if (show) {
            var newPosition = this.calculatePosition(this.props, appendTo);
            this._offset = newPosition.offset;
            this._flipped = !!newPosition.flipped;
        }
        var direction = this._flipped && show ? 'up' : 'down';
        var _c = this.transitionDuration, transitionEnterDuration = _c.transitionEnterDuration, transitionExitDuration = _c.transitionExitDuration;
        var style = Object.assign({}, { position: 'absolute' }, this.props.style || {}, __assign({}, this._offset));
        this._exitingAnimation = this._exitingAnimation || (this._prevShow && !show);
        if ((show || this._exitingAnimation) && appendTo) {
            var popup = (react__WEBPACK_IMPORTED_MODULE_0__["createElement"](_progress_kendo_react_animation__WEBPACK_IMPORTED_MODULE_4__["Slide"], { componentChildClassName: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["classNames"])(popupClass, K_POPUP), className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["classNames"])(className), id: id, onEntered: this.onOpened, onExiting: this.onClosing, onExited: this.onClosed, direction: direction, style: style, transitionEnterDuration: transitionEnterDuration, transitionExitDuration: transitionExitDuration, appear: true }, show ? children : null));
            return react_dom__WEBPACK_IMPORTED_MODULE_1__["createPortal"](popup, appendTo);
        }
        return null;
    };
    Object.defineProperty(Popup.prototype, "transitionDuration", {
        get: function () {
            var animate = this.props.animate;
            var transitionEnterDuration = 0;
            var transitionExitDuration = 0;
            if (animate) {
                if (animate === true) {
                    // Inherit the default duration of the Animation component.
                    transitionEnterDuration = transitionExitDuration = undefined;
                }
                else {
                    transitionEnterDuration = animate.openDuration;
                    transitionExitDuration = animate.closeDuration;
                }
            }
            return { transitionEnterDuration: transitionEnterDuration, transitionExitDuration: transitionExitDuration };
        },
        enumerable: true,
        configurable: true
    });
    Popup.prototype.calculatePosition = function (props, appendTo) {
        if (!appendTo || !Object(_util__WEBPACK_IMPORTED_MODULE_6__["isWindowAvailable"])()) {
            return {
                flipped: false,
                offset: props.offset
            };
        }
        var root = document.createElement('div');
        appendTo.appendChild(root);
        var style = Object.assign({}, props.style || {}, __assign({ visibility: 'hidden' }, DEFAULT_OFFSET));
        var innerClasses = { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["classNames"])(props.popupClass, K_POPUP, 'k-child-animation-container') };
        var popup = (react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", { className: Object(_progress_kendo_react_common__WEBPACK_IMPORTED_MODULE_5__["classNames"])(ANIMATION_CONTAINER, ANIMATION_CONTAINER_SHOWN, props.className), style: style }, react__WEBPACK_IMPORTED_MODULE_0__["Children"].map(props.children, function (child, index) {
            return react__WEBPACK_IMPORTED_MODULE_0__["createElement"]("div", __assign({ key: index }, innerClasses), child);
        })));
        root.innerHTML = react_dom_server__WEBPACK_IMPORTED_MODULE_3__["renderToStaticMarkup"](popup);
        var newPosition = this.position(props, root.firstChild, props.anchor);
        root.parentNode.removeChild(root);
        return newPosition;
    };
    Popup.prototype.attachRepositionHandlers = function (element) {
        var _this = this;
        this.detachRepositionHandlers();
        this._scrollableParents = this._domService.scrollableParents(this.props.anchor || element);
        this._scrollableParents.map(function (p) { return p.addEventListener('scroll', _this.reposition); });
        window.addEventListener('resize', this.reposition);
    };
    Popup.prototype.detachRepositionHandlers = function () {
        var _this = this;
        if (this._scrollableParents) {
            this._scrollableParents.map(function (p) { return p.removeEventListener('scroll', _this.reposition); });
            this._scrollableParents = undefined;
        }
        window.removeEventListener('resize', this.reposition);
    };
    Popup.prototype.reposition = function () {
        this.forceUpdate();
    };
    /**
     * @hidden
     */
    Popup.propTypes = {
        anchor: function (props) {
            var anchor = props.anchor;
            if (anchor && typeof anchor.nodeType !== 'number') {
                return new Error('Invalid prop `anchor` supplied to `Kendo React Popup`. Validation failed.');
            }
        },
        appendTo: function (props) {
            var element = props.appendTo;
            if (element && typeof element.nodeType !== 'number') {
                return new Error('Invalid prop `appendTo` supplied to `Kendo React Popup`. Validation failed.');
            }
        },
        className: prop_types__WEBPACK_IMPORTED_MODULE_2__["string"],
        id: prop_types__WEBPACK_IMPORTED_MODULE_2__["string"],
        popupClass: prop_types__WEBPACK_IMPORTED_MODULE_2__["string"],
        collision: prop_types__WEBPACK_IMPORTED_MODULE_2__["shape"]({
            horizontal: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].fit,
                _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].flip
            ]),
            vertical: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].fit,
                _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].flip
            ])
        }),
        anchorAlign: prop_types__WEBPACK_IMPORTED_MODULE_2__["shape"]({
            horizontal: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].left,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].center,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].right
            ]),
            vertical: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].top,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].center,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].bottom
            ])
        }),
        popupAlign: prop_types__WEBPACK_IMPORTED_MODULE_2__["shape"]({
            horizontal: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].left,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].center,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].right
            ]),
            vertical: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOf"]([
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].top,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].center,
                _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].bottom
            ])
        }),
        offset: prop_types__WEBPACK_IMPORTED_MODULE_2__["shape"]({
            left: prop_types__WEBPACK_IMPORTED_MODULE_2__["number"],
            top: prop_types__WEBPACK_IMPORTED_MODULE_2__["number"]
        }),
        children: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_2__["element"],
            prop_types__WEBPACK_IMPORTED_MODULE_2__["node"]
        ]),
        show: prop_types__WEBPACK_IMPORTED_MODULE_2__["bool"],
        animate: prop_types__WEBPACK_IMPORTED_MODULE_2__["oneOfType"]([
            prop_types__WEBPACK_IMPORTED_MODULE_2__["bool"],
            prop_types__WEBPACK_IMPORTED_MODULE_2__["shape"]({
                openDuration: prop_types__WEBPACK_IMPORTED_MODULE_2__["number"],
                closeDuration: prop_types__WEBPACK_IMPORTED_MODULE_2__["number"]
            })
        ])
    };
    /**
     * @hidden
     */
    Popup.defaultProps = {
        collision: {
            horizontal: _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].fit,
            vertical: _util__WEBPACK_IMPORTED_MODULE_6__["CollisionType"].flip
        },
        anchorAlign: {
            horizontal: _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].left,
            vertical: _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].bottom
        },
        popupAlign: {
            horizontal: _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].left,
            vertical: _util__WEBPACK_IMPORTED_MODULE_6__["AlignPoint"].top
        },
        offset: DEFAULT_OFFSET,
        animate: true,
        show: false
    };
    return Popup;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]));
/* harmony default export */ __webpack_exports__["default"] = (Popup);


/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/main.js":
/*!******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/main.js ***!
  \******************************************************************/
/*! exports provided: Popup */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _Popup__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Popup */ "./node_modules/@progress/kendo-react-popup/dist/es/Popup.js");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "Popup", function() { return _Popup__WEBPACK_IMPORTED_MODULE_0__["default"]; });





/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/services/alignService.js":
/*!***********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/services/alignService.js ***!
  \***********************************************************************************/
/*! exports provided: AlignService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AlignService", function() { return AlignService; });
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../util */ "./node_modules/@progress/kendo-react-popup/dist/es/util.js");

/**
 * @hidden
 */
var AlignService = /** @class */ (function () {
    function AlignService(_dom) {
        this._dom = _dom;
    }
    AlignService.prototype.alignElement = function (settings) {
        var anchor = settings.anchor, element = settings.element, anchorAlign = settings.anchorAlign, elementAlign = settings.elementAlign, offset = settings.offset;
        var fixedMode = !this._dom.hasOffsetParent(element);
        var anchorRect = fixedMode ?
            this.absoluteRect(anchor, element, offset) :
            this.relativeRect(anchor, element, offset);
        return this._dom.align({
            anchorAlign: anchorAlign,
            anchorRect: anchorRect,
            elementAlign: elementAlign,
            elementRect: this._dom.offset(element)
        });
    };
    AlignService.prototype.absoluteRect = function (anchor, element, offset) {
        var dom = this._dom;
        var rect = Object(_util__WEBPACK_IMPORTED_MODULE_0__["eitherRect"])(dom.offset(anchor), offset);
        var stackingOffset = dom.stackingElementOffset(element);
        var removedOffset = Object(_util__WEBPACK_IMPORTED_MODULE_0__["removeStackingOffset"])(rect, stackingOffset);
        var stackingScroll = dom.stackingElementScroll(element);
        var withScroll = dom.addScroll(removedOffset, stackingScroll);
        var scrollPosition = this.elementScrollPosition(anchor, element);
        var result = dom.removeScroll(withScroll, scrollPosition);
        result.left += window.scrollX || window.pageXOffset;
        result.top += window.scrollY || window.pageYOffset;
        return result;
    };
    AlignService.prototype.elementScrollPosition = function (anchor, element) {
        return anchor ? { x: 0, y: 0 } : this._dom.scrollPosition(element);
    };
    AlignService.prototype.relativeRect = function (anchor, element, offset) {
        return Object(_util__WEBPACK_IMPORTED_MODULE_0__["eitherRect"])(this._dom.position(anchor, element), offset);
    };
    return AlignService;
}());



/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/services/domService.js":
/*!*********************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/services/domService.js ***!
  \*********************************************************************************/
/*! exports provided: DOMService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DOMService", function() { return DOMService; });
/* harmony import */ var _progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @progress/kendo-popup-common */ "./node_modules/@progress/kendo-popup-common/dist/es/main.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../util */ "./node_modules/@progress/kendo-react-popup/dist/es/util.js");


/**
 * @hidden
 */
var DOMService = /** @class */ (function () {
    function DOMService() {
    }
    DOMService.prototype.addOffset = function (current, addition) {
        return {
            left: current.left + addition.left,
            top: current.top + addition.top
        };
    };
    DOMService.prototype.align = function (settings) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["align"])(settings);
    };
    DOMService.prototype.boundingOffset = function (el) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["boundingOffset"])(el);
    };
    DOMService.prototype.getWindow = function () {
        return Object(_util__WEBPACK_IMPORTED_MODULE_1__["isWindowAvailable"])() ? window : null;
    };
    DOMService.prototype.isBodyOffset = function (el) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["isBodyOffset"])(el);
    };
    DOMService.prototype.hasOffsetParent = function (el) {
        if (!el) {
            return false;
        }
        var offsetParentEl = el.offsetParent;
        return offsetParentEl &&
            !(offsetParentEl.nodeName === 'BODY' &&
                window.getComputedStyle(offsetParentEl).position === 'static');
    };
    DOMService.prototype.offset = function (el) {
        if (!el) {
            return null;
        }
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["offset"])(el);
    };
    DOMService.prototype.staticOffset = function (element) {
        if (!element) {
            return null;
        }
        var _a = element.style, left = _a.left, top = _a.top;
        element.style.left = '0px';
        element.style.top = '0px';
        var currentOffset = Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["offset"])(element);
        element.style.left = left;
        element.style.top = top;
        return currentOffset;
    };
    DOMService.prototype.position = function (element, popup) {
        if (!element || !popup) {
            return null;
        }
        var parentSibling = Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["siblingContainer"])(element, popup);
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["positionWithScroll"])(element, parentSibling);
    };
    DOMService.prototype.relativeOffset = function (el, currentLocation) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["applyLocationOffset"])(this.offset(el), currentLocation, this.isBodyOffset(el));
    };
    DOMService.prototype.addScroll = function (rect, scroll) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["addScroll"])(rect, scroll);
    };
    DOMService.prototype.removeScroll = function (rect, scroll) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["removeScroll"])(rect, scroll);
    };
    DOMService.prototype.restrictToView = function (settings) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["restrictToView"])(settings);
    };
    DOMService.prototype.scrollPosition = function (el) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["scrollPosition"])(el);
    };
    DOMService.prototype.scrollableParents = function (el) {
        return Object(_util__WEBPACK_IMPORTED_MODULE_1__["scrollableParents"])(el);
    };
    DOMService.prototype.stackingElementOffset = function (el) {
        var relativeContextElement = this.getRelativeContextElement(el);
        if (!relativeContextElement) {
            return null;
        }
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["offset"])(relativeContextElement);
    };
    DOMService.prototype.stackingElementScroll = function (el) {
        var relativeContextElement = this.getRelativeContextElement(el);
        if (!relativeContextElement) {
            return { x: 0, y: 0 };
        }
        return {
            x: relativeContextElement.scrollLeft,
            y: relativeContextElement.scrollTop
        };
    };
    DOMService.prototype.stackingElementViewPort = function (el) {
        var relativeContextElement = this.getRelativeContextElement(el);
        if (!relativeContextElement) {
            return null;
        }
        return {
            height: relativeContextElement.scrollHeight,
            width: relativeContextElement.scrollWidth
        };
    };
    DOMService.prototype.getRelativeContextElement = function (el) {
        if (!el || !_util__WEBPACK_IMPORTED_MODULE_1__["HAS_RELATIVE_STACKING_CONTEXT"]) {
            return null;
        }
        var parent = el.parentElement;
        while (parent) {
            if (window.getComputedStyle(parent).transform !== 'none') {
                return parent;
            }
            parent = parent.parentElement;
        }
        return null;
    };
    DOMService.prototype.useRelativePosition = function (el) {
        return !!this.getRelativeContextElement(el);
    };
    DOMService.prototype.windowViewPort = function (el) {
        return Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["getWindowViewPort"])(el);
    };
    DOMService.prototype.zIndex = function (anchor, container) {
        return Object(_util__WEBPACK_IMPORTED_MODULE_1__["zIndex"])(anchor, container);
    };
    DOMService.prototype.zoomLevel = function () {
        if (!Object(_util__WEBPACK_IMPORTED_MODULE_1__["isDocumentAvailable"])() || !Object(_util__WEBPACK_IMPORTED_MODULE_1__["isWindowAvailable"])()) {
            return 1;
        }
        return parseFloat((document.documentElement.clientWidth / window.innerWidth).toFixed(2));
    };
    DOMService.prototype.isZoomed = function () {
        return this.zoomLevel() > 1;
    };
    return DOMService;
}());



/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/services/positionService.js":
/*!**************************************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/services/positionService.js ***!
  \**************************************************************************************/
/*! exports provided: PositionService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PositionService", function() { return PositionService; });
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../util */ "./node_modules/@progress/kendo-react-popup/dist/es/util.js");

/**
 * @hidden
 */
var PositionService = /** @class */ (function () {
    function PositionService(_dom) {
        this._dom = _dom;
    }
    PositionService.prototype.positionElement = function (settings) {
        var anchor = settings.anchor, currentLocation = settings.currentLocation, element = settings.element, anchorAlign = settings.anchorAlign, elementAlign = settings.elementAlign, collisions = settings.collisions;
        var dom = this._dom;
        var viewPort = settings.viewPort || dom.stackingElementViewPort(element) || dom.windowViewPort(element);
        var anchorRect = Object(_util__WEBPACK_IMPORTED_MODULE_0__["eitherRect"])(dom.offset(anchor), currentLocation);
        var initialElementRect = Object(_util__WEBPACK_IMPORTED_MODULE_0__["replaceOffset"])(dom.staticOffset(element), currentLocation);
        var elementRect = this.elementRect(element, initialElementRect);
        var result = dom.restrictToView({
            anchorAlign: anchorAlign,
            anchorRect: anchorRect,
            collisions: collisions,
            elementAlign: elementAlign,
            elementRect: elementRect,
            viewPort: viewPort
        });
        return {
            flipped: result.flipped,
            offset: dom.addOffset(initialElementRect, result.offset)
        };
    };
    PositionService.prototype.elementRect = function (element, rect) {
        return this._dom.removeScroll(rect, this._dom.scrollPosition(element));
    };
    return PositionService;
}());



/***/ }),

/***/ "./node_modules/@progress/kendo-react-popup/dist/es/util.js":
/*!******************************************************************!*\
  !*** ./node_modules/@progress/kendo-react-popup/dist/es/util.js ***!
  \******************************************************************/
/*! exports provided: eitherRect, replaceOffset, removeStackingOffset, isDifferentOffset, isDocumentAvailable, isWindowAvailable, hasBoundingRect, OVERFLOW_REGEXP, scrollableParents, FRAME_DURATION, hasRelativeStackingContext, HAS_RELATIVE_STACKING_CONTEXT, zIndex, CollisionType, AlignPoint, throttle */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "eitherRect", function() { return eitherRect; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "replaceOffset", function() { return replaceOffset; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "removeStackingOffset", function() { return removeStackingOffset; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isDifferentOffset", function() { return isDifferentOffset; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isDocumentAvailable", function() { return isDocumentAvailable; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isWindowAvailable", function() { return isWindowAvailable; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasBoundingRect", function() { return hasBoundingRect; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OVERFLOW_REGEXP", function() { return OVERFLOW_REGEXP; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "scrollableParents", function() { return scrollableParents; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FRAME_DURATION", function() { return FRAME_DURATION; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasRelativeStackingContext", function() { return hasRelativeStackingContext; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "HAS_RELATIVE_STACKING_CONTEXT", function() { return HAS_RELATIVE_STACKING_CONTEXT; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "zIndex", function() { return zIndex; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CollisionType", function() { return CollisionType; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AlignPoint", function() { return AlignPoint; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "throttle", function() { return throttle; });
/* harmony import */ var _progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @progress/kendo-popup-common */ "./node_modules/@progress/kendo-popup-common/dist/es/main.js");

/**
 * @hidden
 */
var eitherRect = function (rect, offset) {
    if (!rect) {
        return { height: 0, left: offset.left, top: offset.top, width: 0 };
    }
    return rect;
};
/**
 * @hidden
 */
var replaceOffset = function (rect, offset) {
    if (!offset) {
        return rect;
    }
    var result = {
        height: rect.height,
        left: offset.left,
        top: offset.top,
        width: rect.width
    };
    return result;
};
/**
 * @hidden
 */
var removeStackingOffset = function (rect, stackingOffset) {
    if (!stackingOffset) {
        return rect;
    }
    var result = {
        height: rect.height,
        left: rect.left - stackingOffset.left,
        top: rect.top - stackingOffset.top,
        width: rect.width
    };
    return result;
};
/**
 * @hidden
 */
var isDifferentOffset = function (oldOffset, newOffset) {
    var oldLeft = oldOffset.left, oldTop = oldOffset.top;
    var newLeft = newOffset.left, newTop = newOffset.top;
    return Math.abs(oldLeft - newLeft) >= 1 || Math.abs(oldTop - newTop) >= 1;
};
/**
 * @hidden
 */
var isDocumentAvailable = function () {
    return typeof document !== 'undefined' && !!document.body;
};
/**
 * @hidden
 */
var isWindowAvailable = function () {
    return typeof window !== 'undefined';
};
/**
 * @hidden
 */
var hasBoundingRect = function (elem) { return !!elem.getBoundingClientRect; };
/**
 * @hidden
 */
var OVERFLOW_REGEXP = /auto|scroll/;
/**
 * @hidden
 */
var overflowStyle = function (element) {
    var styles = window.getComputedStyle(element);
    return "" + styles.overflow + styles.overflowX + styles.overflowY;
};
/**
 * @hidden
 */
var scrollableParents = function (element) {
    var parentElements = [];
    if (!isDocumentAvailable() || !isWindowAvailable()) {
        return parentElements;
    }
    var parent = element.parentElement;
    while (parent) {
        if (OVERFLOW_REGEXP.test(overflowStyle(parent))) {
            parentElements.push(parent);
        }
        parent = parent.parentElement;
    }
    parentElements.push(window);
    return parentElements;
};
/**
 * @hidden
 */
var FRAME_DURATION = 1000 / 60; // 1000ms divided by 60fps
/**
 * @hidden
 */
var hasRelativeStackingContext = function () {
    if (!isDocumentAvailable()) {
        return false;
    }
    var top = 10;
    var parent = document.createElement('div');
    parent.style.transform = 'matrix(10, 0, 0, 10, 0, 0)';
    parent.innerHTML = "<div style=\"position: fixed; top: " + top + "px;\">child</div>";
    document.body.appendChild(parent);
    var isDifferent = parent.children[0].getBoundingClientRect().top !== top;
    document.body.removeChild(parent);
    return isDifferent;
};
/**
 * @hidden
 */
var HAS_RELATIVE_STACKING_CONTEXT = hasRelativeStackingContext();
/**
 * @hidden
 */
var zIndex = function (anchor, container) {
    if (!anchor || !isDocumentAvailable() || !isWindowAvailable()) {
        return null;
    }
    var sibling = Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["siblingContainer"])(anchor, container);
    if (!sibling) {
        return null;
    }
    var result = [anchor].concat(Object(_progress_kendo_popup_common__WEBPACK_IMPORTED_MODULE_0__["parents"])(anchor, sibling)).reduce(function (index, p) {
        var zIndexStyle = p.style.zIndex || window.getComputedStyle(p).zIndex;
        var current = parseInt(zIndexStyle, 10);
        return current > index ? current : index;
    }, 0);
    return result ? (result + 1) : null;
};
/**
 * @hidden
 */
var CollisionType = {
    fit: 'fit',
    flip: 'flip'
};
/**
 * @hidden
 */
var AlignPoint = {
    left: 'left',
    center: 'center',
    right: 'right',
    bottom: 'bottom',
    top: 'top'
};
/**
 * @hidden
 */
var throttle = function (func, wait, options) {
    if (options === void 0) { options = {}; }
    var timeout, context, args, result;
    var previous = 0;
    options = options || {};
    var later = function () {
        previous = options.leading === false ? 0 : new Date().getTime();
        timeout = null;
        result = func.apply(context, args);
        if (!timeout) {
            context = args = null;
        }
    };
    var throttled = function () {
        var now = new Date().getTime();
        if (!previous && options.leading === false) {
            previous = now;
        }
        var remaining = wait - (now - previous);
        context = this;
        args = arguments;
        if (remaining <= 0 || remaining > wait) {
            if (timeout) {
                clearTimeout(timeout);
                timeout = null;
            }
            previous = now;
            result = func.apply(context, args);
            if (!timeout) {
                context = args = null;
            }
        }
        else if (!timeout && options.trailing !== false) {
            timeout = setTimeout(later, remaining);
        }
        return result;
    };
    return throttled;
};


/***/ }),

/***/ "./node_modules/@telerik/kendo-draggable/dist/es/main.js":
/*!***************************************************************!*\
  !*** ./node_modules/@telerik/kendo-draggable/dist/es/main.js ***!
  \***************************************************************/
/*! exports provided: Draggable, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Draggable", function() { return Draggable; });
var proxy = function (a, b) { return function (e) { return b(a(e)); }; };

var bind = function (el, event, callback) { return el.addEventListener && el.addEventListener(event, callback); };

var unbind = function (el, event, callback) { return el && el.removeEventListener && el.removeEventListener(event, callback); };

var noop = function () { /* empty */ };

var preventDefault = function (e) { return e.preventDefault(); };

var touchRegExp = /touch/;

// 300ms is the usual mouse interval;
// // However, an underpowered mobile device under a heavy load may queue mouse events for a longer period.
var IGNORE_MOUSE_TIMEOUT = 2000;

function normalizeEvent(e) {
    if (e.type.match(touchRegExp)) {
        return {
            pageX: e.changedTouches[0].pageX,
            pageY: e.changedTouches[0].pageY,
            clientX: e.changedTouches[0].clientX,
            clientY: e.changedTouches[0].clientY,
            type: e.type,
            originalEvent: e,
            isTouch: true
        };
    }

    return {
        pageX: e.pageX,
        pageY: e.pageY,
        clientX: e.clientX,
        clientY: e.clientY,
        offsetX: e.offsetX,
        offsetY: e.offsetY,
        type: e.type,
        ctrlKey: e.ctrlKey,
        shiftKey: e.shiftKey,
        altKey: e.altKey,
        originalEvent: e
    };
}

var Draggable = function Draggable(ref) {
    var this$1 = this;
    var press = ref.press; if ( press === void 0 ) press = noop;
    var drag = ref.drag; if ( drag === void 0 ) drag = noop;
    var release = ref.release; if ( release === void 0 ) release = noop;
    var mouseOnly = ref.mouseOnly; if ( mouseOnly === void 0 ) mouseOnly = false;

    this._pressHandler = proxy(normalizeEvent, press);
    this._dragHandler = proxy(normalizeEvent, drag);
    this._releaseHandler = proxy(normalizeEvent, release);
    this._ignoreMouse = false;
    this._mouseOnly = mouseOnly;
    this._touchAction;

    this._touchstart = function (e) {
        if (e.touches.length === 1) {
            this$1._pressHandler(e);
        }
    };

    this._touchmove = function (e) {
        if (e.touches.length === 1) {
            this$1._dragHandler(e);
        }
    };

    this._touchend = function (e) {
        // the last finger has been lifted, and the user is not doing gesture.
        // there might be a better way to handle this.
        if (e.touches.length === 0 && e.changedTouches.length === 1) {
            this$1._releaseHandler(e);
            this$1._ignoreMouse = true;
            setTimeout(this$1._restoreMouse, IGNORE_MOUSE_TIMEOUT);
        }
    };

    this._restoreMouse = function () {
        this$1._ignoreMouse = false;
    };

    this._mousedown = function (e) {
        var which = e.which;

        if ((which && which > 1) || this$1._ignoreMouse) {
            return;
        }

        bind(document, "mousemove", this$1._mousemove);
        bind(document, "mouseup", this$1._mouseup);
        this$1._pressHandler(e);
    };

    this._mousemove = function (e) {
        this$1._dragHandler(e);
    };

    this._mouseup = function (e) {
        unbind(document, "mousemove", this$1._mousemove);
        unbind(document, "mouseup", this$1._mouseup);
        this$1._releaseHandler(e);
    };

    this._pointerdown = function (e) {
        if (e.isPrimary && e.button === 0) {
            bind(document, "pointermove", this$1._pointermove);
            bind(document, "pointerup", this$1._pointerup);
            bind(document, "pointercancel", this$1._pointerup);
            bind(document, "contextmenu", preventDefault);

            this$1._touchAction = e.target.style.touchAction;
            e.target.style.touchAction = "none";

            this$1._pressHandler(e);
        }
    };

    this._pointermove = function (e) {
        if (e.isPrimary) {
            this$1._dragHandler(e);
        }
    };

    this._pointerup = function (e) {
        if (e.isPrimary) {
            unbind(document, "pointermove", this$1._pointermove);
            unbind(document, "pointerup", this$1._pointerup);
            unbind(document, "pointercancel", this$1._pointerup);
            unbind(document, "contextmenu", preventDefault);

            e.target.style.touchAction = this$1._touchAction;

            this$1._releaseHandler(e);
        }
    };
};

Draggable.supportPointerEvent = function supportPointerEvent () {
    return (typeof window !== 'undefined') && window.PointerEvent;
};

Draggable.prototype.bindTo = function bindTo (element) {
    if (element === this._element) {
        return;
    }

    if (this._element) {
        this._unbindFromCurrent();
    }

    this._element = element;
    this._bindToCurrent();
};

Draggable.prototype._bindToCurrent = function _bindToCurrent () {
    var element = this._element;

    if (this._usePointers()) {
        bind(element, "pointerdown", this._pointerdown);
        return;
    }

    bind(element, "mousedown", this._mousedown);

    if (!this._mouseOnly) {
        bind(element, "touchstart", this._touchstart);
        bind(element, "touchmove", this._touchmove);
        bind(element, "touchend", this._touchend);
    }
};

Draggable.prototype._unbindFromCurrent = function _unbindFromCurrent () {
    var element = this._element;

    if (this._usePointers()) {
        unbind(element, "pointerdown", this._pointerdown);
        unbind(document, "pointermove", this._pointermove);
        unbind(document, "pointerup", this._pointerup);
        unbind(document, "contextmenu", preventDefault);
        unbind(document, "pointercancel", this._pointerup);
        return;
    }

    unbind(element, "mousedown", this._mousedown);

    if (!this._mouseOnly) {
        unbind(element, "touchstart", this._touchstart);
        unbind(element, "touchmove", this._touchmove);
        unbind(element, "touchend", this._touchend);
    }
};

Draggable.prototype._usePointers = function _usePointers () {
    return !this._mouseOnly && Draggable.supportPointerEvent();
};

Draggable.prototype.destroy = function destroy () {
    this._unbindFromCurrent();
    this._element = null;
};

// Re-export as "default" field to address a bug
// where the ES Module is imported by CommonJS code.
//
// See https://github.com/telerik/kendo-angular/issues/1314
Draggable.default = Draggable;

// Rollup won't output exports['default'] otherwise
/* harmony default export */ __webpack_exports__["default"] = (Draggable);



/***/ }),

/***/ "./node_modules/dom-helpers/class/addClass.js":
/*!****************************************************!*\
  !*** ./node_modules/dom-helpers/class/addClass.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(/*! @babel/runtime/helpers/interopRequireDefault */ "./node_modules/@babel/runtime/helpers/interopRequireDefault.js");

exports.__esModule = true;
exports.default = addClass;

var _hasClass = _interopRequireDefault(__webpack_require__(/*! ./hasClass */ "./node_modules/dom-helpers/class/hasClass.js"));

function addClass(element, className) {
  if (element.classList) element.classList.add(className);else if (!(0, _hasClass.default)(element, className)) if (typeof element.className === 'string') element.className = element.className + ' ' + className;else element.setAttribute('class', (element.className && element.className.baseVal || '') + ' ' + className);
}

module.exports = exports["default"];

/***/ }),

/***/ "./node_modules/dom-helpers/class/hasClass.js":
/*!****************************************************!*\
  !*** ./node_modules/dom-helpers/class/hasClass.js ***!
  \****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = hasClass;

function hasClass(element, className) {
  if (element.classList) return !!className && element.classList.contains(className);else return (" " + (element.className.baseVal || element.className) + " ").indexOf(" " + className + " ") !== -1;
}

module.exports = exports["default"];

/***/ }),

/***/ "./node_modules/dom-helpers/class/removeClass.js":
/*!*******************************************************!*\
  !*** ./node_modules/dom-helpers/class/removeClass.js ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


function replaceClassName(origClass, classToRemove) {
  return origClass.replace(new RegExp('(^|\\s)' + classToRemove + '(?:\\s|$)', 'g'), '$1').replace(/\s+/g, ' ').replace(/^\s*|\s*$/g, '');
}

module.exports = function removeClass(element, className) {
  if (element.classList) element.classList.remove(className);else if (typeof element.className === 'string') element.className = replaceClassName(element.className, className);else element.setAttribute('class', replaceClassName(element.className && element.className.baseVal || '', className));
};

/***/ }),

/***/ "./node_modules/object-assign/index.js":
/*!*********************************************!*\
  !*** ./node_modules/object-assign/index.js ***!
  \*********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/*
object-assign
(c) Sindre Sorhus
@license MIT
*/


/* eslint-disable no-unused-vars */
var getOwnPropertySymbols = Object.getOwnPropertySymbols;
var hasOwnProperty = Object.prototype.hasOwnProperty;
var propIsEnumerable = Object.prototype.propertyIsEnumerable;

function toObject(val) {
	if (val === null || val === undefined) {
		throw new TypeError('Object.assign cannot be called with null or undefined');
	}

	return Object(val);
}

function shouldUseNative() {
	try {
		if (!Object.assign) {
			return false;
		}

		// Detect buggy property enumeration order in older V8 versions.

		// https://bugs.chromium.org/p/v8/issues/detail?id=4118
		var test1 = new String('abc');  // eslint-disable-line no-new-wrappers
		test1[5] = 'de';
		if (Object.getOwnPropertyNames(test1)[0] === '5') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test2 = {};
		for (var i = 0; i < 10; i++) {
			test2['_' + String.fromCharCode(i)] = i;
		}
		var order2 = Object.getOwnPropertyNames(test2).map(function (n) {
			return test2[n];
		});
		if (order2.join('') !== '0123456789') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test3 = {};
		'abcdefghijklmnopqrst'.split('').forEach(function (letter) {
			test3[letter] = letter;
		});
		if (Object.keys(Object.assign({}, test3)).join('') !==
				'abcdefghijklmnopqrst') {
			return false;
		}

		return true;
	} catch (err) {
		// We don't expect any of the above to throw, but better to be safe.
		return false;
	}
}

module.exports = shouldUseNative() ? Object.assign : function (target, source) {
	var from;
	var to = toObject(target);
	var symbols;

	for (var s = 1; s < arguments.length; s++) {
		from = Object(arguments[s]);

		for (var key in from) {
			if (hasOwnProperty.call(from, key)) {
				to[key] = from[key];
			}
		}

		if (getOwnPropertySymbols) {
			symbols = getOwnPropertySymbols(from);
			for (var i = 0; i < symbols.length; i++) {
				if (propIsEnumerable.call(from, symbols[i])) {
					to[symbols[i]] = from[symbols[i]];
				}
			}
		}
	}

	return to;
};


/***/ }),

/***/ "./node_modules/prop-types/checkPropTypes.js":
/*!***************************************************!*\
  !*** ./node_modules/prop-types/checkPropTypes.js ***!
  \***************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var printWarning = function() {};

if (true) {
  var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "./node_modules/prop-types/lib/ReactPropTypesSecret.js");
  var loggedTypeFailures = {};
  var has = Function.call.bind(Object.prototype.hasOwnProperty);

  printWarning = function(text) {
    var message = 'Warning: ' + text;
    if (typeof console !== 'undefined') {
      console.error(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };
}

/**
 * Assert that the values match with the type specs.
 * Error messages are memorized and will only be shown once.
 *
 * @param {object} typeSpecs Map of name to a ReactPropType
 * @param {object} values Runtime values that need to be type-checked
 * @param {string} location e.g. "prop", "context", "child context"
 * @param {string} componentName Name of the component for error messages.
 * @param {?Function} getStack Returns the component stack.
 * @private
 */
function checkPropTypes(typeSpecs, values, location, componentName, getStack) {
  if (true) {
    for (var typeSpecName in typeSpecs) {
      if (has(typeSpecs, typeSpecName)) {
        var error;
        // Prop type validation may throw. In case they do, we don't want to
        // fail the render phase where it didn't fail before. So we log it.
        // After these have been cleaned up, we'll let them throw.
        try {
          // This is intentionally an invariant that gets caught. It's the same
          // behavior as without this statement except with a better message.
          if (typeof typeSpecs[typeSpecName] !== 'function') {
            var err = Error(
              (componentName || 'React class') + ': ' + location + ' type `' + typeSpecName + '` is invalid; ' +
              'it must be a function, usually from the `prop-types` package, but received `' + typeof typeSpecs[typeSpecName] + '`.'
            );
            err.name = 'Invariant Violation';
            throw err;
          }
          error = typeSpecs[typeSpecName](values, typeSpecName, componentName, location, null, ReactPropTypesSecret);
        } catch (ex) {
          error = ex;
        }
        if (error && !(error instanceof Error)) {
          printWarning(
            (componentName || 'React class') + ': type specification of ' +
            location + ' `' + typeSpecName + '` is invalid; the type checker ' +
            'function must return `null` or an `Error` but returned a ' + typeof error + '. ' +
            'You may have forgotten to pass an argument to the type checker ' +
            'creator (arrayOf, instanceOf, objectOf, oneOf, oneOfType, and ' +
            'shape all require an argument).'
          );
        }
        if (error instanceof Error && !(error.message in loggedTypeFailures)) {
          // Only monitor this failure once because there tends to be a lot of the
          // same error.
          loggedTypeFailures[error.message] = true;

          var stack = getStack ? getStack() : '';

          printWarning(
            'Failed ' + location + ' type: ' + error.message + (stack != null ? stack : '')
          );
        }
      }
    }
  }
}

/**
 * Resets warning cache when testing.
 *
 * @private
 */
checkPropTypes.resetWarningCache = function() {
  if (true) {
    loggedTypeFailures = {};
  }
}

module.exports = checkPropTypes;


/***/ }),

/***/ "./node_modules/prop-types/factoryWithTypeCheckers.js":
/*!************************************************************!*\
  !*** ./node_modules/prop-types/factoryWithTypeCheckers.js ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactIs = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");
var assign = __webpack_require__(/*! object-assign */ "./node_modules/object-assign/index.js");

var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "./node_modules/prop-types/lib/ReactPropTypesSecret.js");
var checkPropTypes = __webpack_require__(/*! ./checkPropTypes */ "./node_modules/prop-types/checkPropTypes.js");

var has = Function.call.bind(Object.prototype.hasOwnProperty);
var printWarning = function() {};

if (true) {
  printWarning = function(text) {
    var message = 'Warning: ' + text;
    if (typeof console !== 'undefined') {
      console.error(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };
}

function emptyFunctionThatReturnsNull() {
  return null;
}

module.exports = function(isValidElement, throwOnDirectAccess) {
  /* global Symbol */
  var ITERATOR_SYMBOL = typeof Symbol === 'function' && Symbol.iterator;
  var FAUX_ITERATOR_SYMBOL = '@@iterator'; // Before Symbol spec.

  /**
   * Returns the iterator method function contained on the iterable object.
   *
   * Be sure to invoke the function with the iterable as context:
   *
   *     var iteratorFn = getIteratorFn(myIterable);
   *     if (iteratorFn) {
   *       var iterator = iteratorFn.call(myIterable);
   *       ...
   *     }
   *
   * @param {?object} maybeIterable
   * @return {?function}
   */
  function getIteratorFn(maybeIterable) {
    var iteratorFn = maybeIterable && (ITERATOR_SYMBOL && maybeIterable[ITERATOR_SYMBOL] || maybeIterable[FAUX_ITERATOR_SYMBOL]);
    if (typeof iteratorFn === 'function') {
      return iteratorFn;
    }
  }

  /**
   * Collection of methods that allow declaration and validation of props that are
   * supplied to React components. Example usage:
   *
   *   var Props = require('ReactPropTypes');
   *   var MyArticle = React.createClass({
   *     propTypes: {
   *       // An optional string prop named "description".
   *       description: Props.string,
   *
   *       // A required enum prop named "category".
   *       category: Props.oneOf(['News','Photos']).isRequired,
   *
   *       // A prop named "dialog" that requires an instance of Dialog.
   *       dialog: Props.instanceOf(Dialog).isRequired
   *     },
   *     render: function() { ... }
   *   });
   *
   * A more formal specification of how these methods are used:
   *
   *   type := array|bool|func|object|number|string|oneOf([...])|instanceOf(...)
   *   decl := ReactPropTypes.{type}(.isRequired)?
   *
   * Each and every declaration produces a function with the same signature. This
   * allows the creation of custom validation functions. For example:
   *
   *  var MyLink = React.createClass({
   *    propTypes: {
   *      // An optional string or URI prop named "href".
   *      href: function(props, propName, componentName) {
   *        var propValue = props[propName];
   *        if (propValue != null && typeof propValue !== 'string' &&
   *            !(propValue instanceof URI)) {
   *          return new Error(
   *            'Expected a string or an URI for ' + propName + ' in ' +
   *            componentName
   *          );
   *        }
   *      }
   *    },
   *    render: function() {...}
   *  });
   *
   * @internal
   */

  var ANONYMOUS = '<<anonymous>>';

  // Important!
  // Keep this list in sync with production version in `./factoryWithThrowingShims.js`.
  var ReactPropTypes = {
    array: createPrimitiveTypeChecker('array'),
    bool: createPrimitiveTypeChecker('boolean'),
    func: createPrimitiveTypeChecker('function'),
    number: createPrimitiveTypeChecker('number'),
    object: createPrimitiveTypeChecker('object'),
    string: createPrimitiveTypeChecker('string'),
    symbol: createPrimitiveTypeChecker('symbol'),

    any: createAnyTypeChecker(),
    arrayOf: createArrayOfTypeChecker,
    element: createElementTypeChecker(),
    elementType: createElementTypeTypeChecker(),
    instanceOf: createInstanceTypeChecker,
    node: createNodeChecker(),
    objectOf: createObjectOfTypeChecker,
    oneOf: createEnumTypeChecker,
    oneOfType: createUnionTypeChecker,
    shape: createShapeTypeChecker,
    exact: createStrictShapeTypeChecker,
  };

  /**
   * inlined Object.is polyfill to avoid requiring consumers ship their own
   * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/is
   */
  /*eslint-disable no-self-compare*/
  function is(x, y) {
    // SameValue algorithm
    if (x === y) {
      // Steps 1-5, 7-10
      // Steps 6.b-6.e: +0 != -0
      return x !== 0 || 1 / x === 1 / y;
    } else {
      // Step 6.a: NaN == NaN
      return x !== x && y !== y;
    }
  }
  /*eslint-enable no-self-compare*/

  /**
   * We use an Error-like object for backward compatibility as people may call
   * PropTypes directly and inspect their output. However, we don't use real
   * Errors anymore. We don't inspect their stack anyway, and creating them
   * is prohibitively expensive if they are created too often, such as what
   * happens in oneOfType() for any type before the one that matched.
   */
  function PropTypeError(message) {
    this.message = message;
    this.stack = '';
  }
  // Make `instanceof Error` still work for returned errors.
  PropTypeError.prototype = Error.prototype;

  function createChainableTypeChecker(validate) {
    if (true) {
      var manualPropTypeCallCache = {};
      var manualPropTypeWarningCount = 0;
    }
    function checkType(isRequired, props, propName, componentName, location, propFullName, secret) {
      componentName = componentName || ANONYMOUS;
      propFullName = propFullName || propName;

      if (secret !== ReactPropTypesSecret) {
        if (throwOnDirectAccess) {
          // New behavior only for users of `prop-types` package
          var err = new Error(
            'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
            'Use `PropTypes.checkPropTypes()` to call them. ' +
            'Read more at http://fb.me/use-check-prop-types'
          );
          err.name = 'Invariant Violation';
          throw err;
        } else if ( true && typeof console !== 'undefined') {
          // Old behavior for people using React.PropTypes
          var cacheKey = componentName + ':' + propName;
          if (
            !manualPropTypeCallCache[cacheKey] &&
            // Avoid spamming the console because they are often not actionable except for lib authors
            manualPropTypeWarningCount < 3
          ) {
            printWarning(
              'You are manually calling a React.PropTypes validation ' +
              'function for the `' + propFullName + '` prop on `' + componentName  + '`. This is deprecated ' +
              'and will throw in the standalone `prop-types` package. ' +
              'You may be seeing this warning due to a third-party PropTypes ' +
              'library. See https://fb.me/react-warning-dont-call-proptypes ' + 'for details.'
            );
            manualPropTypeCallCache[cacheKey] = true;
            manualPropTypeWarningCount++;
          }
        }
      }
      if (props[propName] == null) {
        if (isRequired) {
          if (props[propName] === null) {
            return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required ' + ('in `' + componentName + '`, but its value is `null`.'));
          }
          return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required in ' + ('`' + componentName + '`, but its value is `undefined`.'));
        }
        return null;
      } else {
        return validate(props, propName, componentName, location, propFullName);
      }
    }

    var chainedCheckType = checkType.bind(null, false);
    chainedCheckType.isRequired = checkType.bind(null, true);

    return chainedCheckType;
  }

  function createPrimitiveTypeChecker(expectedType) {
    function validate(props, propName, componentName, location, propFullName, secret) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== expectedType) {
        // `propValue` being instance of, say, date/regexp, pass the 'object'
        // check, but we can offer a more precise error message here rather than
        // 'of type `object`'.
        var preciseType = getPreciseType(propValue);

        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + preciseType + '` supplied to `' + componentName + '`, expected ') + ('`' + expectedType + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createAnyTypeChecker() {
    return createChainableTypeChecker(emptyFunctionThatReturnsNull);
  }

  function createArrayOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside arrayOf.');
      }
      var propValue = props[propName];
      if (!Array.isArray(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an array.'));
      }
      for (var i = 0; i < propValue.length; i++) {
        var error = typeChecker(propValue, i, componentName, location, propFullName + '[' + i + ']', ReactPropTypesSecret);
        if (error instanceof Error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createElementTypeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      if (!isValidElement(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected a single ReactElement.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createElementTypeTypeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      if (!ReactIs.isValidElementType(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected a single ReactElement type.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createInstanceTypeChecker(expectedClass) {
    function validate(props, propName, componentName, location, propFullName) {
      if (!(props[propName] instanceof expectedClass)) {
        var expectedClassName = expectedClass.name || ANONYMOUS;
        var actualClassName = getClassName(props[propName]);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + actualClassName + '` supplied to `' + componentName + '`, expected ') + ('instance of `' + expectedClassName + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createEnumTypeChecker(expectedValues) {
    if (!Array.isArray(expectedValues)) {
      if (true) {
        if (arguments.length > 1) {
          printWarning(
            'Invalid arguments supplied to oneOf, expected an array, got ' + arguments.length + ' arguments. ' +
            'A common mistake is to write oneOf(x, y, z) instead of oneOf([x, y, z]).'
          );
        } else {
          printWarning('Invalid argument supplied to oneOf, expected an array.');
        }
      }
      return emptyFunctionThatReturnsNull;
    }

    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      for (var i = 0; i < expectedValues.length; i++) {
        if (is(propValue, expectedValues[i])) {
          return null;
        }
      }

      var valuesString = JSON.stringify(expectedValues, function replacer(key, value) {
        var type = getPreciseType(value);
        if (type === 'symbol') {
          return String(value);
        }
        return value;
      });
      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of value `' + String(propValue) + '` ' + ('supplied to `' + componentName + '`, expected one of ' + valuesString + '.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createObjectOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside objectOf.');
      }
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an object.'));
      }
      for (var key in propValue) {
        if (has(propValue, key)) {
          var error = typeChecker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
          if (error instanceof Error) {
            return error;
          }
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createUnionTypeChecker(arrayOfTypeCheckers) {
    if (!Array.isArray(arrayOfTypeCheckers)) {
       true ? printWarning('Invalid argument supplied to oneOfType, expected an instance of array.') : undefined;
      return emptyFunctionThatReturnsNull;
    }

    for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
      var checker = arrayOfTypeCheckers[i];
      if (typeof checker !== 'function') {
        printWarning(
          'Invalid argument supplied to oneOfType. Expected an array of check functions, but ' +
          'received ' + getPostfixForTypeWarning(checker) + ' at index ' + i + '.'
        );
        return emptyFunctionThatReturnsNull;
      }
    }

    function validate(props, propName, componentName, location, propFullName) {
      for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
        var checker = arrayOfTypeCheckers[i];
        if (checker(props, propName, componentName, location, propFullName, ReactPropTypesSecret) == null) {
          return null;
        }
      }

      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createNodeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      if (!isNode(props[propName])) {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`, expected a ReactNode.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      for (var key in shapeTypes) {
        var checker = shapeTypes[key];
        if (!checker) {
          continue;
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createStrictShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      // We need to check all keys in case some are required but missing from
      // props.
      var allKeys = assign({}, props[propName], shapeTypes);
      for (var key in allKeys) {
        var checker = shapeTypes[key];
        if (!checker) {
          return new PropTypeError(
            'Invalid ' + location + ' `' + propFullName + '` key `' + key + '` supplied to `' + componentName + '`.' +
            '\nBad object: ' + JSON.stringify(props[propName], null, '  ') +
            '\nValid keys: ' +  JSON.stringify(Object.keys(shapeTypes), null, '  ')
          );
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }

    return createChainableTypeChecker(validate);
  }

  function isNode(propValue) {
    switch (typeof propValue) {
      case 'number':
      case 'string':
      case 'undefined':
        return true;
      case 'boolean':
        return !propValue;
      case 'object':
        if (Array.isArray(propValue)) {
          return propValue.every(isNode);
        }
        if (propValue === null || isValidElement(propValue)) {
          return true;
        }

        var iteratorFn = getIteratorFn(propValue);
        if (iteratorFn) {
          var iterator = iteratorFn.call(propValue);
          var step;
          if (iteratorFn !== propValue.entries) {
            while (!(step = iterator.next()).done) {
              if (!isNode(step.value)) {
                return false;
              }
            }
          } else {
            // Iterator will provide entry [k,v] tuples rather than values.
            while (!(step = iterator.next()).done) {
              var entry = step.value;
              if (entry) {
                if (!isNode(entry[1])) {
                  return false;
                }
              }
            }
          }
        } else {
          return false;
        }

        return true;
      default:
        return false;
    }
  }

  function isSymbol(propType, propValue) {
    // Native Symbol.
    if (propType === 'symbol') {
      return true;
    }

    // falsy value can't be a Symbol
    if (!propValue) {
      return false;
    }

    // 19.4.3.5 Symbol.prototype[@@toStringTag] === 'Symbol'
    if (propValue['@@toStringTag'] === 'Symbol') {
      return true;
    }

    // Fallback for non-spec compliant Symbols which are polyfilled.
    if (typeof Symbol === 'function' && propValue instanceof Symbol) {
      return true;
    }

    return false;
  }

  // Equivalent of `typeof` but with special handling for array and regexp.
  function getPropType(propValue) {
    var propType = typeof propValue;
    if (Array.isArray(propValue)) {
      return 'array';
    }
    if (propValue instanceof RegExp) {
      // Old webkits (at least until Android 4.0) return 'function' rather than
      // 'object' for typeof a RegExp. We'll normalize this here so that /bla/
      // passes PropTypes.object.
      return 'object';
    }
    if (isSymbol(propType, propValue)) {
      return 'symbol';
    }
    return propType;
  }

  // This handles more types than `getPropType`. Only used for error messages.
  // See `createPrimitiveTypeChecker`.
  function getPreciseType(propValue) {
    if (typeof propValue === 'undefined' || propValue === null) {
      return '' + propValue;
    }
    var propType = getPropType(propValue);
    if (propType === 'object') {
      if (propValue instanceof Date) {
        return 'date';
      } else if (propValue instanceof RegExp) {
        return 'regexp';
      }
    }
    return propType;
  }

  // Returns a string that is postfixed to a warning about an invalid type.
  // For example, "undefined" or "of type array"
  function getPostfixForTypeWarning(value) {
    var type = getPreciseType(value);
    switch (type) {
      case 'array':
      case 'object':
        return 'an ' + type;
      case 'boolean':
      case 'date':
      case 'regexp':
        return 'a ' + type;
      default:
        return type;
    }
  }

  // Returns class name of the object, if any.
  function getClassName(propValue) {
    if (!propValue.constructor || !propValue.constructor.name) {
      return ANONYMOUS;
    }
    return propValue.constructor.name;
  }

  ReactPropTypes.checkPropTypes = checkPropTypes;
  ReactPropTypes.resetWarningCache = checkPropTypes.resetWarningCache;
  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),

/***/ "./node_modules/prop-types/index.js":
/*!******************************************!*\
  !*** ./node_modules/prop-types/index.js ***!
  \******************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (true) {
  var ReactIs = __webpack_require__(/*! react-is */ "./node_modules/react-is/index.js");

  // By explicitly using `prop-types` you are opting into new development behavior.
  // http://fb.me/prop-types-in-prod
  var throwOnDirectAccess = true;
  module.exports = __webpack_require__(/*! ./factoryWithTypeCheckers */ "./node_modules/prop-types/factoryWithTypeCheckers.js")(ReactIs.isElement, throwOnDirectAccess);
} else {}


/***/ }),

/***/ "./node_modules/prop-types/lib/ReactPropTypesSecret.js":
/*!*************************************************************!*\
  !*** ./node_modules/prop-types/lib/ReactPropTypesSecret.js ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),

/***/ "./node_modules/react-dom/cjs/react-dom-server.browser.development.js":
/*!****************************************************************************!*\
  !*** ./node_modules/react-dom/cjs/react-dom-server.browser.development.js ***!
  \****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v16.8.6
 * react-dom-server.browser.development.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */





if (true) {
  (function() {
'use strict';

var _assign = __webpack_require__(/*! object-assign */ "./node_modules/object-assign/index.js");
var React = __webpack_require__(/*! react */ "react");
var checkPropTypes = __webpack_require__(/*! prop-types/checkPropTypes */ "./node_modules/prop-types/checkPropTypes.js");

/**
 * Use invariant() to assert state which your program assumes to be true.
 *
 * Provide sprintf-style format (only %s is supported) and arguments
 * to provide information about what broke and what you were
 * expecting.
 *
 * The invariant message will be stripped in production, but the invariant
 * will remain to ensure logic does not differ in production.
 */

var validateFormat = function () {};

{
  validateFormat = function (format) {
    if (format === undefined) {
      throw new Error('invariant requires an error message argument');
    }
  };
}

function invariant(condition, format, a, b, c, d, e, f) {
  validateFormat(format);

  if (!condition) {
    var error = void 0;
    if (format === undefined) {
      error = new Error('Minified exception occurred; use the non-minified dev environment ' + 'for the full error message and additional helpful warnings.');
    } else {
      var args = [a, b, c, d, e, f];
      var argIndex = 0;
      error = new Error(format.replace(/%s/g, function () {
        return args[argIndex++];
      }));
      error.name = 'Invariant Violation';
    }

    error.framesToPop = 1; // we don't care about invariant's own frame
    throw error;
  }
}

// Relying on the `invariant()` implementation lets us
// preserve the format and params in the www builds.

// TODO: this is special because it gets imported during build.

var ReactVersion = '16.8.6';

/**
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var warningWithoutStack = function () {};

{
  warningWithoutStack = function (condition, format) {
    for (var _len = arguments.length, args = Array(_len > 2 ? _len - 2 : 0), _key = 2; _key < _len; _key++) {
      args[_key - 2] = arguments[_key];
    }

    if (format === undefined) {
      throw new Error('`warningWithoutStack(condition, format, ...args)` requires a warning ' + 'message argument');
    }
    if (args.length > 8) {
      // Check before the condition to catch violations early.
      throw new Error('warningWithoutStack() currently supports at most 8 arguments.');
    }
    if (condition) {
      return;
    }
    if (typeof console !== 'undefined') {
      var argsWithFormat = args.map(function (item) {
        return '' + item;
      });
      argsWithFormat.unshift('Warning: ' + format);

      // We intentionally don't use spread (or .apply) directly because it
      // breaks IE9: https://github.com/facebook/react/issues/13610
      Function.prototype.apply.call(console.error, console, argsWithFormat);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      var argIndex = 0;
      var message = 'Warning: ' + format.replace(/%s/g, function () {
        return args[argIndex++];
      });
      throw new Error(message);
    } catch (x) {}
  };
}

var warningWithoutStack$1 = warningWithoutStack;

// The Symbol used to tag the ReactElement-like types. If there is no native Symbol
// nor polyfill, then a plain number is used for performance.
var hasSymbol = typeof Symbol === 'function' && Symbol.for;


var REACT_PORTAL_TYPE = hasSymbol ? Symbol.for('react.portal') : 0xeaca;
var REACT_FRAGMENT_TYPE = hasSymbol ? Symbol.for('react.fragment') : 0xeacb;
var REACT_STRICT_MODE_TYPE = hasSymbol ? Symbol.for('react.strict_mode') : 0xeacc;
var REACT_PROFILER_TYPE = hasSymbol ? Symbol.for('react.profiler') : 0xead2;
var REACT_PROVIDER_TYPE = hasSymbol ? Symbol.for('react.provider') : 0xeacd;
var REACT_CONTEXT_TYPE = hasSymbol ? Symbol.for('react.context') : 0xeace;

var REACT_CONCURRENT_MODE_TYPE = hasSymbol ? Symbol.for('react.concurrent_mode') : 0xeacf;
var REACT_FORWARD_REF_TYPE = hasSymbol ? Symbol.for('react.forward_ref') : 0xead0;
var REACT_SUSPENSE_TYPE = hasSymbol ? Symbol.for('react.suspense') : 0xead1;
var REACT_MEMO_TYPE = hasSymbol ? Symbol.for('react.memo') : 0xead3;
var REACT_LAZY_TYPE = hasSymbol ? Symbol.for('react.lazy') : 0xead4;

var Resolved = 1;


function refineResolvedLazyComponent(lazyComponent) {
  return lazyComponent._status === Resolved ? lazyComponent._result : null;
}

function getWrappedName(outerType, innerType, wrapperName) {
  var functionName = innerType.displayName || innerType.name || '';
  return outerType.displayName || (functionName !== '' ? wrapperName + '(' + functionName + ')' : wrapperName);
}

function getComponentName(type) {
  if (type == null) {
    // Host root, text node or just invalid type.
    return null;
  }
  {
    if (typeof type.tag === 'number') {
      warningWithoutStack$1(false, 'Received an unexpected object in getComponentName(). ' + 'This is likely a bug in React. Please file an issue.');
    }
  }
  if (typeof type === 'function') {
    return type.displayName || type.name || null;
  }
  if (typeof type === 'string') {
    return type;
  }
  switch (type) {
    case REACT_CONCURRENT_MODE_TYPE:
      return 'ConcurrentMode';
    case REACT_FRAGMENT_TYPE:
      return 'Fragment';
    case REACT_PORTAL_TYPE:
      return 'Portal';
    case REACT_PROFILER_TYPE:
      return 'Profiler';
    case REACT_STRICT_MODE_TYPE:
      return 'StrictMode';
    case REACT_SUSPENSE_TYPE:
      return 'Suspense';
  }
  if (typeof type === 'object') {
    switch (type.$$typeof) {
      case REACT_CONTEXT_TYPE:
        return 'Context.Consumer';
      case REACT_PROVIDER_TYPE:
        return 'Context.Provider';
      case REACT_FORWARD_REF_TYPE:
        return getWrappedName(type, type.render, 'ForwardRef');
      case REACT_MEMO_TYPE:
        return getComponentName(type.type);
      case REACT_LAZY_TYPE:
        {
          var thenable = type;
          var resolvedThenable = refineResolvedLazyComponent(thenable);
          if (resolvedThenable) {
            return getComponentName(resolvedThenable);
          }
        }
    }
  }
  return null;
}

/**
 * Forked from fbjs/warning:
 * https://github.com/facebook/fbjs/blob/e66ba20ad5be433eb54423f2b097d829324d9de6/packages/fbjs/src/__forks__/warning.js
 *
 * Only change is we use console.warn instead of console.error,
 * and do nothing when 'console' is not supported.
 * This really simplifies the code.
 * ---
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var lowPriorityWarning = function () {};

{
  var printWarning = function (format) {
    for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      args[_key - 1] = arguments[_key];
    }

    var argIndex = 0;
    var message = 'Warning: ' + format.replace(/%s/g, function () {
      return args[argIndex++];
    });
    if (typeof console !== 'undefined') {
      console.warn(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };

  lowPriorityWarning = function (condition, format) {
    if (format === undefined) {
      throw new Error('`lowPriorityWarning(condition, format, ...args)` requires a warning ' + 'message argument');
    }
    if (!condition) {
      for (var _len2 = arguments.length, args = Array(_len2 > 2 ? _len2 - 2 : 0), _key2 = 2; _key2 < _len2; _key2++) {
        args[_key2 - 2] = arguments[_key2];
      }

      printWarning.apply(undefined, [format].concat(args));
    }
  };
}

var lowPriorityWarning$1 = lowPriorityWarning;

var ReactSharedInternals = React.__SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED;

// Prevent newer renderers from RTE when used with older react package versions.
// Current owner and dispatcher used to share the same ref,
// but PR #14548 split them out to better support the react-debug-tools package.
if (!ReactSharedInternals.hasOwnProperty('ReactCurrentDispatcher')) {
  ReactSharedInternals.ReactCurrentDispatcher = {
    current: null
  };
}

/**
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var warning = warningWithoutStack$1;

{
  warning = function (condition, format) {
    if (condition) {
      return;
    }
    var ReactDebugCurrentFrame = ReactSharedInternals.ReactDebugCurrentFrame;
    var stack = ReactDebugCurrentFrame.getStackAddendum();
    // eslint-disable-next-line react-internal/warning-and-invariant-args

    for (var _len = arguments.length, args = Array(_len > 2 ? _len - 2 : 0), _key = 2; _key < _len; _key++) {
      args[_key - 2] = arguments[_key];
    }

    warningWithoutStack$1.apply(undefined, [false, format + '%s'].concat(args, [stack]));
  };
}

var warning$1 = warning;

var BEFORE_SLASH_RE = /^(.*)[\\\/]/;

var describeComponentFrame = function (name, source, ownerName) {
  var sourceInfo = '';
  if (source) {
    var path = source.fileName;
    var fileName = path.replace(BEFORE_SLASH_RE, '');
    {
      // In DEV, include code for a common special case:
      // prefer "folder/index.js" instead of just "index.js".
      if (/^index\./.test(fileName)) {
        var match = path.match(BEFORE_SLASH_RE);
        if (match) {
          var pathBeforeSlash = match[1];
          if (pathBeforeSlash) {
            var folderName = pathBeforeSlash.replace(BEFORE_SLASH_RE, '');
            fileName = folderName + '/' + fileName;
          }
        }
      }
    }
    sourceInfo = ' (at ' + fileName + ':' + source.lineNumber + ')';
  } else if (ownerName) {
    sourceInfo = ' (created by ' + ownerName + ')';
  }
  return '\n    in ' + (name || 'Unknown') + sourceInfo;
};

// Helps identify side effects in begin-phase lifecycle hooks and setState reducers:


// In some cases, StrictMode should also double-render lifecycles.
// This can be confusing for tests though,
// And it can be bad for performance in production.
// This feature flag can be used to control the behavior:


// To preserve the "Pause on caught exceptions" behavior of the debugger, we
// replay the begin phase of a failed component inside invokeGuardedCallback.


// Warn about deprecated, async-unsafe lifecycles; relates to RFC #6:
var warnAboutDeprecatedLifecycles = false;

// Gather advanced timing metrics for Profiler subtrees.


// Trace which interactions trigger each commit.


// Only used in www builds.
var enableSuspenseServerRenderer = false; // TODO: true? Here it might just be false.

// Only used in www builds.


// Only used in www builds.


// React Fire: prevent the value and checked attributes from syncing
// with their related DOM properties


// These APIs will no longer be "unstable" in the upcoming 16.7 release,
// Control this behavior with a flag to support 16.6 minor releases in the meanwhile.

var ReactDebugCurrentFrame$1 = void 0;
var didWarnAboutInvalidateContextType = void 0;
{
  ReactDebugCurrentFrame$1 = ReactSharedInternals.ReactDebugCurrentFrame;
  didWarnAboutInvalidateContextType = new Set();
}

var emptyObject = {};
{
  Object.freeze(emptyObject);
}

function maskContext(type, context) {
  var contextTypes = type.contextTypes;
  if (!contextTypes) {
    return emptyObject;
  }
  var maskedContext = {};
  for (var contextName in contextTypes) {
    maskedContext[contextName] = context[contextName];
  }
  return maskedContext;
}

function checkContextTypes(typeSpecs, values, location) {
  {
    checkPropTypes(typeSpecs, values, location, 'Component', ReactDebugCurrentFrame$1.getCurrentStack);
  }
}

function validateContextBounds(context, threadID) {
  // If we don't have enough slots in this context to store this threadID,
  // fill it in without leaving any holes to ensure that the VM optimizes
  // this as non-holey index properties.
  // (Note: If `react` package is < 16.6, _threadCount is undefined.)
  for (var i = context._threadCount | 0; i <= threadID; i++) {
    // We assume that this is the same as the defaultValue which might not be
    // true if we're rendering inside a secondary renderer but they are
    // secondary because these use cases are very rare.
    context[i] = context._currentValue2;
    context._threadCount = i + 1;
  }
}

function processContext(type, context, threadID) {
  var contextType = type.contextType;
  {
    if ('contextType' in type) {
      var isValid =
      // Allow null for conditional declaration
      contextType === null || contextType !== undefined && contextType.$$typeof === REACT_CONTEXT_TYPE && contextType._context === undefined; // Not a <Context.Consumer>

      if (!isValid && !didWarnAboutInvalidateContextType.has(type)) {
        didWarnAboutInvalidateContextType.add(type);

        var addendum = '';
        if (contextType === undefined) {
          addendum = ' However, it is set to undefined. ' + 'This can be caused by a typo or by mixing up named and default imports. ' + 'This can also happen due to a circular dependency, so ' + 'try moving the createContext() call to a separate file.';
        } else if (typeof contextType !== 'object') {
          addendum = ' However, it is set to a ' + typeof contextType + '.';
        } else if (contextType.$$typeof === REACT_PROVIDER_TYPE) {
          addendum = ' Did you accidentally pass the Context.Provider instead?';
        } else if (contextType._context !== undefined) {
          // <Context.Consumer>
          addendum = ' Did you accidentally pass the Context.Consumer instead?';
        } else {
          addendum = ' However, it is set to an object with keys {' + Object.keys(contextType).join(', ') + '}.';
        }
        warningWithoutStack$1(false, '%s defines an invalid contextType. ' + 'contextType should point to the Context object returned by React.createContext().%s', getComponentName(type) || 'Component', addendum);
      }
    }
  }
  if (typeof contextType === 'object' && contextType !== null) {
    validateContextBounds(contextType, threadID);
    return contextType[threadID];
  } else {
    var maskedContext = maskContext(type, context);
    {
      if (type.contextTypes) {
        checkContextTypes(type.contextTypes, maskedContext, 'context');
      }
    }
    return maskedContext;
  }
}

// Allocates a new index for each request. Tries to stay as compact as possible so that these
// indices can be used to reference a tightly packaged array. As opposed to being used in a Map.
// The first allocated index is 1.

var nextAvailableThreadIDs = new Uint16Array(16);
for (var i = 0; i < 15; i++) {
  nextAvailableThreadIDs[i] = i + 1;
}
nextAvailableThreadIDs[15] = 0;

function growThreadCountAndReturnNextAvailable() {
  var oldArray = nextAvailableThreadIDs;
  var oldSize = oldArray.length;
  var newSize = oldSize * 2;
  !(newSize <= 0x10000) ? invariant(false, 'Maximum number of concurrent React renderers exceeded. This can happen if you are not properly destroying the Readable provided by React. Ensure that you call .destroy() on it if you no longer want to read from it, and did not read to the end. If you use .pipe() this should be automatic.') : void 0;
  var newArray = new Uint16Array(newSize);
  newArray.set(oldArray);
  nextAvailableThreadIDs = newArray;
  nextAvailableThreadIDs[0] = oldSize + 1;
  for (var _i = oldSize; _i < newSize - 1; _i++) {
    nextAvailableThreadIDs[_i] = _i + 1;
  }
  nextAvailableThreadIDs[newSize - 1] = 0;
  return oldSize;
}

function allocThreadID() {
  var nextID = nextAvailableThreadIDs[0];
  if (nextID === 0) {
    return growThreadCountAndReturnNextAvailable();
  }
  nextAvailableThreadIDs[0] = nextAvailableThreadIDs[nextID];
  return nextID;
}

function freeThreadID(id) {
  nextAvailableThreadIDs[id] = nextAvailableThreadIDs[0];
  nextAvailableThreadIDs[0] = id;
}

// A reserved attribute.
// It is handled by React separately and shouldn't be written to the DOM.
var RESERVED = 0;

// A simple string attribute.
// Attributes that aren't in the whitelist are presumed to have this type.
var STRING = 1;

// A string attribute that accepts booleans in React. In HTML, these are called
// "enumerated" attributes with "true" and "false" as possible values.
// When true, it should be set to a "true" string.
// When false, it should be set to a "false" string.
var BOOLEANISH_STRING = 2;

// A real boolean attribute.
// When true, it should be present (set either to an empty string or its name).
// When false, it should be omitted.
var BOOLEAN = 3;

// An attribute that can be used as a flag as well as with a value.
// When true, it should be present (set either to an empty string or its name).
// When false, it should be omitted.
// For any other value, should be present with that value.
var OVERLOADED_BOOLEAN = 4;

// An attribute that must be numeric or parse as a numeric.
// When falsy, it should be removed.
var NUMERIC = 5;

// An attribute that must be positive numeric or parse as a positive numeric.
// When falsy, it should be removed.
var POSITIVE_NUMERIC = 6;

/* eslint-disable max-len */
var ATTRIBUTE_NAME_START_CHAR = ':A-Z_a-z\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u02FF\\u0370-\\u037D\\u037F-\\u1FFF\\u200C-\\u200D\\u2070-\\u218F\\u2C00-\\u2FEF\\u3001-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFFD';
/* eslint-enable max-len */
var ATTRIBUTE_NAME_CHAR = ATTRIBUTE_NAME_START_CHAR + '\\-.0-9\\u00B7\\u0300-\\u036F\\u203F-\\u2040';


var ROOT_ATTRIBUTE_NAME = 'data-reactroot';
var VALID_ATTRIBUTE_NAME_REGEX = new RegExp('^[' + ATTRIBUTE_NAME_START_CHAR + '][' + ATTRIBUTE_NAME_CHAR + ']*$');

var hasOwnProperty$1 = Object.prototype.hasOwnProperty;
var illegalAttributeNameCache = {};
var validatedAttributeNameCache = {};

function isAttributeNameSafe(attributeName) {
  if (hasOwnProperty$1.call(validatedAttributeNameCache, attributeName)) {
    return true;
  }
  if (hasOwnProperty$1.call(illegalAttributeNameCache, attributeName)) {
    return false;
  }
  if (VALID_ATTRIBUTE_NAME_REGEX.test(attributeName)) {
    validatedAttributeNameCache[attributeName] = true;
    return true;
  }
  illegalAttributeNameCache[attributeName] = true;
  {
    warning$1(false, 'Invalid attribute name: `%s`', attributeName);
  }
  return false;
}

function shouldIgnoreAttribute(name, propertyInfo, isCustomComponentTag) {
  if (propertyInfo !== null) {
    return propertyInfo.type === RESERVED;
  }
  if (isCustomComponentTag) {
    return false;
  }
  if (name.length > 2 && (name[0] === 'o' || name[0] === 'O') && (name[1] === 'n' || name[1] === 'N')) {
    return true;
  }
  return false;
}

function shouldRemoveAttributeWithWarning(name, value, propertyInfo, isCustomComponentTag) {
  if (propertyInfo !== null && propertyInfo.type === RESERVED) {
    return false;
  }
  switch (typeof value) {
    case 'function':
    // $FlowIssue symbol is perfectly valid here
    case 'symbol':
      // eslint-disable-line
      return true;
    case 'boolean':
      {
        if (isCustomComponentTag) {
          return false;
        }
        if (propertyInfo !== null) {
          return !propertyInfo.acceptsBooleans;
        } else {
          var prefix = name.toLowerCase().slice(0, 5);
          return prefix !== 'data-' && prefix !== 'aria-';
        }
      }
    default:
      return false;
  }
}

function shouldRemoveAttribute(name, value, propertyInfo, isCustomComponentTag) {
  if (value === null || typeof value === 'undefined') {
    return true;
  }
  if (shouldRemoveAttributeWithWarning(name, value, propertyInfo, isCustomComponentTag)) {
    return true;
  }
  if (isCustomComponentTag) {
    return false;
  }
  if (propertyInfo !== null) {
    switch (propertyInfo.type) {
      case BOOLEAN:
        return !value;
      case OVERLOADED_BOOLEAN:
        return value === false;
      case NUMERIC:
        return isNaN(value);
      case POSITIVE_NUMERIC:
        return isNaN(value) || value < 1;
    }
  }
  return false;
}

function getPropertyInfo(name) {
  return properties.hasOwnProperty(name) ? properties[name] : null;
}

function PropertyInfoRecord(name, type, mustUseProperty, attributeName, attributeNamespace) {
  this.acceptsBooleans = type === BOOLEANISH_STRING || type === BOOLEAN || type === OVERLOADED_BOOLEAN;
  this.attributeName = attributeName;
  this.attributeNamespace = attributeNamespace;
  this.mustUseProperty = mustUseProperty;
  this.propertyName = name;
  this.type = type;
}

// When adding attributes to this list, be sure to also add them to
// the `possibleStandardNames` module to ensure casing and incorrect
// name warnings.
var properties = {};

// These props are reserved by React. They shouldn't be written to the DOM.
['children', 'dangerouslySetInnerHTML',
// TODO: This prevents the assignment of defaultValue to regular
// elements (not just inputs). Now that ReactDOMInput assigns to the
// defaultValue property -- do we need this?
'defaultValue', 'defaultChecked', 'innerHTML', 'suppressContentEditableWarning', 'suppressHydrationWarning', 'style'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, RESERVED, false, // mustUseProperty
  name, // attributeName
  null);
} // attributeNamespace
);

// A few React string attributes have a different name.
// This is a mapping from React prop names to the attribute names.
[['acceptCharset', 'accept-charset'], ['className', 'class'], ['htmlFor', 'for'], ['httpEquiv', 'http-equiv']].forEach(function (_ref) {
  var name = _ref[0],
      attributeName = _ref[1];

  properties[name] = new PropertyInfoRecord(name, STRING, false, // mustUseProperty
  attributeName, // attributeName
  null);
} // attributeNamespace
);

// These are "enumerated" HTML attributes that accept "true" and "false".
// In React, we let users pass `true` and `false` even though technically
// these aren't boolean attributes (they are coerced to strings).
['contentEditable', 'draggable', 'spellCheck', 'value'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, BOOLEANISH_STRING, false, // mustUseProperty
  name.toLowerCase(), // attributeName
  null);
} // attributeNamespace
);

// These are "enumerated" SVG attributes that accept "true" and "false".
// In React, we let users pass `true` and `false` even though technically
// these aren't boolean attributes (they are coerced to strings).
// Since these are SVG attributes, their attribute names are case-sensitive.
['autoReverse', 'externalResourcesRequired', 'focusable', 'preserveAlpha'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, BOOLEANISH_STRING, false, // mustUseProperty
  name, // attributeName
  null);
} // attributeNamespace
);

// These are HTML boolean attributes.
['allowFullScreen', 'async',
// Note: there is a special case that prevents it from being written to the DOM
// on the client side because the browsers are inconsistent. Instead we call focus().
'autoFocus', 'autoPlay', 'controls', 'default', 'defer', 'disabled', 'formNoValidate', 'hidden', 'loop', 'noModule', 'noValidate', 'open', 'playsInline', 'readOnly', 'required', 'reversed', 'scoped', 'seamless',
// Microdata
'itemScope'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, BOOLEAN, false, // mustUseProperty
  name.toLowerCase(), // attributeName
  null);
} // attributeNamespace
);

// These are the few React props that we set as DOM properties
// rather than attributes. These are all booleans.
['checked',
// Note: `option.selected` is not updated if `select.multiple` is
// disabled with `removeAttribute`. We have special logic for handling this.
'multiple', 'muted', 'selected'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, BOOLEAN, true, // mustUseProperty
  name, // attributeName
  null);
} // attributeNamespace
);

// These are HTML attributes that are "overloaded booleans": they behave like
// booleans, but can also accept a string value.
['capture', 'download'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, OVERLOADED_BOOLEAN, false, // mustUseProperty
  name, // attributeName
  null);
} // attributeNamespace
);

// These are HTML attributes that must be positive numbers.
['cols', 'rows', 'size', 'span'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, POSITIVE_NUMERIC, false, // mustUseProperty
  name, // attributeName
  null);
} // attributeNamespace
);

// These are HTML attributes that must be numbers.
['rowSpan', 'start'].forEach(function (name) {
  properties[name] = new PropertyInfoRecord(name, NUMERIC, false, // mustUseProperty
  name.toLowerCase(), // attributeName
  null);
} // attributeNamespace
);

var CAMELIZE = /[\-\:]([a-z])/g;
var capitalize = function (token) {
  return token[1].toUpperCase();
};

// This is a list of all SVG attributes that need special casing, namespacing,
// or boolean value assignment. Regular attributes that just accept strings
// and have the same names are omitted, just like in the HTML whitelist.
// Some of these attributes can be hard to find. This list was created by
// scrapping the MDN documentation.
['accent-height', 'alignment-baseline', 'arabic-form', 'baseline-shift', 'cap-height', 'clip-path', 'clip-rule', 'color-interpolation', 'color-interpolation-filters', 'color-profile', 'color-rendering', 'dominant-baseline', 'enable-background', 'fill-opacity', 'fill-rule', 'flood-color', 'flood-opacity', 'font-family', 'font-size', 'font-size-adjust', 'font-stretch', 'font-style', 'font-variant', 'font-weight', 'glyph-name', 'glyph-orientation-horizontal', 'glyph-orientation-vertical', 'horiz-adv-x', 'horiz-origin-x', 'image-rendering', 'letter-spacing', 'lighting-color', 'marker-end', 'marker-mid', 'marker-start', 'overline-position', 'overline-thickness', 'paint-order', 'panose-1', 'pointer-events', 'rendering-intent', 'shape-rendering', 'stop-color', 'stop-opacity', 'strikethrough-position', 'strikethrough-thickness', 'stroke-dasharray', 'stroke-dashoffset', 'stroke-linecap', 'stroke-linejoin', 'stroke-miterlimit', 'stroke-opacity', 'stroke-width', 'text-anchor', 'text-decoration', 'text-rendering', 'underline-position', 'underline-thickness', 'unicode-bidi', 'unicode-range', 'units-per-em', 'v-alphabetic', 'v-hanging', 'v-ideographic', 'v-mathematical', 'vector-effect', 'vert-adv-y', 'vert-origin-x', 'vert-origin-y', 'word-spacing', 'writing-mode', 'xmlns:xlink', 'x-height'].forEach(function (attributeName) {
  var name = attributeName.replace(CAMELIZE, capitalize);
  properties[name] = new PropertyInfoRecord(name, STRING, false, // mustUseProperty
  attributeName, null);
} // attributeNamespace
);

// String SVG attributes with the xlink namespace.
['xlink:actuate', 'xlink:arcrole', 'xlink:href', 'xlink:role', 'xlink:show', 'xlink:title', 'xlink:type'].forEach(function (attributeName) {
  var name = attributeName.replace(CAMELIZE, capitalize);
  properties[name] = new PropertyInfoRecord(name, STRING, false, // mustUseProperty
  attributeName, 'http://www.w3.org/1999/xlink');
});

// String SVG attributes with the xml namespace.
['xml:base', 'xml:lang', 'xml:space'].forEach(function (attributeName) {
  var name = attributeName.replace(CAMELIZE, capitalize);
  properties[name] = new PropertyInfoRecord(name, STRING, false, // mustUseProperty
  attributeName, 'http://www.w3.org/XML/1998/namespace');
});

// These attribute exists both in HTML and SVG.
// The attribute name is case-sensitive in SVG so we can't just use
// the React name like we do for attributes that exist only in HTML.
['tabIndex', 'crossOrigin'].forEach(function (attributeName) {
  properties[attributeName] = new PropertyInfoRecord(attributeName, STRING, false, // mustUseProperty
  attributeName.toLowerCase(), // attributeName
  null);
} // attributeNamespace
);

// code copied and modified from escape-html
/**
 * Module variables.
 * @private
 */

var matchHtmlRegExp = /["'&<>]/;

/**
 * Escapes special characters and HTML entities in a given html string.
 *
 * @param  {string} string HTML string to escape for later insertion
 * @return {string}
 * @public
 */

function escapeHtml(string) {
  var str = '' + string;
  var match = matchHtmlRegExp.exec(str);

  if (!match) {
    return str;
  }

  var escape = void 0;
  var html = '';
  var index = void 0;
  var lastIndex = 0;

  for (index = match.index; index < str.length; index++) {
    switch (str.charCodeAt(index)) {
      case 34:
        // "
        escape = '&quot;';
        break;
      case 38:
        // &
        escape = '&amp;';
        break;
      case 39:
        // '
        escape = '&#x27;'; // modified from escape-html; used to be '&#39'
        break;
      case 60:
        // <
        escape = '&lt;';
        break;
      case 62:
        // >
        escape = '&gt;';
        break;
      default:
        continue;
    }

    if (lastIndex !== index) {
      html += str.substring(lastIndex, index);
    }

    lastIndex = index + 1;
    html += escape;
  }

  return lastIndex !== index ? html + str.substring(lastIndex, index) : html;
}
// end code copied and modified from escape-html

/**
 * Escapes text to prevent scripting attacks.
 *
 * @param {*} text Text value to escape.
 * @return {string} An escaped string.
 */
function escapeTextForBrowser(text) {
  if (typeof text === 'boolean' || typeof text === 'number') {
    // this shortcircuit helps perf for types that we know will never have
    // special characters, especially given that this function is used often
    // for numeric dom ids.
    return '' + text;
  }
  return escapeHtml(text);
}

/**
 * Escapes attribute value to prevent scripting attacks.
 *
 * @param {*} value Value to escape.
 * @return {string} An escaped string.
 */
function quoteAttributeValueForBrowser(value) {
  return '"' + escapeTextForBrowser(value) + '"';
}

/**
 * Operations for dealing with DOM properties.
 */

/**
 * Creates markup for the ID property.
 *
 * @param {string} id Unescaped ID.
 * @return {string} Markup string.
 */


function createMarkupForRoot() {
  return ROOT_ATTRIBUTE_NAME + '=""';
}

/**
 * Creates markup for a property.
 *
 * @param {string} name
 * @param {*} value
 * @return {?string} Markup string, or null if the property was invalid.
 */
function createMarkupForProperty(name, value) {
  var propertyInfo = getPropertyInfo(name);
  if (name !== 'style' && shouldIgnoreAttribute(name, propertyInfo, false)) {
    return '';
  }
  if (shouldRemoveAttribute(name, value, propertyInfo, false)) {
    return '';
  }
  if (propertyInfo !== null) {
    var attributeName = propertyInfo.attributeName;
    var type = propertyInfo.type;

    if (type === BOOLEAN || type === OVERLOADED_BOOLEAN && value === true) {
      return attributeName + '=""';
    } else {
      return attributeName + '=' + quoteAttributeValueForBrowser(value);
    }
  } else if (isAttributeNameSafe(name)) {
    return name + '=' + quoteAttributeValueForBrowser(value);
  }
  return '';
}

/**
 * Creates markup for a custom property.
 *
 * @param {string} name
 * @param {*} value
 * @return {string} Markup string, or empty string if the property was invalid.
 */
function createMarkupForCustomAttribute(name, value) {
  if (!isAttributeNameSafe(name) || value == null) {
    return '';
  }
  return name + '=' + quoteAttributeValueForBrowser(value);
}

/**
 * inlined Object.is polyfill to avoid requiring consumers ship their own
 * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/is
 */
function is(x, y) {
  return x === y && (x !== 0 || 1 / x === 1 / y) || x !== x && y !== y // eslint-disable-line no-self-compare
  ;
}

var currentlyRenderingComponent = null;
var firstWorkInProgressHook = null;
var workInProgressHook = null;
// Whether the work-in-progress hook is a re-rendered hook
var isReRender = false;
// Whether an update was scheduled during the currently executing render pass.
var didScheduleRenderPhaseUpdate = false;
// Lazily created map of render-phase updates
var renderPhaseUpdates = null;
// Counter to prevent infinite loops.
var numberOfReRenders = 0;
var RE_RENDER_LIMIT = 25;

var isInHookUserCodeInDev = false;

// In DEV, this is the name of the currently executing primitive hook
var currentHookNameInDev = void 0;

function resolveCurrentlyRenderingComponent() {
  !(currentlyRenderingComponent !== null) ? invariant(false, 'Invalid hook call. Hooks can only be called inside of the body of a function component. This could happen for one of the following reasons:\n1. You might have mismatching versions of React and the renderer (such as React DOM)\n2. You might be breaking the Rules of Hooks\n3. You might have more than one copy of React in the same app\nSee https://fb.me/react-invalid-hook-call for tips about how to debug and fix this problem.') : void 0;
  {
    !!isInHookUserCodeInDev ? warning$1(false, 'Do not call Hooks inside useEffect(...), useMemo(...), or other built-in Hooks. ' + 'You can only call Hooks at the top level of your React function. ' + 'For more information, see ' + 'https://fb.me/rules-of-hooks') : void 0;
  }
  return currentlyRenderingComponent;
}

function areHookInputsEqual(nextDeps, prevDeps) {
  if (prevDeps === null) {
    {
      warning$1(false, '%s received a final argument during this render, but not during ' + 'the previous render. Even though the final argument is optional, ' + 'its type cannot change between renders.', currentHookNameInDev);
    }
    return false;
  }

  {
    // Don't bother comparing lengths in prod because these arrays should be
    // passed inline.
    if (nextDeps.length !== prevDeps.length) {
      warning$1(false, 'The final argument passed to %s changed size between renders. The ' + 'order and size of this array must remain constant.\n\n' + 'Previous: %s\n' + 'Incoming: %s', currentHookNameInDev, '[' + nextDeps.join(', ') + ']', '[' + prevDeps.join(', ') + ']');
    }
  }
  for (var i = 0; i < prevDeps.length && i < nextDeps.length; i++) {
    if (is(nextDeps[i], prevDeps[i])) {
      continue;
    }
    return false;
  }
  return true;
}

function createHook() {
  if (numberOfReRenders > 0) {
    invariant(false, 'Rendered more hooks than during the previous render');
  }
  return {
    memoizedState: null,
    queue: null,
    next: null
  };
}

function createWorkInProgressHook() {
  if (workInProgressHook === null) {
    // This is the first hook in the list
    if (firstWorkInProgressHook === null) {
      isReRender = false;
      firstWorkInProgressHook = workInProgressHook = createHook();
    } else {
      // There's already a work-in-progress. Reuse it.
      isReRender = true;
      workInProgressHook = firstWorkInProgressHook;
    }
  } else {
    if (workInProgressHook.next === null) {
      isReRender = false;
      // Append to the end of the list
      workInProgressHook = workInProgressHook.next = createHook();
    } else {
      // There's already a work-in-progress. Reuse it.
      isReRender = true;
      workInProgressHook = workInProgressHook.next;
    }
  }
  return workInProgressHook;
}

function prepareToUseHooks(componentIdentity) {
  currentlyRenderingComponent = componentIdentity;
  {
    isInHookUserCodeInDev = false;
  }

  // The following should have already been reset
  // didScheduleRenderPhaseUpdate = false;
  // firstWorkInProgressHook = null;
  // numberOfReRenders = 0;
  // renderPhaseUpdates = null;
  // workInProgressHook = null;
}

function finishHooks(Component, props, children, refOrContext) {
  // This must be called after every function component to prevent hooks from
  // being used in classes.

  while (didScheduleRenderPhaseUpdate) {
    // Updates were scheduled during the render phase. They are stored in
    // the `renderPhaseUpdates` map. Call the component again, reusing the
    // work-in-progress hooks and applying the additional updates on top. Keep
    // restarting until no more updates are scheduled.
    didScheduleRenderPhaseUpdate = false;
    numberOfReRenders += 1;

    // Start over from the beginning of the list
    workInProgressHook = null;

    children = Component(props, refOrContext);
  }
  currentlyRenderingComponent = null;
  firstWorkInProgressHook = null;
  numberOfReRenders = 0;
  renderPhaseUpdates = null;
  workInProgressHook = null;
  {
    isInHookUserCodeInDev = false;
  }

  // These were reset above
  // currentlyRenderingComponent = null;
  // didScheduleRenderPhaseUpdate = false;
  // firstWorkInProgressHook = null;
  // numberOfReRenders = 0;
  // renderPhaseUpdates = null;
  // workInProgressHook = null;

  return children;
}

function readContext(context, observedBits) {
  var threadID = currentThreadID;
  validateContextBounds(context, threadID);
  {
    !!isInHookUserCodeInDev ? warning$1(false, 'Context can only be read while React is rendering. ' + 'In classes, you can read it in the render method or getDerivedStateFromProps. ' + 'In function components, you can read it directly in the function body, but not ' + 'inside Hooks like useReducer() or useMemo().') : void 0;
  }
  return context[threadID];
}

function useContext(context, observedBits) {
  {
    currentHookNameInDev = 'useContext';
  }
  resolveCurrentlyRenderingComponent();
  var threadID = currentThreadID;
  validateContextBounds(context, threadID);
  return context[threadID];
}

function basicStateReducer(state, action) {
  return typeof action === 'function' ? action(state) : action;
}

function useState(initialState) {
  {
    currentHookNameInDev = 'useState';
  }
  return useReducer(basicStateReducer,
  // useReducer has a special case to support lazy useState initializers
  initialState);
}

function useReducer(reducer, initialArg, init) {
  {
    if (reducer !== basicStateReducer) {
      currentHookNameInDev = 'useReducer';
    }
  }
  currentlyRenderingComponent = resolveCurrentlyRenderingComponent();
  workInProgressHook = createWorkInProgressHook();
  if (isReRender) {
    // This is a re-render. Apply the new render phase updates to the previous
    var _queue = workInProgressHook.queue;
    var _dispatch = _queue.dispatch;
    if (renderPhaseUpdates !== null) {
      // Render phase updates are stored in a map of queue -> linked list
      var firstRenderPhaseUpdate = renderPhaseUpdates.get(_queue);
      if (firstRenderPhaseUpdate !== undefined) {
        renderPhaseUpdates.delete(_queue);
        var newState = workInProgressHook.memoizedState;
        var update = firstRenderPhaseUpdate;
        do {
          // Process this render phase update. We don't have to check the
          // priority because it will always be the same as the current
          // render's.
          var _action = update.action;
          {
            isInHookUserCodeInDev = true;
          }
          newState = reducer(newState, _action);
          {
            isInHookUserCodeInDev = false;
          }
          update = update.next;
        } while (update !== null);

        workInProgressHook.memoizedState = newState;

        return [newState, _dispatch];
      }
    }
    return [workInProgressHook.memoizedState, _dispatch];
  } else {
    {
      isInHookUserCodeInDev = true;
    }
    var initialState = void 0;
    if (reducer === basicStateReducer) {
      // Special case for `useState`.
      initialState = typeof initialArg === 'function' ? initialArg() : initialArg;
    } else {
      initialState = init !== undefined ? init(initialArg) : initialArg;
    }
    {
      isInHookUserCodeInDev = false;
    }
    workInProgressHook.memoizedState = initialState;
    var _queue2 = workInProgressHook.queue = {
      last: null,
      dispatch: null
    };
    var _dispatch2 = _queue2.dispatch = dispatchAction.bind(null, currentlyRenderingComponent, _queue2);
    return [workInProgressHook.memoizedState, _dispatch2];
  }
}

function useMemo(nextCreate, deps) {
  currentlyRenderingComponent = resolveCurrentlyRenderingComponent();
  workInProgressHook = createWorkInProgressHook();

  var nextDeps = deps === undefined ? null : deps;

  if (workInProgressHook !== null) {
    var prevState = workInProgressHook.memoizedState;
    if (prevState !== null) {
      if (nextDeps !== null) {
        var prevDeps = prevState[1];
        if (areHookInputsEqual(nextDeps, prevDeps)) {
          return prevState[0];
        }
      }
    }
  }

  {
    isInHookUserCodeInDev = true;
  }
  var nextValue = nextCreate();
  {
    isInHookUserCodeInDev = false;
  }
  workInProgressHook.memoizedState = [nextValue, nextDeps];
  return nextValue;
}

function useRef(initialValue) {
  currentlyRenderingComponent = resolveCurrentlyRenderingComponent();
  workInProgressHook = createWorkInProgressHook();
  var previousRef = workInProgressHook.memoizedState;
  if (previousRef === null) {
    var ref = { current: initialValue };
    {
      Object.seal(ref);
    }
    workInProgressHook.memoizedState = ref;
    return ref;
  } else {
    return previousRef;
  }
}

function useLayoutEffect(create, inputs) {
  {
    currentHookNameInDev = 'useLayoutEffect';
  }
  warning$1(false, 'useLayoutEffect does nothing on the server, because its effect cannot ' + "be encoded into the server renderer's output format. This will lead " + 'to a mismatch between the initial, non-hydrated UI and the intended ' + 'UI. To avoid this, useLayoutEffect should only be used in ' + 'components that render exclusively on the client. ' + 'See https://fb.me/react-uselayouteffect-ssr for common fixes.');
}

function dispatchAction(componentIdentity, queue, action) {
  !(numberOfReRenders < RE_RENDER_LIMIT) ? invariant(false, 'Too many re-renders. React limits the number of renders to prevent an infinite loop.') : void 0;

  if (componentIdentity === currentlyRenderingComponent) {
    // This is a render phase update. Stash it in a lazily-created map of
    // queue -> linked list of updates. After this render pass, we'll restart
    // and apply the stashed updates on top of the work-in-progress hook.
    didScheduleRenderPhaseUpdate = true;
    var update = {
      action: action,
      next: null
    };
    if (renderPhaseUpdates === null) {
      renderPhaseUpdates = new Map();
    }
    var firstRenderPhaseUpdate = renderPhaseUpdates.get(queue);
    if (firstRenderPhaseUpdate === undefined) {
      renderPhaseUpdates.set(queue, update);
    } else {
      // Append the update to the end of the list.
      var lastRenderPhaseUpdate = firstRenderPhaseUpdate;
      while (lastRenderPhaseUpdate.next !== null) {
        lastRenderPhaseUpdate = lastRenderPhaseUpdate.next;
      }
      lastRenderPhaseUpdate.next = update;
    }
  } else {
    // This means an update has happened after the function component has
    // returned. On the server this is a no-op. In React Fiber, the update
    // would be scheduled for a future render.
  }
}

function useCallback(callback, deps) {
  // Callbacks are passed as they are in the server environment.
  return callback;
}

function noop() {}

var currentThreadID = 0;

function setCurrentThreadID(threadID) {
  currentThreadID = threadID;
}

var Dispatcher = {
  readContext: readContext,
  useContext: useContext,
  useMemo: useMemo,
  useReducer: useReducer,
  useRef: useRef,
  useState: useState,
  useLayoutEffect: useLayoutEffect,
  useCallback: useCallback,
  // useImperativeHandle is not run in the server environment
  useImperativeHandle: noop,
  // Effects are not run in the server environment.
  useEffect: noop,
  // Debugging effect
  useDebugValue: noop
};

var HTML_NAMESPACE = 'http://www.w3.org/1999/xhtml';
var MATH_NAMESPACE = 'http://www.w3.org/1998/Math/MathML';
var SVG_NAMESPACE = 'http://www.w3.org/2000/svg';

var Namespaces = {
  html: HTML_NAMESPACE,
  mathml: MATH_NAMESPACE,
  svg: SVG_NAMESPACE
};

// Assumes there is no parent namespace.
function getIntrinsicNamespace(type) {
  switch (type) {
    case 'svg':
      return SVG_NAMESPACE;
    case 'math':
      return MATH_NAMESPACE;
    default:
      return HTML_NAMESPACE;
  }
}

function getChildNamespace(parentNamespace, type) {
  if (parentNamespace == null || parentNamespace === HTML_NAMESPACE) {
    // No (or default) parent namespace: potential entry point.
    return getIntrinsicNamespace(type);
  }
  if (parentNamespace === SVG_NAMESPACE && type === 'foreignObject') {
    // We're leaving SVG.
    return HTML_NAMESPACE;
  }
  // By default, pass namespace below.
  return parentNamespace;
}

var ReactDebugCurrentFrame$2 = null;

var ReactControlledValuePropTypes = {
  checkPropTypes: null
};

{
  ReactDebugCurrentFrame$2 = ReactSharedInternals.ReactDebugCurrentFrame;

  var hasReadOnlyValue = {
    button: true,
    checkbox: true,
    image: true,
    hidden: true,
    radio: true,
    reset: true,
    submit: true
  };

  var propTypes = {
    value: function (props, propName, componentName) {
      if (hasReadOnlyValue[props.type] || props.onChange || props.readOnly || props.disabled || props[propName] == null) {
        return null;
      }
      return new Error('You provided a `value` prop to a form field without an ' + '`onChange` handler. This will render a read-only field. If ' + 'the field should be mutable use `defaultValue`. Otherwise, ' + 'set either `onChange` or `readOnly`.');
    },
    checked: function (props, propName, componentName) {
      if (props.onChange || props.readOnly || props.disabled || props[propName] == null) {
        return null;
      }
      return new Error('You provided a `checked` prop to a form field without an ' + '`onChange` handler. This will render a read-only field. If ' + 'the field should be mutable use `defaultChecked`. Otherwise, ' + 'set either `onChange` or `readOnly`.');
    }
  };

  /**
   * Provide a linked `value` attribute for controlled forms. You should not use
   * this outside of the ReactDOM controlled form components.
   */
  ReactControlledValuePropTypes.checkPropTypes = function (tagName, props) {
    checkPropTypes(propTypes, props, 'prop', tagName, ReactDebugCurrentFrame$2.getStackAddendum);
  };
}

// For HTML, certain tags should omit their close tag. We keep a whitelist for
// those special-case tags.

var omittedCloseTags = {
  area: true,
  base: true,
  br: true,
  col: true,
  embed: true,
  hr: true,
  img: true,
  input: true,
  keygen: true,
  link: true,
  meta: true,
  param: true,
  source: true,
  track: true,
  wbr: true
  // NOTE: menuitem's close tag should be omitted, but that causes problems.
};

// For HTML, certain tags cannot have children. This has the same purpose as
// `omittedCloseTags` except that `menuitem` should still have its closing tag.

var voidElementTags = _assign({
  menuitem: true
}, omittedCloseTags);

// TODO: We can remove this if we add invariantWithStack()
// or add stack by default to invariants where possible.
var HTML = '__html';

var ReactDebugCurrentFrame$3 = null;
{
  ReactDebugCurrentFrame$3 = ReactSharedInternals.ReactDebugCurrentFrame;
}

function assertValidProps(tag, props) {
  if (!props) {
    return;
  }
  // Note the use of `==` which checks for null or undefined.
  if (voidElementTags[tag]) {
    !(props.children == null && props.dangerouslySetInnerHTML == null) ? invariant(false, '%s is a void element tag and must neither have `children` nor use `dangerouslySetInnerHTML`.%s', tag, ReactDebugCurrentFrame$3.getStackAddendum()) : void 0;
  }
  if (props.dangerouslySetInnerHTML != null) {
    !(props.children == null) ? invariant(false, 'Can only set one of `children` or `props.dangerouslySetInnerHTML`.') : void 0;
    !(typeof props.dangerouslySetInnerHTML === 'object' && HTML in props.dangerouslySetInnerHTML) ? invariant(false, '`props.dangerouslySetInnerHTML` must be in the form `{__html: ...}`. Please visit https://fb.me/react-invariant-dangerously-set-inner-html for more information.') : void 0;
  }
  {
    !(props.suppressContentEditableWarning || !props.contentEditable || props.children == null) ? warning$1(false, 'A component is `contentEditable` and contains `children` managed by ' + 'React. It is now your responsibility to guarantee that none of ' + 'those nodes are unexpectedly modified or duplicated. This is ' + 'probably not intentional.') : void 0;
  }
  !(props.style == null || typeof props.style === 'object') ? invariant(false, 'The `style` prop expects a mapping from style properties to values, not a string. For example, style={{marginRight: spacing + \'em\'}} when using JSX.%s', ReactDebugCurrentFrame$3.getStackAddendum()) : void 0;
}

/**
 * CSS properties which accept numbers but are not in units of "px".
 */
var isUnitlessNumber = {
  animationIterationCount: true,
  borderImageOutset: true,
  borderImageSlice: true,
  borderImageWidth: true,
  boxFlex: true,
  boxFlexGroup: true,
  boxOrdinalGroup: true,
  columnCount: true,
  columns: true,
  flex: true,
  flexGrow: true,
  flexPositive: true,
  flexShrink: true,
  flexNegative: true,
  flexOrder: true,
  gridArea: true,
  gridRow: true,
  gridRowEnd: true,
  gridRowSpan: true,
  gridRowStart: true,
  gridColumn: true,
  gridColumnEnd: true,
  gridColumnSpan: true,
  gridColumnStart: true,
  fontWeight: true,
  lineClamp: true,
  lineHeight: true,
  opacity: true,
  order: true,
  orphans: true,
  tabSize: true,
  widows: true,
  zIndex: true,
  zoom: true,

  // SVG-related properties
  fillOpacity: true,
  floodOpacity: true,
  stopOpacity: true,
  strokeDasharray: true,
  strokeDashoffset: true,
  strokeMiterlimit: true,
  strokeOpacity: true,
  strokeWidth: true
};

/**
 * @param {string} prefix vendor-specific prefix, eg: Webkit
 * @param {string} key style name, eg: transitionDuration
 * @return {string} style name prefixed with `prefix`, properly camelCased, eg:
 * WebkitTransitionDuration
 */
function prefixKey(prefix, key) {
  return prefix + key.charAt(0).toUpperCase() + key.substring(1);
}

/**
 * Support style names that may come passed in prefixed by adding permutations
 * of vendor prefixes.
 */
var prefixes = ['Webkit', 'ms', 'Moz', 'O'];

// Using Object.keys here, or else the vanilla for-in loop makes IE8 go into an
// infinite loop, because it iterates over the newly added props too.
Object.keys(isUnitlessNumber).forEach(function (prop) {
  prefixes.forEach(function (prefix) {
    isUnitlessNumber[prefixKey(prefix, prop)] = isUnitlessNumber[prop];
  });
});

/**
 * Convert a value into the proper css writable value. The style name `name`
 * should be logical (no hyphens), as specified
 * in `CSSProperty.isUnitlessNumber`.
 *
 * @param {string} name CSS property name such as `topMargin`.
 * @param {*} value CSS property value such as `10px`.
 * @return {string} Normalized style value with dimensions applied.
 */
function dangerousStyleValue(name, value, isCustomProperty) {
  // Note that we've removed escapeTextForBrowser() calls here since the
  // whole string will be escaped when the attribute is injected into
  // the markup. If you provide unsafe user data here they can inject
  // arbitrary CSS which may be problematic (I couldn't repro this):
  // https://www.owasp.org/index.php/XSS_Filter_Evasion_Cheat_Sheet
  // http://www.thespanner.co.uk/2007/11/26/ultimate-xss-css-injection/
  // This is not an XSS hole but instead a potential CSS injection issue
  // which has lead to a greater discussion about how we're going to
  // trust URLs moving forward. See #2115901

  var isEmpty = value == null || typeof value === 'boolean' || value === '';
  if (isEmpty) {
    return '';
  }

  if (!isCustomProperty && typeof value === 'number' && value !== 0 && !(isUnitlessNumber.hasOwnProperty(name) && isUnitlessNumber[name])) {
    return value + 'px'; // Presumes implicit 'px' suffix for unitless numbers
  }

  return ('' + value).trim();
}

var uppercasePattern = /([A-Z])/g;
var msPattern = /^ms-/;

/**
 * Hyphenates a camelcased CSS property name, for example:
 *
 *   > hyphenateStyleName('backgroundColor')
 *   < "background-color"
 *   > hyphenateStyleName('MozTransition')
 *   < "-moz-transition"
 *   > hyphenateStyleName('msTransition')
 *   < "-ms-transition"
 *
 * As Modernizr suggests (http://modernizr.com/docs/#prefixed), an `ms` prefix
 * is converted to `-ms-`.
 */
function hyphenateStyleName(name) {
  return name.replace(uppercasePattern, '-$1').toLowerCase().replace(msPattern, '-ms-');
}

function isCustomComponent(tagName, props) {
  if (tagName.indexOf('-') === -1) {
    return typeof props.is === 'string';
  }
  switch (tagName) {
    // These are reserved SVG and MathML elements.
    // We don't mind this whitelist too much because we expect it to never grow.
    // The alternative is to track the namespace in a few places which is convoluted.
    // https://w3c.github.io/webcomponents/spec/custom/#custom-elements-core-concepts
    case 'annotation-xml':
    case 'color-profile':
    case 'font-face':
    case 'font-face-src':
    case 'font-face-uri':
    case 'font-face-format':
    case 'font-face-name':
    case 'missing-glyph':
      return false;
    default:
      return true;
  }
}

var warnValidStyle = function () {};

{
  // 'msTransform' is correct, but the other prefixes should be capitalized
  var badVendoredStyleNamePattern = /^(?:webkit|moz|o)[A-Z]/;
  var msPattern$1 = /^-ms-/;
  var hyphenPattern = /-(.)/g;

  // style values shouldn't contain a semicolon
  var badStyleValueWithSemicolonPattern = /;\s*$/;

  var warnedStyleNames = {};
  var warnedStyleValues = {};
  var warnedForNaNValue = false;
  var warnedForInfinityValue = false;

  var camelize = function (string) {
    return string.replace(hyphenPattern, function (_, character) {
      return character.toUpperCase();
    });
  };

  var warnHyphenatedStyleName = function (name) {
    if (warnedStyleNames.hasOwnProperty(name) && warnedStyleNames[name]) {
      return;
    }

    warnedStyleNames[name] = true;
    warning$1(false, 'Unsupported style property %s. Did you mean %s?', name,
    // As Andi Smith suggests
    // (http://www.andismith.com/blog/2012/02/modernizr-prefixed/), an `-ms` prefix
    // is converted to lowercase `ms`.
    camelize(name.replace(msPattern$1, 'ms-')));
  };

  var warnBadVendoredStyleName = function (name) {
    if (warnedStyleNames.hasOwnProperty(name) && warnedStyleNames[name]) {
      return;
    }

    warnedStyleNames[name] = true;
    warning$1(false, 'Unsupported vendor-prefixed style property %s. Did you mean %s?', name, name.charAt(0).toUpperCase() + name.slice(1));
  };

  var warnStyleValueWithSemicolon = function (name, value) {
    if (warnedStyleValues.hasOwnProperty(value) && warnedStyleValues[value]) {
      return;
    }

    warnedStyleValues[value] = true;
    warning$1(false, "Style property values shouldn't contain a semicolon. " + 'Try "%s: %s" instead.', name, value.replace(badStyleValueWithSemicolonPattern, ''));
  };

  var warnStyleValueIsNaN = function (name, value) {
    if (warnedForNaNValue) {
      return;
    }

    warnedForNaNValue = true;
    warning$1(false, '`NaN` is an invalid value for the `%s` css style property.', name);
  };

  var warnStyleValueIsInfinity = function (name, value) {
    if (warnedForInfinityValue) {
      return;
    }

    warnedForInfinityValue = true;
    warning$1(false, '`Infinity` is an invalid value for the `%s` css style property.', name);
  };

  warnValidStyle = function (name, value) {
    if (name.indexOf('-') > -1) {
      warnHyphenatedStyleName(name);
    } else if (badVendoredStyleNamePattern.test(name)) {
      warnBadVendoredStyleName(name);
    } else if (badStyleValueWithSemicolonPattern.test(value)) {
      warnStyleValueWithSemicolon(name, value);
    }

    if (typeof value === 'number') {
      if (isNaN(value)) {
        warnStyleValueIsNaN(name, value);
      } else if (!isFinite(value)) {
        warnStyleValueIsInfinity(name, value);
      }
    }
  };
}

var warnValidStyle$1 = warnValidStyle;

var ariaProperties = {
  'aria-current': 0, // state
  'aria-details': 0,
  'aria-disabled': 0, // state
  'aria-hidden': 0, // state
  'aria-invalid': 0, // state
  'aria-keyshortcuts': 0,
  'aria-label': 0,
  'aria-roledescription': 0,
  // Widget Attributes
  'aria-autocomplete': 0,
  'aria-checked': 0,
  'aria-expanded': 0,
  'aria-haspopup': 0,
  'aria-level': 0,
  'aria-modal': 0,
  'aria-multiline': 0,
  'aria-multiselectable': 0,
  'aria-orientation': 0,
  'aria-placeholder': 0,
  'aria-pressed': 0,
  'aria-readonly': 0,
  'aria-required': 0,
  'aria-selected': 0,
  'aria-sort': 0,
  'aria-valuemax': 0,
  'aria-valuemin': 0,
  'aria-valuenow': 0,
  'aria-valuetext': 0,
  // Live Region Attributes
  'aria-atomic': 0,
  'aria-busy': 0,
  'aria-live': 0,
  'aria-relevant': 0,
  // Drag-and-Drop Attributes
  'aria-dropeffect': 0,
  'aria-grabbed': 0,
  // Relationship Attributes
  'aria-activedescendant': 0,
  'aria-colcount': 0,
  'aria-colindex': 0,
  'aria-colspan': 0,
  'aria-controls': 0,
  'aria-describedby': 0,
  'aria-errormessage': 0,
  'aria-flowto': 0,
  'aria-labelledby': 0,
  'aria-owns': 0,
  'aria-posinset': 0,
  'aria-rowcount': 0,
  'aria-rowindex': 0,
  'aria-rowspan': 0,
  'aria-setsize': 0
};

var warnedProperties = {};
var rARIA = new RegExp('^(aria)-[' + ATTRIBUTE_NAME_CHAR + ']*$');
var rARIACamel = new RegExp('^(aria)[A-Z][' + ATTRIBUTE_NAME_CHAR + ']*$');

var hasOwnProperty$2 = Object.prototype.hasOwnProperty;

function validateProperty(tagName, name) {
  if (hasOwnProperty$2.call(warnedProperties, name) && warnedProperties[name]) {
    return true;
  }

  if (rARIACamel.test(name)) {
    var ariaName = 'aria-' + name.slice(4).toLowerCase();
    var correctName = ariaProperties.hasOwnProperty(ariaName) ? ariaName : null;

    // If this is an aria-* attribute, but is not listed in the known DOM
    // DOM properties, then it is an invalid aria-* attribute.
    if (correctName == null) {
      warning$1(false, 'Invalid ARIA attribute `%s`. ARIA attributes follow the pattern aria-* and must be lowercase.', name);
      warnedProperties[name] = true;
      return true;
    }
    // aria-* attributes should be lowercase; suggest the lowercase version.
    if (name !== correctName) {
      warning$1(false, 'Invalid ARIA attribute `%s`. Did you mean `%s`?', name, correctName);
      warnedProperties[name] = true;
      return true;
    }
  }

  if (rARIA.test(name)) {
    var lowerCasedName = name.toLowerCase();
    var standardName = ariaProperties.hasOwnProperty(lowerCasedName) ? lowerCasedName : null;

    // If this is an aria-* attribute, but is not listed in the known DOM
    // DOM properties, then it is an invalid aria-* attribute.
    if (standardName == null) {
      warnedProperties[name] = true;
      return false;
    }
    // aria-* attributes should be lowercase; suggest the lowercase version.
    if (name !== standardName) {
      warning$1(false, 'Unknown ARIA attribute `%s`. Did you mean `%s`?', name, standardName);
      warnedProperties[name] = true;
      return true;
    }
  }

  return true;
}

function warnInvalidARIAProps(type, props) {
  var invalidProps = [];

  for (var key in props) {
    var isValid = validateProperty(type, key);
    if (!isValid) {
      invalidProps.push(key);
    }
  }

  var unknownPropString = invalidProps.map(function (prop) {
    return '`' + prop + '`';
  }).join(', ');

  if (invalidProps.length === 1) {
    warning$1(false, 'Invalid aria prop %s on <%s> tag. ' + 'For details, see https://fb.me/invalid-aria-prop', unknownPropString, type);
  } else if (invalidProps.length > 1) {
    warning$1(false, 'Invalid aria props %s on <%s> tag. ' + 'For details, see https://fb.me/invalid-aria-prop', unknownPropString, type);
  }
}

function validateProperties(type, props) {
  if (isCustomComponent(type, props)) {
    return;
  }
  warnInvalidARIAProps(type, props);
}

var didWarnValueNull = false;

function validateProperties$1(type, props) {
  if (type !== 'input' && type !== 'textarea' && type !== 'select') {
    return;
  }

  if (props != null && props.value === null && !didWarnValueNull) {
    didWarnValueNull = true;
    if (type === 'select' && props.multiple) {
      warning$1(false, '`value` prop on `%s` should not be null. ' + 'Consider using an empty array when `multiple` is set to `true` ' + 'to clear the component or `undefined` for uncontrolled components.', type);
    } else {
      warning$1(false, '`value` prop on `%s` should not be null. ' + 'Consider using an empty string to clear the component or `undefined` ' + 'for uncontrolled components.', type);
    }
  }
}

/**
 * Registers plugins so that they can extract and dispatch events.
 *
 * @see {EventPluginHub}
 */

/**
 * Ordered list of injected plugins.
 */


/**
 * Mapping from event name to dispatch config
 */


/**
 * Mapping from registration name to plugin module
 */
var registrationNameModules = {};

/**
 * Mapping from registration name to event name
 */


/**
 * Mapping from lowercase registration names to the properly cased version,
 * used to warn in the case of missing event handlers. Available
 * only in true.
 * @type {Object}
 */
var possibleRegistrationNames = {};
// Trust the developer to only use possibleRegistrationNames in true

/**
 * Injects an ordering of plugins (by plugin name). This allows the ordering
 * to be decoupled from injection of the actual plugins so that ordering is
 * always deterministic regardless of packaging, on-the-fly injection, etc.
 *
 * @param {array} InjectedEventPluginOrder
 * @internal
 * @see {EventPluginHub.injection.injectEventPluginOrder}
 */


/**
 * Injects plugins to be used by `EventPluginHub`. The plugin names must be
 * in the ordering injected by `injectEventPluginOrder`.
 *
 * Plugins can be injected as part of page initialization or on-the-fly.
 *
 * @param {object} injectedNamesToPlugins Map from names to plugin modules.
 * @internal
 * @see {EventPluginHub.injection.injectEventPluginsByName}
 */

// When adding attributes to the HTML or SVG whitelist, be sure to
// also add them to this module to ensure casing and incorrect name
// warnings.
var possibleStandardNames = {
  // HTML
  accept: 'accept',
  acceptcharset: 'acceptCharset',
  'accept-charset': 'acceptCharset',
  accesskey: 'accessKey',
  action: 'action',
  allowfullscreen: 'allowFullScreen',
  alt: 'alt',
  as: 'as',
  async: 'async',
  autocapitalize: 'autoCapitalize',
  autocomplete: 'autoComplete',
  autocorrect: 'autoCorrect',
  autofocus: 'autoFocus',
  autoplay: 'autoPlay',
  autosave: 'autoSave',
  capture: 'capture',
  cellpadding: 'cellPadding',
  cellspacing: 'cellSpacing',
  challenge: 'challenge',
  charset: 'charSet',
  checked: 'checked',
  children: 'children',
  cite: 'cite',
  class: 'className',
  classid: 'classID',
  classname: 'className',
  cols: 'cols',
  colspan: 'colSpan',
  content: 'content',
  contenteditable: 'contentEditable',
  contextmenu: 'contextMenu',
  controls: 'controls',
  controlslist: 'controlsList',
  coords: 'coords',
  crossorigin: 'crossOrigin',
  dangerouslysetinnerhtml: 'dangerouslySetInnerHTML',
  data: 'data',
  datetime: 'dateTime',
  default: 'default',
  defaultchecked: 'defaultChecked',
  defaultvalue: 'defaultValue',
  defer: 'defer',
  dir: 'dir',
  disabled: 'disabled',
  download: 'download',
  draggable: 'draggable',
  enctype: 'encType',
  for: 'htmlFor',
  form: 'form',
  formmethod: 'formMethod',
  formaction: 'formAction',
  formenctype: 'formEncType',
  formnovalidate: 'formNoValidate',
  formtarget: 'formTarget',
  frameborder: 'frameBorder',
  headers: 'headers',
  height: 'height',
  hidden: 'hidden',
  high: 'high',
  href: 'href',
  hreflang: 'hrefLang',
  htmlfor: 'htmlFor',
  httpequiv: 'httpEquiv',
  'http-equiv': 'httpEquiv',
  icon: 'icon',
  id: 'id',
  innerhtml: 'innerHTML',
  inputmode: 'inputMode',
  integrity: 'integrity',
  is: 'is',
  itemid: 'itemID',
  itemprop: 'itemProp',
  itemref: 'itemRef',
  itemscope: 'itemScope',
  itemtype: 'itemType',
  keyparams: 'keyParams',
  keytype: 'keyType',
  kind: 'kind',
  label: 'label',
  lang: 'lang',
  list: 'list',
  loop: 'loop',
  low: 'low',
  manifest: 'manifest',
  marginwidth: 'marginWidth',
  marginheight: 'marginHeight',
  max: 'max',
  maxlength: 'maxLength',
  media: 'media',
  mediagroup: 'mediaGroup',
  method: 'method',
  min: 'min',
  minlength: 'minLength',
  multiple: 'multiple',
  muted: 'muted',
  name: 'name',
  nomodule: 'noModule',
  nonce: 'nonce',
  novalidate: 'noValidate',
  open: 'open',
  optimum: 'optimum',
  pattern: 'pattern',
  placeholder: 'placeholder',
  playsinline: 'playsInline',
  poster: 'poster',
  preload: 'preload',
  profile: 'profile',
  radiogroup: 'radioGroup',
  readonly: 'readOnly',
  referrerpolicy: 'referrerPolicy',
  rel: 'rel',
  required: 'required',
  reversed: 'reversed',
  role: 'role',
  rows: 'rows',
  rowspan: 'rowSpan',
  sandbox: 'sandbox',
  scope: 'scope',
  scoped: 'scoped',
  scrolling: 'scrolling',
  seamless: 'seamless',
  selected: 'selected',
  shape: 'shape',
  size: 'size',
  sizes: 'sizes',
  span: 'span',
  spellcheck: 'spellCheck',
  src: 'src',
  srcdoc: 'srcDoc',
  srclang: 'srcLang',
  srcset: 'srcSet',
  start: 'start',
  step: 'step',
  style: 'style',
  summary: 'summary',
  tabindex: 'tabIndex',
  target: 'target',
  title: 'title',
  type: 'type',
  usemap: 'useMap',
  value: 'value',
  width: 'width',
  wmode: 'wmode',
  wrap: 'wrap',

  // SVG
  about: 'about',
  accentheight: 'accentHeight',
  'accent-height': 'accentHeight',
  accumulate: 'accumulate',
  additive: 'additive',
  alignmentbaseline: 'alignmentBaseline',
  'alignment-baseline': 'alignmentBaseline',
  allowreorder: 'allowReorder',
  alphabetic: 'alphabetic',
  amplitude: 'amplitude',
  arabicform: 'arabicForm',
  'arabic-form': 'arabicForm',
  ascent: 'ascent',
  attributename: 'attributeName',
  attributetype: 'attributeType',
  autoreverse: 'autoReverse',
  azimuth: 'azimuth',
  basefrequency: 'baseFrequency',
  baselineshift: 'baselineShift',
  'baseline-shift': 'baselineShift',
  baseprofile: 'baseProfile',
  bbox: 'bbox',
  begin: 'begin',
  bias: 'bias',
  by: 'by',
  calcmode: 'calcMode',
  capheight: 'capHeight',
  'cap-height': 'capHeight',
  clip: 'clip',
  clippath: 'clipPath',
  'clip-path': 'clipPath',
  clippathunits: 'clipPathUnits',
  cliprule: 'clipRule',
  'clip-rule': 'clipRule',
  color: 'color',
  colorinterpolation: 'colorInterpolation',
  'color-interpolation': 'colorInterpolation',
  colorinterpolationfilters: 'colorInterpolationFilters',
  'color-interpolation-filters': 'colorInterpolationFilters',
  colorprofile: 'colorProfile',
  'color-profile': 'colorProfile',
  colorrendering: 'colorRendering',
  'color-rendering': 'colorRendering',
  contentscripttype: 'contentScriptType',
  contentstyletype: 'contentStyleType',
  cursor: 'cursor',
  cx: 'cx',
  cy: 'cy',
  d: 'd',
  datatype: 'datatype',
  decelerate: 'decelerate',
  descent: 'descent',
  diffuseconstant: 'diffuseConstant',
  direction: 'direction',
  display: 'display',
  divisor: 'divisor',
  dominantbaseline: 'dominantBaseline',
  'dominant-baseline': 'dominantBaseline',
  dur: 'dur',
  dx: 'dx',
  dy: 'dy',
  edgemode: 'edgeMode',
  elevation: 'elevation',
  enablebackground: 'enableBackground',
  'enable-background': 'enableBackground',
  end: 'end',
  exponent: 'exponent',
  externalresourcesrequired: 'externalResourcesRequired',
  fill: 'fill',
  fillopacity: 'fillOpacity',
  'fill-opacity': 'fillOpacity',
  fillrule: 'fillRule',
  'fill-rule': 'fillRule',
  filter: 'filter',
  filterres: 'filterRes',
  filterunits: 'filterUnits',
  floodopacity: 'floodOpacity',
  'flood-opacity': 'floodOpacity',
  floodcolor: 'floodColor',
  'flood-color': 'floodColor',
  focusable: 'focusable',
  fontfamily: 'fontFamily',
  'font-family': 'fontFamily',
  fontsize: 'fontSize',
  'font-size': 'fontSize',
  fontsizeadjust: 'fontSizeAdjust',
  'font-size-adjust': 'fontSizeAdjust',
  fontstretch: 'fontStretch',
  'font-stretch': 'fontStretch',
  fontstyle: 'fontStyle',
  'font-style': 'fontStyle',
  fontvariant: 'fontVariant',
  'font-variant': 'fontVariant',
  fontweight: 'fontWeight',
  'font-weight': 'fontWeight',
  format: 'format',
  from: 'from',
  fx: 'fx',
  fy: 'fy',
  g1: 'g1',
  g2: 'g2',
  glyphname: 'glyphName',
  'glyph-name': 'glyphName',
  glyphorientationhorizontal: 'glyphOrientationHorizontal',
  'glyph-orientation-horizontal': 'glyphOrientationHorizontal',
  glyphorientationvertical: 'glyphOrientationVertical',
  'glyph-orientation-vertical': 'glyphOrientationVertical',
  glyphref: 'glyphRef',
  gradienttransform: 'gradientTransform',
  gradientunits: 'gradientUnits',
  hanging: 'hanging',
  horizadvx: 'horizAdvX',
  'horiz-adv-x': 'horizAdvX',
  horizoriginx: 'horizOriginX',
  'horiz-origin-x': 'horizOriginX',
  ideographic: 'ideographic',
  imagerendering: 'imageRendering',
  'image-rendering': 'imageRendering',
  in2: 'in2',
  in: 'in',
  inlist: 'inlist',
  intercept: 'intercept',
  k1: 'k1',
  k2: 'k2',
  k3: 'k3',
  k4: 'k4',
  k: 'k',
  kernelmatrix: 'kernelMatrix',
  kernelunitlength: 'kernelUnitLength',
  kerning: 'kerning',
  keypoints: 'keyPoints',
  keysplines: 'keySplines',
  keytimes: 'keyTimes',
  lengthadjust: 'lengthAdjust',
  letterspacing: 'letterSpacing',
  'letter-spacing': 'letterSpacing',
  lightingcolor: 'lightingColor',
  'lighting-color': 'lightingColor',
  limitingconeangle: 'limitingConeAngle',
  local: 'local',
  markerend: 'markerEnd',
  'marker-end': 'markerEnd',
  markerheight: 'markerHeight',
  markermid: 'markerMid',
  'marker-mid': 'markerMid',
  markerstart: 'markerStart',
  'marker-start': 'markerStart',
  markerunits: 'markerUnits',
  markerwidth: 'markerWidth',
  mask: 'mask',
  maskcontentunits: 'maskContentUnits',
  maskunits: 'maskUnits',
  mathematical: 'mathematical',
  mode: 'mode',
  numoctaves: 'numOctaves',
  offset: 'offset',
  opacity: 'opacity',
  operator: 'operator',
  order: 'order',
  orient: 'orient',
  orientation: 'orientation',
  origin: 'origin',
  overflow: 'overflow',
  overlineposition: 'overlinePosition',
  'overline-position': 'overlinePosition',
  overlinethickness: 'overlineThickness',
  'overline-thickness': 'overlineThickness',
  paintorder: 'paintOrder',
  'paint-order': 'paintOrder',
  panose1: 'panose1',
  'panose-1': 'panose1',
  pathlength: 'pathLength',
  patterncontentunits: 'patternContentUnits',
  patterntransform: 'patternTransform',
  patternunits: 'patternUnits',
  pointerevents: 'pointerEvents',
  'pointer-events': 'pointerEvents',
  points: 'points',
  pointsatx: 'pointsAtX',
  pointsaty: 'pointsAtY',
  pointsatz: 'pointsAtZ',
  prefix: 'prefix',
  preservealpha: 'preserveAlpha',
  preserveaspectratio: 'preserveAspectRatio',
  primitiveunits: 'primitiveUnits',
  property: 'property',
  r: 'r',
  radius: 'radius',
  refx: 'refX',
  refy: 'refY',
  renderingintent: 'renderingIntent',
  'rendering-intent': 'renderingIntent',
  repeatcount: 'repeatCount',
  repeatdur: 'repeatDur',
  requiredextensions: 'requiredExtensions',
  requiredfeatures: 'requiredFeatures',
  resource: 'resource',
  restart: 'restart',
  result: 'result',
  results: 'results',
  rotate: 'rotate',
  rx: 'rx',
  ry: 'ry',
  scale: 'scale',
  security: 'security',
  seed: 'seed',
  shaperendering: 'shapeRendering',
  'shape-rendering': 'shapeRendering',
  slope: 'slope',
  spacing: 'spacing',
  specularconstant: 'specularConstant',
  specularexponent: 'specularExponent',
  speed: 'speed',
  spreadmethod: 'spreadMethod',
  startoffset: 'startOffset',
  stddeviation: 'stdDeviation',
  stemh: 'stemh',
  stemv: 'stemv',
  stitchtiles: 'stitchTiles',
  stopcolor: 'stopColor',
  'stop-color': 'stopColor',
  stopopacity: 'stopOpacity',
  'stop-opacity': 'stopOpacity',
  strikethroughposition: 'strikethroughPosition',
  'strikethrough-position': 'strikethroughPosition',
  strikethroughthickness: 'strikethroughThickness',
  'strikethrough-thickness': 'strikethroughThickness',
  string: 'string',
  stroke: 'stroke',
  strokedasharray: 'strokeDasharray',
  'stroke-dasharray': 'strokeDasharray',
  strokedashoffset: 'strokeDashoffset',
  'stroke-dashoffset': 'strokeDashoffset',
  strokelinecap: 'strokeLinecap',
  'stroke-linecap': 'strokeLinecap',
  strokelinejoin: 'strokeLinejoin',
  'stroke-linejoin': 'strokeLinejoin',
  strokemiterlimit: 'strokeMiterlimit',
  'stroke-miterlimit': 'strokeMiterlimit',
  strokewidth: 'strokeWidth',
  'stroke-width': 'strokeWidth',
  strokeopacity: 'strokeOpacity',
  'stroke-opacity': 'strokeOpacity',
  suppresscontenteditablewarning: 'suppressContentEditableWarning',
  suppresshydrationwarning: 'suppressHydrationWarning',
  surfacescale: 'surfaceScale',
  systemlanguage: 'systemLanguage',
  tablevalues: 'tableValues',
  targetx: 'targetX',
  targety: 'targetY',
  textanchor: 'textAnchor',
  'text-anchor': 'textAnchor',
  textdecoration: 'textDecoration',
  'text-decoration': 'textDecoration',
  textlength: 'textLength',
  textrendering: 'textRendering',
  'text-rendering': 'textRendering',
  to: 'to',
  transform: 'transform',
  typeof: 'typeof',
  u1: 'u1',
  u2: 'u2',
  underlineposition: 'underlinePosition',
  'underline-position': 'underlinePosition',
  underlinethickness: 'underlineThickness',
  'underline-thickness': 'underlineThickness',
  unicode: 'unicode',
  unicodebidi: 'unicodeBidi',
  'unicode-bidi': 'unicodeBidi',
  unicoderange: 'unicodeRange',
  'unicode-range': 'unicodeRange',
  unitsperem: 'unitsPerEm',
  'units-per-em': 'unitsPerEm',
  unselectable: 'unselectable',
  valphabetic: 'vAlphabetic',
  'v-alphabetic': 'vAlphabetic',
  values: 'values',
  vectoreffect: 'vectorEffect',
  'vector-effect': 'vectorEffect',
  version: 'version',
  vertadvy: 'vertAdvY',
  'vert-adv-y': 'vertAdvY',
  vertoriginx: 'vertOriginX',
  'vert-origin-x': 'vertOriginX',
  vertoriginy: 'vertOriginY',
  'vert-origin-y': 'vertOriginY',
  vhanging: 'vHanging',
  'v-hanging': 'vHanging',
  videographic: 'vIdeographic',
  'v-ideographic': 'vIdeographic',
  viewbox: 'viewBox',
  viewtarget: 'viewTarget',
  visibility: 'visibility',
  vmathematical: 'vMathematical',
  'v-mathematical': 'vMathematical',
  vocab: 'vocab',
  widths: 'widths',
  wordspacing: 'wordSpacing',
  'word-spacing': 'wordSpacing',
  writingmode: 'writingMode',
  'writing-mode': 'writingMode',
  x1: 'x1',
  x2: 'x2',
  x: 'x',
  xchannelselector: 'xChannelSelector',
  xheight: 'xHeight',
  'x-height': 'xHeight',
  xlinkactuate: 'xlinkActuate',
  'xlink:actuate': 'xlinkActuate',
  xlinkarcrole: 'xlinkArcrole',
  'xlink:arcrole': 'xlinkArcrole',
  xlinkhref: 'xlinkHref',
  'xlink:href': 'xlinkHref',
  xlinkrole: 'xlinkRole',
  'xlink:role': 'xlinkRole',
  xlinkshow: 'xlinkShow',
  'xlink:show': 'xlinkShow',
  xlinktitle: 'xlinkTitle',
  'xlink:title': 'xlinkTitle',
  xlinktype: 'xlinkType',
  'xlink:type': 'xlinkType',
  xmlbase: 'xmlBase',
  'xml:base': 'xmlBase',
  xmllang: 'xmlLang',
  'xml:lang': 'xmlLang',
  xmlns: 'xmlns',
  'xml:space': 'xmlSpace',
  xmlnsxlink: 'xmlnsXlink',
  'xmlns:xlink': 'xmlnsXlink',
  xmlspace: 'xmlSpace',
  y1: 'y1',
  y2: 'y2',
  y: 'y',
  ychannelselector: 'yChannelSelector',
  z: 'z',
  zoomandpan: 'zoomAndPan'
};

var validateProperty$1 = function () {};

{
  var warnedProperties$1 = {};
  var _hasOwnProperty = Object.prototype.hasOwnProperty;
  var EVENT_NAME_REGEX = /^on./;
  var INVALID_EVENT_NAME_REGEX = /^on[^A-Z]/;
  var rARIA$1 = new RegExp('^(aria)-[' + ATTRIBUTE_NAME_CHAR + ']*$');
  var rARIACamel$1 = new RegExp('^(aria)[A-Z][' + ATTRIBUTE_NAME_CHAR + ']*$');

  validateProperty$1 = function (tagName, name, value, canUseEventSystem) {
    if (_hasOwnProperty.call(warnedProperties$1, name) && warnedProperties$1[name]) {
      return true;
    }

    var lowerCasedName = name.toLowerCase();
    if (lowerCasedName === 'onfocusin' || lowerCasedName === 'onfocusout') {
      warning$1(false, 'React uses onFocus and onBlur instead of onFocusIn and onFocusOut. ' + 'All React events are normalized to bubble, so onFocusIn and onFocusOut ' + 'are not needed/supported by React.');
      warnedProperties$1[name] = true;
      return true;
    }

    // We can't rely on the event system being injected on the server.
    if (canUseEventSystem) {
      if (registrationNameModules.hasOwnProperty(name)) {
        return true;
      }
      var registrationName = possibleRegistrationNames.hasOwnProperty(lowerCasedName) ? possibleRegistrationNames[lowerCasedName] : null;
      if (registrationName != null) {
        warning$1(false, 'Invalid event handler property `%s`. Did you mean `%s`?', name, registrationName);
        warnedProperties$1[name] = true;
        return true;
      }
      if (EVENT_NAME_REGEX.test(name)) {
        warning$1(false, 'Unknown event handler property `%s`. It will be ignored.', name);
        warnedProperties$1[name] = true;
        return true;
      }
    } else if (EVENT_NAME_REGEX.test(name)) {
      // If no event plugins have been injected, we are in a server environment.
      // So we can't tell if the event name is correct for sure, but we can filter
      // out known bad ones like `onclick`. We can't suggest a specific replacement though.
      if (INVALID_EVENT_NAME_REGEX.test(name)) {
        warning$1(false, 'Invalid event handler property `%s`. ' + 'React events use the camelCase naming convention, for example `onClick`.', name);
      }
      warnedProperties$1[name] = true;
      return true;
    }

    // Let the ARIA attribute hook validate ARIA attributes
    if (rARIA$1.test(name) || rARIACamel$1.test(name)) {
      return true;
    }

    if (lowerCasedName === 'innerhtml') {
      warning$1(false, 'Directly setting property `innerHTML` is not permitted. ' + 'For more information, lookup documentation on `dangerouslySetInnerHTML`.');
      warnedProperties$1[name] = true;
      return true;
    }

    if (lowerCasedName === 'aria') {
      warning$1(false, 'The `aria` attribute is reserved for future use in React. ' + 'Pass individual `aria-` attributes instead.');
      warnedProperties$1[name] = true;
      return true;
    }

    if (lowerCasedName === 'is' && value !== null && value !== undefined && typeof value !== 'string') {
      warning$1(false, 'Received a `%s` for a string attribute `is`. If this is expected, cast ' + 'the value to a string.', typeof value);
      warnedProperties$1[name] = true;
      return true;
    }

    if (typeof value === 'number' && isNaN(value)) {
      warning$1(false, 'Received NaN for the `%s` attribute. If this is expected, cast ' + 'the value to a string.', name);
      warnedProperties$1[name] = true;
      return true;
    }

    var propertyInfo = getPropertyInfo(name);
    var isReserved = propertyInfo !== null && propertyInfo.type === RESERVED;

    // Known attributes should match the casing specified in the property config.
    if (possibleStandardNames.hasOwnProperty(lowerCasedName)) {
      var standardName = possibleStandardNames[lowerCasedName];
      if (standardName !== name) {
        warning$1(false, 'Invalid DOM property `%s`. Did you mean `%s`?', name, standardName);
        warnedProperties$1[name] = true;
        return true;
      }
    } else if (!isReserved && name !== lowerCasedName) {
      // Unknown attributes should have lowercase casing since that's how they
      // will be cased anyway with server rendering.
      warning$1(false, 'React does not recognize the `%s` prop on a DOM element. If you ' + 'intentionally want it to appear in the DOM as a custom ' + 'attribute, spell it as lowercase `%s` instead. ' + 'If you accidentally passed it from a parent component, remove ' + 'it from the DOM element.', name, lowerCasedName);
      warnedProperties$1[name] = true;
      return true;
    }

    if (typeof value === 'boolean' && shouldRemoveAttributeWithWarning(name, value, propertyInfo, false)) {
      if (value) {
        warning$1(false, 'Received `%s` for a non-boolean attribute `%s`.\n\n' + 'If you want to write it to the DOM, pass a string instead: ' + '%s="%s" or %s={value.toString()}.', value, name, name, value, name);
      } else {
        warning$1(false, 'Received `%s` for a non-boolean attribute `%s`.\n\n' + 'If you want to write it to the DOM, pass a string instead: ' + '%s="%s" or %s={value.toString()}.\n\n' + 'If you used to conditionally omit it with %s={condition && value}, ' + 'pass %s={condition ? value : undefined} instead.', value, name, name, value, name, name, name);
      }
      warnedProperties$1[name] = true;
      return true;
    }

    // Now that we've validated casing, do not validate
    // data types for reserved props
    if (isReserved) {
      return true;
    }

    // Warn when a known attribute is a bad type
    if (shouldRemoveAttributeWithWarning(name, value, propertyInfo, false)) {
      warnedProperties$1[name] = true;
      return false;
    }

    // Warn when passing the strings 'false' or 'true' into a boolean prop
    if ((value === 'false' || value === 'true') && propertyInfo !== null && propertyInfo.type === BOOLEAN) {
      warning$1(false, 'Received the string `%s` for the boolean attribute `%s`. ' + '%s ' + 'Did you mean %s={%s}?', value, name, value === 'false' ? 'The browser will interpret it as a truthy value.' : 'Although this works, it will not work as expected if you pass the string "false".', name, value);
      warnedProperties$1[name] = true;
      return true;
    }

    return true;
  };
}

var warnUnknownProperties = function (type, props, canUseEventSystem) {
  var unknownProps = [];
  for (var key in props) {
    var isValid = validateProperty$1(type, key, props[key], canUseEventSystem);
    if (!isValid) {
      unknownProps.push(key);
    }
  }

  var unknownPropString = unknownProps.map(function (prop) {
    return '`' + prop + '`';
  }).join(', ');
  if (unknownProps.length === 1) {
    warning$1(false, 'Invalid value for prop %s on <%s> tag. Either remove it from the element, ' + 'or pass a string or number value to keep it in the DOM. ' + 'For details, see https://fb.me/react-attribute-behavior', unknownPropString, type);
  } else if (unknownProps.length > 1) {
    warning$1(false, 'Invalid values for props %s on <%s> tag. Either remove them from the element, ' + 'or pass a string or number value to keep them in the DOM. ' + 'For details, see https://fb.me/react-attribute-behavior', unknownPropString, type);
  }
};

function validateProperties$2(type, props, canUseEventSystem) {
  if (isCustomComponent(type, props)) {
    return;
  }
  warnUnknownProperties(type, props, canUseEventSystem);
}

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

// Based on reading the React.Children implementation. TODO: type this somewhere?

var toArray = React.Children.toArray;

// This is only used in DEV.
// Each entry is `this.stack` from a currently executing renderer instance.
// (There may be more than one because ReactDOMServer is reentrant).
// Each stack is an array of frames which may contain nested stacks of elements.
var currentDebugStacks = [];

var ReactCurrentDispatcher = ReactSharedInternals.ReactCurrentDispatcher;
var ReactDebugCurrentFrame = void 0;
var prevGetCurrentStackImpl = null;
var getCurrentServerStackImpl = function () {
  return '';
};
var describeStackFrame = function (element) {
  return '';
};

var validatePropertiesInDevelopment = function (type, props) {};
var pushCurrentDebugStack = function (stack) {};
var pushElementToDebugStack = function (element) {};
var popCurrentDebugStack = function () {};
var hasWarnedAboutUsingContextAsConsumer = false;

{
  ReactDebugCurrentFrame = ReactSharedInternals.ReactDebugCurrentFrame;

  validatePropertiesInDevelopment = function (type, props) {
    validateProperties(type, props);
    validateProperties$1(type, props);
    validateProperties$2(type, props, /* canUseEventSystem */false);
  };

  describeStackFrame = function (element) {
    var source = element._source;
    var type = element.type;
    var name = getComponentName(type);
    var ownerName = null;
    return describeComponentFrame(name, source, ownerName);
  };

  pushCurrentDebugStack = function (stack) {
    currentDebugStacks.push(stack);

    if (currentDebugStacks.length === 1) {
      // We are entering a server renderer.
      // Remember the previous (e.g. client) global stack implementation.
      prevGetCurrentStackImpl = ReactDebugCurrentFrame.getCurrentStack;
      ReactDebugCurrentFrame.getCurrentStack = getCurrentServerStackImpl;
    }
  };

  pushElementToDebugStack = function (element) {
    // For the innermost executing ReactDOMServer call,
    var stack = currentDebugStacks[currentDebugStacks.length - 1];
    // Take the innermost executing frame (e.g. <Foo>),
    var frame = stack[stack.length - 1];
    // and record that it has one more element associated with it.
    frame.debugElementStack.push(element);
    // We only need this because we tail-optimize single-element
    // children and directly handle them in an inner loop instead of
    // creating separate frames for them.
  };

  popCurrentDebugStack = function () {
    currentDebugStacks.pop();

    if (currentDebugStacks.length === 0) {
      // We are exiting the server renderer.
      // Restore the previous (e.g. client) global stack implementation.
      ReactDebugCurrentFrame.getCurrentStack = prevGetCurrentStackImpl;
      prevGetCurrentStackImpl = null;
    }
  };

  getCurrentServerStackImpl = function () {
    if (currentDebugStacks.length === 0) {
      // Nothing is currently rendering.
      return '';
    }
    // ReactDOMServer is reentrant so there may be multiple calls at the same time.
    // Take the frames from the innermost call which is the last in the array.
    var frames = currentDebugStacks[currentDebugStacks.length - 1];
    var stack = '';
    // Go through every frame in the stack from the innermost one.
    for (var i = frames.length - 1; i >= 0; i--) {
      var frame = frames[i];
      // Every frame might have more than one debug element stack entry associated with it.
      // This is because single-child nesting doesn't create materialized frames.
      // Instead it would push them through `pushElementToDebugStack()`.
      var _debugElementStack = frame.debugElementStack;
      for (var ii = _debugElementStack.length - 1; ii >= 0; ii--) {
        stack += describeStackFrame(_debugElementStack[ii]);
      }
    }
    return stack;
  };
}

var didWarnDefaultInputValue = false;
var didWarnDefaultChecked = false;
var didWarnDefaultSelectValue = false;
var didWarnDefaultTextareaValue = false;
var didWarnInvalidOptionChildren = false;
var didWarnAboutNoopUpdateForComponent = {};
var didWarnAboutBadClass = {};
var didWarnAboutDeprecatedWillMount = {};
var didWarnAboutUndefinedDerivedState = {};
var didWarnAboutUninitializedState = {};
var valuePropNames = ['value', 'defaultValue'];
var newlineEatingTags = {
  listing: true,
  pre: true,
  textarea: true
};

// We accept any tag to be rendered but since this gets injected into arbitrary
// HTML, we want to make sure that it's a safe tag.
// http://www.w3.org/TR/REC-xml/#NT-Name
var VALID_TAG_REGEX = /^[a-zA-Z][a-zA-Z:_\.\-\d]*$/; // Simplified subset
var validatedTagCache = {};
function validateDangerousTag(tag) {
  if (!validatedTagCache.hasOwnProperty(tag)) {
    !VALID_TAG_REGEX.test(tag) ? invariant(false, 'Invalid tag: %s', tag) : void 0;
    validatedTagCache[tag] = true;
  }
}

var styleNameCache = {};
var processStyleName = function (styleName) {
  if (styleNameCache.hasOwnProperty(styleName)) {
    return styleNameCache[styleName];
  }
  var result = hyphenateStyleName(styleName);
  styleNameCache[styleName] = result;
  return result;
};

function createMarkupForStyles(styles) {
  var serialized = '';
  var delimiter = '';
  for (var styleName in styles) {
    if (!styles.hasOwnProperty(styleName)) {
      continue;
    }
    var isCustomProperty = styleName.indexOf('--') === 0;
    var styleValue = styles[styleName];
    {
      if (!isCustomProperty) {
        warnValidStyle$1(styleName, styleValue);
      }
    }
    if (styleValue != null) {
      serialized += delimiter + processStyleName(styleName) + ':';
      serialized += dangerousStyleValue(styleName, styleValue, isCustomProperty);

      delimiter = ';';
    }
  }
  return serialized || null;
}

function warnNoop(publicInstance, callerName) {
  {
    var _constructor = publicInstance.constructor;
    var componentName = _constructor && getComponentName(_constructor) || 'ReactClass';
    var warningKey = componentName + '.' + callerName;
    if (didWarnAboutNoopUpdateForComponent[warningKey]) {
      return;
    }

    warningWithoutStack$1(false, '%s(...): Can only update a mounting component. ' + 'This usually means you called %s() outside componentWillMount() on the server. ' + 'This is a no-op.\n\nPlease check the code for the %s component.', callerName, callerName, componentName);
    didWarnAboutNoopUpdateForComponent[warningKey] = true;
  }
}

function shouldConstruct(Component) {
  return Component.prototype && Component.prototype.isReactComponent;
}

function getNonChildrenInnerMarkup(props) {
  var innerHTML = props.dangerouslySetInnerHTML;
  if (innerHTML != null) {
    if (innerHTML.__html != null) {
      return innerHTML.__html;
    }
  } else {
    var content = props.children;
    if (typeof content === 'string' || typeof content === 'number') {
      return escapeTextForBrowser(content);
    }
  }
  return null;
}

function flattenTopLevelChildren(children) {
  if (!React.isValidElement(children)) {
    return toArray(children);
  }
  var element = children;
  if (element.type !== REACT_FRAGMENT_TYPE) {
    return [element];
  }
  var fragmentChildren = element.props.children;
  if (!React.isValidElement(fragmentChildren)) {
    return toArray(fragmentChildren);
  }
  var fragmentChildElement = fragmentChildren;
  return [fragmentChildElement];
}

function flattenOptionChildren(children) {
  if (children === undefined || children === null) {
    return children;
  }
  var content = '';
  // Flatten children and warn if they aren't strings or numbers;
  // invalid types are ignored.
  React.Children.forEach(children, function (child) {
    if (child == null) {
      return;
    }
    content += child;
    {
      if (!didWarnInvalidOptionChildren && typeof child !== 'string' && typeof child !== 'number') {
        didWarnInvalidOptionChildren = true;
        warning$1(false, 'Only strings and numbers are supported as <option> children.');
      }
    }
  });
  return content;
}

var hasOwnProperty = Object.prototype.hasOwnProperty;
var STYLE = 'style';
var RESERVED_PROPS = {
  children: null,
  dangerouslySetInnerHTML: null,
  suppressContentEditableWarning: null,
  suppressHydrationWarning: null
};

function createOpenTagMarkup(tagVerbatim, tagLowercase, props, namespace, makeStaticMarkup, isRootElement) {
  var ret = '<' + tagVerbatim;

  for (var propKey in props) {
    if (!hasOwnProperty.call(props, propKey)) {
      continue;
    }
    var propValue = props[propKey];
    if (propValue == null) {
      continue;
    }
    if (propKey === STYLE) {
      propValue = createMarkupForStyles(propValue);
    }
    var markup = null;
    if (isCustomComponent(tagLowercase, props)) {
      if (!RESERVED_PROPS.hasOwnProperty(propKey)) {
        markup = createMarkupForCustomAttribute(propKey, propValue);
      }
    } else {
      markup = createMarkupForProperty(propKey, propValue);
    }
    if (markup) {
      ret += ' ' + markup;
    }
  }

  // For static pages, no need to put React ID and checksum. Saves lots of
  // bytes.
  if (makeStaticMarkup) {
    return ret;
  }

  if (isRootElement) {
    ret += ' ' + createMarkupForRoot();
  }
  return ret;
}

function validateRenderResult(child, type) {
  if (child === undefined) {
    invariant(false, '%s(...): Nothing was returned from render. This usually means a return statement is missing. Or, to render nothing, return null.', getComponentName(type) || 'Component');
  }
}

function resolve(child, context, threadID) {
  while (React.isValidElement(child)) {
    // Safe because we just checked it's an element.
    var element = child;
    var Component = element.type;
    {
      pushElementToDebugStack(element);
    }
    if (typeof Component !== 'function') {
      break;
    }
    processChild(element, Component);
  }

  // Extra closure so queue and replace can be captured properly
  function processChild(element, Component) {
    var publicContext = processContext(Component, context, threadID);

    var queue = [];
    var replace = false;
    var updater = {
      isMounted: function (publicInstance) {
        return false;
      },
      enqueueForceUpdate: function (publicInstance) {
        if (queue === null) {
          warnNoop(publicInstance, 'forceUpdate');
          return null;
        }
      },
      enqueueReplaceState: function (publicInstance, completeState) {
        replace = true;
        queue = [completeState];
      },
      enqueueSetState: function (publicInstance, currentPartialState) {
        if (queue === null) {
          warnNoop(publicInstance, 'setState');
          return null;
        }
        queue.push(currentPartialState);
      }
    };

    var inst = void 0;
    if (shouldConstruct(Component)) {
      inst = new Component(element.props, publicContext, updater);

      if (typeof Component.getDerivedStateFromProps === 'function') {
        {
          if (inst.state === null || inst.state === undefined) {
            var componentName = getComponentName(Component) || 'Unknown';
            if (!didWarnAboutUninitializedState[componentName]) {
              warningWithoutStack$1(false, '`%s` uses `getDerivedStateFromProps` but its initial state is ' + '%s. This is not recommended. Instead, define the initial state by ' + 'assigning an object to `this.state` in the constructor of `%s`. ' + 'This ensures that `getDerivedStateFromProps` arguments have a consistent shape.', componentName, inst.state === null ? 'null' : 'undefined', componentName);
              didWarnAboutUninitializedState[componentName] = true;
            }
          }
        }

        var partialState = Component.getDerivedStateFromProps.call(null, element.props, inst.state);

        {
          if (partialState === undefined) {
            var _componentName = getComponentName(Component) || 'Unknown';
            if (!didWarnAboutUndefinedDerivedState[_componentName]) {
              warningWithoutStack$1(false, '%s.getDerivedStateFromProps(): A valid state object (or null) must be returned. ' + 'You have returned undefined.', _componentName);
              didWarnAboutUndefinedDerivedState[_componentName] = true;
            }
          }
        }

        if (partialState != null) {
          inst.state = _assign({}, inst.state, partialState);
        }
      }
    } else {
      {
        if (Component.prototype && typeof Component.prototype.render === 'function') {
          var _componentName2 = getComponentName(Component) || 'Unknown';

          if (!didWarnAboutBadClass[_componentName2]) {
            warningWithoutStack$1(false, "The <%s /> component appears to have a render method, but doesn't extend React.Component. " + 'This is likely to cause errors. Change %s to extend React.Component instead.', _componentName2, _componentName2);
            didWarnAboutBadClass[_componentName2] = true;
          }
        }
      }
      var componentIdentity = {};
      prepareToUseHooks(componentIdentity);
      inst = Component(element.props, publicContext, updater);
      inst = finishHooks(Component, element.props, inst, publicContext);

      if (inst == null || inst.render == null) {
        child = inst;
        validateRenderResult(child, Component);
        return;
      }
    }

    inst.props = element.props;
    inst.context = publicContext;
    inst.updater = updater;

    var initialState = inst.state;
    if (initialState === undefined) {
      inst.state = initialState = null;
    }
    if (typeof inst.UNSAFE_componentWillMount === 'function' || typeof inst.componentWillMount === 'function') {
      if (typeof inst.componentWillMount === 'function') {
        {
          if (warnAboutDeprecatedLifecycles && inst.componentWillMount.__suppressDeprecationWarning !== true) {
            var _componentName3 = getComponentName(Component) || 'Unknown';

            if (!didWarnAboutDeprecatedWillMount[_componentName3]) {
              lowPriorityWarning$1(false, '%s: componentWillMount() is deprecated and will be ' + 'removed in the next major version. Read about the motivations ' + 'behind this change: ' + 'https://fb.me/react-async-component-lifecycle-hooks' + '\n\n' + 'As a temporary workaround, you can rename to ' + 'UNSAFE_componentWillMount instead.', _componentName3);
              didWarnAboutDeprecatedWillMount[_componentName3] = true;
            }
          }
        }

        // In order to support react-lifecycles-compat polyfilled components,
        // Unsafe lifecycles should not be invoked for any component with the new gDSFP.
        if (typeof Component.getDerivedStateFromProps !== 'function') {
          inst.componentWillMount();
        }
      }
      if (typeof inst.UNSAFE_componentWillMount === 'function' && typeof Component.getDerivedStateFromProps !== 'function') {
        // In order to support react-lifecycles-compat polyfilled components,
        // Unsafe lifecycles should not be invoked for any component with the new gDSFP.
        inst.UNSAFE_componentWillMount();
      }
      if (queue.length) {
        var oldQueue = queue;
        var oldReplace = replace;
        queue = null;
        replace = false;

        if (oldReplace && oldQueue.length === 1) {
          inst.state = oldQueue[0];
        } else {
          var nextState = oldReplace ? oldQueue[0] : inst.state;
          var dontMutate = true;
          for (var i = oldReplace ? 1 : 0; i < oldQueue.length; i++) {
            var partial = oldQueue[i];
            var _partialState = typeof partial === 'function' ? partial.call(inst, nextState, element.props, publicContext) : partial;
            if (_partialState != null) {
              if (dontMutate) {
                dontMutate = false;
                nextState = _assign({}, nextState, _partialState);
              } else {
                _assign(nextState, _partialState);
              }
            }
          }
          inst.state = nextState;
        }
      } else {
        queue = null;
      }
    }
    child = inst.render();

    {
      if (child === undefined && inst.render._isMockFunction) {
        // This is probably bad practice. Consider warning here and
        // deprecating this convenience.
        child = null;
      }
    }
    validateRenderResult(child, Component);

    var childContext = void 0;
    if (typeof inst.getChildContext === 'function') {
      var childContextTypes = Component.childContextTypes;
      if (typeof childContextTypes === 'object') {
        childContext = inst.getChildContext();
        for (var contextKey in childContext) {
          !(contextKey in childContextTypes) ? invariant(false, '%s.getChildContext(): key "%s" is not defined in childContextTypes.', getComponentName(Component) || 'Unknown', contextKey) : void 0;
        }
      } else {
        warningWithoutStack$1(false, '%s.getChildContext(): childContextTypes must be defined in order to ' + 'use getChildContext().', getComponentName(Component) || 'Unknown');
      }
    }
    if (childContext) {
      context = _assign({}, context, childContext);
    }
  }
  return { child: child, context: context };
}

var ReactDOMServerRenderer = function () {
  // DEV-only

  // TODO: type this more strictly:
  function ReactDOMServerRenderer(children, makeStaticMarkup) {
    _classCallCheck(this, ReactDOMServerRenderer);

    var flatChildren = flattenTopLevelChildren(children);

    var topFrame = {
      type: null,
      // Assume all trees start in the HTML namespace (not totally true, but
      // this is what we did historically)
      domNamespace: Namespaces.html,
      children: flatChildren,
      childIndex: 0,
      context: emptyObject,
      footer: ''
    };
    {
      topFrame.debugElementStack = [];
    }
    this.threadID = allocThreadID();
    this.stack = [topFrame];
    this.exhausted = false;
    this.currentSelectValue = null;
    this.previousWasTextNode = false;
    this.makeStaticMarkup = makeStaticMarkup;
    this.suspenseDepth = 0;

    // Context (new API)
    this.contextIndex = -1;
    this.contextStack = [];
    this.contextValueStack = [];
    {
      this.contextProviderStack = [];
    }
  }

  ReactDOMServerRenderer.prototype.destroy = function destroy() {
    if (!this.exhausted) {
      this.exhausted = true;
      this.clearProviders();
      freeThreadID(this.threadID);
    }
  };

  /**
   * Note: We use just two stacks regardless of how many context providers you have.
   * Providers are always popped in the reverse order to how they were pushed
   * so we always know on the way down which provider you'll encounter next on the way up.
   * On the way down, we push the current provider, and its context value *before*
   * we mutated it, onto the stacks. Therefore, on the way up, we always know which
   * provider needs to be "restored" to which value.
   * https://github.com/facebook/react/pull/12985#issuecomment-396301248
   */

  ReactDOMServerRenderer.prototype.pushProvider = function pushProvider(provider) {
    var index = ++this.contextIndex;
    var context = provider.type._context;
    var threadID = this.threadID;
    validateContextBounds(context, threadID);
    var previousValue = context[threadID];

    // Remember which value to restore this context to on our way up.
    this.contextStack[index] = context;
    this.contextValueStack[index] = previousValue;
    {
      // Only used for push/pop mismatch warnings.
      this.contextProviderStack[index] = provider;
    }

    // Mutate the current value.
    context[threadID] = provider.props.value;
  };

  ReactDOMServerRenderer.prototype.popProvider = function popProvider(provider) {
    var index = this.contextIndex;
    {
      !(index > -1 && provider === this.contextProviderStack[index]) ? warningWithoutStack$1(false, 'Unexpected pop.') : void 0;
    }

    var context = this.contextStack[index];
    var previousValue = this.contextValueStack[index];

    // "Hide" these null assignments from Flow by using `any`
    // because conceptually they are deletions--as long as we
    // promise to never access values beyond `this.contextIndex`.
    this.contextStack[index] = null;
    this.contextValueStack[index] = null;
    {
      this.contextProviderStack[index] = null;
    }
    this.contextIndex--;

    // Restore to the previous value we stored as we were walking down.
    // We've already verified that this context has been expanded to accommodate
    // this thread id, so we don't need to do it again.
    context[this.threadID] = previousValue;
  };

  ReactDOMServerRenderer.prototype.clearProviders = function clearProviders() {
    // Restore any remaining providers on the stack to previous values
    for (var index = this.contextIndex; index >= 0; index--) {
      var _context = this.contextStack[index];
      var previousValue = this.contextValueStack[index];
      _context[this.threadID] = previousValue;
    }
  };

  ReactDOMServerRenderer.prototype.read = function read(bytes) {
    if (this.exhausted) {
      return null;
    }

    var prevThreadID = currentThreadID;
    setCurrentThreadID(this.threadID);
    var prevDispatcher = ReactCurrentDispatcher.current;
    ReactCurrentDispatcher.current = Dispatcher;
    try {
      // Markup generated within <Suspense> ends up buffered until we know
      // nothing in that boundary suspended
      var out = [''];
      var suspended = false;
      while (out[0].length < bytes) {
        if (this.stack.length === 0) {
          this.exhausted = true;
          freeThreadID(this.threadID);
          break;
        }
        var frame = this.stack[this.stack.length - 1];
        if (suspended || frame.childIndex >= frame.children.length) {
          var _footer = frame.footer;
          if (_footer !== '') {
            this.previousWasTextNode = false;
          }
          this.stack.pop();
          if (frame.type === 'select') {
            this.currentSelectValue = null;
          } else if (frame.type != null && frame.type.type != null && frame.type.type.$$typeof === REACT_PROVIDER_TYPE) {
            var provider = frame.type;
            this.popProvider(provider);
          } else if (frame.type === REACT_SUSPENSE_TYPE) {
            this.suspenseDepth--;
            var buffered = out.pop();

            if (suspended) {
              suspended = false;
              // If rendering was suspended at this boundary, render the fallbackFrame
              var _fallbackFrame = frame.fallbackFrame;
              !_fallbackFrame ? invariant(false, 'suspense fallback not found, something is broken') : void 0;
              this.stack.push(_fallbackFrame);
              // Skip flushing output since we're switching to the fallback
              continue;
            } else {
              out[this.suspenseDepth] += buffered;
            }
          }

          // Flush output
          out[this.suspenseDepth] += _footer;
          continue;
        }
        var child = frame.children[frame.childIndex++];

        var outBuffer = '';
        {
          pushCurrentDebugStack(this.stack);
          // We're starting work on this frame, so reset its inner stack.
          frame.debugElementStack.length = 0;
        }
        try {
          outBuffer += this.render(child, frame.context, frame.domNamespace);
        } catch (err) {
          if (enableSuspenseServerRenderer && typeof err.then === 'function') {
            suspended = true;
          } else {
            throw err;
          }
        } finally {
          {
            popCurrentDebugStack();
          }
        }
        if (out.length <= this.suspenseDepth) {
          out.push('');
        }
        out[this.suspenseDepth] += outBuffer;
      }
      return out[0];
    } finally {
      ReactCurrentDispatcher.current = prevDispatcher;
      setCurrentThreadID(prevThreadID);
    }
  };

  ReactDOMServerRenderer.prototype.render = function render(child, context, parentNamespace) {
    if (typeof child === 'string' || typeof child === 'number') {
      var text = '' + child;
      if (text === '') {
        return '';
      }
      if (this.makeStaticMarkup) {
        return escapeTextForBrowser(text);
      }
      if (this.previousWasTextNode) {
        return '<!-- -->' + escapeTextForBrowser(text);
      }
      this.previousWasTextNode = true;
      return escapeTextForBrowser(text);
    } else {
      var nextChild = void 0;

      var _resolve = resolve(child, context, this.threadID);

      nextChild = _resolve.child;
      context = _resolve.context;

      if (nextChild === null || nextChild === false) {
        return '';
      } else if (!React.isValidElement(nextChild)) {
        if (nextChild != null && nextChild.$$typeof != null) {
          // Catch unexpected special types early.
          var $$typeof = nextChild.$$typeof;
          !($$typeof !== REACT_PORTAL_TYPE) ? invariant(false, 'Portals are not currently supported by the server renderer. Render them conditionally so that they only appear on the client render.') : void 0;
          // Catch-all to prevent an infinite loop if React.Children.toArray() supports some new type.
          invariant(false, 'Unknown element-like object type: %s. This is likely a bug in React. Please file an issue.', $$typeof.toString());
        }
        var nextChildren = toArray(nextChild);
        var frame = {
          type: null,
          domNamespace: parentNamespace,
          children: nextChildren,
          childIndex: 0,
          context: context,
          footer: ''
        };
        {
          frame.debugElementStack = [];
        }
        this.stack.push(frame);
        return '';
      }
      // Safe because we just checked it's an element.
      var nextElement = nextChild;
      var elementType = nextElement.type;

      if (typeof elementType === 'string') {
        return this.renderDOM(nextElement, context, parentNamespace);
      }

      switch (elementType) {
        case REACT_STRICT_MODE_TYPE:
        case REACT_CONCURRENT_MODE_TYPE:
        case REACT_PROFILER_TYPE:
        case REACT_FRAGMENT_TYPE:
          {
            var _nextChildren = toArray(nextChild.props.children);
            var _frame = {
              type: null,
              domNamespace: parentNamespace,
              children: _nextChildren,
              childIndex: 0,
              context: context,
              footer: ''
            };
            {
              _frame.debugElementStack = [];
            }
            this.stack.push(_frame);
            return '';
          }
        case REACT_SUSPENSE_TYPE:
          {
            if (enableSuspenseServerRenderer) {
              var fallback = nextChild.props.fallback;
              if (fallback === undefined) {
                // If there is no fallback, then this just behaves as a fragment.
                var _nextChildren3 = toArray(nextChild.props.children);
                var _frame3 = {
                  type: null,
                  domNamespace: parentNamespace,
                  children: _nextChildren3,
                  childIndex: 0,
                  context: context,
                  footer: ''
                };
                {
                  _frame3.debugElementStack = [];
                }
                this.stack.push(_frame3);
                return '';
              }
              var fallbackChildren = toArray(fallback);
              var _nextChildren2 = toArray(nextChild.props.children);
              var _fallbackFrame2 = {
                type: null,
                domNamespace: parentNamespace,
                children: fallbackChildren,
                childIndex: 0,
                context: context,
                footer: '',
                out: ''
              };
              var _frame2 = {
                fallbackFrame: _fallbackFrame2,
                type: REACT_SUSPENSE_TYPE,
                domNamespace: parentNamespace,
                children: _nextChildren2,
                childIndex: 0,
                context: context,
                footer: '<!--/$-->'
              };
              {
                _frame2.debugElementStack = [];
                _fallbackFrame2.debugElementStack = [];
              }
              this.stack.push(_frame2);
              this.suspenseDepth++;
              return '<!--$-->';
            } else {
              invariant(false, 'ReactDOMServer does not yet support Suspense.');
            }
          }
        // eslint-disable-next-line-no-fallthrough
        default:
          break;
      }
      if (typeof elementType === 'object' && elementType !== null) {
        switch (elementType.$$typeof) {
          case REACT_FORWARD_REF_TYPE:
            {
              var element = nextChild;
              var _nextChildren4 = void 0;
              var componentIdentity = {};
              prepareToUseHooks(componentIdentity);
              _nextChildren4 = elementType.render(element.props, element.ref);
              _nextChildren4 = finishHooks(elementType.render, element.props, _nextChildren4, element.ref);
              _nextChildren4 = toArray(_nextChildren4);
              var _frame4 = {
                type: null,
                domNamespace: parentNamespace,
                children: _nextChildren4,
                childIndex: 0,
                context: context,
                footer: ''
              };
              {
                _frame4.debugElementStack = [];
              }
              this.stack.push(_frame4);
              return '';
            }
          case REACT_MEMO_TYPE:
            {
              var _element = nextChild;
              var _nextChildren5 = [React.createElement(elementType.type, _assign({ ref: _element.ref }, _element.props))];
              var _frame5 = {
                type: null,
                domNamespace: parentNamespace,
                children: _nextChildren5,
                childIndex: 0,
                context: context,
                footer: ''
              };
              {
                _frame5.debugElementStack = [];
              }
              this.stack.push(_frame5);
              return '';
            }
          case REACT_PROVIDER_TYPE:
            {
              var provider = nextChild;
              var nextProps = provider.props;
              var _nextChildren6 = toArray(nextProps.children);
              var _frame6 = {
                type: provider,
                domNamespace: parentNamespace,
                children: _nextChildren6,
                childIndex: 0,
                context: context,
                footer: ''
              };
              {
                _frame6.debugElementStack = [];
              }

              this.pushProvider(provider);

              this.stack.push(_frame6);
              return '';
            }
          case REACT_CONTEXT_TYPE:
            {
              var reactContext = nextChild.type;
              // The logic below for Context differs depending on PROD or DEV mode. In
              // DEV mode, we create a separate object for Context.Consumer that acts
              // like a proxy to Context. This proxy object adds unnecessary code in PROD
              // so we use the old behaviour (Context.Consumer references Context) to
              // reduce size and overhead. The separate object references context via
              // a property called "_context", which also gives us the ability to check
              // in DEV mode if this property exists or not and warn if it does not.
              {
                if (reactContext._context === undefined) {
                  // This may be because it's a Context (rather than a Consumer).
                  // Or it may be because it's older React where they're the same thing.
                  // We only want to warn if we're sure it's a new React.
                  if (reactContext !== reactContext.Consumer) {
                    if (!hasWarnedAboutUsingContextAsConsumer) {
                      hasWarnedAboutUsingContextAsConsumer = true;
                      warning$1(false, 'Rendering <Context> directly is not supported and will be removed in ' + 'a future major release. Did you mean to render <Context.Consumer> instead?');
                    }
                  }
                } else {
                  reactContext = reactContext._context;
                }
              }
              var _nextProps = nextChild.props;
              var threadID = this.threadID;
              validateContextBounds(reactContext, threadID);
              var nextValue = reactContext[threadID];

              var _nextChildren7 = toArray(_nextProps.children(nextValue));
              var _frame7 = {
                type: nextChild,
                domNamespace: parentNamespace,
                children: _nextChildren7,
                childIndex: 0,
                context: context,
                footer: ''
              };
              {
                _frame7.debugElementStack = [];
              }
              this.stack.push(_frame7);
              return '';
            }
          case REACT_LAZY_TYPE:
            invariant(false, 'ReactDOMServer does not yet support lazy-loaded components.');
        }
      }

      var info = '';
      {
        var owner = nextElement._owner;
        if (elementType === undefined || typeof elementType === 'object' && elementType !== null && Object.keys(elementType).length === 0) {
          info += ' You likely forgot to export your component from the file ' + "it's defined in, or you might have mixed up default and " + 'named imports.';
        }
        var ownerName = owner ? getComponentName(owner) : null;
        if (ownerName) {
          info += '\n\nCheck the render method of `' + ownerName + '`.';
        }
      }
      invariant(false, 'Element type is invalid: expected a string (for built-in components) or a class/function (for composite components) but got: %s.%s', elementType == null ? elementType : typeof elementType, info);
    }
  };

  ReactDOMServerRenderer.prototype.renderDOM = function renderDOM(element, context, parentNamespace) {
    var tag = element.type.toLowerCase();

    var namespace = parentNamespace;
    if (parentNamespace === Namespaces.html) {
      namespace = getIntrinsicNamespace(tag);
    }

    {
      if (namespace === Namespaces.html) {
        // Should this check be gated by parent namespace? Not sure we want to
        // allow <SVG> or <mATH>.
        !(tag === element.type) ? warning$1(false, '<%s /> is using incorrect casing. ' + 'Use PascalCase for React components, ' + 'or lowercase for HTML elements.', element.type) : void 0;
      }
    }

    validateDangerousTag(tag);

    var props = element.props;
    if (tag === 'input') {
      {
        ReactControlledValuePropTypes.checkPropTypes('input', props);

        if (props.checked !== undefined && props.defaultChecked !== undefined && !didWarnDefaultChecked) {
          warning$1(false, '%s contains an input of type %s with both checked and defaultChecked props. ' + 'Input elements must be either controlled or uncontrolled ' + '(specify either the checked prop, or the defaultChecked prop, but not ' + 'both). Decide between using a controlled or uncontrolled input ' + 'element and remove one of these props. More info: ' + 'https://fb.me/react-controlled-components', 'A component', props.type);
          didWarnDefaultChecked = true;
        }
        if (props.value !== undefined && props.defaultValue !== undefined && !didWarnDefaultInputValue) {
          warning$1(false, '%s contains an input of type %s with both value and defaultValue props. ' + 'Input elements must be either controlled or uncontrolled ' + '(specify either the value prop, or the defaultValue prop, but not ' + 'both). Decide between using a controlled or uncontrolled input ' + 'element and remove one of these props. More info: ' + 'https://fb.me/react-controlled-components', 'A component', props.type);
          didWarnDefaultInputValue = true;
        }
      }

      props = _assign({
        type: undefined
      }, props, {
        defaultChecked: undefined,
        defaultValue: undefined,
        value: props.value != null ? props.value : props.defaultValue,
        checked: props.checked != null ? props.checked : props.defaultChecked
      });
    } else if (tag === 'textarea') {
      {
        ReactControlledValuePropTypes.checkPropTypes('textarea', props);
        if (props.value !== undefined && props.defaultValue !== undefined && !didWarnDefaultTextareaValue) {
          warning$1(false, 'Textarea elements must be either controlled or uncontrolled ' + '(specify either the value prop, or the defaultValue prop, but not ' + 'both). Decide between using a controlled or uncontrolled textarea ' + 'and remove one of these props. More info: ' + 'https://fb.me/react-controlled-components');
          didWarnDefaultTextareaValue = true;
        }
      }

      var initialValue = props.value;
      if (initialValue == null) {
        var defaultValue = props.defaultValue;
        // TODO (yungsters): Remove support for children content in <textarea>.
        var textareaChildren = props.children;
        if (textareaChildren != null) {
          {
            warning$1(false, 'Use the `defaultValue` or `value` props instead of setting ' + 'children on <textarea>.');
          }
          !(defaultValue == null) ? invariant(false, 'If you supply `defaultValue` on a <textarea>, do not pass children.') : void 0;
          if (Array.isArray(textareaChildren)) {
            !(textareaChildren.length <= 1) ? invariant(false, '<textarea> can only have at most one child.') : void 0;
            textareaChildren = textareaChildren[0];
          }

          defaultValue = '' + textareaChildren;
        }
        if (defaultValue == null) {
          defaultValue = '';
        }
        initialValue = defaultValue;
      }

      props = _assign({}, props, {
        value: undefined,
        children: '' + initialValue
      });
    } else if (tag === 'select') {
      {
        ReactControlledValuePropTypes.checkPropTypes('select', props);

        for (var i = 0; i < valuePropNames.length; i++) {
          var propName = valuePropNames[i];
          if (props[propName] == null) {
            continue;
          }
          var isArray = Array.isArray(props[propName]);
          if (props.multiple && !isArray) {
            warning$1(false, 'The `%s` prop supplied to <select> must be an array if ' + '`multiple` is true.', propName);
          } else if (!props.multiple && isArray) {
            warning$1(false, 'The `%s` prop supplied to <select> must be a scalar ' + 'value if `multiple` is false.', propName);
          }
        }

        if (props.value !== undefined && props.defaultValue !== undefined && !didWarnDefaultSelectValue) {
          warning$1(false, 'Select elements must be either controlled or uncontrolled ' + '(specify either the value prop, or the defaultValue prop, but not ' + 'both). Decide between using a controlled or uncontrolled select ' + 'element and remove one of these props. More info: ' + 'https://fb.me/react-controlled-components');
          didWarnDefaultSelectValue = true;
        }
      }
      this.currentSelectValue = props.value != null ? props.value : props.defaultValue;
      props = _assign({}, props, {
        value: undefined
      });
    } else if (tag === 'option') {
      var selected = null;
      var selectValue = this.currentSelectValue;
      var optionChildren = flattenOptionChildren(props.children);
      if (selectValue != null) {
        var value = void 0;
        if (props.value != null) {
          value = props.value + '';
        } else {
          value = optionChildren;
        }
        selected = false;
        if (Array.isArray(selectValue)) {
          // multiple
          for (var j = 0; j < selectValue.length; j++) {
            if ('' + selectValue[j] === value) {
              selected = true;
              break;
            }
          }
        } else {
          selected = '' + selectValue === value;
        }

        props = _assign({
          selected: undefined,
          children: undefined
        }, props, {
          selected: selected,
          children: optionChildren
        });
      }
    }

    {
      validatePropertiesInDevelopment(tag, props);
    }

    assertValidProps(tag, props);

    var out = createOpenTagMarkup(element.type, tag, props, namespace, this.makeStaticMarkup, this.stack.length === 1);
    var footer = '';
    if (omittedCloseTags.hasOwnProperty(tag)) {
      out += '/>';
    } else {
      out += '>';
      footer = '</' + element.type + '>';
    }
    var children = void 0;
    var innerMarkup = getNonChildrenInnerMarkup(props);
    if (innerMarkup != null) {
      children = [];
      if (newlineEatingTags[tag] && innerMarkup.charAt(0) === '\n') {
        // text/html ignores the first character in these tags if it's a newline
        // Prefer to break application/xml over text/html (for now) by adding
        // a newline specifically to get eaten by the parser. (Alternately for
        // textareas, replacing "^\n" with "\r\n" doesn't get eaten, and the first
        // \r is normalized out by HTMLTextAreaElement#value.)
        // See: <http://www.w3.org/TR/html-polyglot/#newlines-in-textarea-and-pre>
        // See: <http://www.w3.org/TR/html5/syntax.html#element-restrictions>
        // See: <http://www.w3.org/TR/html5/syntax.html#newlines>
        // See: Parsing of "textarea" "listing" and "pre" elements
        //  from <http://www.w3.org/TR/html5/syntax.html#parsing-main-inbody>
        out += '\n';
      }
      out += innerMarkup;
    } else {
      children = toArray(props.children);
    }
    var frame = {
      domNamespace: getChildNamespace(parentNamespace, element.type),
      type: tag,
      children: children,
      childIndex: 0,
      context: context,
      footer: footer
    };
    {
      frame.debugElementStack = [];
    }
    this.stack.push(frame);
    this.previousWasTextNode = false;
    return out;
  };

  return ReactDOMServerRenderer;
}();

/**
 * Render a ReactElement to its initial HTML. This should only be used on the
 * server.
 * See https://reactjs.org/docs/react-dom-server.html#rendertostring
 */
function renderToString(element) {
  var renderer = new ReactDOMServerRenderer(element, false);
  try {
    var markup = renderer.read(Infinity);
    return markup;
  } finally {
    renderer.destroy();
  }
}

/**
 * Similar to renderToString, except this doesn't create extra DOM attributes
 * such as data-react-id that React uses internally.
 * See https://reactjs.org/docs/react-dom-server.html#rendertostaticmarkup
 */
function renderToStaticMarkup(element) {
  var renderer = new ReactDOMServerRenderer(element, true);
  try {
    var markup = renderer.read(Infinity);
    return markup;
  } finally {
    renderer.destroy();
  }
}

function renderToNodeStream() {
  invariant(false, 'ReactDOMServer.renderToNodeStream(): The streaming API is not available in the browser. Use ReactDOMServer.renderToString() instead.');
}

function renderToStaticNodeStream() {
  invariant(false, 'ReactDOMServer.renderToStaticNodeStream(): The streaming API is not available in the browser. Use ReactDOMServer.renderToStaticMarkup() instead.');
}

// Note: when changing this, also consider https://github.com/facebook/react/issues/11526
var ReactDOMServerBrowser = {
  renderToString: renderToString,
  renderToStaticMarkup: renderToStaticMarkup,
  renderToNodeStream: renderToNodeStream,
  renderToStaticNodeStream: renderToStaticNodeStream,
  version: ReactVersion
};

var ReactDOMServerBrowser$1 = Object.freeze({
	default: ReactDOMServerBrowser
});

var ReactDOMServer = ( ReactDOMServerBrowser$1 && ReactDOMServerBrowser ) || ReactDOMServerBrowser$1;

// TODO: decide on the top-level export form.
// This is hacky but makes it work with both Rollup and Jest
var server_browser = ReactDOMServer.default || ReactDOMServer;

module.exports = server_browser;
  })();
}


/***/ }),

/***/ "./node_modules/react-dom/server.browser.js":
/*!**************************************************!*\
  !*** ./node_modules/react-dom/server.browser.js ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (false) {} else {
  module.exports = __webpack_require__(/*! ./cjs/react-dom-server.browser.development.js */ "./node_modules/react-dom/cjs/react-dom-server.browser.development.js");
}


/***/ }),

/***/ "./node_modules/react-is/cjs/react-is.development.js":
/*!***********************************************************!*\
  !*** ./node_modules/react-is/cjs/react-is.development.js ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v16.8.4
 * react-is.development.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */





if (true) {
  (function() {
'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

// The Symbol used to tag the ReactElement-like types. If there is no native Symbol
// nor polyfill, then a plain number is used for performance.
var hasSymbol = typeof Symbol === 'function' && Symbol.for;

var REACT_ELEMENT_TYPE = hasSymbol ? Symbol.for('react.element') : 0xeac7;
var REACT_PORTAL_TYPE = hasSymbol ? Symbol.for('react.portal') : 0xeaca;
var REACT_FRAGMENT_TYPE = hasSymbol ? Symbol.for('react.fragment') : 0xeacb;
var REACT_STRICT_MODE_TYPE = hasSymbol ? Symbol.for('react.strict_mode') : 0xeacc;
var REACT_PROFILER_TYPE = hasSymbol ? Symbol.for('react.profiler') : 0xead2;
var REACT_PROVIDER_TYPE = hasSymbol ? Symbol.for('react.provider') : 0xeacd;
var REACT_CONTEXT_TYPE = hasSymbol ? Symbol.for('react.context') : 0xeace;
var REACT_ASYNC_MODE_TYPE = hasSymbol ? Symbol.for('react.async_mode') : 0xeacf;
var REACT_CONCURRENT_MODE_TYPE = hasSymbol ? Symbol.for('react.concurrent_mode') : 0xeacf;
var REACT_FORWARD_REF_TYPE = hasSymbol ? Symbol.for('react.forward_ref') : 0xead0;
var REACT_SUSPENSE_TYPE = hasSymbol ? Symbol.for('react.suspense') : 0xead1;
var REACT_MEMO_TYPE = hasSymbol ? Symbol.for('react.memo') : 0xead3;
var REACT_LAZY_TYPE = hasSymbol ? Symbol.for('react.lazy') : 0xead4;

function isValidElementType(type) {
  return typeof type === 'string' || typeof type === 'function' ||
  // Note: its typeof might be other than 'symbol' or 'number' if it's a polyfill.
  type === REACT_FRAGMENT_TYPE || type === REACT_CONCURRENT_MODE_TYPE || type === REACT_PROFILER_TYPE || type === REACT_STRICT_MODE_TYPE || type === REACT_SUSPENSE_TYPE || typeof type === 'object' && type !== null && (type.$$typeof === REACT_LAZY_TYPE || type.$$typeof === REACT_MEMO_TYPE || type.$$typeof === REACT_PROVIDER_TYPE || type.$$typeof === REACT_CONTEXT_TYPE || type.$$typeof === REACT_FORWARD_REF_TYPE);
}

/**
 * Forked from fbjs/warning:
 * https://github.com/facebook/fbjs/blob/e66ba20ad5be433eb54423f2b097d829324d9de6/packages/fbjs/src/__forks__/warning.js
 *
 * Only change is we use console.warn instead of console.error,
 * and do nothing when 'console' is not supported.
 * This really simplifies the code.
 * ---
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var lowPriorityWarning = function () {};

{
  var printWarning = function (format) {
    for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      args[_key - 1] = arguments[_key];
    }

    var argIndex = 0;
    var message = 'Warning: ' + format.replace(/%s/g, function () {
      return args[argIndex++];
    });
    if (typeof console !== 'undefined') {
      console.warn(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };

  lowPriorityWarning = function (condition, format) {
    if (format === undefined) {
      throw new Error('`lowPriorityWarning(condition, format, ...args)` requires a warning ' + 'message argument');
    }
    if (!condition) {
      for (var _len2 = arguments.length, args = Array(_len2 > 2 ? _len2 - 2 : 0), _key2 = 2; _key2 < _len2; _key2++) {
        args[_key2 - 2] = arguments[_key2];
      }

      printWarning.apply(undefined, [format].concat(args));
    }
  };
}

var lowPriorityWarning$1 = lowPriorityWarning;

function typeOf(object) {
  if (typeof object === 'object' && object !== null) {
    var $$typeof = object.$$typeof;
    switch ($$typeof) {
      case REACT_ELEMENT_TYPE:
        var type = object.type;

        switch (type) {
          case REACT_ASYNC_MODE_TYPE:
          case REACT_CONCURRENT_MODE_TYPE:
          case REACT_FRAGMENT_TYPE:
          case REACT_PROFILER_TYPE:
          case REACT_STRICT_MODE_TYPE:
          case REACT_SUSPENSE_TYPE:
            return type;
          default:
            var $$typeofType = type && type.$$typeof;

            switch ($$typeofType) {
              case REACT_CONTEXT_TYPE:
              case REACT_FORWARD_REF_TYPE:
              case REACT_PROVIDER_TYPE:
                return $$typeofType;
              default:
                return $$typeof;
            }
        }
      case REACT_LAZY_TYPE:
      case REACT_MEMO_TYPE:
      case REACT_PORTAL_TYPE:
        return $$typeof;
    }
  }

  return undefined;
}

// AsyncMode is deprecated along with isAsyncMode
var AsyncMode = REACT_ASYNC_MODE_TYPE;
var ConcurrentMode = REACT_CONCURRENT_MODE_TYPE;
var ContextConsumer = REACT_CONTEXT_TYPE;
var ContextProvider = REACT_PROVIDER_TYPE;
var Element = REACT_ELEMENT_TYPE;
var ForwardRef = REACT_FORWARD_REF_TYPE;
var Fragment = REACT_FRAGMENT_TYPE;
var Lazy = REACT_LAZY_TYPE;
var Memo = REACT_MEMO_TYPE;
var Portal = REACT_PORTAL_TYPE;
var Profiler = REACT_PROFILER_TYPE;
var StrictMode = REACT_STRICT_MODE_TYPE;
var Suspense = REACT_SUSPENSE_TYPE;

var hasWarnedAboutDeprecatedIsAsyncMode = false;

// AsyncMode should be deprecated
function isAsyncMode(object) {
  {
    if (!hasWarnedAboutDeprecatedIsAsyncMode) {
      hasWarnedAboutDeprecatedIsAsyncMode = true;
      lowPriorityWarning$1(false, 'The ReactIs.isAsyncMode() alias has been deprecated, ' + 'and will be removed in React 17+. Update your code to use ' + 'ReactIs.isConcurrentMode() instead. It has the exact same API.');
    }
  }
  return isConcurrentMode(object) || typeOf(object) === REACT_ASYNC_MODE_TYPE;
}
function isConcurrentMode(object) {
  return typeOf(object) === REACT_CONCURRENT_MODE_TYPE;
}
function isContextConsumer(object) {
  return typeOf(object) === REACT_CONTEXT_TYPE;
}
function isContextProvider(object) {
  return typeOf(object) === REACT_PROVIDER_TYPE;
}
function isElement(object) {
  return typeof object === 'object' && object !== null && object.$$typeof === REACT_ELEMENT_TYPE;
}
function isForwardRef(object) {
  return typeOf(object) === REACT_FORWARD_REF_TYPE;
}
function isFragment(object) {
  return typeOf(object) === REACT_FRAGMENT_TYPE;
}
function isLazy(object) {
  return typeOf(object) === REACT_LAZY_TYPE;
}
function isMemo(object) {
  return typeOf(object) === REACT_MEMO_TYPE;
}
function isPortal(object) {
  return typeOf(object) === REACT_PORTAL_TYPE;
}
function isProfiler(object) {
  return typeOf(object) === REACT_PROFILER_TYPE;
}
function isStrictMode(object) {
  return typeOf(object) === REACT_STRICT_MODE_TYPE;
}
function isSuspense(object) {
  return typeOf(object) === REACT_SUSPENSE_TYPE;
}

exports.typeOf = typeOf;
exports.AsyncMode = AsyncMode;
exports.ConcurrentMode = ConcurrentMode;
exports.ContextConsumer = ContextConsumer;
exports.ContextProvider = ContextProvider;
exports.Element = Element;
exports.ForwardRef = ForwardRef;
exports.Fragment = Fragment;
exports.Lazy = Lazy;
exports.Memo = Memo;
exports.Portal = Portal;
exports.Profiler = Profiler;
exports.StrictMode = StrictMode;
exports.Suspense = Suspense;
exports.isValidElementType = isValidElementType;
exports.isAsyncMode = isAsyncMode;
exports.isConcurrentMode = isConcurrentMode;
exports.isContextConsumer = isContextConsumer;
exports.isContextProvider = isContextProvider;
exports.isElement = isElement;
exports.isForwardRef = isForwardRef;
exports.isFragment = isFragment;
exports.isLazy = isLazy;
exports.isMemo = isMemo;
exports.isPortal = isPortal;
exports.isProfiler = isProfiler;
exports.isStrictMode = isStrictMode;
exports.isSuspense = isSuspense;
  })();
}


/***/ }),

/***/ "./node_modules/react-is/index.js":
/*!****************************************!*\
  !*** ./node_modules/react-is/index.js ***!
  \****************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (false) {} else {
  module.exports = __webpack_require__(/*! ./cjs/react-is.development.js */ "./node_modules/react-is/cjs/react-is.development.js");
}


/***/ }),

/***/ "./node_modules/react-lifecycles-compat/react-lifecycles-compat.es.js":
/*!****************************************************************************!*\
  !*** ./node_modules/react-lifecycles-compat/react-lifecycles-compat.es.js ***!
  \****************************************************************************/
/*! exports provided: polyfill */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "polyfill", function() { return polyfill; });
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

function componentWillMount() {
  // Call this.constructor.gDSFP to support sub-classes.
  var state = this.constructor.getDerivedStateFromProps(this.props, this.state);
  if (state !== null && state !== undefined) {
    this.setState(state);
  }
}

function componentWillReceiveProps(nextProps) {
  // Call this.constructor.gDSFP to support sub-classes.
  // Use the setState() updater to ensure state isn't stale in certain edge cases.
  function updater(prevState) {
    var state = this.constructor.getDerivedStateFromProps(nextProps, prevState);
    return state !== null && state !== undefined ? state : null;
  }
  // Binding "this" is important for shallow renderer support.
  this.setState(updater.bind(this));
}

function componentWillUpdate(nextProps, nextState) {
  try {
    var prevProps = this.props;
    var prevState = this.state;
    this.props = nextProps;
    this.state = nextState;
    this.__reactInternalSnapshotFlag = true;
    this.__reactInternalSnapshot = this.getSnapshotBeforeUpdate(
      prevProps,
      prevState
    );
  } finally {
    this.props = prevProps;
    this.state = prevState;
  }
}

// React may warn about cWM/cWRP/cWU methods being deprecated.
// Add a flag to suppress these warnings for this special case.
componentWillMount.__suppressDeprecationWarning = true;
componentWillReceiveProps.__suppressDeprecationWarning = true;
componentWillUpdate.__suppressDeprecationWarning = true;

function polyfill(Component) {
  var prototype = Component.prototype;

  if (!prototype || !prototype.isReactComponent) {
    throw new Error('Can only polyfill class components');
  }

  if (
    typeof Component.getDerivedStateFromProps !== 'function' &&
    typeof prototype.getSnapshotBeforeUpdate !== 'function'
  ) {
    return Component;
  }

  // If new component APIs are defined, "unsafe" lifecycles won't be called.
  // Error if any of these lifecycles are present,
  // Because they would work differently between older and newer (16.3+) versions of React.
  var foundWillMountName = null;
  var foundWillReceivePropsName = null;
  var foundWillUpdateName = null;
  if (typeof prototype.componentWillMount === 'function') {
    foundWillMountName = 'componentWillMount';
  } else if (typeof prototype.UNSAFE_componentWillMount === 'function') {
    foundWillMountName = 'UNSAFE_componentWillMount';
  }
  if (typeof prototype.componentWillReceiveProps === 'function') {
    foundWillReceivePropsName = 'componentWillReceiveProps';
  } else if (typeof prototype.UNSAFE_componentWillReceiveProps === 'function') {
    foundWillReceivePropsName = 'UNSAFE_componentWillReceiveProps';
  }
  if (typeof prototype.componentWillUpdate === 'function') {
    foundWillUpdateName = 'componentWillUpdate';
  } else if (typeof prototype.UNSAFE_componentWillUpdate === 'function') {
    foundWillUpdateName = 'UNSAFE_componentWillUpdate';
  }
  if (
    foundWillMountName !== null ||
    foundWillReceivePropsName !== null ||
    foundWillUpdateName !== null
  ) {
    var componentName = Component.displayName || Component.name;
    var newApiName =
      typeof Component.getDerivedStateFromProps === 'function'
        ? 'getDerivedStateFromProps()'
        : 'getSnapshotBeforeUpdate()';

    throw Error(
      'Unsafe legacy lifecycles will not be called for components using new component APIs.\n\n' +
        componentName +
        ' uses ' +
        newApiName +
        ' but also contains the following legacy lifecycles:' +
        (foundWillMountName !== null ? '\n  ' + foundWillMountName : '') +
        (foundWillReceivePropsName !== null
          ? '\n  ' + foundWillReceivePropsName
          : '') +
        (foundWillUpdateName !== null ? '\n  ' + foundWillUpdateName : '') +
        '\n\nThe above lifecycles should be removed. Learn more about this warning here:\n' +
        'https://fb.me/react-async-component-lifecycle-hooks'
    );
  }

  // React <= 16.2 does not support static getDerivedStateFromProps.
  // As a workaround, use cWM and cWRP to invoke the new static lifecycle.
  // Newer versions of React will ignore these lifecycles if gDSFP exists.
  if (typeof Component.getDerivedStateFromProps === 'function') {
    prototype.componentWillMount = componentWillMount;
    prototype.componentWillReceiveProps = componentWillReceiveProps;
  }

  // React <= 16.2 does not support getSnapshotBeforeUpdate.
  // As a workaround, use cWU to invoke the new lifecycle.
  // Newer versions of React will ignore that lifecycle if gSBU exists.
  if (typeof prototype.getSnapshotBeforeUpdate === 'function') {
    if (typeof prototype.componentDidUpdate !== 'function') {
      throw new Error(
        'Cannot polyfill getSnapshotBeforeUpdate() for components that do not define componentDidUpdate() on the prototype'
      );
    }

    prototype.componentWillUpdate = componentWillUpdate;

    var componentDidUpdate = prototype.componentDidUpdate;

    prototype.componentDidUpdate = function componentDidUpdatePolyfill(
      prevProps,
      prevState,
      maybeSnapshot
    ) {
      // 16.3+ will not execute our will-update method;
      // It will pass a snapshot value to did-update though.
      // Older versions will require our polyfilled will-update value.
      // We need to handle both cases, but can't just check for the presence of "maybeSnapshot",
      // Because for <= 15.x versions this might be a "prevContext" object.
      // We also can't just check "__reactInternalSnapshot",
      // Because get-snapshot might return a falsy value.
      // So check for the explicit __reactInternalSnapshotFlag flag to determine behavior.
      var snapshot = this.__reactInternalSnapshotFlag
        ? this.__reactInternalSnapshot
        : maybeSnapshot;

      componentDidUpdate.call(this, prevProps, prevState, snapshot);
    };
  }

  return Component;
}




/***/ }),

/***/ "./node_modules/react-transition-group/CSSTransition.js":
/*!**************************************************************!*\
  !*** ./node_modules/react-transition-group/CSSTransition.js ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var PropTypes = _interopRequireWildcard(__webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js"));

var _addClass = _interopRequireDefault(__webpack_require__(/*! dom-helpers/class/addClass */ "./node_modules/dom-helpers/class/addClass.js"));

var _removeClass = _interopRequireDefault(__webpack_require__(/*! dom-helpers/class/removeClass */ "./node_modules/dom-helpers/class/removeClass.js"));

var _react = _interopRequireDefault(__webpack_require__(/*! react */ "react"));

var _Transition = _interopRequireDefault(__webpack_require__(/*! ./Transition */ "./node_modules/react-transition-group/Transition.js"));

var _PropTypes = __webpack_require__(/*! ./utils/PropTypes */ "./node_modules/react-transition-group/utils/PropTypes.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var addClass = function addClass(node, classes) {
  return node && classes && classes.split(' ').forEach(function (c) {
    return (0, _addClass.default)(node, c);
  });
};

var removeClass = function removeClass(node, classes) {
  return node && classes && classes.split(' ').forEach(function (c) {
    return (0, _removeClass.default)(node, c);
  });
};
/**
 * A transition component inspired by the excellent
 * [ng-animate](http://www.nganimate.org/) library, you should use it if you're
 * using CSS transitions or animations. It's built upon the
 * [`Transition`](https://reactcommunity.org/react-transition-group/transition)
 * component, so it inherits all of its props.
 *
 * `CSSTransition` applies a pair of class names during the `appear`, `enter`,
 * and `exit` states of the transition. The first class is applied and then a
 * second `*-active` class in order to activate the CSSS transition. After the
 * transition, matching `*-done` class names are applied to persist the
 * transition state.
 *
 * ```jsx
 * function App() {
 *   const [inProp, setInProp] = useState(false);
 *   return (
 *     <div>
 *       <CSSTransition in={inProp} timeout={200} classNames="my-node">
 *         <div>
 *           {"I'll receive my-node-* classes"}
 *         </div>
 *       </CSSTransition>
 *       <button type="button" onClick={() => setInProp(true)}>
 *         Click to Enter
 *       </button>
 *     </div>
 *   );
 * }
 * ```
 *
 * When the `in` prop is set to `true`, the child component will first receive
 * the class `example-enter`, then the `example-enter-active` will be added in
 * the next tick. `CSSTransition` [forces a
 * reflow](https://github.com/reactjs/react-transition-group/blob/5007303e729a74be66a21c3e2205e4916821524b/src/CSSTransition.js#L208-L215)
 * between before adding the `example-enter-active`. This is an important trick
 * because it allows us to transition between `example-enter` and
 * `example-enter-active` even though they were added immediately one after
 * another. Most notably, this is what makes it possible for us to animate
 * _appearance_.
 *
 * ```css
 * .my-node-enter {
 *   opacity: 0;
 * }
 * .my-node-enter-active {
 *   opacity: 1;
 *   transition: opacity 200ms;
 * }
 * .my-node-exit {
 *   opacity: 1;
 * }
 * .my-node-exit-active {
 *   opacity: 0;
 *   transition: opacity: 200ms;
 * }
 * ```
 *
 * `*-active` classes represent which styles you want to animate **to**.
 */


var CSSTransition =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(CSSTransition, _React$Component);

  function CSSTransition() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.onEnter = function (node, appearing) {
      var _this$getClassNames = _this.getClassNames(appearing ? 'appear' : 'enter'),
          className = _this$getClassNames.className;

      _this.removeClasses(node, 'exit');

      addClass(node, className);

      if (_this.props.onEnter) {
        _this.props.onEnter(node, appearing);
      }
    };

    _this.onEntering = function (node, appearing) {
      var _this$getClassNames2 = _this.getClassNames(appearing ? 'appear' : 'enter'),
          activeClassName = _this$getClassNames2.activeClassName;

      _this.reflowAndAddClass(node, activeClassName);

      if (_this.props.onEntering) {
        _this.props.onEntering(node, appearing);
      }
    };

    _this.onEntered = function (node, appearing) {
      var appearClassName = _this.getClassNames('appear').doneClassName;

      var enterClassName = _this.getClassNames('enter').doneClassName;

      var doneClassName = appearing ? appearClassName + " " + enterClassName : enterClassName;

      _this.removeClasses(node, appearing ? 'appear' : 'enter');

      addClass(node, doneClassName);

      if (_this.props.onEntered) {
        _this.props.onEntered(node, appearing);
      }
    };

    _this.onExit = function (node) {
      var _this$getClassNames3 = _this.getClassNames('exit'),
          className = _this$getClassNames3.className;

      _this.removeClasses(node, 'appear');

      _this.removeClasses(node, 'enter');

      addClass(node, className);

      if (_this.props.onExit) {
        _this.props.onExit(node);
      }
    };

    _this.onExiting = function (node) {
      var _this$getClassNames4 = _this.getClassNames('exit'),
          activeClassName = _this$getClassNames4.activeClassName;

      _this.reflowAndAddClass(node, activeClassName);

      if (_this.props.onExiting) {
        _this.props.onExiting(node);
      }
    };

    _this.onExited = function (node) {
      var _this$getClassNames5 = _this.getClassNames('exit'),
          doneClassName = _this$getClassNames5.doneClassName;

      _this.removeClasses(node, 'exit');

      addClass(node, doneClassName);

      if (_this.props.onExited) {
        _this.props.onExited(node);
      }
    };

    _this.getClassNames = function (type) {
      var classNames = _this.props.classNames;
      var isStringClassNames = typeof classNames === 'string';
      var prefix = isStringClassNames && classNames ? classNames + '-' : '';
      var className = isStringClassNames ? prefix + type : classNames[type];
      var activeClassName = isStringClassNames ? className + '-active' : classNames[type + 'Active'];
      var doneClassName = isStringClassNames ? className + '-done' : classNames[type + 'Done'];
      return {
        className: className,
        activeClassName: activeClassName,
        doneClassName: doneClassName
      };
    };

    return _this;
  }

  var _proto = CSSTransition.prototype;

  _proto.removeClasses = function removeClasses(node, type) {
    var _this$getClassNames6 = this.getClassNames(type),
        className = _this$getClassNames6.className,
        activeClassName = _this$getClassNames6.activeClassName,
        doneClassName = _this$getClassNames6.doneClassName;

    className && removeClass(node, className);
    activeClassName && removeClass(node, activeClassName);
    doneClassName && removeClass(node, doneClassName);
  };

  _proto.reflowAndAddClass = function reflowAndAddClass(node, className) {
    // This is for to force a repaint,
    // which is necessary in order to transition styles when adding a class name.
    if (className) {
      /* eslint-disable no-unused-expressions */
      node && node.scrollTop;
      /* eslint-enable no-unused-expressions */

      addClass(node, className);
    }
  };

  _proto.render = function render() {
    var props = _extends({}, this.props);

    delete props.classNames;
    return _react.default.createElement(_Transition.default, _extends({}, props, {
      onEnter: this.onEnter,
      onEntered: this.onEntered,
      onEntering: this.onEntering,
      onExit: this.onExit,
      onExiting: this.onExiting,
      onExited: this.onExited
    }));
  };

  return CSSTransition;
}(_react.default.Component);

CSSTransition.defaultProps = {
  classNames: ''
};
CSSTransition.propTypes =  true ? _extends({}, _Transition.default.propTypes, {
  /**
   * The animation classNames applied to the component as it enters, exits or
   * has finished the transition. A single name can be provided and it will be
   * suffixed for each stage: e.g.
   *
   * `classNames="fade"` applies `fade-enter`, `fade-enter-active`,
   * `fade-enter-done`, `fade-exit`, `fade-exit-active`, `fade-exit-done`,
   * `fade-appear`, `fade-appear-active`, and `fade-appear-done`.
   *
   * **Note**: `fade-appear-done` and `fade-enter-done` will _both_ be applied.
   * This allows you to define different behavior for when appearing is done and
   * when regular entering is done, using selectors like
   * `.fade-enter-done:not(.fade-appear-done)`. For example, you could apply an
   * epic entrance animation when element first appears in the DOM using
   * [Animate.css](https://daneden.github.io/animate.css/). Otherwise you can
   * simply use `fade-enter-done` for defining both cases.
   *
   * Each individual classNames can also be specified independently like:
   *
   * ```js
   * classNames={{
   *  appear: 'my-appear',
   *  appearActive: 'my-active-appear',
   *  appearDone: 'my-done-appear',
   *  enter: 'my-enter',
   *  enterActive: 'my-active-enter',
   *  enterDone: 'my-done-enter',
   *  exit: 'my-exit',
   *  exitActive: 'my-active-exit',
   *  exitDone: 'my-done-exit',
   * }}
   * ```
   *
   * If you want to set these classes using CSS Modules:
   *
   * ```js
   * import styles from './styles.css';
   * ```
   *
   * you might want to use camelCase in your CSS file, that way could simply
   * spread them instead of listing them one by one:
   *
   * ```js
   * classNames={{ ...styles }}
   * ```
   *
   * @type {string | {
   *  appear?: string,
   *  appearActive?: string,
   *  appearDone?: string,
   *  enter?: string,
   *  enterActive?: string,
   *  enterDone?: string,
   *  exit?: string,
   *  exitActive?: string,
   *  exitDone?: string,
   * }}
   */
  classNames: _PropTypes.classNamesShape,

  /**
   * A `<Transition>` callback fired immediately after the 'enter' or 'appear' class is
   * applied.
   *
   * @type Function(node: HtmlElement, isAppearing: bool)
   */
  onEnter: PropTypes.func,

  /**
   * A `<Transition>` callback fired immediately after the 'enter-active' or
   * 'appear-active' class is applied.
   *
   * @type Function(node: HtmlElement, isAppearing: bool)
   */
  onEntering: PropTypes.func,

  /**
   * A `<Transition>` callback fired immediately after the 'enter' or
   * 'appear' classes are **removed** and the `done` class is added to the DOM node.
   *
   * @type Function(node: HtmlElement, isAppearing: bool)
   */
  onEntered: PropTypes.func,

  /**
   * A `<Transition>` callback fired immediately after the 'exit' class is
   * applied.
   *
   * @type Function(node: HtmlElement)
   */
  onExit: PropTypes.func,

  /**
   * A `<Transition>` callback fired immediately after the 'exit-active' is applied.
   *
   * @type Function(node: HtmlElement)
   */
  onExiting: PropTypes.func,

  /**
   * A `<Transition>` callback fired immediately after the 'exit' classes
   * are **removed** and the `exit-done` class is added to the DOM node.
   *
   * @type Function(node: HtmlElement)
   */
  onExited: PropTypes.func
}) : undefined;
var _default = CSSTransition;
exports.default = _default;
module.exports = exports["default"];

/***/ }),

/***/ "./node_modules/react-transition-group/ReplaceTransition.js":
/*!******************************************************************!*\
  !*** ./node_modules/react-transition-group/ReplaceTransition.js ***!
  \******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js"));

var _react = _interopRequireDefault(__webpack_require__(/*! react */ "react"));

var _reactDom = __webpack_require__(/*! react-dom */ "react-dom");

var _TransitionGroup = _interopRequireDefault(__webpack_require__(/*! ./TransitionGroup */ "./node_modules/react-transition-group/TransitionGroup.js"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

/**
 * The `<ReplaceTransition>` component is a specialized `Transition` component
 * that animates between two children.
 *
 * ```jsx
 * <ReplaceTransition in>
 *   <Fade><div>I appear first</div></Fade>
 *   <Fade><div>I replace the above</div></Fade>
 * </ReplaceTransition>
 * ```
 */
var ReplaceTransition =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(ReplaceTransition, _React$Component);

  function ReplaceTransition() {
    var _this;

    for (var _len = arguments.length, _args = new Array(_len), _key = 0; _key < _len; _key++) {
      _args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(_args)) || this;

    _this.handleEnter = function () {
      for (var _len2 = arguments.length, args = new Array(_len2), _key2 = 0; _key2 < _len2; _key2++) {
        args[_key2] = arguments[_key2];
      }

      return _this.handleLifecycle('onEnter', 0, args);
    };

    _this.handleEntering = function () {
      for (var _len3 = arguments.length, args = new Array(_len3), _key3 = 0; _key3 < _len3; _key3++) {
        args[_key3] = arguments[_key3];
      }

      return _this.handleLifecycle('onEntering', 0, args);
    };

    _this.handleEntered = function () {
      for (var _len4 = arguments.length, args = new Array(_len4), _key4 = 0; _key4 < _len4; _key4++) {
        args[_key4] = arguments[_key4];
      }

      return _this.handleLifecycle('onEntered', 0, args);
    };

    _this.handleExit = function () {
      for (var _len5 = arguments.length, args = new Array(_len5), _key5 = 0; _key5 < _len5; _key5++) {
        args[_key5] = arguments[_key5];
      }

      return _this.handleLifecycle('onExit', 1, args);
    };

    _this.handleExiting = function () {
      for (var _len6 = arguments.length, args = new Array(_len6), _key6 = 0; _key6 < _len6; _key6++) {
        args[_key6] = arguments[_key6];
      }

      return _this.handleLifecycle('onExiting', 1, args);
    };

    _this.handleExited = function () {
      for (var _len7 = arguments.length, args = new Array(_len7), _key7 = 0; _key7 < _len7; _key7++) {
        args[_key7] = arguments[_key7];
      }

      return _this.handleLifecycle('onExited', 1, args);
    };

    return _this;
  }

  var _proto = ReplaceTransition.prototype;

  _proto.handleLifecycle = function handleLifecycle(handler, idx, originalArgs) {
    var _child$props;

    var children = this.props.children;

    var child = _react.default.Children.toArray(children)[idx];

    if (child.props[handler]) (_child$props = child.props)[handler].apply(_child$props, originalArgs);
    if (this.props[handler]) this.props[handler]((0, _reactDom.findDOMNode)(this));
  };

  _proto.render = function render() {
    var _this$props = this.props,
        children = _this$props.children,
        inProp = _this$props.in,
        props = _objectWithoutPropertiesLoose(_this$props, ["children", "in"]);

    var _React$Children$toArr = _react.default.Children.toArray(children),
        first = _React$Children$toArr[0],
        second = _React$Children$toArr[1];

    delete props.onEnter;
    delete props.onEntering;
    delete props.onEntered;
    delete props.onExit;
    delete props.onExiting;
    delete props.onExited;
    return _react.default.createElement(_TransitionGroup.default, props, inProp ? _react.default.cloneElement(first, {
      key: 'first',
      onEnter: this.handleEnter,
      onEntering: this.handleEntering,
      onEntered: this.handleEntered
    }) : _react.default.cloneElement(second, {
      key: 'second',
      onEnter: this.handleExit,
      onEntering: this.handleExiting,
      onEntered: this.handleExited
    }));
  };

  return ReplaceTransition;
}(_react.default.Component);

ReplaceTransition.propTypes =  true ? {
  in: _propTypes.default.bool.isRequired,
  children: function children(props, propName) {
    if (_react.default.Children.count(props[propName]) !== 2) return new Error("\"" + propName + "\" must be exactly two transition components.");
    return null;
  }
} : undefined;
var _default = ReplaceTransition;
exports.default = _default;
module.exports = exports["default"];

/***/ }),

/***/ "./node_modules/react-transition-group/Transition.js":
/*!***********************************************************!*\
  !*** ./node_modules/react-transition-group/Transition.js ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = exports.EXITING = exports.ENTERED = exports.ENTERING = exports.EXITED = exports.UNMOUNTED = void 0;

var PropTypes = _interopRequireWildcard(__webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js"));

var _react = _interopRequireDefault(__webpack_require__(/*! react */ "react"));

var _reactDom = _interopRequireDefault(__webpack_require__(/*! react-dom */ "react-dom"));

var _reactLifecyclesCompat = __webpack_require__(/*! react-lifecycles-compat */ "./node_modules/react-lifecycles-compat/react-lifecycles-compat.es.js");

var _PropTypes = __webpack_require__(/*! ./utils/PropTypes */ "./node_modules/react-transition-group/utils/PropTypes.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var UNMOUNTED = 'unmounted';
exports.UNMOUNTED = UNMOUNTED;
var EXITED = 'exited';
exports.EXITED = EXITED;
var ENTERING = 'entering';
exports.ENTERING = ENTERING;
var ENTERED = 'entered';
exports.ENTERED = ENTERED;
var EXITING = 'exiting';
/**
 * The Transition component lets you describe a transition from one component
 * state to another _over time_ with a simple declarative API. Most commonly
 * it's used to animate the mounting and unmounting of a component, but can also
 * be used to describe in-place transition states as well.
 *
 * ---
 *
 * **Note**: `Transition` is a platform-agnostic base component. If you're using
 * transitions in CSS, you'll probably want to use
 * [`CSSTransition`](https://reactcommunity.org/react-transition-group/css-transition)
 * instead. It inherits all the features of `Transition`, but contains
 * additional features necessary to play nice with CSS transitions (hence the
 * name of the component).
 *
 * ---
 *
 * By default the `Transition` component does not alter the behavior of the
 * component it renders, it only tracks "enter" and "exit" states for the
 * components. It's up to you to give meaning and effect to those states. For
 * example we can add styles to a component when it enters or exits:
 *
 * ```jsx
 * import { Transition } from 'react-transition-group';
 *
 * const duration = 300;
 *
 * const defaultStyle = {
 *   transition: `opacity ${duration}ms ease-in-out`,
 *   opacity: 0,
 * }
 *
 * const transitionStyles = {
 *   entering: { opacity: 0 },
 *   entered:  { opacity: 1 },
 * };
 *
 * const Fade = ({ in: inProp }) => (
 *   <Transition in={inProp} timeout={duration}>
 *     {state => (
 *       <div style={{
 *         ...defaultStyle,
 *         ...transitionStyles[state]
 *       }}>
 *         I'm a fade Transition!
 *       </div>
 *     )}
 *   </Transition>
 * );
 * ```
 *
 * There are 4 main states a Transition can be in:
 *  - `'entering'`
 *  - `'entered'`
 *  - `'exiting'`
 *  - `'exited'`
 *
 * Transition state is toggled via the `in` prop. When `true` the component
 * begins the "Enter" stage. During this stage, the component will shift from
 * its current transition state, to `'entering'` for the duration of the
 * transition and then to the `'entered'` stage once it's complete. Let's take
 * the following example (we'll use the
 * [useState](https://reactjs.org/docs/hooks-reference.html#usestate) hook):
 *
 * ```jsx
 * function App() {
 *   const [inProp, setInProp] = useState(false);
 *   return (
 *     <div>
 *       <Transition in={inProp} timeout={500}>
 *         {state => (
 *           // ...
 *         )}
 *       </Transition>
 *       <button onClick={() => setInProp(true)}>
 *         Click to Enter
 *       </button>
 *     </div>
 *   );
 * }
 * ```
 *
 * When the button is clicked the component will shift to the `'entering'` state
 * and stay there for 500ms (the value of `timeout`) before it finally switches
 * to `'entered'`.
 *
 * When `in` is `false` the same thing happens except the state moves from
 * `'exiting'` to `'exited'`.
 */

exports.EXITING = EXITING;

var Transition =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Transition, _React$Component);

  function Transition(props, context) {
    var _this;

    _this = _React$Component.call(this, props, context) || this;
    var parentGroup = context.transitionGroup; // In the context of a TransitionGroup all enters are really appears

    var appear = parentGroup && !parentGroup.isMounting ? props.enter : props.appear;
    var initialStatus;
    _this.appearStatus = null;

    if (props.in) {
      if (appear) {
        initialStatus = EXITED;
        _this.appearStatus = ENTERING;
      } else {
        initialStatus = ENTERED;
      }
    } else {
      if (props.unmountOnExit || props.mountOnEnter) {
        initialStatus = UNMOUNTED;
      } else {
        initialStatus = EXITED;
      }
    }

    _this.state = {
      status: initialStatus
    };
    _this.nextCallback = null;
    return _this;
  }

  var _proto = Transition.prototype;

  _proto.getChildContext = function getChildContext() {
    return {
      transitionGroup: null // allows for nested Transitions

    };
  };

  Transition.getDerivedStateFromProps = function getDerivedStateFromProps(_ref, prevState) {
    var nextIn = _ref.in;

    if (nextIn && prevState.status === UNMOUNTED) {
      return {
        status: EXITED
      };
    }

    return null;
  }; // getSnapshotBeforeUpdate(prevProps) {
  //   let nextStatus = null
  //   if (prevProps !== this.props) {
  //     const { status } = this.state
  //     if (this.props.in) {
  //       if (status !== ENTERING && status !== ENTERED) {
  //         nextStatus = ENTERING
  //       }
  //     } else {
  //       if (status === ENTERING || status === ENTERED) {
  //         nextStatus = EXITING
  //       }
  //     }
  //   }
  //   return { nextStatus }
  // }


  _proto.componentDidMount = function componentDidMount() {
    this.updateStatus(true, this.appearStatus);
  };

  _proto.componentDidUpdate = function componentDidUpdate(prevProps) {
    var nextStatus = null;

    if (prevProps !== this.props) {
      var status = this.state.status;

      if (this.props.in) {
        if (status !== ENTERING && status !== ENTERED) {
          nextStatus = ENTERING;
        }
      } else {
        if (status === ENTERING || status === ENTERED) {
          nextStatus = EXITING;
        }
      }
    }

    this.updateStatus(false, nextStatus);
  };

  _proto.componentWillUnmount = function componentWillUnmount() {
    this.cancelNextCallback();
  };

  _proto.getTimeouts = function getTimeouts() {
    var timeout = this.props.timeout;
    var exit, enter, appear;
    exit = enter = appear = timeout;

    if (timeout != null && typeof timeout !== 'number') {
      exit = timeout.exit;
      enter = timeout.enter; // TODO: remove fallback for next major

      appear = timeout.appear !== undefined ? timeout.appear : enter;
    }

    return {
      exit: exit,
      enter: enter,
      appear: appear
    };
  };

  _proto.updateStatus = function updateStatus(mounting, nextStatus) {
    if (mounting === void 0) {
      mounting = false;
    }

    if (nextStatus !== null) {
      // nextStatus will always be ENTERING or EXITING.
      this.cancelNextCallback();

      var node = _reactDom.default.findDOMNode(this);

      if (nextStatus === ENTERING) {
        this.performEnter(node, mounting);
      } else {
        this.performExit(node);
      }
    } else if (this.props.unmountOnExit && this.state.status === EXITED) {
      this.setState({
        status: UNMOUNTED
      });
    }
  };

  _proto.performEnter = function performEnter(node, mounting) {
    var _this2 = this;

    var enter = this.props.enter;
    var appearing = this.context.transitionGroup ? this.context.transitionGroup.isMounting : mounting;
    var timeouts = this.getTimeouts();
    var enterTimeout = appearing ? timeouts.appear : timeouts.enter; // no enter animation skip right to ENTERED
    // if we are mounting and running this it means appear _must_ be set

    if (!mounting && !enter) {
      this.safeSetState({
        status: ENTERED
      }, function () {
        _this2.props.onEntered(node);
      });
      return;
    }

    this.props.onEnter(node, appearing);
    this.safeSetState({
      status: ENTERING
    }, function () {
      _this2.props.onEntering(node, appearing);

      _this2.onTransitionEnd(node, enterTimeout, function () {
        _this2.safeSetState({
          status: ENTERED
        }, function () {
          _this2.props.onEntered(node, appearing);
        });
      });
    });
  };

  _proto.performExit = function performExit(node) {
    var _this3 = this;

    var exit = this.props.exit;
    var timeouts = this.getTimeouts(); // no exit animation skip right to EXITED

    if (!exit) {
      this.safeSetState({
        status: EXITED
      }, function () {
        _this3.props.onExited(node);
      });
      return;
    }

    this.props.onExit(node);
    this.safeSetState({
      status: EXITING
    }, function () {
      _this3.props.onExiting(node);

      _this3.onTransitionEnd(node, timeouts.exit, function () {
        _this3.safeSetState({
          status: EXITED
        }, function () {
          _this3.props.onExited(node);
        });
      });
    });
  };

  _proto.cancelNextCallback = function cancelNextCallback() {
    if (this.nextCallback !== null) {
      this.nextCallback.cancel();
      this.nextCallback = null;
    }
  };

  _proto.safeSetState = function safeSetState(nextState, callback) {
    // This shouldn't be necessary, but there are weird race conditions with
    // setState callbacks and unmounting in testing, so always make sure that
    // we can cancel any pending setState callbacks after we unmount.
    callback = this.setNextCallback(callback);
    this.setState(nextState, callback);
  };

  _proto.setNextCallback = function setNextCallback(callback) {
    var _this4 = this;

    var active = true;

    this.nextCallback = function (event) {
      if (active) {
        active = false;
        _this4.nextCallback = null;
        callback(event);
      }
    };

    this.nextCallback.cancel = function () {
      active = false;
    };

    return this.nextCallback;
  };

  _proto.onTransitionEnd = function onTransitionEnd(node, timeout, handler) {
    this.setNextCallback(handler);
    var doesNotHaveTimeoutOrListener = timeout == null && !this.props.addEndListener;

    if (!node || doesNotHaveTimeoutOrListener) {
      setTimeout(this.nextCallback, 0);
      return;
    }

    if (this.props.addEndListener) {
      this.props.addEndListener(node, this.nextCallback);
    }

    if (timeout != null) {
      setTimeout(this.nextCallback, timeout);
    }
  };

  _proto.render = function render() {
    var status = this.state.status;

    if (status === UNMOUNTED) {
      return null;
    }

    var _this$props = this.props,
        children = _this$props.children,
        childProps = _objectWithoutPropertiesLoose(_this$props, ["children"]); // filter props for Transtition


    delete childProps.in;
    delete childProps.mountOnEnter;
    delete childProps.unmountOnExit;
    delete childProps.appear;
    delete childProps.enter;
    delete childProps.exit;
    delete childProps.timeout;
    delete childProps.addEndListener;
    delete childProps.onEnter;
    delete childProps.onEntering;
    delete childProps.onEntered;
    delete childProps.onExit;
    delete childProps.onExiting;
    delete childProps.onExited;

    if (typeof children === 'function') {
      return children(status, childProps);
    }

    var child = _react.default.Children.only(children);

    return _react.default.cloneElement(child, childProps);
  };

  return Transition;
}(_react.default.Component);

Transition.contextTypes = {
  transitionGroup: PropTypes.object
};
Transition.childContextTypes = {
  transitionGroup: function transitionGroup() {}
};
Transition.propTypes =  true ? {
  /**
   * A `function` child can be used instead of a React element. This function is
   * called with the current transition status (`'entering'`, `'entered'`,
   * `'exiting'`, `'exited'`, `'unmounted'`), which can be used to apply context
   * specific props to a component.
   *
   * ```jsx
   * <Transition in={this.state.in} timeout={150}>
   *   {state => (
   *     <MyComponent className={`fade fade-${state}`} />
   *   )}
   * </Transition>
   * ```
   */
  children: PropTypes.oneOfType([PropTypes.func.isRequired, PropTypes.element.isRequired]).isRequired,

  /**
   * Show the component; triggers the enter or exit states
   */
  in: PropTypes.bool,

  /**
   * By default the child component is mounted immediately along with
   * the parent `Transition` component. If you want to "lazy mount" the component on the
   * first `in={true}` you can set `mountOnEnter`. After the first enter transition the component will stay
   * mounted, even on "exited", unless you also specify `unmountOnExit`.
   */
  mountOnEnter: PropTypes.bool,

  /**
   * By default the child component stays mounted after it reaches the `'exited'` state.
   * Set `unmountOnExit` if you'd prefer to unmount the component after it finishes exiting.
   */
  unmountOnExit: PropTypes.bool,

  /**
   * Normally a component is not transitioned if it is shown when the `<Transition>` component mounts.
   * If you want to transition on the first mount set `appear` to `true`, and the
   * component will transition in as soon as the `<Transition>` mounts.
   *
   * > Note: there are no specific "appear" states. `appear` only adds an additional `enter` transition.
   */
  appear: PropTypes.bool,

  /**
   * Enable or disable enter transitions.
   */
  enter: PropTypes.bool,

  /**
   * Enable or disable exit transitions.
   */
  exit: PropTypes.bool,

  /**
   * The duration of the transition, in milliseconds.
   * Required unless `addEndListener` is provided.
   *
   * You may specify a single timeout for all transitions:
   *
   * ```jsx
   * timeout={500}
   * ```
   *
   * or individually:
   *
   * ```jsx
   * timeout={{
   *  appear: 500,
   *  enter: 300,
   *  exit: 500,
   * }}
   * ```
   *
   * - `appear` defaults to the value of `enter`
   * - `enter` defaults to `0`
   * - `exit` defaults to `0`
   *
   * @type {number | { enter?: number, exit?: number, appear?: number }}
   */
  timeout: function timeout(props) {
    var pt = _PropTypes.timeoutsShape;
    if (!props.addEndListener) pt = pt.isRequired;

    for (var _len = arguments.length, args = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      args[_key - 1] = arguments[_key];
    }

    return pt.apply(void 0, [props].concat(args));
  },

  /**
   * Add a custom transition end trigger. Called with the transitioning
   * DOM node and a `done` callback. Allows for more fine grained transition end
   * logic. **Note:** Timeouts are still used as a fallback if provided.
   *
   * ```jsx
   * addEndListener={(node, done) => {
   *   // use the css transitionend event to mark the finish of a transition
   *   node.addEventListener('transitionend', done, false);
   * }}
   * ```
   */
  addEndListener: PropTypes.func,

  /**
   * Callback fired before the "entering" status is applied. An extra parameter
   * `isAppearing` is supplied to indicate if the enter stage is occurring on the initial mount
   *
   * @type Function(node: HtmlElement, isAppearing: bool) -> void
   */
  onEnter: PropTypes.func,

  /**
   * Callback fired after the "entering" status is applied. An extra parameter
   * `isAppearing` is supplied to indicate if the enter stage is occurring on the initial mount
   *
   * @type Function(node: HtmlElement, isAppearing: bool)
   */
  onEntering: PropTypes.func,

  /**
   * Callback fired after the "entered" status is applied. An extra parameter
   * `isAppearing` is supplied to indicate if the enter stage is occurring on the initial mount
   *
   * @type Function(node: HtmlElement, isAppearing: bool) -> void
   */
  onEntered: PropTypes.func,

  /**
   * Callback fired before the "exiting" status is applied.
   *
   * @type Function(node: HtmlElement) -> void
   */
  onExit: PropTypes.func,

  /**
   * Callback fired after the "exiting" status is applied.
   *
   * @type Function(node: HtmlElement) -> void
   */
  onExiting: PropTypes.func,

  /**
   * Callback fired after the "exited" status is applied.
   *
   * @type Function(node: HtmlElement) -> void
   */
  onExited: PropTypes.func // Name the function so it is clearer in the documentation

} : undefined;

function noop() {}

Transition.defaultProps = {
  in: false,
  mountOnEnter: false,
  unmountOnExit: false,
  appear: false,
  enter: true,
  exit: true,
  onEnter: noop,
  onEntering: noop,
  onEntered: noop,
  onExit: noop,
  onExiting: noop,
  onExited: noop
};
Transition.UNMOUNTED = 0;
Transition.EXITED = 1;
Transition.ENTERING = 2;
Transition.ENTERED = 3;
Transition.EXITING = 4;

var _default = (0, _reactLifecyclesCompat.polyfill)(Transition);

exports.default = _default;

/***/ }),

/***/ "./node_modules/react-transition-group/TransitionGroup.js":
/*!****************************************************************!*\
  !*** ./node_modules/react-transition-group/TransitionGroup.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js"));

var _react = _interopRequireDefault(__webpack_require__(/*! react */ "react"));

var _reactLifecyclesCompat = __webpack_require__(/*! react-lifecycles-compat */ "./node_modules/react-lifecycles-compat/react-lifecycles-compat.es.js");

var _ChildMapping = __webpack_require__(/*! ./utils/ChildMapping */ "./node_modules/react-transition-group/utils/ChildMapping.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

var values = Object.values || function (obj) {
  return Object.keys(obj).map(function (k) {
    return obj[k];
  });
};

var defaultProps = {
  component: 'div',
  childFactory: function childFactory(child) {
    return child;
  }
  /**
   * The `<TransitionGroup>` component manages a set of transition components
   * (`<Transition>` and `<CSSTransition>`) in a list. Like with the transition
   * components, `<TransitionGroup>` is a state machine for managing the mounting
   * and unmounting of components over time.
   *
   * Consider the example below. As items are removed or added to the TodoList the
   * `in` prop is toggled automatically by the `<TransitionGroup>`.
   *
   * Note that `<TransitionGroup>`  does not define any animation behavior!
   * Exactly _how_ a list item animates is up to the individual transition
   * component. This means you can mix and match animations across different list
   * items.
   */

};

var TransitionGroup =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(TransitionGroup, _React$Component);

  function TransitionGroup(props, context) {
    var _this;

    _this = _React$Component.call(this, props, context) || this;

    var handleExited = _this.handleExited.bind(_assertThisInitialized(_assertThisInitialized(_this))); // Initial children should all be entering, dependent on appear


    _this.state = {
      handleExited: handleExited,
      firstRender: true
    };
    return _this;
  }

  var _proto = TransitionGroup.prototype;

  _proto.getChildContext = function getChildContext() {
    return {
      transitionGroup: {
        isMounting: !this.appeared
      }
    };
  };

  _proto.componentDidMount = function componentDidMount() {
    this.appeared = true;
    this.mounted = true;
  };

  _proto.componentWillUnmount = function componentWillUnmount() {
    this.mounted = false;
  };

  TransitionGroup.getDerivedStateFromProps = function getDerivedStateFromProps(nextProps, _ref) {
    var prevChildMapping = _ref.children,
        handleExited = _ref.handleExited,
        firstRender = _ref.firstRender;
    return {
      children: firstRender ? (0, _ChildMapping.getInitialChildMapping)(nextProps, handleExited) : (0, _ChildMapping.getNextChildMapping)(nextProps, prevChildMapping, handleExited),
      firstRender: false
    };
  };

  _proto.handleExited = function handleExited(child, node) {
    var currentChildMapping = (0, _ChildMapping.getChildMapping)(this.props.children);
    if (child.key in currentChildMapping) return;

    if (child.props.onExited) {
      child.props.onExited(node);
    }

    if (this.mounted) {
      this.setState(function (state) {
        var children = _extends({}, state.children);

        delete children[child.key];
        return {
          children: children
        };
      });
    }
  };

  _proto.render = function render() {
    var _this$props = this.props,
        Component = _this$props.component,
        childFactory = _this$props.childFactory,
        props = _objectWithoutPropertiesLoose(_this$props, ["component", "childFactory"]);

    var children = values(this.state.children).map(childFactory);
    delete props.appear;
    delete props.enter;
    delete props.exit;

    if (Component === null) {
      return children;
    }

    return _react.default.createElement(Component, props, children);
  };

  return TransitionGroup;
}(_react.default.Component);

TransitionGroup.childContextTypes = {
  transitionGroup: _propTypes.default.object.isRequired
};
TransitionGroup.propTypes =  true ? {
  /**
   * `<TransitionGroup>` renders a `<div>` by default. You can change this
   * behavior by providing a `component` prop.
   * If you use React v16+ and would like to avoid a wrapping `<div>` element
   * you can pass in `component={null}`. This is useful if the wrapping div
   * borks your css styles.
   */
  component: _propTypes.default.any,

  /**
   * A set of `<Transition>` components, that are toggled `in` and out as they
   * leave. the `<TransitionGroup>` will inject specific transition props, so
   * remember to spread them through if you are wrapping the `<Transition>` as
   * with our `<Fade>` example.
   *
   * While this component is meant for multiple `Transition` or `CSSTransition`
   * children, sometimes you may want to have a single transition child with
   * content that you want to be transitioned out and in when you change it
   * (e.g. routes, images etc.) In that case you can change the `key` prop of
   * the transition child as you change its content, this will cause
   * `TransitionGroup` to transition the child out and back in.
   */
  children: _propTypes.default.node,

  /**
   * A convenience prop that enables or disables appear animations
   * for all children. Note that specifying this will override any defaults set
   * on individual children Transitions.
   */
  appear: _propTypes.default.bool,

  /**
   * A convenience prop that enables or disables enter animations
   * for all children. Note that specifying this will override any defaults set
   * on individual children Transitions.
   */
  enter: _propTypes.default.bool,

  /**
   * A convenience prop that enables or disables exit animations
   * for all children. Note that specifying this will override any defaults set
   * on individual children Transitions.
   */
  exit: _propTypes.default.bool,

  /**
   * You may need to apply reactive updates to a child as it is exiting.
   * This is generally done by using `cloneElement` however in the case of an exiting
   * child the element has already been removed and not accessible to the consumer.
   *
   * If you do need to update a child as it leaves you can provide a `childFactory`
   * to wrap every child, even the ones that are leaving.
   *
   * @type Function(child: ReactElement) -> ReactElement
   */
  childFactory: _propTypes.default.func
} : undefined;
TransitionGroup.defaultProps = defaultProps;

var _default = (0, _reactLifecyclesCompat.polyfill)(TransitionGroup);

exports.default = _default;
module.exports = exports["default"];

/***/ }),

/***/ "./node_modules/react-transition-group/index.js":
/*!******************************************************!*\
  !*** ./node_modules/react-transition-group/index.js ***!
  \******************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _CSSTransition = _interopRequireDefault(__webpack_require__(/*! ./CSSTransition */ "./node_modules/react-transition-group/CSSTransition.js"));

var _ReplaceTransition = _interopRequireDefault(__webpack_require__(/*! ./ReplaceTransition */ "./node_modules/react-transition-group/ReplaceTransition.js"));

var _TransitionGroup = _interopRequireDefault(__webpack_require__(/*! ./TransitionGroup */ "./node_modules/react-transition-group/TransitionGroup.js"));

var _Transition = _interopRequireDefault(__webpack_require__(/*! ./Transition */ "./node_modules/react-transition-group/Transition.js"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

module.exports = {
  Transition: _Transition.default,
  TransitionGroup: _TransitionGroup.default,
  ReplaceTransition: _ReplaceTransition.default,
  CSSTransition: _CSSTransition.default
};

/***/ }),

/***/ "./node_modules/react-transition-group/utils/ChildMapping.js":
/*!*******************************************************************!*\
  !*** ./node_modules/react-transition-group/utils/ChildMapping.js ***!
  \*******************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.getChildMapping = getChildMapping;
exports.mergeChildMappings = mergeChildMappings;
exports.getInitialChildMapping = getInitialChildMapping;
exports.getNextChildMapping = getNextChildMapping;

var _react = __webpack_require__(/*! react */ "react");

/**
 * Given `this.props.children`, return an object mapping key to child.
 *
 * @param {*} children `this.props.children`
 * @return {object} Mapping of key to child
 */
function getChildMapping(children, mapFn) {
  var mapper = function mapper(child) {
    return mapFn && (0, _react.isValidElement)(child) ? mapFn(child) : child;
  };

  var result = Object.create(null);
  if (children) _react.Children.map(children, function (c) {
    return c;
  }).forEach(function (child) {
    // run the map function here instead so that the key is the computed one
    result[child.key] = mapper(child);
  });
  return result;
}
/**
 * When you're adding or removing children some may be added or removed in the
 * same render pass. We want to show *both* since we want to simultaneously
 * animate elements in and out. This function takes a previous set of keys
 * and a new set of keys and merges them with its best guess of the correct
 * ordering. In the future we may expose some of the utilities in
 * ReactMultiChild to make this easy, but for now React itself does not
 * directly have this concept of the union of prevChildren and nextChildren
 * so we implement it here.
 *
 * @param {object} prev prev children as returned from
 * `ReactTransitionChildMapping.getChildMapping()`.
 * @param {object} next next children as returned from
 * `ReactTransitionChildMapping.getChildMapping()`.
 * @return {object} a key set that contains all keys in `prev` and all keys
 * in `next` in a reasonable order.
 */


function mergeChildMappings(prev, next) {
  prev = prev || {};
  next = next || {};

  function getValueForKey(key) {
    return key in next ? next[key] : prev[key];
  } // For each key of `next`, the list of keys to insert before that key in
  // the combined list


  var nextKeysPending = Object.create(null);
  var pendingKeys = [];

  for (var prevKey in prev) {
    if (prevKey in next) {
      if (pendingKeys.length) {
        nextKeysPending[prevKey] = pendingKeys;
        pendingKeys = [];
      }
    } else {
      pendingKeys.push(prevKey);
    }
  }

  var i;
  var childMapping = {};

  for (var nextKey in next) {
    if (nextKeysPending[nextKey]) {
      for (i = 0; i < nextKeysPending[nextKey].length; i++) {
        var pendingNextKey = nextKeysPending[nextKey][i];
        childMapping[nextKeysPending[nextKey][i]] = getValueForKey(pendingNextKey);
      }
    }

    childMapping[nextKey] = getValueForKey(nextKey);
  } // Finally, add the keys which didn't appear before any key in `next`


  for (i = 0; i < pendingKeys.length; i++) {
    childMapping[pendingKeys[i]] = getValueForKey(pendingKeys[i]);
  }

  return childMapping;
}

function getProp(child, prop, props) {
  return props[prop] != null ? props[prop] : child.props[prop];
}

function getInitialChildMapping(props, onExited) {
  return getChildMapping(props.children, function (child) {
    return (0, _react.cloneElement)(child, {
      onExited: onExited.bind(null, child),
      in: true,
      appear: getProp(child, 'appear', props),
      enter: getProp(child, 'enter', props),
      exit: getProp(child, 'exit', props)
    });
  });
}

function getNextChildMapping(nextProps, prevChildMapping, onExited) {
  var nextChildMapping = getChildMapping(nextProps.children);
  var children = mergeChildMappings(prevChildMapping, nextChildMapping);
  Object.keys(children).forEach(function (key) {
    var child = children[key];
    if (!(0, _react.isValidElement)(child)) return;
    var hasPrev = key in prevChildMapping;
    var hasNext = key in nextChildMapping;
    var prevChild = prevChildMapping[key];
    var isLeaving = (0, _react.isValidElement)(prevChild) && !prevChild.props.in; // item is new (entering)

    if (hasNext && (!hasPrev || isLeaving)) {
      // console.log('entering', key)
      children[key] = (0, _react.cloneElement)(child, {
        onExited: onExited.bind(null, child),
        in: true,
        exit: getProp(child, 'exit', nextProps),
        enter: getProp(child, 'enter', nextProps)
      });
    } else if (!hasNext && hasPrev && !isLeaving) {
      // item is old (exiting)
      // console.log('leaving', key)
      children[key] = (0, _react.cloneElement)(child, {
        in: false
      });
    } else if (hasNext && hasPrev && (0, _react.isValidElement)(prevChild)) {
      // item hasn't changed transition states
      // copy over the last transition props;
      // console.log('unchanged', key)
      children[key] = (0, _react.cloneElement)(child, {
        onExited: onExited.bind(null, child),
        in: prevChild.props.in,
        exit: getProp(child, 'exit', nextProps),
        enter: getProp(child, 'enter', nextProps)
      });
    }
  });
  return children;
}

/***/ }),

/***/ "./node_modules/react-transition-group/utils/PropTypes.js":
/*!****************************************************************!*\
  !*** ./node_modules/react-transition-group/utils/PropTypes.js ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.classNamesShape = exports.timeoutsShape = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var timeoutsShape =  true ? _propTypes.default.oneOfType([_propTypes.default.number, _propTypes.default.shape({
  enter: _propTypes.default.number,
  exit: _propTypes.default.number,
  appear: _propTypes.default.number
}).isRequired]) : undefined;
exports.timeoutsShape = timeoutsShape;
var classNamesShape =  true ? _propTypes.default.oneOfType([_propTypes.default.string, _propTypes.default.shape({
  enter: _propTypes.default.string,
  exit: _propTypes.default.string,
  active: _propTypes.default.string
}), _propTypes.default.shape({
  enter: _propTypes.default.string,
  enterDone: _propTypes.default.string,
  enterActive: _propTypes.default.string,
  exit: _propTypes.default.string,
  exitDone: _propTypes.default.string,
  exitActive: _propTypes.default.string
})]) : undefined;
exports.classNamesShape = classNamesShape;

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "React" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = React;

/***/ }),

/***/ "react-dom":
/*!***************************!*\
  !*** external "ReactDOM" ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ReactDOM;

/***/ })

/******/ });
//# sourceMappingURL=contextMenu.js.map