/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./Assets/Values/js/components/valuesEditor/index.ts":
/*!***********************************************************!*\
  !*** ./Assets/Values/js/components/valuesEditor/index.ts ***!
  \***********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__)\n/* harmony export */ });\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! vue */ \"vue\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_0__);\n\r\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = ((initialData, element) => {\r\n    return new (vue__WEBPACK_IMPORTED_MODULE_0___default())({\r\n        el: element,\r\n        data: {\r\n            items: [],\r\n            newValue: '',\r\n        },\r\n        computed: {\r\n            hasValues() {\r\n                return this.items.length > 0;\r\n            },\r\n            value() {\r\n                return JSON.stringify(this.items);\r\n            },\r\n        },\r\n        mounted: function () {\r\n            this.items = initialData;\r\n        },\r\n        methods: {\r\n            add: function () {\r\n                if (!this.newValue) {\r\n                    return;\r\n                }\r\n                this.items.push(this.newValue);\r\n                this.newValue = '';\r\n            },\r\n            remove: function (index) {\r\n                this.items.splice(index, 1);\r\n            },\r\n        },\r\n    });\r\n});\r\n\n\n//# sourceURL=webpack://etch.orchardcore.lever/./Assets/Values/js/components/valuesEditor/index.ts?");

/***/ }),

/***/ "./Assets/Values/js/index.ts":
/*!***********************************!*\
  !*** ./Assets/Values/js/index.ts ***!
  \***********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _components_valuesEditor__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./components/valuesEditor */ \"./Assets/Values/js/components/valuesEditor/index.ts\");\n\r\nwindow.initializeValuesEditor = (el) => {\r\n    const $hiddenDataField = document.getElementById($(el).data('for'));\r\n    if (!$hiddenDataField) {\r\n        return;\r\n    }\r\n    (0,_components_valuesEditor__WEBPACK_IMPORTED_MODULE_0__[\"default\"])($($hiddenDataField).data('init'), el);\r\n};\r\n\n\n//# sourceURL=webpack://etch.orchardcore.lever/./Assets/Values/js/index.ts?");

/***/ }),

/***/ "vue":
/*!**********************!*\
  !*** external "Vue" ***!
  \**********************/
/***/ ((module) => {

module.exports = Vue;

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("./Assets/Values/js/index.ts");
/******/ 	
/******/ })()
;