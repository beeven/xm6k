package xmindKeygen;

import javassist.ClassPool;
import javassist.CtClass;
import javassist.CtMethod;
import javassist.NotFoundException;

/**
 * Created by Beeven on 11/4/14.
 */
public class Injector {

    Generator generator;

    public Injector() {
        generator = new Generator();
    }


    public void injectPublicKey() throws NotFoundException {
        ClassPool cp = ClassPool.getDefault();
        CtClass cc = cp.getCtClass("net.xmind.verify.internal.LicenseVerifier$VerificationJob");
        CtMethod cm = cc.getDeclaredMethod("getCommonVerifierParameters");
    }
}
