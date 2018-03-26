const config = require('../config');
const DEVELOPMENT = config.environment.isDevelopment;
const gulp = require('gulp');
const runSequence = require('run-sequence');

// TODO run browserSync after all tasks finished

const codeTasks = ['images', 'svg', 'styles'];
if (!DEVELOPMENT) codeTasks.push('js');

const devSequence = ['clean', codeTasks, 'tpl', 'copySgAssets'];
if (config.environment.isApi) {
  devSequence.push('api');
}
const buildSequence = devSequence;
const sequence = DEVELOPMENT ? devSequence : buildSequence;

gulp.task('prepare', () => runSequence(...sequence));

module.exports = {
  buildSequence
};
