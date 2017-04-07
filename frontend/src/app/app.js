/**
 * https://github.com/Keyamoon/svgxuse
 * If you do not use SVG <use xlink:href="…"> elements, remove svgxuse module
 */
// import 'svgxuse';
// import configureStore from './store';
import { init, render } from './init';

const app = {
  run() {
    this.static();
    this.react();
  },

  /* Static JavaScript classes */
  static() {
    init('spotfire', document.getElementsByClassName('js-spotfire'));
    init('num-format', document.getElementsByClassName('js-num-format'));
    init('tabs', document.getElementsByClassName('js-tabs'));
    init('collapse', document.getElementsByClassName('js-collapse'));
  },

  /* React */
  react() {
    /* Configure Redux store */
    // window.store = configureStore();
    render('StyleguideInput', document.querySelectorAll('.styleguide-input'), { store: false });
  },
};

/* Global scope reference */
window.app = app;

/* Run */
app.run();
