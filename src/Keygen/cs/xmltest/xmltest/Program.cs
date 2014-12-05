using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace xmltest
{
    class Program
    {
        static string doc1 = @"<root><elem1><elem11></elem11><optionalElem></optionalElem></elem1></root>";
        static string doc2 = @"<root><elem1><elem11></elem11></elem1></root>";
        static void Main(string[] args)
        {
            var parsedDoc1 = XDocument.Parse(doc1);
            var parsedDoc2 = XDocument.Parse(doc2); 
            try
            {
                var optionalElem = parsedDoc1.Root.Element("elem1").Element("optionalElem");
                Console.WriteLine(optionalElem.Value);
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                var optionalElem = parsedDoc2.Root.Element("elem1").Element("optionalElem");
                Console.WriteLine(optionalElem.Value);
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
