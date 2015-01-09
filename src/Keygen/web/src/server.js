var express = require("express"),
    app = express(),
    bodyParser = require("body-parser");

var generator = require("./generator"),
    dataProvider = require("./dataProvider"),
    mailer = require("./mailer");

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended:true}));

app.use(express.static(__dirname + "/public"));


var verifyCert = function(req, res, next) {
    if(!req.secure && req.header('x-forwarded-proto') != 'https') {
        console.error("Not using ssl");
        return res.send(403,"Not using ssl");
    }
    if(!req.client.authorized) {
        var e = new Error("Unauthorized: Client certificate required "
                            + "(" + req.client.authorizationError + ")");
        e.status = 401;
        return next(e);
    }
    var cert = req.connection.getPeerCertificate();
    if(!cert || !Object.keys(cert).length) {
        var e = new Error("Client certificate was authenticated but certificate " +
                    "information could not be retrieved.");
        e.status = 500;
        return next(e);
    }
    console.log(cert);
    next();
}
app.post("/log/:email",function(req,res){
    var email = req.params.email;
    var data = {
        email: email,
        info: req.body,
        ip: req.headers['x-real-ip'] || req.ip
    };

    dataProvider.log(data);
    dataProvider.checkBlacklist(data).then(function(result){
        if(result) {
            res.json({
                License: generator.generateLicense(email)
            });
        } else {
            res.status(403).end();
        }
    });

});

app.post("/sendmail",function(req,res){
    var email = req.param('mailaddr');
    if(email != null) {
        mailer.sendSerial(email.trim())
        .then(function(){
            res.send("Mail sent");
        })
        .fail(function(err){
            res.status(500).send(err);
            console.log(err);
        });
    } else {
        res.status(403).end();
    }
    

});

app.get("/test",verifyCert,function(req,res){
    res.send("Hello");
});
app.all("/test/:email",verifyCert,function(req,res){
    res.send("Hello");
});


var http = require("http"),
    https = require("https"),
    fs = require('fs');
var httpsOptions = {
    key: fs.readFileSync('/etc/nginx/certificates/server.key'),
    cert: fs.readFileSync('/etc/nginx/certificates/server.crt'),
    ca: [fs.readFileSync('/etc/nginx/certificates/ca.crt')],
    honorCipherOrder: true,
    requestCert: true,
    rejectUnauthorized: false
}
https.createServer(httpsOptions,app).listen(443);
http.createServer(app).listen(3000);
