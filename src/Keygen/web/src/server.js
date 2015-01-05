var express = require("express"),
    app = express(),
    bodyParser = require("body-parser");

var generator = require("./generator"),
    dataProvider = require("./dataProvider"),
    mailer = require("./mailer");

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended:true}));

app.use(express.static(__dirname + "/public"));

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
    mailer.sendSerial(email)
        .then(function(){
            res.send("Mail sent");
        })
        .fail(function(err){
            res.status(500).send(err);
            console.log(err);
        });

});

app.get("/",function(req,res){
    res.send("Hello");
});

app.listen(3000);
