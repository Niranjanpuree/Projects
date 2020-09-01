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
/******/ 	return __webpack_require__(__webpack_require__.s = "./Scripts/Component/NotificationSummary/index.tsx");
/******/ })
/************************************************************************/
/******/ ({

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

/***/ "./Scripts/Component/NotificationSummary/NotificationSummary.tsx":
/*!***********************************************************************!*\
  !*** ./Scripts/Component/NotificationSummary/NotificationSummary.tsx ***!
  \***********************************************************************/
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

var NotificationSummary = function (_React$Component) {
    _inherits(NotificationSummary, _React$Component);

    function NotificationSummary(props) {
        _classCallCheck(this, NotificationSummary);

        var _this = _possibleConstructorReturn(this, (NotificationSummary.__proto__ || Object.getPrototypeOf(NotificationSummary)).call(this, props));

        _this.dataList = [];
        _this.handleClick = _this.handleClick.bind(_this);
        _this.individualNotificationClick = _this.individualNotificationClick.bind(_this);
        _this.state = {
            notificationList: [],
            unreadNotificationList: [],
            notificationProps: ''
        };
        return _this;
    }

    _createClass(NotificationSummary, [{
        key: "componentDidUpdate",
        value: function componentDidUpdate(prevProps, prevState, snapshot) {
            var sender = this;
            setTimeout(function () {
                sender.getDesktopNotification();
            }, 30000);
        }
    }, {
        key: "getDesktopNotification",
        value: function getDesktopNotification() {
            var sender = this;
            Remote_1.Remote.get("/Notification/GetUnReadDesktopNotification", function (response) {
                sender.dataList = response;
                sender.setState({
                    unreadNotificationList: response
                });
            }, function (err) {
                window.Dialog.alert(err);
            });
        }
    }, {
        key: "componentDidMount",
        value: function componentDidMount() {
            this.getDesktopNotification();
        }
    }, {
        key: "render",
        value: function render() {
            var _this2 = this;

            return React.createElement(React.Fragment, null, React.createElement("a", { className: "dropdown-toggle", href: "#", role: "button", "data-toggle": "dropdown", onClick: this.handleClick, id: "notificationSummary" }, React.createElement("i", { className: "menu-icon material-icons" }, "notifications"), React.createElement("span", { className: this.state.unreadNotificationList.length === 0 ? "d-none" : "notification-count" }, this.state.unreadNotificationList.length)), React.createElement("div", { className: "dropdown-menu dropdown-menu-right", "aria-labelledby": "notificationSummary" }, React.createElement("div", { className: "notification-list" }, React.createElement("ul", { className: "list-unstyled" }, this.state.unreadNotificationList.map(function (value, index) {
                return React.createElement("li", { key: index }, React.createElement("span", { onClick: function onClick() {
                        return _this2.individualNotificationClick(value);
                    }, className: "notification-item individualNotification" }, React.createElement("p", { className: "notification-subject" }, value.subject), React.createElement("p", { className: "notification-message-additional" }, value.additionalMessage), React.createElement("div", { className: "notification-item-footer" }, React.createElement("span", { className: "notification-item-created" }, value.createdByName), React.createElement("span", { className: "notification-item-date" }, React.createElement("i", { className: "k-icon k-i-clock" }), value.createdOnFormatDateTime))));
            }), this.state.unreadNotificationList.length === 0 && React.createElement("li", { className: "notification-empty p-3 text-center" }, React.createElement("i", { className: "material-icons text-muted" }, "notifications_off"), React.createElement("p", { className: "mb-0" }, "There isn't any new notification."))), React.createElement("div", { className: "card-footer p-0" }, React.createElement("a", { href: this.props.siteUrl + "/admin/inbox", className: "btn-block btn btn-sm btn-light p-2 rounded-bottom text-primary" }, "View All")))));
        }
    }, {
        key: "handleClick",
        value: function handleClick() {
            this.setState({
                notificationList: this.dataList
            });
        }
    }, {
        key: "refresh",
        value: function refresh() {}
    }, {
        key: "individualNotificationClick",
        value: function individualNotificationClick(notificationProps) {
            Remote_1.Remote.postData("/Notification/EditUserResponseByNotificationMessageId", notificationProps.notificationMessageGuid, function (data) {}, function (error) {});
            window.location.href = this.props.siteUrl + "/Admin/Inbox?notificationMessageGuid=" + notificationProps.notificationMessageGuid;
        }
    }]);

    return NotificationSummary;
}(React.Component);

exports.NotificationSummary = NotificationSummary;

/***/ }),

/***/ "./Scripts/Component/NotificationSummary/index.tsx":
/*!*********************************************************!*\
  !*** ./Scripts/Component/NotificationSummary/index.tsx ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var ReactDOM = __webpack_require__(/*! react-dom */ "react-dom");
var NotificationSummary_1 = __webpack_require__(/*! ./NotificationSummary */ "./Scripts/Component/NotificationSummary/NotificationSummary.tsx");
function loadNotificationSummary(siteUrl) {
    ReactDOM.render(React.createElement(NotificationSummary_1.NotificationSummary, { parentDomId: "notificationSummary", siteUrl: siteUrl }), document.getElementById("notificationSummary"));
}
window.notificationSummary = { pageView: { loadNotificationSummary: loadNotificationSummary } };

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
//# sourceMappingURL=notificationSummary.js.map