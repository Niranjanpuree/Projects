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
/******/ 	return __webpack_require__(__webpack_require__.s = "./Scripts/Component/OfficeDirectory/index.tsx");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Scripts/Common/AlphabetFilter/AlphabetFilter.tsx":
/*!**********************************************************!*\
  !*** ./Scripts/Common/AlphabetFilter/AlphabetFilter.tsx ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");

var AlphabetFilter = function (_React$Component) {
    _inherits(AlphabetFilter, _React$Component);

    function AlphabetFilter(props) {
        _classCallCheck(this, AlphabetFilter);

        return _possibleConstructorReturn(this, (AlphabetFilter.__proto__ || Object.getPrototypeOf(AlphabetFilter)).call(this, props));
    }

    _createClass(AlphabetFilter, [{
        key: "render",
        value: function render() {
            var _this2 = this;

            var alphabet = ["All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
            var renderAlphabet = alphabet.map(function (alpha) {
                return React.createElement("li", { key: alpha, className: _this2.props.filterBy === alpha ? 'list-inline-item k-link active' : 'list-inline-item', onClick: function onClick(m) {
                        return _this2.props.onClickFilter(alpha, _this2.props.parent);
                    } }, " ", alpha);
            });
            return React.createElement("ul", { className: "list-inline alphabetical-sort" }, renderAlphabet);
        }
    }]);

    return AlphabetFilter;
}(React.Component);

exports.AlphabetFilter = AlphabetFilter;

/***/ }),

/***/ "./Scripts/Common/ExportFile/ExportFile.tsx":
/*!**************************************************!*\
  !*** ./Scripts/Common/ExportFile/ExportFile.tsx ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var Remote_1 = __webpack_require__(/*! ../../Common/Remote/Remote */ "./Scripts/Common/Remote/Remote.tsx");

var ExportFile = function (_React$Component) {
    _inherits(ExportFile, _React$Component);

    function ExportFile(props) {
        _classCallCheck(this, ExportFile);

        var _this = _possibleConstructorReturn(this, (ExportFile.__proto__ || Object.getPrototypeOf(ExportFile)).call(this, props));

        _this.setState({
            columns: []
        });
        _this.onExportClick = _this.onExportClick.bind(_this);
        _this.loadHeader = _this.loadHeader.bind(_this);
        return _this;
    }

    _createClass(ExportFile, [{
        key: "componentDidMount",
        value: function componentDidMount() {
            this.loadHeader();
        }
    }, {
        key: "loadHeader",
        value: function loadHeader() {
            var _this2 = this;

            var fieldUrl = "/GridFields/" + this.props.columnKey;
            Remote_1.Remote.get(fieldUrl, function (fieldList) {
                var column = [];
                for (var i in fieldList) {
                    if (Number(i) == fieldList.length - 1) {
                        column.push({
                            //                        ...arrData[i],
                            field: fieldList[i].fieldName,
                            title: fieldList[i].fieldLabel,
                            locked: false,
                            lockable: false
                        });
                    } else {
                        column.push({
                            //                        ...arrData[i],
                            field: fieldList[i].fieldName,
                            title: fieldList[i].fieldLabel
                        });
                    }
                }
                _this2.setState({ columns: column });
            }, function (error) {
                alert(error);
            });
        }
    }, {
        key: "onExportClick",
        value: function onExportClick() {
            this.loadHeader();
            var $this = this;
            var gridColumns = this.state.columns;
            var data = [];
            var url = this.props.exportURL + "&isExport=true";
            Remote_1.Remote.get(url, function (dataList) {
                for (var i = 0; i < dataList.data.length; i++) {
                    data.push(dataList.data[i]);
                }
                var grid = $("<div id='pdfGrid'></div>");
                grid.kendoGrid({
                    toolbar: ["pdf", "excel"],
                    pdf: {
                        allPages: true,
                        avoidLinks: true,
                        paperSize: "A4",
                        margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                        landscape: true,
                        repeatHeaders: true,
                        template: $("#page-template").html(),
                        scale: 0.8
                    },
                    dataSource: data,
                    height: 550,
                    sortable: true,
                    pageable: {
                        refresh: true,
                        pageSizes: true,
                        buttonCount: 5
                    },
                    columns: gridColumns
                });
                grid.data("kendoGrid").saveAsExcel();
            }, function (error) {
                alert(error);
            });
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement("button", { className: "k-button k-primary", onClick: this.onExportClick }, "Export");
        }
    }]);

    return ExportFile;
}(React.Component);

exports.ExportFile = ExportFile;

/***/ }),

/***/ "./Scripts/Common/Pagination/Pagination.tsx":
/*!**************************************************!*\
  !*** ./Scripts/Common/Pagination/Pagination.tsx ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");

var Pagination = function (_React$Component) {
    _inherits(Pagination, _React$Component);

    function Pagination(props) {
        _classCallCheck(this, Pagination);

        var _this = _possibleConstructorReturn(this, (Pagination.__proto__ || Object.getPrototypeOf(Pagination)).call(this, props));

        _this.state = {
            pageLimit: 5,
            pageInfo: ""
        };
        return _this;
    }

    _createClass(Pagination, [{
        key: "render",
        value: function render() {
            var _this2 = this;

            var pageNumbers = [];
            var a = Math.ceil(this.props.totalCount / this.props.pageSize);
            var totalPages = Math.ceil(this.props.totalCount / this.props.pageSize);
            var startPage = void 0,
                endPage = void 0;
            if (totalPages <= 10) {
                startPage = 1;
                endPage = totalPages;
            } else {
                if (this.props.activePage <= 6) {
                    startPage = 1;
                    endPage = 10;
                } else if (this.props.activePage + 4 >= totalPages) {
                    startPage = totalPages - 9;
                    endPage = totalPages;
                } else {
                    startPage = this.props.activePage - 5;
                    endPage = this.props.activePage + 4;
                }
            }
            var startIndex = (this.props.activePage - 1) * this.props.pageSize;
            var endIndex = Math.min(startIndex + this.props.pageSize - 1, this.props.totalCount - 1);
            for (var i = startPage; i <= endPage; i++) {
                pageNumbers.push(i);
            }
            var renderPageList = pageNumbers.map(function (number) {
                return React.createElement("li", { key: number }, React.createElement("a", { href: "javascript:void(0)", className: _this2.props.activePage === number ? 'k-state-selected' : 'k-link', onClick: function onClick() {
                        return _this2.props.onPageChange(number, (number - 1) * _this2.props.pageSize);
                    } }, number));
            });
            return React.createElement("div", { className: "k-pager-wrap k-grid-pager k-widget" }, React.createElement("a", { href: "javascript:void(0);", className: this.props.activePage === 1 ? 'k-link k-pager-nav k-pager-first k-state-disabled' : 'k-link k-pager-nav k-pager-first', onClick: function onClick() {
                    return _this2.props.onPageChange(1, 0);
                }, title: "Go to the first page" }, React.createElement("span", { className: "k-icon k-i-seek-w", "aria-label": "Go to the first page" })), React.createElement("a", { href: "#", className: this.props.activePage === 1 ? 'k-link k-pager-nav k-state-disabled' : 'k-link k-pager-nav', onClick: function onClick() {
                    return _this2.props.onPageChange(_this2.props.activePage - 1, (_this2.props.activePage - 2) * _this2.props.pageSize);
                }, title: "Go to the previous page" }, React.createElement("span", { className: "k-icon k-i-arrow-w", "aria-label": "Go to the previous page" })), React.createElement("ul", { className: "k-pager-numbers k-reset" }, renderPageList), React.createElement("a", { href: "#", className: this.props.activePage === totalPages ? 'k-link k-pager-nav k-pager-last k-state-disabled' : 'k-link k-pager-nav k-pager-last', onClick: function onClick() {
                    return _this2.props.onPageChange(_this2.props.activePage + 1, _this2.props.activePage * _this2.props.pageSize);
                }, title: "Go to the next page" }, React.createElement("span", { className: "k-icon k-i-arrow-e", "aria-label": "Go to the next page" })), React.createElement("a", { href: "#", className: this.props.activePage === totalPages ? 'k-link k-pager-nav k-pager-last k-state-disabled' : 'k-link k-pager-nav k-pager-last', onClick: function onClick() {
                    return _this2.props.onPageChange(Math.ceil(_this2.props.totalCount / _this2.props.pageSize), Math.ceil(_this2.props.totalCount / _this2.props.pageSize) * _this2.props.pageSize - _this2.props.pageSize);
                }, title: "Go to the last page" }, React.createElement("span", { className: "k-icon k-i-seek-e", "aria-label": "Go to the last page" })), React.createElement("label", { className: "k-pager-sizes k-label" }, React.createElement("select", { onChange: this.props.onPageSizeChange }, React.createElement("option", { value: "12" }, " 12 "), React.createElement("option", { value: "24" }, " 24 "), React.createElement("option", { value: "36" }, " 36 "), React.createElement("option", { value: "1000" }, " All ")), "items per page"), this.props.pageSize !== 1000 && React.createElement("div", { className: "k-pager-info k-label" }, (this.props.activePage - 1) * this.props.pageSize + 1, " - ", (this.props.activePage - 1) * this.props.pageSize + this.props.pageSize, " of ", this.props.totalCount, " items"), this.props.pageSize === 1000 && React.createElement("div", { className: "k-pager-info k-label" }, "Total ", this.props.totalCount, " items"));
        }
    }]);

    return Pagination;
}(React.Component);

exports.Pagination = Pagination;

/***/ }),

/***/ "./Scripts/Common/Remote/Remote.tsx":
/*!******************************************!*\
  !*** ./Scripts/Common/Remote/Remote.tsx ***!
  \******************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var __awaiter = undefined && undefined.__awaiter || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) {
            try {
                step(generator.next(value));
            } catch (e) {
                reject(e);
            }
        }
        function rejected(value) {
            try {
                step(generator["throw"](value));
            } catch (e) {
                reject(e);
            }
        }
        function step(result) {
            result.done ? resolve(result.value) : new P(function (resolve) {
                resolve(result.value);
            }).then(fulfilled, rejected);
        }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
__webpack_require__(/*! whatwg-fetch */ "./node_modules/whatwg-fetch/fetch.js");

var Remote = function () {
    function Remote() {
        _classCallCheck(this, Remote);
    }

    _createClass(Remote, null, [{
        key: "post",
        value: function post(url, form, callback, error) {
            var data = void 0;
            data = JSON.stringify(Remote.serializeToJson(form));
            fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }).then(function (response) {
                if (response.status == 200) return response.json();else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (!result.error) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "postAsync",
        value: function postAsync(url, form) {
            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee() {
                var data;
                return regeneratorRuntime.wrap(function _callee$(_context) {
                    while (1) {
                        switch (_context.prev = _context.next) {
                            case 0:
                                data = void 0;

                                data = JSON.stringify(Remote.serializeToJson(form));
                                _context.next = 4;
                                return fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });

                            case 4:
                                return _context.abrupt("return", _context.sent);

                            case 5:
                            case "end":
                                return _context.stop();
                        }
                    }
                }, _callee, this);
            }));
        }
    }, {
        key: "postData",
        value: function postData(url, data, callback, error) {
            fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }).then(function (response) {
                if (response.status == 200) return response.json();else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (result && !result.error) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "postDataAsync",
        value: function postDataAsync(url, data) {
            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee2() {
                return regeneratorRuntime.wrap(function _callee2$(_context2) {
                    while (1) {
                        switch (_context2.prev = _context2.next) {
                            case 0:
                                return _context2.abrupt("return", fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }));

                            case 1:
                            case "end":
                                return _context2.stop();
                        }
                    }
                }, _callee2, this);
            }));
        }
    }, {
        key: "postPlainFormData",
        value: function postPlainFormData(url, data, callback, error) {
            fetch(url, { credentials: 'include', method: 'post', body: data }).then(function (response) {
                if (response.status == 200) return response;else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (result.ok) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "postDataText",
        value: function postDataText(url, data, callback, error) {
            fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }).then(function (response) {
                if (response.status == 200) return response.text();else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (!result.error) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "get",
        value: function get(url, callback, error) {
            var dt = new Date().getTime();
            var url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
            fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }).then(function (response) {
                if (response.status == 200) return response.json();else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (result && !result.error) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "getAsync",
        value: function getAsync(url) {
            var url;
            return __awaiter(this, void 0, void 0, /*#__PURE__*/regeneratorRuntime.mark(function _callee3() {
                var dt;
                return regeneratorRuntime.wrap(function _callee3$(_context3) {
                    while (1) {
                        switch (_context3.prev = _context3.next) {
                            case 0:
                                dt = new Date().getTime();

                                url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
                                _context3.next = 4;
                                return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });

                            case 4:
                                return _context3.abrupt("return", _context3.sent);

                            case 5:
                            case "end":
                                return _context3.stop();
                        }
                    }
                }, _callee3, this);
            }));
        }
    }, {
        key: "getText",
        value: function getText(url, callback, error) {
            fetch(url, { credentials: 'include', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } }).then(function (response) {
                if (response.status == 200) return response.text();else return { error: true, statusText: response.statusText, originalMessage: response };
            }).catch(function (reason) {
                Remote.onError(reason, error);
            }).then(function (result) {
                if (!result.error) callback(result);else {
                    Remote.onError(result, error);
                }
            });
        }
    }, {
        key: "serializeToJson",
        value: function serializeToJson(serializer) {
            var data = serializer.serialize().split("&");
            var obj = {};
            for (var key in data) {
                if (obj[data[key].split("=")[0]]) {
                    if (typeof obj[data[key].split("=")[0]] == "string") {
                        var val = obj[data[key].split("=")[0]];
                        obj[data[key].split("=")[0]] = [];
                        obj[data[key].split("=")[0]].push(val);
                    }
                    obj[data[key].split("=")[0]].push(decodeURIComponent(data[key].split("=")[1]));
                } else {
                    obj[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
                }
            }
            return obj;
        }
    }, {
        key: "onError",
        value: function onError(data, callback) {
            var message = "";
            if (data && data.responseJSON && data.responseJSON.Message) {
                message += data.responseJSON.Message + "<br/>";
            } else if (data) {
                for (var i in data.responseJSON) {
                    if (data.responseJSON[i] != undefined && data.responseJSON[i].errors != undefined && data.responseJSON[i].errors.length > 0) {
                        message += data.responseJSON[i].errors[0].errorMessage + "<br/>";
                    }
                }
            }
            if (message == "" && data) message = data.statusText;
            callback(message);
        }
    }, {
        key: "download",
        value: function download(url, callback, error) {
            return $.fileDownload(url).done(callback).fail(error);
        }
    }]);

    return Remote;
}();

exports.Remote = Remote;

/***/ }),

/***/ "./Scripts/Component/OfficeDirectory/OfficeDirectory.tsx":
/*!***************************************************************!*\
  !*** ./Scripts/Component/OfficeDirectory/OfficeDirectory.tsx ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var Remote_1 = __webpack_require__(/*! ../../Common/Remote/Remote */ "./Scripts/Common/Remote/Remote.tsx");
var OfficeDirectoryBlock_1 = __webpack_require__(/*! ./OfficeDirectoryBlock */ "./Scripts/Component/OfficeDirectory/OfficeDirectoryBlock.tsx");
var Pagination_1 = __webpack_require__(/*! ../../Common/Pagination/Pagination */ "./Scripts/Common/Pagination/Pagination.tsx");
var AlphabetFilter_1 = __webpack_require__(/*! ../../Common/AlphabetFilter/AlphabetFilter */ "./Scripts/Common/AlphabetFilter/AlphabetFilter.tsx");
var ExportFile_1 = __webpack_require__(/*! ../../Common/ExportFile/ExportFile */ "./Scripts/Common/ExportFile/ExportFile.tsx");

var OfficeDirectory = function (_React$Component) {
    _inherits(OfficeDirectory, _React$Component);

    function OfficeDirectory(props) {
        _classCallCheck(this, OfficeDirectory);

        var _this = _possibleConstructorReturn(this, (OfficeDirectory.__proto__ || Object.getPrototypeOf(OfficeDirectory)).call(this, props));

        _this.state = {
            officeList: [],
            recordCount: 0,
            searchValue: '',
            skip: 0,
            pageSize: 12,
            filterBy: 'All',
            activeItem: 1,
            dataUrl: ""
        };
        _this.onSearch = _this.onSearch.bind(_this);
        _this.onPageChange = _this.onPageChange.bind(_this);
        _this.onPageSizeChange = _this.onPageSizeChange.bind(_this);
        _this.onClickFilter = _this.onClickFilter.bind(_this);
        _this.onClear = _this.onClear.bind(_this);
        _this.onKeypress = _this.onKeypress.bind(_this);
        _this.resetPage = _this.resetPage.bind(_this);
        return _this;
    }

    _createClass(OfficeDirectory, [{
        key: "renderBlock",
        value: function renderBlock() {
            var items = [];
            for (var i in this.state.officeList) {
                items.push(React.createElement(OfficeDirectoryBlock_1.OfficeDirectoryBlock, { key: i, properties: this.state.officeList[i] }));
            }
            return items;
        }
    }, {
        key: "resetPage",
        value: function resetPage() {
            this.setState({ activeItem: 1, skip: 0 });
        }
    }, {
        key: "child",
        value: function child() {
            React.createElement(ExportFile_1.ExportFile, { exportURL: this.state.dataUrl, columnKey: "Office" });
        }
    }, {
        key: "onPageChange",
        value: function onPageChange(pageNumber, offset) {
            this.setState({ activeItem: pageNumber });
            this.setState({ skip: offset }, this.loadData);
        }
    }, {
        key: "onSearch",
        value: function onSearch(e) {
            this.resetPage();
            this.setState({ filterBy: "All" });
            this.setState({ searchValue: this.searchTextBox.value }, this.loadData);
        }
    }, {
        key: "onClear",
        value: function onClear() {
            this.setState({ filterBy: "All" });
            this.searchTextBox.value = "";
            this.setState({ searchValue: "" }, this.loadData);
        }
    }, {
        key: "onKeypress",
        value: function onKeypress(e) {
            if (e.keyCode === 13) {
                this.setState({ filterBy: "All" });
                this.setState({ searchValue: this.searchTextBox.value }, this.loadData);
                this.resetPage();
            }
        }
    }, {
        key: "loadData",
        value: function loadData() {
            var $this = this;
            var url = this.props.dataUrl + "?searchValue=" + this.state.searchValue + "&filterBy=" + this.state.filterBy + "&pageSize=" + this.state.pageSize + "&skip=" + this.state.skip + "&sortField=&sortDirection=asc";
            this.setState({ dataUrl: url });
            Remote_1.Remote.get(url, function (data) {
                $this.setState({ officeList: data.data, recordCount: data.total });
            }, function (error) {
                alert(error);
            });
        }
    }, {
        key: "onPageSizeChange",
        value: function onPageSizeChange(e) {
            this.setState({ pageSize: parseInt(e.target.value) }, this.loadData);
        }
    }, {
        key: "onClickFilter",
        value: function onClickFilter(val, parent) {
            parent.setState({ activeItem: 1, skip: 0 });
            this.resetPage();
            parent.setState({ filterBy: val }, this.loadData);
        }
    }, {
        key: "componentDidMount",
        value: function componentDidMount() {
            this.loadData();
            this.resetPage();
        }
    }, {
        key: "render",
        value: function render() {
            var _this2 = this;

            return React.createElement("div", null, React.createElement("div", { className: "row" }, React.createElement("div", { className: "col" }, React.createElement("div", { className: "search-form-r" }, React.createElement("div", { className: "input-group mr-3" }, React.createElement("input", { defaultValue: this.state.searchValue, type: 'text', onChange: this.onKeypress, onKeyDown: this.onKeypress, placeholder: "Search", ref: function ref(c) {
                    _this2.searchTextBox = c;
                }, className: "form-control k-textbox" }), React.createElement("div", { className: "input-group-append", onClick: this.onSearch }, React.createElement("div", { className: "input-group-text" }, React.createElement("a", { href: "javascript:void(0)", className: "k-icon k-i-search" }, "\xA0"))))), React.createElement("div", { className: "clearfix d-block d-md-none" }), React.createElement("div", { className: "mt-3 mt-md-0" }, React.createElement(AlphabetFilter_1.AlphabetFilter, { filterBy: this.state.filterBy, parent: this, onClickFilter: this.onClickFilter }))), React.createElement("div", { className: "col-md-auto col-sm-12 mt-3 mt-md-0" }, React.createElement(ExportFile_1.ExportFile, { exportURL: this.state.dataUrl, columnKey: "Office" }))), React.createElement("div", { className: "row" }, this.state.searchValue !== "" && React.createElement("div", { className: "col search-pills-container" }, React.createElement("div", { className: "badge badge-pill badge-secondary" }, this.state.searchValue, " ", React.createElement("a", { href: "javascript:void(0)", onClick: this.onClear, id: "clearSearch", className: "pill-close" }, React.createElement("i", { className: "material-icons" }, "close"))))), React.createElement("div", { className: "row office-directory k-grid-container" }, this.state.officeList.length ? this.renderBlock() : React.createElement("p", { className: "col-sm-12 text-center" }, "No Records")), React.createElement(Pagination_1.Pagination, { totalCount: this.state.recordCount, pageSize: this.state.pageSize, activePage: this.state.activeItem, onPageChange: this.onPageChange, onPageSizeChange: this.onPageSizeChange }));
        }
    }]);

    return OfficeDirectory;
}(React.Component);

exports.OfficeDirectory = OfficeDirectory;

/***/ }),

/***/ "./Scripts/Component/OfficeDirectory/OfficeDirectoryBlock.tsx":
/*!********************************************************************!*\
  !*** ./Scripts/Component/OfficeDirectory/OfficeDirectoryBlock.tsx ***!
  \********************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");

var OfficeDirectoryBlock = function (_React$Component) {
    _inherits(OfficeDirectoryBlock, _React$Component);

    function OfficeDirectoryBlock(props) {
        _classCallCheck(this, OfficeDirectoryBlock);

        var _this = _possibleConstructorReturn(this, (OfficeDirectoryBlock.__proto__ || Object.getPrototypeOf(OfficeDirectoryBlock)).call(this, props));

        _this.state = {
            showDetails: false
        };
        _this.officeClicked = _this.officeClicked.bind(_this);
        return _this;
    }

    _createClass(OfficeDirectoryBlock, [{
        key: "officeClicked",
        value: function officeClicked(e) {
            this.setState({ showDetails: true });
        }
    }, {
        key: "render",
        value: function render() {
            debugger;
            return React.createElement("div", { className: "col-sm-6 col-md-4 col-lg-4 col-xl-3" }, React.createElement("div", { className: "item-block" }, React.createElement("h4", null, " ", React.createElement("span", { className: "text-primary" }, "  ", this.props.properties.officeCode, " "), " ", React.createElement("small", null, this.props.properties.officeName), " "), React.createElement("div", { className: "office-location mt-4" }, React.createElement("i", { className: "material-icons" }, "place"), React.createElement("label", { className: "mb-0 text-muted font-weight-bold" }, "Physical Address"), React.createElement("p", { className: "mb-0" }, this.props.properties.physicalAddress || "Not Available")), React.createElement("div", { className: "office-location mt-4" }, React.createElement("i", { className: "material-icons" }, "place"), React.createElement("label", { className: "mb-0 text-muted font-weight-bold" }, "Mailing Address"), React.createElement("p", { className: "mb-0" }, this.props.properties.mailingAddress || "Not Available")), React.createElement("div", { className: "row office-contact mt-4" }, React.createElement("div", { className: "col-md-6 d-flex align-items-center" }, React.createElement("i", { className: "material-icons" }, "phone"), React.createElement("span", null, this.props.properties.phone || "Not Available")), React.createElement("div", { className: "col-md-6 d-flex align-items-center" }, React.createElement("i", { className: "material-icons" }, "print"), React.createElement("span", null, this.props.properties.fax || "Not Available")))));
        }
    }]);

    return OfficeDirectoryBlock;
}(React.Component);

exports.OfficeDirectoryBlock = OfficeDirectoryBlock;

/***/ }),

/***/ "./Scripts/Component/OfficeDirectory/index.tsx":
/*!*****************************************************!*\
  !*** ./Scripts/Component/OfficeDirectory/index.tsx ***!
  \*****************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var ReactDOM = __webpack_require__(/*! react-dom */ "react-dom");
var OfficeDirectory_1 = __webpack_require__(/*! ./OfficeDirectory */ "./Scripts/Component/OfficeDirectory/OfficeDirectory.tsx");
if (document.getElementById("UserOfficeGrid")) {
    ReactDOM.render(React.createElement(OfficeDirectory_1.OfficeDirectory, { dataUrl: "/OfficeDirectory/Get", exportUrl: "/OfficeDirectory/Get?searchValue=&filterBy=All&pageSize=100&skip=0&sortField=&sortDirection=asc" }), document.getElementById("UserOfficeGrid"));
}

/***/ }),

/***/ "./node_modules/whatwg-fetch/fetch.js":
/*!********************************************!*\
  !*** ./node_modules/whatwg-fetch/fetch.js ***!
  \********************************************/
/*! exports provided: Headers, Request, Response, DOMException, fetch */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Headers", function() { return Headers; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Request", function() { return Request; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Response", function() { return Response; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DOMException", function() { return DOMException; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "fetch", function() { return fetch; });
var support = {
  searchParams: 'URLSearchParams' in self,
  iterable: 'Symbol' in self && 'iterator' in Symbol,
  blob:
    'FileReader' in self &&
    'Blob' in self &&
    (function() {
      try {
        new Blob()
        return true
      } catch (e) {
        return false
      }
    })(),
  formData: 'FormData' in self,
  arrayBuffer: 'ArrayBuffer' in self
}

function isDataView(obj) {
  return obj && DataView.prototype.isPrototypeOf(obj)
}

if (support.arrayBuffer) {
  var viewClasses = [
    '[object Int8Array]',
    '[object Uint8Array]',
    '[object Uint8ClampedArray]',
    '[object Int16Array]',
    '[object Uint16Array]',
    '[object Int32Array]',
    '[object Uint32Array]',
    '[object Float32Array]',
    '[object Float64Array]'
  ]

  var isArrayBufferView =
    ArrayBuffer.isView ||
    function(obj) {
      return obj && viewClasses.indexOf(Object.prototype.toString.call(obj)) > -1
    }
}

function normalizeName(name) {
  if (typeof name !== 'string') {
    name = String(name)
  }
  if (/[^a-z0-9\-#$%&'*+.^_`|~]/i.test(name)) {
    throw new TypeError('Invalid character in header field name')
  }
  return name.toLowerCase()
}

function normalizeValue(value) {
  if (typeof value !== 'string') {
    value = String(value)
  }
  return value
}

// Build a destructive iterator for the value list
function iteratorFor(items) {
  var iterator = {
    next: function() {
      var value = items.shift()
      return {done: value === undefined, value: value}
    }
  }

  if (support.iterable) {
    iterator[Symbol.iterator] = function() {
      return iterator
    }
  }

  return iterator
}

function Headers(headers) {
  this.map = {}

  if (headers instanceof Headers) {
    headers.forEach(function(value, name) {
      this.append(name, value)
    }, this)
  } else if (Array.isArray(headers)) {
    headers.forEach(function(header) {
      this.append(header[0], header[1])
    }, this)
  } else if (headers) {
    Object.getOwnPropertyNames(headers).forEach(function(name) {
      this.append(name, headers[name])
    }, this)
  }
}

Headers.prototype.append = function(name, value) {
  name = normalizeName(name)
  value = normalizeValue(value)
  var oldValue = this.map[name]
  this.map[name] = oldValue ? oldValue + ', ' + value : value
}

Headers.prototype['delete'] = function(name) {
  delete this.map[normalizeName(name)]
}

Headers.prototype.get = function(name) {
  name = normalizeName(name)
  return this.has(name) ? this.map[name] : null
}

Headers.prototype.has = function(name) {
  return this.map.hasOwnProperty(normalizeName(name))
}

Headers.prototype.set = function(name, value) {
  this.map[normalizeName(name)] = normalizeValue(value)
}

Headers.prototype.forEach = function(callback, thisArg) {
  for (var name in this.map) {
    if (this.map.hasOwnProperty(name)) {
      callback.call(thisArg, this.map[name], name, this)
    }
  }
}

Headers.prototype.keys = function() {
  var items = []
  this.forEach(function(value, name) {
    items.push(name)
  })
  return iteratorFor(items)
}

Headers.prototype.values = function() {
  var items = []
  this.forEach(function(value) {
    items.push(value)
  })
  return iteratorFor(items)
}

Headers.prototype.entries = function() {
  var items = []
  this.forEach(function(value, name) {
    items.push([name, value])
  })
  return iteratorFor(items)
}

if (support.iterable) {
  Headers.prototype[Symbol.iterator] = Headers.prototype.entries
}

function consumed(body) {
  if (body.bodyUsed) {
    return Promise.reject(new TypeError('Already read'))
  }
  body.bodyUsed = true
}

function fileReaderReady(reader) {
  return new Promise(function(resolve, reject) {
    reader.onload = function() {
      resolve(reader.result)
    }
    reader.onerror = function() {
      reject(reader.error)
    }
  })
}

function readBlobAsArrayBuffer(blob) {
  var reader = new FileReader()
  var promise = fileReaderReady(reader)
  reader.readAsArrayBuffer(blob)
  return promise
}

function readBlobAsText(blob) {
  var reader = new FileReader()
  var promise = fileReaderReady(reader)
  reader.readAsText(blob)
  return promise
}

function readArrayBufferAsText(buf) {
  var view = new Uint8Array(buf)
  var chars = new Array(view.length)

  for (var i = 0; i < view.length; i++) {
    chars[i] = String.fromCharCode(view[i])
  }
  return chars.join('')
}

function bufferClone(buf) {
  if (buf.slice) {
    return buf.slice(0)
  } else {
    var view = new Uint8Array(buf.byteLength)
    view.set(new Uint8Array(buf))
    return view.buffer
  }
}

function Body() {
  this.bodyUsed = false

  this._initBody = function(body) {
    this._bodyInit = body
    if (!body) {
      this._bodyText = ''
    } else if (typeof body === 'string') {
      this._bodyText = body
    } else if (support.blob && Blob.prototype.isPrototypeOf(body)) {
      this._bodyBlob = body
    } else if (support.formData && FormData.prototype.isPrototypeOf(body)) {
      this._bodyFormData = body
    } else if (support.searchParams && URLSearchParams.prototype.isPrototypeOf(body)) {
      this._bodyText = body.toString()
    } else if (support.arrayBuffer && support.blob && isDataView(body)) {
      this._bodyArrayBuffer = bufferClone(body.buffer)
      // IE 10-11 can't handle a DataView body.
      this._bodyInit = new Blob([this._bodyArrayBuffer])
    } else if (support.arrayBuffer && (ArrayBuffer.prototype.isPrototypeOf(body) || isArrayBufferView(body))) {
      this._bodyArrayBuffer = bufferClone(body)
    } else {
      this._bodyText = body = Object.prototype.toString.call(body)
    }

    if (!this.headers.get('content-type')) {
      if (typeof body === 'string') {
        this.headers.set('content-type', 'text/plain;charset=UTF-8')
      } else if (this._bodyBlob && this._bodyBlob.type) {
        this.headers.set('content-type', this._bodyBlob.type)
      } else if (support.searchParams && URLSearchParams.prototype.isPrototypeOf(body)) {
        this.headers.set('content-type', 'application/x-www-form-urlencoded;charset=UTF-8')
      }
    }
  }

  if (support.blob) {
    this.blob = function() {
      var rejected = consumed(this)
      if (rejected) {
        return rejected
      }

      if (this._bodyBlob) {
        return Promise.resolve(this._bodyBlob)
      } else if (this._bodyArrayBuffer) {
        return Promise.resolve(new Blob([this._bodyArrayBuffer]))
      } else if (this._bodyFormData) {
        throw new Error('could not read FormData body as blob')
      } else {
        return Promise.resolve(new Blob([this._bodyText]))
      }
    }

    this.arrayBuffer = function() {
      if (this._bodyArrayBuffer) {
        return consumed(this) || Promise.resolve(this._bodyArrayBuffer)
      } else {
        return this.blob().then(readBlobAsArrayBuffer)
      }
    }
  }

  this.text = function() {
    var rejected = consumed(this)
    if (rejected) {
      return rejected
    }

    if (this._bodyBlob) {
      return readBlobAsText(this._bodyBlob)
    } else if (this._bodyArrayBuffer) {
      return Promise.resolve(readArrayBufferAsText(this._bodyArrayBuffer))
    } else if (this._bodyFormData) {
      throw new Error('could not read FormData body as text')
    } else {
      return Promise.resolve(this._bodyText)
    }
  }

  if (support.formData) {
    this.formData = function() {
      return this.text().then(decode)
    }
  }

  this.json = function() {
    return this.text().then(JSON.parse)
  }

  return this
}

// HTTP methods whose capitalization should be normalized
var methods = ['DELETE', 'GET', 'HEAD', 'OPTIONS', 'POST', 'PUT']

function normalizeMethod(method) {
  var upcased = method.toUpperCase()
  return methods.indexOf(upcased) > -1 ? upcased : method
}

function Request(input, options) {
  options = options || {}
  var body = options.body

  if (input instanceof Request) {
    if (input.bodyUsed) {
      throw new TypeError('Already read')
    }
    this.url = input.url
    this.credentials = input.credentials
    if (!options.headers) {
      this.headers = new Headers(input.headers)
    }
    this.method = input.method
    this.mode = input.mode
    this.signal = input.signal
    if (!body && input._bodyInit != null) {
      body = input._bodyInit
      input.bodyUsed = true
    }
  } else {
    this.url = String(input)
  }

  this.credentials = options.credentials || this.credentials || 'same-origin'
  if (options.headers || !this.headers) {
    this.headers = new Headers(options.headers)
  }
  this.method = normalizeMethod(options.method || this.method || 'GET')
  this.mode = options.mode || this.mode || null
  this.signal = options.signal || this.signal
  this.referrer = null

  if ((this.method === 'GET' || this.method === 'HEAD') && body) {
    throw new TypeError('Body not allowed for GET or HEAD requests')
  }
  this._initBody(body)
}

Request.prototype.clone = function() {
  return new Request(this, {body: this._bodyInit})
}

function decode(body) {
  var form = new FormData()
  body
    .trim()
    .split('&')
    .forEach(function(bytes) {
      if (bytes) {
        var split = bytes.split('=')
        var name = split.shift().replace(/\+/g, ' ')
        var value = split.join('=').replace(/\+/g, ' ')
        form.append(decodeURIComponent(name), decodeURIComponent(value))
      }
    })
  return form
}

function parseHeaders(rawHeaders) {
  var headers = new Headers()
  // Replace instances of \r\n and \n followed by at least one space or horizontal tab with a space
  // https://tools.ietf.org/html/rfc7230#section-3.2
  var preProcessedHeaders = rawHeaders.replace(/\r?\n[\t ]+/g, ' ')
  preProcessedHeaders.split(/\r?\n/).forEach(function(line) {
    var parts = line.split(':')
    var key = parts.shift().trim()
    if (key) {
      var value = parts.join(':').trim()
      headers.append(key, value)
    }
  })
  return headers
}

Body.call(Request.prototype)

function Response(bodyInit, options) {
  if (!options) {
    options = {}
  }

  this.type = 'default'
  this.status = options.status === undefined ? 200 : options.status
  this.ok = this.status >= 200 && this.status < 300
  this.statusText = 'statusText' in options ? options.statusText : 'OK'
  this.headers = new Headers(options.headers)
  this.url = options.url || ''
  this._initBody(bodyInit)
}

Body.call(Response.prototype)

Response.prototype.clone = function() {
  return new Response(this._bodyInit, {
    status: this.status,
    statusText: this.statusText,
    headers: new Headers(this.headers),
    url: this.url
  })
}

Response.error = function() {
  var response = new Response(null, {status: 0, statusText: ''})
  response.type = 'error'
  return response
}

var redirectStatuses = [301, 302, 303, 307, 308]

Response.redirect = function(url, status) {
  if (redirectStatuses.indexOf(status) === -1) {
    throw new RangeError('Invalid status code')
  }

  return new Response(null, {status: status, headers: {location: url}})
}

var DOMException = self.DOMException
try {
  new DOMException()
} catch (err) {
  DOMException = function(message, name) {
    this.message = message
    this.name = name
    var error = Error(message)
    this.stack = error.stack
  }
  DOMException.prototype = Object.create(Error.prototype)
  DOMException.prototype.constructor = DOMException
}

function fetch(input, init) {
  return new Promise(function(resolve, reject) {
    var request = new Request(input, init)

    if (request.signal && request.signal.aborted) {
      return reject(new DOMException('Aborted', 'AbortError'))
    }

    var xhr = new XMLHttpRequest()

    function abortXhr() {
      xhr.abort()
    }

    xhr.onload = function() {
      var options = {
        status: xhr.status,
        statusText: xhr.statusText,
        headers: parseHeaders(xhr.getAllResponseHeaders() || '')
      }
      options.url = 'responseURL' in xhr ? xhr.responseURL : options.headers.get('X-Request-URL')
      var body = 'response' in xhr ? xhr.response : xhr.responseText
      resolve(new Response(body, options))
    }

    xhr.onerror = function() {
      reject(new TypeError('Network request failed'))
    }

    xhr.ontimeout = function() {
      reject(new TypeError('Network request failed'))
    }

    xhr.onabort = function() {
      reject(new DOMException('Aborted', 'AbortError'))
    }

    xhr.open(request.method, request.url, true)

    if (request.credentials === 'include') {
      xhr.withCredentials = true
    } else if (request.credentials === 'omit') {
      xhr.withCredentials = false
    }

    if ('responseType' in xhr && support.blob) {
      xhr.responseType = 'blob'
    }

    request.headers.forEach(function(value, name) {
      xhr.setRequestHeader(name, value)
    })

    if (request.signal) {
      request.signal.addEventListener('abort', abortXhr)

      xhr.onreadystatechange = function() {
        // DONE (success or failure)
        if (xhr.readyState === 4) {
          request.signal.removeEventListener('abort', abortXhr)
        }
      }
    }

    xhr.send(typeof request._bodyInit === 'undefined' ? null : request._bodyInit)
  })
}

fetch.polyfill = true

if (!self.fetch) {
  self.fetch = fetch
  self.Headers = Headers
  self.Request = Request
  self.Response = Response
}


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
//# sourceMappingURL=officegrid.js.map