const config = require('../config');
const environment = config.environment;
const PRODUCTION = !environment.isDevelopment;
const gulp = require('gulp');
const gulpif = require('gulp-if');
const eslint = require('gulp-eslint');
const eslintConfig = require('eslint-config-actum').getConfig({ environment: false });

const lint = (globs) => {
  const options = {
    configFile: eslintConfig,
    rules: {
      "indent": ["error", 2],
      "class-methods-use-this": [0, {"exceptMethods": ["render", "componentDidMount"]}]
    }
  };

  return gulp.src(globs)
    .pipe(eslint(options))
    .pipe(eslint.format());
};

gulp.task('eslint:app', () => lint(config.JS_ALL));

gulp.task('eslint', ['eslint:app']);
