
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    public static class ModelMapUtil
    {
        /// <summary>
        /// 将源对象属性复制到目标对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object obj) where T : new()
        {
            var result = new T();
            var properties = obj.GetType().GetProperties();

            //存储源对象属性
            Dictionary<string, PropertyInfo> propertiesDic = new Dictionary<string, PropertyInfo>();
            foreach (var item in properties)
            {
                propertiesDic.Add(item.Name, item);
            }

            var resultProperties = result.GetType().GetProperties();

            foreach (var j in resultProperties)
            {
                try
                {
                    //自定义属性处理别名,目标中声明后，会用此别名，从源中找
                    DescriptionAttribute desc = (DescriptionAttribute)j.GetCustomAttributes(false).FirstOrDefault(f => f.GetType() == typeof(DescriptionAttribute));
                    if (desc != null)
                    {
                        var desName = desc.Description;
                        if (propertiesDic.ContainsKey(desName))
                        {
                            j.SetValue(result, propertiesDic[desName].GetValue(obj));
                            continue;
                        }
                    }
                    else
                    {
                        if (propertiesDic.ContainsKey(j.Name))
                        {
                            j.SetValue(result, propertiesDic[j.Name].GetValue(obj));
                        }
                    }
                }
                catch (System.Exception)
                {
                    try
                    {
                        j.SetValue(result, Activator.CreateInstance(j.PropertyType));
                    }
                    catch (System.Exception)
                    {
                        Console.WriteLine("转换前后类型不一致");
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 将源list复制到目标list
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IList<TOut> MapListTo<TIn, TOut>(this List<TIn> list) where TOut : new()
        {
            var result = new List<TOut>();
            foreach (var item in list)
            {
                try
                {
                    result.Add(item.MapTo<TOut>());
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }
    }
}