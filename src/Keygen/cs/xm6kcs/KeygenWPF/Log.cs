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
        public static String logUri = "http://log.beeven.me/xm6k/";

        public Task LogViaHttp(String content)
        {
            return Task.Factory.StartNew(() =>
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json; chartset=UTF-8");
                    try
                    {
                        var stream = client.OpenWrite(new Uri(logUri + Uri.EscapeUriString(content)));
                        var data = GetMachineInformation();
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MachineInfo));
                        serializer.WriteObject(stream, data);
                        stream.Close();
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }
            });
        }

        public void LogEmail(String email)
        {
            try
            {
                LogViaHttp(email);
            }
            catch(WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        
    }
}
