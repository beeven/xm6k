var isWin = /^win/.test(process.platform);
var isMac = /^darwin/.test(process.platform);
var isLinux32 = /^linux/.test(process.platform);
var isLinux64 = /^linux64/.test(process.platform);

var os = "unknown";

if (isWin)
    os = "win";
if (isMac)
    os = "mac";
if (isLinux32)
    os = "linux32";
if (isLinux64)
    os = "linux64";

var nwVer = '0.10.5';

var nwExec = "";

if (!isMac)
    nwExec = "cd cache/" + nwVer + "/" + os + " && nw ../../../src";
else
    nwExec = "cd cache/" + nwVer + "/" + os + " && open -n -a node-webkit ../../../src";


console.log("OS: " + os);

module.exports = function(grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('./package.json'),
        nodewebkit: {
            options: {
                version: nwVer,
                build_dir: './',
                mac: isMac,
                win: isWin,
                linux32: isLinux32,
                linux64: isLinux64,
                keep_nw: false,
                zip: false,
                mac_icns:'./src/images/angular-desktop-app.icns'
            },
            src: ['./src/**/*']
        },
        copy:{
          debug: {
            files:[
              {expand:true,src:['bootstrap.css'], dest:"./src/css/",cwd:"./src/lib/bootstrap/dist/css/"},
              {expand:true,src:['./src/lib/bootstrap/dist/fonts/*'], dest:"./src/fonts/",flatten:true},
              {expand:true,src:['jquery.js'], dest:"./src/js/vendor/",cwd:"./src/lib/jquery/dist/"},
              {expand:true,src:['./src/lib/requirejs*/*.js'],dest:"./src/js/vendor/",flatten:true},
              {expand:true,src:['angular.min.js'],dest:"./src/js/vendor/",cwd:"./src/lib/angular/"},
              {expand:true,src:['ui-bootstrap-tpls.js'],dest:"./src/js/vendor/",cwd:"./src/lib/angular-bootstrap/"},
              {expand:true,src:['angular-ui-router.js'],dest:"./src/js/vendor/",cwd:"./src/lib/angular-ui-router/release/"}
            ]
          },
          release: {

          }
        },
        stylus:{
          debug: {
            options: {
              urlfunc: 'embedurl',
            },
            files: {
              './src/css/app.css': './src/css/app.styl'
            }
          }
        },
        watch: {
          css: {
            files: ['./src/css/**/*.styl'],
            tasks: ['stylus']
          }
        },
        clean: ["./releases/**/*"],
        shell: {
            install: {
                command: function() {
                    return 'bower cache clean && bower install && cd src && npm install';
                },
                options: {
                    stdout: true,
                    stderr: true,
                    stdin: true
                }
            },
            run: {
                command: function() {
                    return nwExec;
                },
                options: {
                    stdout: true,
                    stderr: true,
                    stdin: true
                }

            }
        }

    });

    grunt.loadNpmTasks('grunt-node-webkit-builder');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-shell');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-stylus');
    grunt.registerTask('default', ['shell:run']);
    grunt.registerTask('run', ['default']);
    grunt.registerTask('install', ['shell:install', 'nodewebkit']);
    grunt.registerTask('build', ['nodewebkit']);


};
