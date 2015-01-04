var crypt = require("crypto");
var b32 = require('b32');
//var process = require("process");
var privateKeyString = "-----BEGIN RSA PRIVATE KEY-----\n" +
            "MIICWwIBAAKBgQC7M22HWJKXzWLqWkaZdWnw40Nf4ibBv3Ug2FJfPrYVJyJzv9PR\n" +
            "BbmKNndf19pKGW7DE/Gd9EPK0PwIw7NzFwIdSxLtvdBVDQ33tXo/sj9jaZ4+i1+h\n" +
            "hiS+lIk6DqvEQeQqWYxT4/n842Hf5X+SFnfeI+h4NzJXnAfiuP9Sv9idjwIDAQAB\n" +
            "AoGAArjfud0e3Jg+/PttFWQwszEK1MUcHSskj+K1Z+8ohKw6AwbiFKMJrKnYGIux\n" +
            "/+vYwXtzwwE/Tx6024fE/0JxZGdKLwYdJg7A1cRcWHXHWvxm7ZFV6g6txBNokilr\n" +
            "Du+0YC1cx0pcFXZxtDqVIHyOcD0oGTBzRXdZRaDlGTqPoKECQQDbT3jYnVH+BP/I\n" +
            "MqhsEZ8n+e/qbopXnKgZ+r/J19WH134Hp3tFSlc/TmE1MM8JcNaeCAPxU0lTjopQ\n" +
            "MEhB85cxAkEA2oTIW53g+DsPyC3Z3B8OMYwjk+sTQv6eFnREi/RXSSScz1qY6yTQ\n" +
            "v25gy1fgaod8wJ15OrvjZ7Ry9V+HObPQvwJAes7e8hXuoxtzjEzpyVJ42G76bUGn\n" +
            "UHZWH+4tVb76QM5oMasUuXFut/GRN7L0TOoWFHqSkqG77wFj7JGM9PmOEQJAKoCH\n" +
            "I+K6Xi3GrQ3WAQ19Sj/Sr7OaZH53qzBkIMCetZhMc7xQX/QiMgm7A/IMsUu3BRPb\n" +
            "/4OKAKLFdVpmR9/kqwJAVehtNPMws2eJbKO9rgK9f7z+SHzDWhLA0x9KL+wAylL3\n" +
            "WF2Be4VpExuD53dxiEyMbLOJ1aLaKzycXa1DB7Cp0Q==\n" +
            "-----END RSA PRIVATE KEY-----";


exports.generateLicense = function generateLicense(email) {
  var first20 = "XAXM35I1512345678901";
  var keyInfo = "Ae;\t&2}w#n`If5!nu6#{!hBA1IMDx%\n";
  var keyBase = new Buffer(first20+email+keyInfo);
  var signer = crypt.createSign("RSA-SHA256");
  signer.update(keyBase);
  var signature = signer.sign(privateKeyString);
  var license = b32.encodeSync(signature).toString();
  return "---BEGIN LICENSE KEY---\n"
      + first20 + license  + "\n"
      + "---END LICENSE KEY---";
};
