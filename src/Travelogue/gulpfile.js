var gulp = require('gulp');

//Using package to minifying files 
uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/"
};

//Getting Path of Javascript files 
paths.js = paths.webroot + "js/**/*.js";

//Path to Writing minified Files after minifying
paths.Destination = paths.webroot + "MinifiedJavascriptfiles";

// Task Name [MinifyingTask]
gulp.task('MinifyingTask', function () {   // path to your files
    gulp.src(paths.js)
        // concat files
        .pipe(uglify())
        // Writing files to Destination
        .pipe(gulp.dest(paths.Destination));
});