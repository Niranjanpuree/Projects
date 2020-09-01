var gulp = require('gulp');
var del = require('del');
var sass = require('gulp-sass');
var sourcemaps = require('gulp-sourcemaps');
var autoprefixer = require('gulp-autoprefixer');
var plumber = require('gulp-plumber');
var notify = require("gulp-notify");
var browserSync = require('browser-sync').create();
var concat = require('gulp-concat');
var cmq = require('gulp-combine-mq');
var filter = require('gulp-filter');
var sassGlob = require('gulp-sass-glob');
var wait = require('gulp-wait');
var filter = require('gulp-filter');
var cache = require('gulp-cache');
var uglify = require('gulp-uglify'),
    minifycss = require('gulp-uglifycss');

var paths = {
    scripts: ['Scripts/**/*.js', 'Scripts/**/*.ts', 'Scripts/**/*.map'],
    all_scss: 'wwwroot/scss/**/*scss',
    scss: 'wwwroot/scss/custom/**/*scss',
    css: 'wwwroot/css/',
    vendor: 'wwwroot/scss/vendor/**/*scss'
};
var autoprefixBrowsers = ['> 1%', 'last 2 versions', 'firefox >= 4', 'safari 7', 'safari 8', 'IE 8', 'IE 9', 'IE 10', 'IE 11'];

gulp.task('styles', function (e) {
    var onError = function (err) {
        notify.onError({
            title: "Gulp Sass",
            subtitle: "Failure",
            message: "<%= error.message %>",
            sound: "Beep"
        })(err);
        this.emit('end');
    };
    gulp.src(paths.scss)
        .pipe(wait(100))
        .pipe(sassGlob())
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourcemaps.init())
        .pipe(sass())
        .pipe(concat('northwind.css'))
        .pipe(sourcemaps.write({
            includeContent: false
        }))
        .pipe(sourcemaps.init({
            loadMaps: true
        }))
        .pipe(autoprefixer({ browsers: autoprefixBrowsers }))
        .pipe(sourcemaps.write('.'))
        .pipe(plumber.stop())
        .pipe(gulp.dest(paths.css))
        .pipe(browserSync.reload({
            stream: true
        }))
    e();
});

gulp.task('styles_vendor', function (e) {
    var onError = function (err) {
        notify.onError({
            title: "Gulp Sass",
            subtitle: "Failure",
            message: "<%= error.message %>",
            sound: "Beep"
        })(err);
        this.emit('end');
    };
    gulp.src([paths.vendor])
        .pipe(wait(100))
        .pipe(plumber({
            errorHandler: onError
        }))
        .pipe(sourcemaps.init())
        .pipe(sass())
        .pipe(sourcemaps.write({
            includeContent: false
        }))
        .pipe(sourcemaps.init({
            loadMaps: true
        }))
        .pipe(autoprefixer())
        .pipe(sourcemaps.write('.'))
        .pipe(plumber.stop())
        .pipe(gulp.dest(paths.css))
        .pipe(browserSync.reload({
            stream: true
        }))
    e();
});



gulp.task('styles-build', function (e) {
    gulp.src(paths.scss)
        .pipe(sassGlob())
        .pipe(plumber())
        .pipe(sass({
            includePaths: [paths.scss]
        }))
        .pipe(concat('northwind.css'))
        .pipe(autoprefixer({ browsers: autoprefixBrowsers }))
        .pipe(plumber.stop())
        .pipe(gulp.dest(paths.css))
        .pipe(filter('**/*.css')) // Filtering stream to only css files
        .pipe(cmq()) // Combines Media Queries
        .pipe(minifycss({
            //maxLineLen: 80//(length of string to be in one line css)
        }))
        .pipe(gulp.dest(paths.css))
        .pipe(browserSync.reload({
            stream: true
        }))
        .pipe(notify({
            message: 'Styles for build task complete',
            onLast: true
        }))
    e();
});

gulp.task('styles_vendor-build', function (e) {
    gulp.src(paths.vendor)
        .pipe(plumber())
        .pipe(sass({
            includePaths: [paths.scss]
        }))
        .pipe(autoprefixer())
        .pipe(plumber.stop())
        .pipe(gulp.dest(paths.css))
        .pipe(filter('**/*.css')) // Filtering stream to only css files
        .pipe(cmq()) // Combines Media Queries
        .pipe(minifycss({
            //maxLineLen: 80//(length of string to be in one line css)
        }))
        .pipe(gulp.dest(paths.css))
        .pipe(browserSync.reload({
            stream: true
        }))
        .pipe(notify({
            message: 'Styles for build task complete',
            onLast: true
        }))
    e();
});

gulp.task('clear', function (e) {
    cache.clearAll();
    e();
});

gulp.task('watch', function () {
    browserSync.init({
        proxy: "localhost:59440/",
        // server: {
        //     baseDir: './'
        // },
        port: 9999,
        open: 'external'
    });
    gulp.watch([paths.scss, paths.vendor], gulp.series('styles', 'styles_vendor'));
    gulp.watch('./*.html').on("change", browserSync.reload);
    gulp.watch('./*.cshtml').on("change", browserSync.reload);
});


gulp.task('run', gulp.series('clear', gulp.parallel('styles', 'styles_vendor', 'watch'), function () {}));

gulp.task('build', gulp.series('clear', gulp.parallel('styles-build', 'styles_vendor-build'), function (e) {
    e();
}));

gulp.task('default', function () {
    gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/js'))
});