var nodemailer = require("nodemailer"),
    //markdown = require("nodemailer-markdown").markdown,
    licGen = require("./generator"),
    Q = require("q")

var transporter = nodemailer.createTransport({
    service: "QQ",
    auth: {
        user: "35239520@qq.com",
        pass: "########"
    }
});

//transporter.use("compile",markdown());


var sendMail = function(addr, content,version){
    var deferred = Q.defer();
    var mailOptions = {
        from: "Beeven Yip <35239520@qq.com>",
        to: addr,
        subject: "XMind License for 3.5.2",
        html: content,
        attachments: [
            {
                path: __dirname + "/public/attachments/net.xmind.verify_3.5.2.201504270119.jar"
            },
            {
                path: __dirname + "/public/attachments/org.xmind.meggy_3.5.2.201504270119.jar"
            }
        ],
        debug: true
    };
    
    if(version == "3.5.1") {
    mailOptions = {
        from: "Beeven Yip <35239520@qq.com>",
        to: addr,
        subject: "XMind License for 3.5.1",
        html: content,
        attachments: [
            {
                path: __dirname + "/public/attachments/net.xmind.verify_3.5.1.201411201906.jar"
            },
            {
                path: __dirname + "/public/attachments/org.xmind.meggy_3.5.1.201411201906.jar"
            }
        ],
        debug: true
    };
    }

    transporter.sendMail(mailOptions,function(err,info){
        if(err) {
            deferred.reject(err);
        } else {
            deferred.resolve(info);
        }
    });

    return deferred.promise;
};

var sendSerial = function(addr,version) {
    var serial = licGen.generateLicense(addr.toLowerCase());

    var content = "<p><b>"+serial.replace(/\n/g,"<br/>")+"</b></p>"
                + "<p>XMind 还没安装的话，可以到百度盘共享下载 <a href=\"http://pan.baidu.com/s/1jGIMc3W\">http://pan.baidu.com/s/1jGIMc3W</a></p>"
                + "<p>1. 分别下载邮件中的两个附件，不用解压缩</p>"
                    + "<p>2. 关闭xmind</p>"
                    + "<p>3. 把附件直接拖到XMind安装目录的Plugins目录下进行覆盖。（一般是C:\Program Files(x86)\XMind\plugins）<br/>"
                    + "Mac的话，是用Finder找到XMind，右键，显示包内容，放到Contents/Resources/plugins下面。<br/>"
                    + "(如果Mac显示程序已损坏，则到系统设置-&gt;安全-&gt;只允许Mac App Store的软件运行，改成允许任何来源)</p>"
                    + "<p>4.打开XMind，帮助-&gt;序列号-&gt;输入序列号按钮，填入你的邮箱"
                    + " " + addr.toLowerCase() + " "
                    + "和上述序列号(粗体部分，包含---BEGIN和 END---)</p>";
    return sendMail(addr,content,version);
};

exports.sendMail = sendMail;
exports.sendSerial = sendSerial;
