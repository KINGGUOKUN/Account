using System.IO;
using System.Reflection;

namespace GuoKun.Configuration
{
    /// <summary>
    /// 工具类
    /// </summary>
    public sealed class Utility
    {
        /// <summary>
        /// 从程序集中读取嵌入式资源文件
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFromAssembly(Assembly assembly, string path)
        {
            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
