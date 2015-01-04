var nodemailer = require("nodemailer"),
    markdown = require("nodemailer-markdown").markdown,
    licGen = require("./generator"),
    Q = require("q")

var transporter = nodemailer.createTransport({
    service: "Hotmail",
    auth: {
        user: "beeven@hotmail.com",
        pass: "qwer*1234"
    }
});

transporter.use("compile",markdown());


var sendMail = function(addr, content){
    var deferred = Q.defer();
    var mailOptions = {
        from: "Beeven Yip <beeven@hotmail.com>",
        to: addr,
        subject: "XMind License",
        markdown: content,
        attachments: [
            {
                path: __dirname + "/public/attachments/net.xmind.verify_3.5.1.201411201906.jar"
            },
            {
                path: __dirname + "/public/attachments/org.xmind.meggy_3.5.1.201411201906.jar"
            }
        ],
    };

    transporter.sendMail(mailOptions,function(err,info){
        if(err) {
            deferred.reject(err);
        } else {
            deferred.resolve(info);
        }
    });

    return deferred.promise;
};

var sendSerial = function(addr) {
    var serial = licGen.generateLicense(addr);
    var content = "**"+serial+"**\n\n"
                + "先不要打开XMind，两个文件不用解压缩，放到XMind的安装目录的Plugins目录下面替换。\n\n"
                    + "一般是C:\\Program Files(x86)\\XMind\\plugins\n\n"
                    + "Mac的话，是用Finder找到XMind，右键，显示包内容，放到Contents/Resources/plugins下面。\n\n"
                    + "然后打开XMind，帮助、序列号、输入序列号按钮，填入你的邮箱和这个序列号(粗体部分，包含---BEGIN和 END---)\n\n";
    return sendMail(addr,content);
};

exports.sendMail = sendMail;
exports.sendSerial = sendSerial;
