import React from 'react'; // eslint-disable-line no-unused-vars
import { render as DOMRender } from 'react-dom';
import { Provider } from 'react-redux'; // eslint-disable-line no-unused-vars
import { resolve } from 'path';

/* Dynamic module request */
function request(moduleName, callback, isComponent = true) {
  /* Request static classes and React Components from different folders */
  const rootFolder = isComponent ? './components' : './static';
  const modulePath = `.${resolve(rootFolder, moduleName, 'index.js')}`;

  System.import(`${modulePath}`)
    .then(module => callback(module))
    .catch(error => new Error(`Render: Errow while rendering Component "${module.name}" (${module}}): ${error}`));
}

/* Initialize simple JavaScript modules */
export function init(moduleName, containers, ...options) {
  containers = Array.from(containers);

  if (containers.length) {
    request(moduleName, (Module) => { // eslint-disable-line arrow-body-style
      /* eslint-disable new-cap */
      return containers.map(container => new Module.default(container, ...options));
    }, false);
    /* eslint-enable */
  }

  return false;
}

/* Initialize React Components */
export function render(componentName, containers, options = {
  store: true // true: use Redux store; false: don't
}) {
  /* Convert containers to Array */
  containers = Array.from(containers);

  if (containers.length) {
    request(componentName, (module) => {
      const Component = module.default;

      /* eslint-disable array-callback-return */
      return containers.map((container) => {
        /* Configure initial props */
        let initialProps = {};
        /* eslint-disable no-prototype-builtins */
        Component.hasOwnProperty('configureProps') && (initialProps = Component.configureProps(container));
        /* eslint-enable */

        let RenderOutput = <Component {...initialProps} />;
        options.store && (RenderOutput = (<Provider store={window.store}>{RenderOutput}</Provider>));
        /* eslint-disable new-cap */
        DOMRender(RenderOutput, container);
        /* eslint-enable */
      });
    });
  }

  return false;
}
