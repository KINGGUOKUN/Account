using Microsoft.Practices.Unity.Configuration;
using System.IO;
using System.Xml;

namespace GuoKun.Configuration
{
    /// <summary>
    /// 自定义UnityContainer配置节
    /// </summary>
    public class CustomUnityConfigurationSection : UnityConfigurationSection
    {
        public CustomUnityConfigurationSection(string unity)
        {
            base.DeserializeSection(XmlReader.Create(new StringReader(unity)));
        }
    }
}
