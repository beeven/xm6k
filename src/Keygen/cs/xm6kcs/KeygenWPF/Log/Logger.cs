using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Management;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;


namespace KeygenWPF.Log
{
    public class Logger
    {
        public static String logUri = "https://beevenubuntu.cloudapp.net/xm6k/";

        public Task<String> LogViaHttp(String mailAddress)
        {
            return Task<String>.Factory.StartNew(() =>
            {
                X509Certificate cert = new X509Certificate(Properties.Resources.clientCert);
                using (CertificateWebClient client = new CertificateWebClient(cert))
                {
                    
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json; chartset=UTF-8");
                    try
                    {
                        //var stream = client.OpenWrite(new Uri(logUri + Uri.EscapeUriString(content)));
                        var data = GetMachineInformation();
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MachineInfo));
                        MemoryStream stream = new MemoryStream();
                        serializer.WriteObject(stream, data);
                        byte[] response = client.UploadData(new Uri(logUri + Uri.EscapeUriString(mailAddress)), stream.ToArray());
                        

                        DataContractJsonSerializer resSerializer = new DataContractJsonSerializer(typeof(ResponseLicense));
                        ResponseLicense license = (ResponseLicense)resSerializer.ReadObject(new MemoryStream(response));
                        return license.License;
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                                throw new MaximumUsageException("已超出最大使用次数, 请发邮件到 35239520@qq.com 说明情况",ex);
                        }
                        throw new ConnectionExeption("无法连接到计算服务器, 请确保网络畅通",ex);
                    }
                    
                }
            });
        }

        public Task<String> LogEmail(String email)
        {
            
                return LogViaHttp(email);
            
        }

        private MachineInfo GetMachineInformation()
        {

            MachineInfo result = new MachineInfo();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");

            

            foreach(var obj in searcher.Get())
            {
                CPUInfo info = new CPUInfo();
                info.Name = (string)obj.GetPropertyValue("Name");
                info.ProcessorID = (string)obj.GetPropertyValue("ProcessorID");
                result.CPU.Add(info);
            }
            return result;
        }

        [DataContract]
        public class MachineInfo
        {
            [DataMember]
            public List<CPUInfo> CPU { get; set; }

            public MachineInfo()
            {
                CPU = new List<CPUInfo>();
            }
        }

        [DataContract]
        public class CPUInfo
        {
            [DataMember]
            public String Name{get;set;}
            [DataMember]
            public String ProcessorID{get;set;}
        }

        [DataContract]
        public class ResponseLicense
        {
            [DataMember]
            public String License { get; set; }
        }
        
    }
}
