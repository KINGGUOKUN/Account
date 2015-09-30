using System.Configuration;
using System.Xml;

namespace GuoKun.Configuration
{
    /// <summary>
    /// 启动配置节处理类
    /// </summary>
    public sealed class BootstrapperConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            BootstrapperConfiguration bootstapperConfiguration = new BootstrapperConfiguration();
            foreach(XmlElement element in section.ChildNodes)
            {
                switch(element.Name)
                {
                    case "log":
                        bootstapperConfiguration.LogName = element.GetAttribute("logName");
                        bootstapperConfiguration.LogConfig = element.GetAttribute("logConfig");
                        break;
                    case "dbConfigFile":
                        bootstapperConfiguration.DbConfigFile = element.GetAttribute("fileName");
                        break;
                }
            }

            return bootstapperConfiguration;
        }
    }
}
