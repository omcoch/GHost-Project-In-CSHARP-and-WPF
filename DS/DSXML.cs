using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DS
{
    public static class DSXML
    {
        private static string filePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, "DS", "XML");
        private static XElement orderRoot = null;
        private static XElement guestRequestRoot = null;
        private static XElement hostRoot = null;
        private static XElement hostingUnitRoot = null;
        private static XElement configRoot = null;
        private static XElement bankBranchRoot = null;
        public static string orderPath = Path.Combine(filePath, "Order.xml");
        public static string guestRequestPath = Path.Combine(filePath, "GuestRequest.xml");
        public static string hostPath = Path.Combine(filePath, "Host.xml");
        public static string hostingUnitPath = Path.Combine(filePath, "HostingUnit.xml");
        public static string configPath = Path.Combine(filePath, "Config.xml");
        public static string bankBranchPath = Path.Combine(filePath, "BankBranch.xml");

        static DSXML()
        {
            bool exists = Directory.Exists(filePath);
            if (!exists)
            {
                Directory.CreateDirectory(filePath);
            }

            if (!File.Exists(orderPath))
            {
                CreateFile("Orders", orderPath);
            }
            orderRoot = LoadData(orderPath);

            if (!File.Exists(hostPath))
            {
                CreateFile("Hosts", hostPath);
            }
            hostRoot = LoadData(hostPath);

            if (!File.Exists(hostingUnitPath))
            {
                SaveToXMLSerialize(new List<BE.HostingUnit>(), hostingUnitPath);
            }
            hostingUnitRoot = LoadData(hostingUnitPath);


            if (!File.Exists(guestRequestPath))
            {
                CreateFile("GuestRequests", guestRequestPath);
            }
            guestRequestRoot = LoadData(guestRequestPath);

            if (!File.Exists(configPath))
            {
                CreateFile("Config", configPath);
                configRoot = LoadData(configPath);
                // הכנסת ערכי ברירת מחדל למספרים רצים
                configRoot.Add(new XElement("guestRequestSerialKey", 10000000));
                configRoot.Add(new XElement("orderSerialKey", 10000000));
                configRoot.Add(new XElement("hostingUnitSerialKey", 10000000));
                configRoot.Add(new XElement("ErroeMessage", ""));
                SaveConfigs();
            }
            else
                configRoot = LoadData(configPath);

        }
        private static void CreateFile(string typename, string path)
        {
            XElement root = new XElement(typename);
            root.Save(path);
        }

        public static void SaveOrders()
        {
            orderRoot.Save(orderPath);
        }

        public static void SaveHosts()
        {
            hostRoot.Save(hostPath);
        }

        public static void SaveGuestRequests()
        {
            guestRequestRoot.Save(guestRequestPath);
        }

        public static void SaveConfigs()
        {
            configRoot.Save(configPath);
        }

        public static XElement Orders
        {
            get
            {
                orderRoot = LoadData(orderPath);
                return orderRoot;
            }
        }

        public static XElement Hosts
        {
            get
            {
                hostRoot = LoadData(hostPath);
                return hostRoot;
            }
        }

        public static XElement HostingUnits
        {
            get
            {
                hostingUnitRoot = LoadData(hostingUnitPath);
                return hostingUnitRoot;
            }
        }

        public static XElement GuestRequests
        {
            get
            {
                guestRequestRoot = LoadData(guestRequestPath);
                return guestRequestRoot;
            }
        }

        public static XElement Configs
        {
            get
            {
                configRoot = LoadData(configPath);
                return configRoot;
            }
        }

        public static XElement BankBranches
        {
            get
            {
                bankBranchRoot = LoadData(bankBranchPath);
                return bankBranchRoot;
            }
        }

        private static XElement LoadData(string path)
        {
            XElement root;
            try
            {
                root = XElement.Load(path);
            }
            catch
            {
                throw new FileNotFoundException("שגיאה בטעינת הקובץ בכתובת: "+path);
            }
            return root;
        }


        public static void SaveToXMLSerialize<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXMLSerialize<T>(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            FileStream file = new FileStream(path, FileMode.Open);
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        public static string ToXMLstring<T>(this T toSerialize)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static T ToObject<T>(this string toDeserialize)
        {
            using (StringReader textReader = new StringReader(toDeserialize))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static void DownloadBankXml()
        {
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, bankBranchPath);                
            }
            catch (Exception)
            {
                string xmlServerPath = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, bankBranchPath);
            }
            finally
            {
                wc.Dispose();
            }
            BE.Configuration.BanksXmlFinish = true;
        }
    }
}
