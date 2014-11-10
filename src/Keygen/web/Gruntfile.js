module.exports = function(grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON("./package.json"),
        shell: {
            run: {
                command: function() {

                }
            }
        }
    });

    grunt.loadNpmTasks("grunt-shell");

    grunt.registerTask('run',['shell:run']);
};
