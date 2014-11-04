import org.bouncycastle.asn1.pkcs.PrivateKeyInfo;
import org.bouncycastle.asn1.pkcs.RSAPrivateKey;
import org.bouncycastle.crypto.digests.MD5Digest;
import org.bouncycastle.crypto.engines.RSAEngine;
import org.bouncycastle.crypto.params.RSAPrivateCrtKeyParameters;
import org.bouncycastle.crypto.signers.PSSSigner;
import org.bouncycastle.openssl.PEMKeyPair;
import org.bouncycastle.openssl.PEMParser;

import java.io.StringReader;
import java.util.Scanner;

/**
 * Created by beeven on 11/4/14.
 */



public class Generator {
    public static final String privateKeyString = "-----BEGIN RSA PRIVATE KEY-----\n" +
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

    public static byte[] sign(byte[] bytesToSign) throws Exception {
        PEMParser parser = new PEMParser(new StringReader(privateKeyString));
        Object keyPair = parser.readObject();

        PEMKeyPair pemKeyPair = (PEMKeyPair) keyPair;
        PrivateKeyInfo privateKeyInfo = pemKeyPair.getPrivateKeyInfo();
        RSAPrivateKey privateKey = RSAPrivateKey.getInstance(privateKeyInfo.parsePrivateKey());
        RSAPrivateCrtKeyParameters kparam = new RSAPrivateCrtKeyParameters(privateKey.getModulus(),
                privateKey.getPublicExponent(),
                privateKey.getPrivateExponent(),
                privateKey.getPrime1(),
                privateKey.getPrime2(),
                privateKey.getExponent1(),
                privateKey.getExponent2(),
                privateKey.getCoefficient());

        PSSSigner signer = new PSSSigner(new RSAEngine(), new MD5Digest(), 32);
        signer.init(true, kparam);
        signer.update(bytesToSign, 0, bytesToSign.length);
        return signer.generateSignature();
    }
    public static String generateLicenseKey(String licenseEmail)
            throws Exception {
        StringBuilder builder = new StringBuilder();
        builder.append("X"); // prefix
        builder.append("A"); // key type
        builder.append("XM"); // vendor name
        builder.append("3"); // major version
        builder.append("5"); // minor version
        builder.append("I"); // licensee type
        builder.append("15"); // years of upgrade
        builder.append("12345678901");

        String first20 = builder.toString();

        String keyInfo = "Ae;\t&2}w#n`If5!nu6#{!hBA1IMDx%\n";
        byte[] keyBase = (first20 + licenseEmail + keyInfo).getBytes("UTF-8");
        byte[] signature = sign(keyBase);
        String signatureString = Base32.encode(signature);
        return first20 + signatureString;
    }

    public static void main(String[] args) throws Exception {
        System.out.print("Enter email: ");
        Scanner in = new Scanner(System.in);
        String email = in.nextLine();
        String license = generateLicenseKey(email);
        System.out.println("License: ");
        System.out.println("---BEGIN LICENSE KEY---");
        System.out.println(license);
        System.out.println("---END LICENSE KEY---");
    }
}
