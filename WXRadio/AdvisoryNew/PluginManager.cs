using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using WXRadio.WeatherManager.Plugin;

namespace WXRadio.WeatherManager
{
    public class PluginManager
    {
        public event EventHandler<BasePlugin> PluginInitialized;

        private HashSet<BasePlugin> _loadedPlugins = new HashSet<BasePlugin>();
        private Dictionary<string, BasePlugin> _pluginsByName = new Dictionary<string, BasePlugin>();
        private bool _hasInitialized;

        private static PluginManager _pluginManager;
        public static PluginManager INSTANCE
        {
            get
            {
                if (_pluginManager == null)
                {
                    _pluginManager = new PluginManager();
                }

                return _pluginManager;
            }
        }

        private PluginManager()
        {
            IEnumerable<string> files = Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\plugins").GetFiles().Select(fi => fi.FullName).Where(f => f.EndsWith(".dll"));

            foreach(string file in files)
            {
                try
                {
                    Assembly.LoadFrom(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Could not load assembly from location {0}: {1}", file, ex.ToString()));
                }
            }
        }

        public void Initialize()
        {
            if (_hasInitialized)
            {
                throw new InvalidOperationException("The Plugin Manager has already been initialized");
            }

            string configFileLocation = AppDomain.CurrentDomain.BaseDirectory + "config\\Weather Manager.cfg";

            WeatherManagerConfiguration weatherManagerConfiguration = new WeatherManagerConfiguration();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(WeatherManagerConfiguration));
            if (!File.Exists(configFileLocation))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "config");
                using (FileStream stream = new FileStream(configFileLocation, FileMode.Create))
                {
                    xmlSerializer.Serialize(stream, weatherManagerConfiguration);
                }
            }

            using (FileStream stream = new FileStream(configFileLocation, FileMode.Open))
            {
                weatherManagerConfiguration = (WeatherManagerConfiguration)xmlSerializer.Deserialize(stream);
            }

            WeatherManagerConfiguration.INSTANCE = weatherManagerConfiguration;

            Type basePluginType = typeof(BasePlugin);
            foreach(Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t != basePluginType && basePluginType.IsAssignableFrom(t)))
            {
                BasePlugin basePlugin = (BasePlugin)Activator.CreateInstance(type);
                basePlugin.PreInitialize();
                _loadedPlugins.Add(basePlugin);
                _pluginsByName.Add(basePlugin.PluginID, basePlugin);
            }

            foreach(BasePlugin basePlugin in _loadedPlugins)
            {
                basePlugin.Initialize();
                PluginInitialized?.Invoke(this, basePlugin);
            }

            foreach(BasePlugin basePlugin in _loadedPlugins)
            {
                basePlugin.PostInitialize();
            }

            _hasInitialized = true;
        }

        public BasePlugin GetPluginByID(string id)
        {
            if (_pluginsByName.ContainsKey(id))
            {
                return _pluginsByName[id];
            }

            return null;
        }

        public IReadOnlyCollection<BasePlugin> GetPlugins()
        {
            return _loadedPlugins;
        }

        public void Shutdown()
        {
            if (!_hasInitialized)
            {
                return;
            }

            foreach(BasePlugin basePlugin in _loadedPlugins)
            {
                basePlugin.Shutdown();
            }
        }
    }
}
