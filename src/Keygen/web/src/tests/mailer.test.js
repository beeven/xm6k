

var mailer = require("../mailer");

mailer.sendSerial("35239520@qq.com")
    .then(function(info){
        console.log("Success:",info);
    })
    .fail(function(err){
        console.log("Error:",err);
    });
