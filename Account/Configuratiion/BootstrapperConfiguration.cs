
namespace GuoKun.Configuration
{
    /// <summary>
    /// 启动项配置节对应的配置类
    /// </summary>
    public sealed class BootstrapperConfiguration
    {
        /// <summary>
        /// 日志对象名称
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// 日志配置文件名称
        /// </summary>
        public string LogConfig { get; set; }

        /// <summary>
        /// 数据库配置文件名称
        /// </summary>
        public string DbConfigFile { get; set; }
    }
}
