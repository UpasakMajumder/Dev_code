const config = require('../config');
const DEVELOPMENT = config.environment.isDevelopment;
const gulp = require('gulp');
const gutil = require('gulp-util');
const gwatch = require('gulp-watch');
const browserSync = require('browser-sync');
const copyToClipboard = require('copy-paste').copy;
const runSequence = require('run-sequence');
const port = config.PORT;
const webpackDevMiddleware = require('webpack-dev-middleware');
const webpackConfig = require('../webpack.config.js');
const webpack = require('webpack');
const compiler = webpack(webpackConfig);

gulp.task('serve', ['prepare'], () => {
  const baseDir = DEVELOPMENT ? [
    config.DEVELOPMENT_BASE,
    config.BUILD_BASE,
    config.NPM,
    config.STYLEGUIDE_BASE

  ] : config.BUILD_BASE;

  browserSync({
    port,
    server: {baseDir},
    open: false,
    middleware: [
      webpackDevMiddleware(compiler, {
        publicPath: webpackConfig.output.publicPath,
        historyApiFallback: true,
        /* Make sure to set {lazy: false} in order to use aggregate timout */
        lazy: false,
        watchOptions: {
          /* Wait for any other changes before bundling */
          aggregateTimeout: 1500
        },
        stats: {
          colors: true,
          version: false,
          hash: false,
          chunks: false,
          chunkModules: false
        }
      })
    ]
  }, (unknown, bs) => {
    const finalPort = bs.options.get('port');
    copyToClipboard(
      `localhost:${finalPort}`,
      () => gutil.log(gutil.colors.green('Local server address has been copied to your clipboard'))
    )
  });

  const watch = (glob, tasks) => gwatch(glob, () => runSequence(...tasks));

  if (DEVELOPMENT) {
    watch(config.CSS_ALL, ['styles', 'copySgAssets']);
    watch(config.IMAGES_ALL, ['images', 'tpl']);
    watch(config.SVG_SPRITE_ALL, ['svg', 'tpl']);
    watch(config.TEMPLATE_ALL, ['tpl']);
    watch(config.API, ['api-reload']);
  }
});
