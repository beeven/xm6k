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


namespace KeygenWPF
{
    public class Log
    {
        public static String logUri = "http://localhost:3000/";

        public Task<String> LogViaHttp(String content)
        {
            return Task<String>.Factory.StartNew(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json; chartset=UTF-8");
                    try
                    {
                        //var stream = client.OpenWrite(new Uri(logUri + Uri.EscapeUriString(content)));
                        var data = GetMachineInformation();
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MachineInfo));
                        MemoryStream stream = new MemoryStream();
                        serializer.WriteObject(stream, data);
                        byte[] response = client.UploadData(new Uri(logUri + Uri.EscapeUriString(content)), stream.ToArray());
                        

                        DataContractJsonSerializer resSerializer = new DataContractJsonSerializer(typeof(ResponseLicense));
                        ResponseLicense license = (ResponseLicense)resSerializer.ReadObject(new MemoryStream(response));
                        return license.License;
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            if(((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                                return "已超过最大使用次数";
                        }
                            return "无法连接到服务器";
                        
                        
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
