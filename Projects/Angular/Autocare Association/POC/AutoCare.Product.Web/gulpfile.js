/// <binding AfterBuild='copy-npmlibs, copy-aclibs' Clean='clean:aclibs, clean:npmlibs' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

"use strict";
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    npmSrc: "./node_modules/",
    npmDest: "./Scripts/lib/npmlibs",
    aclibSrc: "./Scripts/lib/aclibs",
    aclibDest: "./app/lib/aclibs",
};

var npmlibs = [
    paths.npmSrc + '/@angular/**/*.js',
    paths.npmSrc + '/zone.js/**/*.js',
    paths.npmSrc + '/reflect-metadata/**/*.js',
    paths.npmSrc + '/systemjs/**/*.js',
    paths.npmSrc + '/es6-shim/**/*.js',
    paths.npmSrc + '/rxjs/**/*.js',
    paths.npmSrc + '/ng2-bs3-modal/**/*.js',
    paths.npmSrc + '/core-js/**/*.js',
];

var aclibs = [
    paths.aclibSrc + '/ac-grid/*.html',
    paths.aclibSrc + '/ac-grid/*.css',
    paths.aclibSrc + '/ac-autocomplete/*.html',
    paths.aclibSrc + '/ac-autocomplete/*.css',
    paths.aclibSrc + '/ac-fileuploader/*.html',
    paths.aclibSrc + '/ac-fileuploader/*.css',
    paths.aclibSrc + '/ng2-toastr/**/*.js',
    paths.aclibSrc + '/ng2-datetime/*.html',
    paths.aclibSrc + '/ng2-datetime/*.css',
      paths.aclibSrc + '/ng2-datetime/*.js',

]

gulp.task("clean:npmlibs", function (cb) {
    rimraf(paths.npmDest, cb);
});

gulp.task("clean:aclibs", function (cb) {
    rimraf(paths.aclibDest, cb);
});

gulp.task("clean", ["clean:npmlibs", "clean:aclibs"]);


gulp.task("copy-npmlibs", function () {
    return gulp.src(npmlibs)
         .pipe(gulp.dest(function (file) {
             return file.base.replace('node_modules', 'Scripts/lib/npmlibs');
         }));
});

gulp.task("copy-aclibs", function () {
    return gulp.src(aclibs)
         .pipe(gulp.dest(function (file) {
             return file.base.replace('Scripts', 'app');
         }));
});