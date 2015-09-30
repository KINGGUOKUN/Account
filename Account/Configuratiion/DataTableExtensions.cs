using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GuoKun.Configuration
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// DataTable扩展方法：将DataTable类型转化为指定类型的实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dateTimeToString">是否需要将日期转换为字符串，默认为转换,值为true</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });//获取类型的属性集合

                SetList<T>(list, infos, dt);
            }

            return list;
        }

        private static void SetList<T>(List<T> list, List<PropertyInfo> infos, DataTable dt) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)//判断属性在不为空
                    {
                        try
                        {
                            p.SetValue(model, dr[p.Name], null);//设置
                        }
                        catch { }
                    }
                });
                list.Add(model);
            }
        }
    }
}
