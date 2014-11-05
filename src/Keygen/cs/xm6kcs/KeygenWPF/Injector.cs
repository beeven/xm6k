using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeygenWPF
{
    public class Injector
    {
        public const String jar1 = "net.xmind.verify_3.5.0.201410310637";
        public const String jar2 = "org.xmind.meggy_3.5.0.201410310637";
        public const String keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\xmind\shell\open\command";
        public const String keyPath2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\XMind Ltd\XMind";
        public bool inject(String path)
        {
            throw new NotImplementedException();
        }

        public String findPathFromRegistory()
        {
            throw new NotImplementedException();
        }
    }
}
