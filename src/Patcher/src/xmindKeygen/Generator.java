package xmindKeygen;

import org.bouncycastle.asn1.ASN1InputStream;
import org.bouncycastle.asn1.ASN1Sequence;
import org.bouncycastle.asn1.pkcs.PrivateKeyInfo;
import org.bouncycastle.asn1.pkcs.RSAPrivateKey;
import org.bouncycastle.asn1.pkcs.RSAPublicKey;
import org.bouncycastle.asn1.x509.SubjectPublicKeyInfo;
import org.bouncycastle.crypto.digests.MD5Digest;
import org.bouncycastle.crypto.engines.RSAEngine;
import org.bouncycastle.crypto.params.RSAPrivateCrtKeyParameters;
import org.bouncycastle.crypto.signers.PSSSigner;
import org.bouncycastle.openssl.PEMKeyPair;
import org.bouncycastle.openssl.PEMParser;
import org.bouncycastle.util.encoders.Base64;

import java.io.IOException;
import java.io.StringReader;
import java.security.KeyFactory;
import java.security.PublicKey;
import java.security.spec.RSAPublicKeySpec;
import java.security.spec.X509EncodedKeySpec;

/**
 * Created by Beeven on 11/4/14.
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


    public byte[] getPublicKeyInX509Encoded() throws Exception {
        PEMParser parser = new PEMParser(new StringReader(privateKeyString));
        PEMKeyPair keyPair = (PEMKeyPair)parser.readObject();
        SubjectPublicKeyInfo publicKeyInfo = keyPair.getPublicKeyInfo();
        ASN1Sequence sequence = ASN1Sequence.getInstance(publicKeyInfo);
        return Base64.encode(sequence.getEncoded());
    }

    public String getPublicKeyCode() throws Exception {
        byte[] key = getPublicKeyInX509Encoded();
        StringBuilder builder = new StringBuilder("{");
        for(byte b : key) {
            builder.append(Byte.toString(b)+",");
        }
        builder.setCharAt(builder.length()-1,'}');
        return builder.toString();
    }

    public static void main(String[] args) throws Exception{
        Generator g = new Generator();
        System.out.println(g.getPublicKeyCode());
    }
}
