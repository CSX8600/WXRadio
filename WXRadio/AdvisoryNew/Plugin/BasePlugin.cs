using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WXRadio.WeatherManager.Plugin
{
    public abstract class BasePlugin
    {
        public abstract string PluginID { get; }
        public abstract string FriendlyName { get; }
        public virtual void PreInitialize() { }
        public virtual void Initialize() { }
        public virtual void PostInitialize() { }
        public virtual void Shutdown() { }
        public abstract Control GetDashboardControl();

        private string GetConfigFileLocation()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\config\\" + FriendlyName + ".cfg"; ;
        }

        protected T GetConfigFile<T>()
        {
            string configFile = GetConfigFileLocation();

            Directory.CreateDirectory(configFile.Substring(0, configFile.LastIndexOf('\\')));
            
            if (!File.Exists(configFile))
            {
                using (File.Create(configFile)) { }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T config = Activator.CreateInstance<T>();
            bool serializeNew = false;
            using (Stream stream = File.OpenRead(configFile))
            {
                try
                {
                    config = (T)xmlSerializer.Deserialize(stream);
                }
                catch(InvalidOperationException ex)
                {
                    if (!(ex.InnerException is XmlException))
                    {
                        throw ex;
                    }

                    serializeNew = true;
                }
            }

            if (serializeNew)
            {
                using (Stream stream = File.OpenWrite(configFile))
                {
                    xmlSerializer.Serialize(stream, config);
                }
            }

            return config;
        }

        protected void SaveConfigFile<T>(T config)
        {
            string configFile = GetConfigFileLocation();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            File.Delete(configFile);
            using (FileStream stream = File.OpenWrite(configFile))
            {
                xmlSerializer.Serialize(stream, config);
            }
        }
    }
}
