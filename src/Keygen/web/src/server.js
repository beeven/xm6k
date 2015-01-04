var express = require("express"),
    app = express(),
    bodyParser = require("body-parser");

var generator = require("./generator"),
    dataProvider = require("./dataProvider");

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
    console.log(data);

    dataProvider.log(data);
    dataProvider.checkBlacklist(data).then(function(result){
        console.log("check result: ",result);
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
    console.log(req.body);
    var email = req.param('mailaddr');
    console.log(email);
    res.send("Mail sent");
});

app.get("/",function(req,res){
    res.send("Hello");
});

app.listen(3000);
