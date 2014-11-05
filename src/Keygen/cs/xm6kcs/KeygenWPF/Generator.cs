using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeygenWPF
{
    public class Generator
    {
        private const String privateKeyString = "-----BEGIN RSA PRIVATE KEY-----\n" +
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

        public String generateLicenseKey(String licenseEmail)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("X"); // prefix
            builder.Append("A"); // key type
            builder.Append("XM"); // vendor name
            builder.Append("3"); // major version
            builder.Append("5"); // minor version
            builder.Append("I"); // licensee type
            builder.Append("15"); // years of upgrade
            builder.Append("12345678901");

            String first20 = builder.ToString();

            String keyInfo = "Ae;\t&2}w#n`If5!nu6#{!hBA1IMDx%\n";
            byte[] keyBase = System.Text.Encoding.UTF8.GetBytes(first20 + licenseEmail + keyInfo);
            byte[] signature = sign(keyBase);
            String signatureString = Base32.encode(signature);
            return first20 + signatureString;
        }

        public byte[] sign(byte[] bytesToSign)
        {
            PemReader reader = new PemReader(new System.IO.StringReader(privateKeyString));
            AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)reader.ReadObject();
            AsymmetricKeyParameter privateKey = keyPair.Private;
            ISigner signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(bytesToSign,0,bytesToSign.Length);
            return signer.GenerateSignature();
        }

    }
}
