using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static string orderPath = Path.Combine(filePath, "Order.xml");
        private static string guestRequestPath = Path.Combine(filePath, "GuestRequest.xml");
        private static string hostPath = Path.Combine(filePath, "Host.xml");
        private static string hostingUnitPath = Path.Combine(filePath, "HostingUnit.xml");
        private static string configPath = Path.Combine(filePath, "Config.xml");

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
                CreateFile("HostingUnits", hostingUnitPath);
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

        public static void SaveHostingUnits()
        {
            hostingUnitRoot.Save(hostingUnitPath);
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
            path = Path.Combine(filePath, path + ".xml");
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        public static T LoadFromXMLSerialize<T>(string path)
        {
            path = Path.Combine(filePath, path + ".xml");
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
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
    }
}
