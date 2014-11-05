package xmindKeygen;

import javassist.*;

import java.io.IOException;

/**
 * Created by Beeven on 11/4/14.
 */
public class Injector {

    Generator generator;

    public Injector() {
        generator = new Generator();
    }

    public void injectAll() throws Exception {
        injectPublicKey();
        injectStartup();
        injectMeggy();
    }


    public void injectPublicKey() throws Exception {
        ClassPool cp = ClassPool.getDefault();
        CtClass cc = cp.getCtClass("net.xmind.verify.internal.LicenseVerifier$VerificationJob");
        CtMethod cm = cc.getDeclaredMethod("getCommonVerifierParameters");
        String publicKeyCode = generator.getPublicKeyCode();
        cm.setBody("{ return new Object[] {\"Ae;\\t&2}w#n`If5!nu6#{!hBA1IMDx%\\n\", new byte[] " + publicKeyCode + "}; }");
        cc.writeFile(".");
    }

    public void injectStartup() throws Exception {
        ClassPool cp = ClassPool.getDefault();
        CtClass cc = cp.getCtClass("net.xmind.verify.ui.internal.StartupVerifier");
        CtMethod method = cc.getDeclaredMethod("verify");
        method.setBody("{}");
        cc.writeFile(".");
    }

    public void injectMeggy() throws Exception {
        ClassPool cp = ClassPool.getDefault();
        CtClass cc = cp.getCtClass("org.xmind.ui.internal.meggy.PluginGuardian");
        CtMethod method = cc.getDeclaredMethod("checkPluginsSignatureAgainst");
        method.setBody("{ return true;}");
        cc.writeFile(".");
    }

}
