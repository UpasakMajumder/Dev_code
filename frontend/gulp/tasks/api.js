const config = require('../config');
const gulp = require('gulp');
const gutil = require('gulp-util');
const enableDestroy = require('server-destroy');
const jsonServer = require('json-server');
const getPageItemsController = require('../../src/api/controllers/getPageItems');
const modifyAddressController = require('../../src/api/controllers/settingsAddresses/modifyAddress');
var server;

function requireUncached(module) {
  delete require.cache[require.resolve(module)];
  return require(module)
}

function start(cb) {
  const api = requireUncached('../../src/api/api');
  const app = jsonServer.create();
  const router = jsonServer.router(api());
  const middleware = jsonServer.defaults();

  app.use(jsonServer.bodyParser);

  /* custom routes */
  app.get(...getPageItemsController);
  app.post(...modifyAddressController);

  app.use(middleware);
  app.use(router);

  server = app.listen(config.API_PORT, () => {
    gutil.log(
      gutil.colors.green(`JSON Server is running…`),
      gutil.colors.gray(`http://localhost:${config.API_PORT}`)
    );
  });
  enableDestroy(server);
}

gulp.task('api', start);

gulp.task('api-reload', (cb) => {
  gutil.log(gutil.colors.gray('api has changed, reloading...'));
  server && server.destroy();
  start();
  return cb();
});
