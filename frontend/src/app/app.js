/**
 * https://github.com/Keyamoon/svgxuse
 * If you do not use SVG <use xlink:href="…"> elements, remove svgxuse module
 */
// import 'svgxuse';
// import configureStore from './store';
import { init } from './init';

const app = {
  run() {
    this.static();
    this.react();
  },

  /* Static JavaScript classes */
  static() {
    init('spotfire', document.getElementsByClassName('js-spotfire'));
  },

  /* React */
  react() {
    /* Configure Redux store */
    // window.store = configureStore();

    // render('Gallery', document.querySelector('.gallery'), { store: false });
  },
};

/* Global scope reference */
window.app = app;

/* Run */
app.run();
