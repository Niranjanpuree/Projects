System.registerDynamic("ng2-toastr/src/toast-container.component", ["@angular/core", "./toast-options", "@angular/platform-browser"], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  var core_1 = $__require('@angular/core');
  var toast_options_1 = $__require('./toast-options');
  var platform_browser_1 = $__require('@angular/platform-browser');
  var ToastContainer = (function() {
    function ToastContainer(sanitizer, options) {
      this.sanitizer = sanitizer;
      this.position = 'fixed';
      this.messageClass = 'toast-message';
      this.titleClass = 'toast-title';
      this.positionClass = 'toast-top-right';
      this.toasts = [];
      this.maxShown = 5;
      this.animate = 'fade';
      if (options) {
        Object.assign(this, options);
      }
    }
    ToastContainer.prototype.addToast = function(toast) {
      if (this.positionClass.indexOf('top') > 0) {
        this.toasts.push(toast);
        if (this.toasts.length > this.maxShown) {
          this.toasts.splice(0, (this.toasts.length - this.maxShown));
        }
      } else {
        this.toasts.unshift(toast);
        if (this.toasts.length > this.maxShown) {
          this.toasts.splice(this.maxShown, (this.toasts.length - this.maxShown));
        }
      }
    };
    ToastContainer.prototype.removeToast = function(toastId) {
      this.toasts = this.toasts.filter(function(toast) {
        return toast.id !== toastId;
      });
    };
    ToastContainer.prototype.removeAllToasts = function() {
      this.toasts = [];
    };
    ToastContainer.prototype.dismiss = function(toast) {
      if (!toast.autoDismiss) {
        this.removeToast(toast.id);
      }
    };
    ToastContainer.prototype.anyToast = function() {
      return this.toasts.length > 0;
    };
    ToastContainer.prototype.findToast = function(toastId) {
      for (var _i = 0,
          _a = this.toasts; _i < _a.length; _i++) {
        var toast = _a[_i];
        if (toast.id === toastId) {
          return toast;
        }
      }
      return null;
    };
    ToastContainer.decorators = [{
      type: core_1.Component,
      args: [{
        selector: 'toast-container',
        template: "\n    <div id=\"toast-container\" [style.position]=\"position\" class=\"{{positionClass}}\">\n      <div *ngFor=\"let toast of toasts\" [@inOut]=\"animate\" class=\"toast toast-{{toast.type}}\" (click)=\"dismiss(toast)\">\n        <div *ngIf=\"toast.title\" class=\"{{toast.titleClass || titleClass}}\">{{toast.title}}</div>\n        <div [ngSwitch]=\"toast.enableHTML\">\n          <span *ngSwitchCase=\"true\" [innerHTML]=\"sanitizer.bypassSecurityTrustHtml(toast.message)\"></span>\n          <span *ngSwitchDefault class=\"{{toast.messageClass || messageClass}}\">{{toast.message}}</span>\n        </div>              \n      </div>\n    </div>\n    ",
        animations: [core_1.trigger('inOut', [core_1.state('flyRight, flyLeft', core_1.style({
          opacity: 1,
          transform: 'translateX(0)'
        })), core_1.state('fade', core_1.style({opacity: 1})), core_1.state('slideDown, slideUp', core_1.style({
          opacity: 1,
          transform: 'translateY(0)'
        })), core_1.transition('void => flyRight', [core_1.style({
          opacity: 0,
          transform: 'translateX(100%)'
        }), core_1.animate('0.2s ease-in')]), core_1.transition('flyRight => void', [core_1.animate('0.2s 10 ease-out', core_1.style({
          opacity: 0,
          transform: 'translateX(100%)'
        }))]), core_1.transition('void => flyLeft', [core_1.style({
          opacity: 0,
          transform: 'translateX(-100%)'
        }), core_1.animate('0.2s ease-in')]), core_1.transition('flyLeft => void', [core_1.animate('0.2s 10 ease-out', core_1.style({
          opacity: 0,
          transform: 'translateX(-100%)'
        }))]), core_1.transition('void => fade', [core_1.style({opacity: 0}), core_1.animate('0.3s ease-in')]), core_1.transition('fade => void', [core_1.animate('0.3s 10 ease-out', core_1.style({opacity: 0}))]), core_1.transition('void => slideDown', [core_1.style({
          opacity: 0,
          transform: 'translateY(-200%)'
        }), core_1.animate('0.3s ease-in')]), core_1.transition('slideDown => void', [core_1.animate('0.3s 10 ease-out', core_1.style({
          opacity: 0,
          transform: 'translateY(-200%)'
        }))]), core_1.transition('void => slideUp', [core_1.style({
          opacity: 0,
          transform: 'translateY(200%)'
        }), core_1.animate('0.3s ease-in')]), core_1.transition('slideUp => void', [core_1.animate('0.3s 10 ease-out', core_1.style({
          opacity: 0,
          transform: 'translateY(200%)'
        }))])])]
      }]
    }];
    ToastContainer.ctorParameters = [{type: platform_browser_1.DomSanitizer}, {
      type: toast_options_1.ToastOptions,
      decorators: [{type: core_1.Optional}]
    }];
    return ToastContainer;
  }());
  exports.ToastContainer = ToastContainer;
  global.define = __define;
  return module.exports;
});

System.registerDynamic("ng2-toastr/src/toast", [], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  var Toast = (function() {
    function Toast(type, message, title) {
      this.type = type;
      this.message = message;
      this.title = title;
      this.autoDismiss = true;
      this.enableHTML = false;
    }
    return Toast;
  }());
  exports.Toast = Toast;
  global.define = __define;
  return module.exports;
});

System.registerDynamic("ng2-toastr/src/toast-manager", ["@angular/core", "./toast-container.component", "./toast-options", "./toast"], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  var core_1 = $__require('@angular/core');
  var toast_container_component_1 = $__require('./toast-container.component');
  var toast_options_1 = $__require('./toast-options');
  var toast_1 = $__require('./toast');
  var ToastsManager = (function() {
    function ToastsManager(componentFactoryResolver, appRef, options) {
      this.componentFactoryResolver = componentFactoryResolver;
      this.appRef = appRef;
      this.options = {
        autoDismiss: true,
        toastLife: 3000
      };
      this.index = 0;
      if (options) {
        Object.assign(this.options, options);
      }
    }
    ToastsManager.prototype.show = function(toast, options) {
      if (!this.container) {
        if (!this.appRef['_rootComponents'].length) {
          console.error('Application root component cannot be found. Try accessing application reference in the later life cycle of angular app.');
          return;
        }
        var appContainer = this.appRef['_rootComponents'][0]['_hostElement'].vcRef;
        var providers = core_1.ReflectiveInjector.resolve([{
          provide: toast_options_1.ToastOptions,
          useValue: this.options
        }]);
        var toastFactory = this.componentFactoryResolver.resolveComponentFactory(toast_container_component_1.ToastContainer);
        var childInjector = core_1.ReflectiveInjector.fromResolvedProviders(providers, appContainer.parentInjector);
        this.container = appContainer.createComponent(toastFactory, appContainer.length, childInjector);
        this.setupToast(toast, options);
      } else {
        this.setupToast(toast, options);
      }
    };
    ToastsManager.prototype.createTimeout = function(toastId, timeout) {
      var _this = this;
      var life = timeout || this.options.toastLife;
      setTimeout(function() {
        _this.clearToast(toastId);
      }, life);
    };
    ToastsManager.prototype.setupToast = function(toast, options) {
      toast.id = ++this.index;
      if (options && typeof(options.messageClass) === 'string') {
        toast.messageClass = options.messageClass;
      }
      if (options && typeof(options.titleClass) === 'string') {
        toast.titleClass = options.titleClass;
      }
      if (options && typeof(options.enableHTML) === 'boolean') {
        toast.enableHTML = options.enableHTML;
      }
      if (options && typeof(options.autoDismiss) === 'boolean') {
        toast.autoDismiss = options.autoDismiss;
      } else {
        toast.autoDismiss = this.options.autoDismiss;
      }
      if (options && typeof(options.toastLife) === 'number') {
        toast.autoDismiss = true;
        this.createTimeout(toast.id, options.toastLife);
      } else if (toast.autoDismiss) {
        this.createTimeout(toast.id);
      }
      this.container.instance.addToast(toast);
    };
    ToastsManager.prototype.clearToast = function(toastId) {
      if (this.container) {
        var instance = this.container.instance;
        instance.removeToast(toastId);
        if (!instance.anyToast()) {
          this.dispose();
        }
      }
    };
    ToastsManager.prototype.clearAllToasts = function() {
      if (this.container) {
        var instance = this.container.instance;
        instance.removeAllToasts();
        if (!instance.anyToast()) {
          this.dispose();
        }
      }
    };
    ToastsManager.prototype.dispose = function() {
      this.container.destroy();
      this.container = null;
    };
    ToastsManager.prototype.error = function(message, title, options) {
      var toast = new toast_1.Toast('error', message, title);
      this.show(toast, options);
    };
    ToastsManager.prototype.info = function(message, title, options) {
      var toast = new toast_1.Toast('info', message, title);
      this.show(toast, options);
    };
    ToastsManager.prototype.success = function(message, title, options) {
      var toast = new toast_1.Toast('success', message, title);
      this.show(toast, options);
    };
    ToastsManager.prototype.warning = function(message, title, options) {
      var toast = new toast_1.Toast('warning', message, title);
      this.show(toast, options);
    };
    ToastsManager.prototype.custom = function(message, title, options) {
      var toast = new toast_1.Toast('custom', message, title);
      this.show(toast, options);
    };
    ToastsManager.decorators = [{type: core_1.Injectable}];
    ToastsManager.ctorParameters = [{type: core_1.ComponentFactoryResolver}, {type: core_1.ApplicationRef}, {
      type: toast_options_1.ToastOptions,
      decorators: [{type: core_1.Optional}]
    }];
    return ToastsManager;
  }());
  exports.ToastsManager = ToastsManager;
  global.define = __define;
  return module.exports;
});

System.registerDynamic("ng2-toastr/src/toast-options", ["@angular/core"], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  var core_1 = $__require('@angular/core');
  var ToastOptions = (function() {
    function ToastOptions(options) {
      this.enableHTML = false;
      this.animate = 'fade';
      Object.assign(this, options);
    }
    ToastOptions.decorators = [{type: core_1.Injectable}];
    ToastOptions.ctorParameters = [{type: Object}];
    return ToastOptions;
  }());
  exports.ToastOptions = ToastOptions;
  global.define = __define;
  return module.exports;
});

System.registerDynamic("ng2-toastr/src/toast.module", ["@angular/core", "@angular/common", "./toast-container.component", "./toast-manager", "./toast-options"], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  var core_1 = $__require('@angular/core');
  var common_1 = $__require('@angular/common');
  var toast_container_component_1 = $__require('./toast-container.component');
  var toast_manager_1 = $__require('./toast-manager');
  var toast_options_1 = $__require('./toast-options');
  var ToastModule = (function() {
    function ToastModule() {}
    ToastModule.forRoot = function(config) {
      return {
        ngModule: ToastModule,
        providers: [{
          provide: toast_options_1.ToastOptions,
          useValue: config
        }]
      };
    };
    ToastModule.decorators = [{
      type: core_1.NgModule,
      args: [{
        imports: [common_1.CommonModule],
        declarations: [toast_container_component_1.ToastContainer],
        exports: [toast_container_component_1.ToastContainer],
        providers: [toast_manager_1.ToastsManager],
        entryComponents: [toast_container_component_1.ToastContainer]
      }]
    }];
    ToastModule.ctorParameters = [];
    return ToastModule;
  }());
  exports.ToastModule = ToastModule;
  global.define = __define;
  return module.exports;
});

System.registerDynamic("ng2-toastr/ng2-toastr", ["./src/toast", "./src/toast-manager", "./src/toast-container.component", "./src/toast-options", "./src/toast.module"], true, function($__require, exports, module) {
  "use strict";
  ;
  var global = this,
      __define = global.define;
  global.define = undefined;
  function __export(m) {
    for (var p in m)
      if (!exports.hasOwnProperty(p))
        exports[p] = m[p];
  }
  __export($__require('./src/toast'));
  __export($__require('./src/toast-manager'));
  __export($__require('./src/toast-container.component'));
  __export($__require('./src/toast-options'));
  __export($__require('./src/toast.module'));
  global.define = __define;
  return module.exports;
});

//# sourceMappingURL=ng2-toastr.js.map