using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace KeygenWPF.Log
{
    public class CertificateWebClient : WebClient
    {
        private readonly X509Certificate certificate;
        public CertificateWebClient(X509Certificate cert)
        {
            certificate = cert;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (Object obj, X509Certificate X509certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors) =>
                {
                    return true;
                };
            request.ClientCertificates.Add(certificate);
            return request;
        }
    }
}
