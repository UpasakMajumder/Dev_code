webpackJsonp([6],{327:function(i,t,n){try{(function(){"use strict";function i(i,t){if(!(i instanceof t))throw new TypeError("Cannot call a class as a function")}Object.defineProperty(t,"__esModule",{value:!0});var n=function(){function i(i,t){for(var n=0;n<t.length;n++){var o=t[n];o.enumerable=o.enumerable||!1,o.configurable=!0,"value"in o&&(o.writable=!0),Object.defineProperty(i,o.key,o)}}return function(t,n,o){return n&&i(t.prototype,n),o&&i(t,o),t}}(),o=function(){function t(n){i(this,t),this.container=n,this.timeForAnimation=+n.dataset.timeForAnimation,this.timeToRemove=+n.dataset.timeToRemove;this.notification={};for(var o=1;o<=3;o+=1)this.notification[o]=void 0}return n(t,[{key:"addNotification",value:function(i){function t(){var t=o.container.querySelector('[data-notification-type="'+i+'"]'),e=t.cloneNode(!0);return e.querySelector(".js-notification-closer").addEventListener("click",function(i){var t=i.currentTarget.parentNode;t.classList.add("hide"),n(t),setTimeout(function(){t.remove()},o.timeForAnimation)}),e}function n(i){var t=Object.keys(o.notification),n=Object.values(o.notification),e=n.indexOf(i),a=t[e];o.notification[a]=void 0,Object.keys(o.notification).forEach(function(i){if(i>a&&o.notification[i]){var t=o.notification[i],n=i-1;t.classList.remove("show-"+i),t.classList.add("show-"+n),o.notification[n]=t,o.notification[i]=void 0}})}var o=this;!function(){var i=Object.keys(o.notification).length,t=o.notification[i];t&&(t.classList.add("hide"),o.notification[i]=void 0,setTimeout(function(){t.remove()},o.timeForAnimation))}(),function(){for(var i=Object.keys(o.notification).length,t=i;t>0;t-=1)if(t!==i){var n=o.notification[t];if(n){n.classList.remove("show-"+t);var e=t+1;n.classList.add("show-"+e),o.notification[e]=n}}}(),function(){var i=t();o.notification[1]=i,o.container.appendChild(i),window.getComputedStyle(i).opacity,i.classList.add("show-1"),setTimeout(function(){i.classList.add("hide")},o.timeToRemove),setTimeout(function(){i.remove()},o.timeForAnimation+o.timeToRemove)}()}}]),t}();t.default=o}).call(this)}finally{}}});