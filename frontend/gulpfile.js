const config = require('./gulp/config');
config.environment.check();
process.env.NODE_ENV = config.environment.type;

const gulp = require('gulp');

const normalizedPath = require('path').join(__dirname, './gulp/tasks');

require('fs').readdirSync(normalizedPath).forEach(function(file) {
  require('./gulp/tasks/' + file);
});

/* API */
gulp.task('default', ['serve']);
gulp.task('build', ['prepare']);
gulp.task('css', ['less']);
