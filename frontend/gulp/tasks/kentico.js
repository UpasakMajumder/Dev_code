const gulp = require('gulp');
const runSequence = require('run-sequence');
const merge = require('merge-stream');
const config = require('../config');

gulp.task('replace-kentico', () => {
  const css = gulp.src(`${config.CSS_BUILD}/*.min.css`)
    .pipe(gulp.dest(`${config.KENTICO_DEST}/css`));

  const js = gulp.src(`${config.JS_BUILD}/**/*`)
    .pipe(gulp.dest(`${config.KENTICO_DEST}/js`));

  const gfx = gulp.src(`${config.GFX_BUILD}/**/*`)
    .pipe(gulp.dest(`${config.KENTICO_DEST}/gfx`));

  return merge(css, js, gfx);
});

gulp.task('kentico', (done) => {
  runSequence('clean', ['images', 'svg', 'styles', 'js'], 'tpl', 'copySgAssets', 'replace-kentico', () => {
    done();
  })
});
