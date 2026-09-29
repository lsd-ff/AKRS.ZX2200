using System;
using System.Reflection;

namespace Akrs.ChangeJson
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns>创建完成的对象</returns>
        /// <exception cref="NullReferenceException">full name 或者assemblyName 无效</exception>
        public static T CreateInstance<T>(string fullName, string assemblyName)
        {
            // 命名空间.类型名,程序集
            string path = fullName + "," + assemblyName;

            // 加载类型
            Type o = Type.GetType(path);

            if (o == null)
            {
                throw new NullReferenceException($"根据 path：{path} 未能获取到type类型 ");
            }

            // 根据类型创建实例
            object obj = Activator.CreateInstance(o, true);

            // 类型转换并返回
            return (T)obj;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns>创建完成的对象</returns>
        /// <exception cref="InvalidOperationException">反射创建对象出现错误</exception>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            try
            {
                // 命名空间.类型名
                string fullName = nameSpace + "." + className;

                // 加载程序集，创建程序集里面的 命名空间.类型名 实例
                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);

                // 类型转换并返回
                return (T)ect;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"反射创建对象异常 assemblyName:{assemblyName},nameSpace:{nameSpace},className:{className} ", ex);
            }
        }

        /// <summary>
        /// 给目标对象与源对象同名的属性赋值
        /// </summary>
        /// <param name="sourceObject">源对象</param>
        /// <param name="destObject">目标对象</param>
        public static void AssignSameProperty(object sourceObject, object destObject)
        {
            PropertyInfo[] sourceInfos = sourceObject.GetType().GetProperties();
            PropertyInfo[] destInfos = destObject.GetType().GetProperties();
            foreach (PropertyInfo destInfo in destInfos)
            {
                foreach (PropertyInfo sourceInfo in sourceInfos)
                {
                    if (destInfo.Name == sourceInfo.Name && destInfo.PropertyType.ToString() == sourceInfo.PropertyType.ToString())
                    {
                        if (destInfo.PropertyType is ValueType)
                        {
                            destInfo.SetValue(destObject, sourceInfo.GetValue(sourceObject, null), null);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
    }
}