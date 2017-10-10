const config = require('../config');
const gulp = require('gulp');
const plumber = require('gulp-plumber');
const webpack = require('webpack');
const notify = require("gulp-notify");
const webpackConfig = require('../webpack.config.js');
const gutil = require('gulp-util');

function bundle(done) {
  webpack(webpackConfig, (error, stats) => {
    if (error) {
      throw new Error(error);
    }
    const jsonStats = stats.toJson();
    const buildError = jsonStats.errors[0];

    if (buildError) {
      throw new Error(buildError);
    }

    gutil.log('[webpack]', stats.toString({
      colors: true,
      version: false,
      hash: false,
      chunks: false,
      chunkModules: false
    }));
    done();
  });
}

gulp.task('js', done => bundle(done));
