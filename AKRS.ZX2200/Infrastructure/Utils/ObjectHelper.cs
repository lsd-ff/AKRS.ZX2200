using static System.Windows.Forms.Control;

namespace AKRS.ZX2200.Infrastructure.Utils
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public static class ObjectHelper
    {
        /// <summary>
        /// 反射复制一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns>T</returns>
        public static T ShallowCopy<T>(this T original) where T : class
        {
            if (original == null)
            {
                return null;
            }

            // 创建一个新实例
            T copy = Activator.CreateInstance<T>();

            // 获取原始对象的所有属性
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // 如果属性是一个引用类型或是List集合，进行浅拷贝
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string)
                    || property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var originalValue = property.GetValue(original);

                    if (originalValue != null)
                    {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            // 如果属性是List集合，复制集合元素
                            var originalList = (System.Collections.IList)originalValue;
                            var copyList = (System.Collections.IList)Activator.CreateInstance(property.PropertyType);

                            foreach (var item in originalList)
                            {
                                copyList.Add(item);
                            }
                            property.SetValue(copy, copyList);

                            // 如果属性是List集合，复制集合元素
                            // var originalList = (System.Collections.IList)originalValue;
                            // var copyList = originalList.Cast<object>().ToList();

                            // property.SetValue(copy, copyList);
                        }
                        else
                        {
                            // 其他引用类型的属性，进行递归浅拷贝
                            var clonedObject = ShallowCopy(originalValue);
                            property.SetValue(copy, clonedObject);
                        }
                    }
                }
                else
                {
                    // 该属性是一个值类型，直接复制
                    var originalValue = property.GetValue(original);
                    property.SetValue(copy, originalValue);
                }
            }

            return copy;
        }

        /// <summary>
        /// 反射克隆一个对象
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut TransReflection<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            var tInType = tIn.GetType();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var itemIn = tInType.GetProperty(itemOut.Name); ;
                if (itemIn != null)
                {
                    itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                }
            }
            return tOut;
        }

        public static T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        /// <summary>
        /// 交换集合中的两个位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public static List<T> Swap<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        /// <summary>
        /// 比较--两个类型一样的实体类对象的值
        /// </summary>
        /// <param name="oneT"></param>
        /// <param name="twoT"></param>
        /// <returns></returns>
        public static bool CompareType<T>(T oneT, T twoT)
        {
            bool result = true;//两个类型作比较时使用,如果有不一样的就false
            Type typeOne = oneT.GetType();
            Type typeTwo = twoT.GetType();
            //如果两个T类型不一样  就不作比较
            if (!typeOne.Equals(typeTwo)) { return false; }
            PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
            PropertyInfo[] pisTwo = typeTwo.GetProperties();
            //如果长度为0返回false
            if (pisOne.Length <= 0 || pisTwo.Length <= 0)
            {
                return false;
            }
            //如果长度不一样，返回false
            if (!(pisOne.Length.Equals(pisTwo.Length))) { return false; }
            //遍历两个T类型，遍历属性，并作比较
            for (int i = 0; i < pisOne.Length; i++)
            {
                //获取属性名
                string oneName = pisOne[i].Name;
                string twoName = pisTwo[i].Name;
                //获取属性的值
                object oneValue = pisOne[i].GetValue(oneT, null);
                object twoValue = pisTwo[i].GetValue(twoT, null);
                //比较,只比较值类型
                if ((pisOne[i].PropertyType.IsValueType || pisOne[i].PropertyType.Name.StartsWith("String")) && (pisTwo[i].PropertyType.IsValueType || pisTwo[i].PropertyType.Name.StartsWith("String")))
                {
                    if (oneName.Equals(twoName))
                    {
                        if (oneValue == null)
                        {
                            if (twoValue != null)
                            {
                                result = false;
                                break; //如果有不一样的就退出循环
                            }
                        }
                        else if (oneValue != null)
                        {
                            if (twoValue != null)
                            {
                                if (!oneValue.Equals(twoValue))
                                {
                                    result = false;
                                    break; //如果有不一样的就退出循环
                                }
                            }
                            else if (twoValue == null)
                            {
                                result = false;
                                break; //如果有不一样的就退出循环
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    //如果对象中的属性是实体类对象，递归遍历比较
                    bool b = CompareType(oneValue, twoValue);
                    if (!b) { result = b; break; }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据名称保存控件参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controls"></param>
        /// <param name="ParaClass"></param>
        public static bool SavaParaByName<T>(ControlCollection controls, T ParaClass) where T : new()
        {
            try
            {
                if (controls == null || ParaClass == null)
                {
                    return false;
                }
                //根据控件名称自动匹配参数
                foreach (Control control in controls)
                {
                    if (control is ToggleSwitch)
                    {
                        ToggleSwitch toggleSwitch = (ToggleSwitch)control;
                        FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        bool Success = false;
                        foreach (FieldInfo FieldInfo in fields)
                        {
                            string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                            if (toggleSwitch.Name == "Sw" + FieldInfoName)
                            {
                                Success = true;
                                FieldInfo.SetValue(ParaClass, toggleSwitch.IsOn);
                            }
                        }

                        if (!Success)
                        {
                            AKRSXtraMessageBox.Show(toggleSwitch.Name + "没有找到相应参数");
                        }
                    }
                    if (control is SpinEdit)
                    {
                        SpinEdit spinEdit = (SpinEdit)control;
                        FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        bool Success = false;
                        foreach (FieldInfo FieldInfo in fields)
                        {
                            string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                            if (spinEdit.Name == "Sp" + FieldInfoName)
                            {
                                Success = true;
                                int Value = (int)spinEdit.Value;
                                FieldInfo.SetValue(ParaClass, Value);
                            }
                        }
                        if (!Success)
                        {
                            AKRSXtraMessageBox.Show(spinEdit.Name + "没有找到相应参数");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
                return false;
                throw;
            }
        }

        /// <summary>
        /// 根据名称自动匹配参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool LoadParaByName<T>(ControlCollection controls, T ParaClass)
        {
            try
            {
                //根据控件名称自动匹配参数
                foreach (Control control in controls)
                {
                    if (control is ToggleSwitch)
                    {
                        ToggleSwitch toggleSwitch = (ToggleSwitch)control;
                        FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        bool Success = false;
                        foreach (FieldInfo FieldInfo in fields)
                        {
                            string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                            if (toggleSwitch.Name == "Sw" + FieldInfoName)
                            {
                                Success = true;
                                toggleSwitch.IsOn = (bool)FieldInfo.GetValue(ParaClass);
                            }
                        }

                        if (!Success)
                        {
                            AKRSXtraMessageBox.Show(toggleSwitch.Name + "没有找到相应参数");
                        }
                    }
                    if (control is SpinEdit)
                    {
                        SpinEdit spinEdit = (SpinEdit)control;
                        FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        bool Success = false;
                        foreach (FieldInfo FieldInfo in fields)
                        {
                            string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                            if (spinEdit.Name == "Sp" + FieldInfoName)
                            {
                                if (FieldInfo.FieldType.Equals(typeof(int)))
                                {
                                    int Value = (int)FieldInfo.GetValue(ParaClass);
                                    spinEdit.Value = (decimal)Value;
                                }
                                if (FieldInfo.FieldType.Equals(typeof(float)))
                                {
                                    float Value = (float)FieldInfo.GetValue(ParaClass);
                                    spinEdit.Value = (decimal)Value;
                                }
                                if (FieldInfo.FieldType.Equals(typeof(double)))
                                {
                                    double Value = (double)FieldInfo.GetValue(ParaClass);
                                    spinEdit.Value = (decimal)Value;
                                }
                                Success = true;
                            }
                        }
                        if (!Success)
                        {
                            AKRSXtraMessageBox.Show(spinEdit.Name + "没有找到相应参数");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 不知道为什么会出现这样的后缀
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string Removek_BackingField(string Name)
        {
            if (Name.Contains("k__BackingField"))
            {
                Name = Name.Remove(Name.Length - 16).Remove(0, 1);
            }
            return Name;
        }

        /// <summary>
        /// 轴可以动的条件,不应该放在这里，应该要有一个公共的运动条件判断类
        /// </summary>
        /// <returns></returns>
        public static bool AxisMoveEnable()
        {
            return false;

        }

       


        /// <summary>
        /// 根据控件名称自动填充控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="ParaClass"></param>
        /// <param name="Ignores"></param>
        /// <returns></returns>
        public static bool LoadParaByName<T>(Control control, T ParaClass, List<Control> Ignores = null)
        {
            try
            {
                if (control == null || ParaClass == null)
                {
                    return false;
                }
                foreach (Control item in control.Controls)
                {
                    if (item is ToggleSwitch || item is SpinEdit || item is TextEdit)
                    {

                        if (item is ToggleSwitch)
                        {

                            bool Ignore = false;
                            ToggleSwitch toggleSwitch = (ToggleSwitch)item;
                            if (Ignores != null)
                            {
                                foreach (Control IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == toggleSwitch.Name)
                                    {
                                        // MessageBox.Show(toggleSwitch.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                bool Success = false;
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                foreach (FieldInfo FieldInfo in fields)
                                {

                                    bool Skip = false;
                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                                    if (toggleSwitch.Name == "Sw" + FieldInfoName)
                                    {
                                        bool Value = (bool)FieldInfo.GetValue(ParaClass);
                                        toggleSwitch.IsOn = Value;
                                        Success = true;


                                    }
                                }
                                if (!Success)
                                {
                                    AKRSXtraMessageBox.Show(toggleSwitch.Name + "没有找到相应参数");
                                }
                            }
                        }


                        else if (item is SpinEdit)
                        {
                            bool Ignore = false;
                            SpinEdit spinEdit = (SpinEdit)item;
                            if (Ignores != null)
                            {
                                foreach (var IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == spinEdit.Name)
                                    {
                                        // MessageBox.Show(spinEdit.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                bool Success = false;
                                foreach (FieldInfo FieldInfo in fields)
                                {
                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);

                                    if (FieldInfo.FieldType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D")
                                    {
                                        if (spinEdit.Name == "Sp" + FieldInfoName + "X")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                spinEdit.Value = 0;
                                            }
                                            else
                                            {
                                                spinEdit.Value = (decimal)aKRSPoint3D.X;
                                            }
                                            Success = true;
                                        }
                                        else if (spinEdit.Name == "Sp" + FieldInfoName + "Y")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                spinEdit.Value = 0;
                                            }
                                            else
                                            {
                                                spinEdit.Value = (decimal)aKRSPoint3D.Y;
                                            }
                                            Success = true;
                                        }
                                        else if (spinEdit.Name == "Sp" + FieldInfoName + "Z")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                spinEdit.Value = 0;
                                            }
                                            else
                                            {
                                                spinEdit.Value = (decimal)aKRSPoint3D.Z;
                                            }
                                           

                                            Success = true;
                                        }
                                    }
                                    else
                                    {
                                        if (spinEdit.Name == "Sp" + FieldInfoName)
                                        {
                                            if (FieldInfo.FieldType.Equals(typeof(int)))
                                            {
                                                int Value = (int)FieldInfo.GetValue(ParaClass);
                                                spinEdit.Value = (decimal)Value;
                                            }
                                            if (FieldInfo.FieldType.Equals(typeof(float)))
                                            {
                                                float Value = (float)FieldInfo.GetValue(ParaClass);
                                                spinEdit.Value = (decimal)Value;
                                            }
                                            if (FieldInfo.FieldType.Equals(typeof(double)))
                                            {
                                                double Value = (double)FieldInfo.GetValue(ParaClass);
                                                spinEdit.Value = (decimal)Value;
                                            }
                                            Success = true;
                                        }



                                    }
                                }
                                if (!Success)
                                    {
                                    AKRSXtraMessageBox.Show(spinEdit.Name + "没有找到相应参数");
                                    }
                              

                            }
                        }

                        else if (item is TextEdit)
                        {
                            bool Ignore = false;
                            TextEdit textEdit = (TextEdit)item;
                            if (Ignores != null)
                            {
                                foreach (var IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == textEdit.Name)
                                    {
                                        // MessageBox.Show(textEdit.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                bool Success = false;
                                foreach (FieldInfo FieldInfo in fields)
                                {

                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                                    if (textEdit.Name == "Tx" + FieldInfoName)
                                    {
                                        string Value = (String)FieldInfo.GetValue(ParaClass);
                                        textEdit.Text = Value;

                                        Success = true;
                                    }

                                }
                                if (!Success)
                                {
                                    AKRSXtraMessageBox.Show(textEdit.Name + "没有找到相应参数");
                                }
                            }

                        }
                    }
                    //递归循环所有控件
                    LoadParaByName<T>(item, ParaClass, Ignores);
                }
                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
                throw;
            }

        }


        /// <summary>
        /// 根据控件名称自动保存参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="ParaClass"></param>
        /// <param name="Ignores"></param>
        /// <returns></returns>
        public static bool SaveParaByName<T>(Control control, T ParaClass, List<Control> Ignores = null)
        {
            try
            {
                if (control == null || ParaClass == null)
                {
                    return false;
                }
                foreach (Control item in control.Controls)
                {
                    if (item is ToggleSwitch || item is SpinEdit||item is TextEdit)
                    {

                        if (item is ToggleSwitch)
                        {
                            bool Ignore = false;
                            ToggleSwitch toggleSwitch = (ToggleSwitch)item;
                            if (Ignores != null)
                            {
                                foreach (Control IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == toggleSwitch.Name)
                                    {
                                        //  MessageBox.Show(toggleSwitch.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                bool Success = false;
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                foreach (FieldInfo FieldInfo in fields)
                                {   
                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                                    if (toggleSwitch.Name == "Sw" + FieldInfoName)
                                    {
                                        FieldInfo.SetValue(ParaClass, toggleSwitch.IsOn);
                                        Success = true;
                                    }
                                }
                                if (!Success)
                                {
                                    AKRSXtraMessageBox.Show(toggleSwitch.Name + "没有找到相应参数");
                                }
                            }
                        }


                        else if (item is SpinEdit)
                        {
                            bool Ignore = false;
                            SpinEdit spinEdit = (SpinEdit)item;
                            if (Ignores != null)
                            {
                                foreach (var IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == spinEdit.Name)
                                    {
                                        // MessageBox.Show(spinEdit.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                bool Success = false;
                                foreach (FieldInfo FieldInfo in fields)
                                {
                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);

                                    if (FieldInfo.FieldType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D")
                                    {
                                        if (spinEdit.Name == "Sp" + FieldInfoName + "X")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                aKRSPoint3D = new AKRSPoint3D();
                                                aKRSPoint3D.X = (double)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, aKRSPoint3D);
                                            }
                                            else
                                            {
                                                aKRSPoint3D.X = (double)spinEdit.Value;
                                            }
                                            Success = true;
                                        }
                                        else if (spinEdit.Name == "Sp" + FieldInfoName + "Y")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                aKRSPoint3D = new AKRSPoint3D();
                                                aKRSPoint3D.Y = (double)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, aKRSPoint3D);
                                            }
                                            else
                                            {
                                                aKRSPoint3D.Y = (double)spinEdit.Value;
                                            }
                                            Success = true;
                                        }
                                        else if (spinEdit.Name == "Sp" + FieldInfoName + "Z")
                                        {
                                            AKRSPoint3D aKRSPoint3D = (AKRSPoint3D)FieldInfo.GetValue(ParaClass);
                                            if (aKRSPoint3D == null)
                                            {
                                                aKRSPoint3D = new AKRSPoint3D();
                                                aKRSPoint3D.Z = (double)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, aKRSPoint3D);
                                            }
                                            else
                                            {
                                                aKRSPoint3D.Z = (double)spinEdit.Value;
                                            }
                                            Success = true;
                                        }
                                    }
                                    else
                                    {
                                        if (spinEdit.Name == "Sp" + FieldInfoName)
                                        {
                                            if (FieldInfo.FieldType.Equals(typeof(int)))
                                            {
                                                int Value = (int)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, Value);
                                            }
                                            if (FieldInfo.FieldType.Equals(typeof(float)))
                                            {
                                                float Value = (float)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, Value);
                                            }
                                            if (FieldInfo.FieldType.Equals(typeof(double)))
                                            {
                                                double Value = (double)spinEdit.Value;
                                                FieldInfo.SetValue(ParaClass, Value);
                                            }
                                            Success = true;
                                        }
                                    }
                                    
                                }
                                if (!Success)
                                {
                                    AKRSXtraMessageBox.Show(spinEdit.Name + "没有找到相应参数");
                                }
                            }

                        }


                        else if (item is TextEdit)
                        {
                            bool Ignore = false;
                            TextEdit textEdit = (TextEdit)item;
                            if (Ignores != null)
                            {
                                foreach (var IgnoreItem in Ignores)
                                {
                                    if (IgnoreItem.Name == textEdit.Name)
                                    {
                                        // MessageBox.Show(textEdit.Name);
                                        Ignore = true;
                                    }
                                }
                            }
                            if (!Ignore)
                            {
                                FieldInfo[] fields = ParaClass.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                                bool Success = false;
                                foreach (FieldInfo FieldInfo in fields)
                                {
                                    string FieldInfoName = Removek_BackingField(FieldInfo.Name);
                                    if (textEdit.Name == "Tx" + FieldInfoName)
                                    {
                                        string Value = textEdit.Text.ToString();
                                        FieldInfo.SetValue(ParaClass, Value);
                                        Success = true;
                                    }
                                }
                                if (!Success)
                                {
                                    AKRSXtraMessageBox.Show(textEdit.Name + "没有找到相应参数");
                                }
                            }

                        }
                    }

                    // 递归循环所有控件
                    SaveParaByName<T>(item, ParaClass, Ignores);
                }
                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 获取名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        static public string GetObjectName<T>(Expression<Func<T>> memberExpression)

        {

            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;

            return expressionBody.Member.Name;

        }

        /// <summary>
        /// 自动执行当前动作的保存方法
        /// </summary>
        /// <param name="parentControl"></param>
        public static void Save(Control parentControl)
        {
            if (parentControl == null)
            {
                return;
            }
            foreach (Control control in parentControl.Controls)
            {
                if (control.Name.StartsWith("Uc"))
                {
                    foreach (MethodInfo methodInfo in control.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                    {
                        if (control.Name.StartsWith("Uc") && methodInfo.Name == "Save")
                        {
                            methodInfo.Invoke(control, new object[] { });
                            return;
                        }
                    }

                }
                Save(control);
            }
        }

        /// <summary>
        /// 保存所有控件的数据
        /// </summary>
        /// <param name="parentControl"></param>
        public static void SaveAll(Control parentControl)
        {
            if (parentControl == null)
            {
                return;
            }


            foreach (FieldInfo fieldInfo in parentControl.GetType().GetFields())
            {
                if (fieldInfo.Name == "UcDispenseDemarcate")
                {
                    AKRSXtraMessageBox.Show("true");

                }
            }
            foreach (Control control in parentControl.Controls)
            {
                if (control.Name.StartsWith("Uc"))
                {
                    foreach (MethodInfo methodInfo in control.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                    {
                        if (control.Name.StartsWith("Uc") && methodInfo.Name == "Save")
                        {
                            methodInfo.Invoke(control, new object[] { });
                            return;
                        }
                    }

                }
                //SaveAll(control);
            }
        }

        /// <summary>
        /// 自动执行当前动作的取消方法
        /// </summary>
        /// <param name="parentControl"></param>
        public static void Cancel(Control parentControl)
        {
            if (parentControl == null)
            {
                return;
            }
            foreach (Control control in parentControl.Controls)
            {
                if (control.Name.StartsWith("Uc"))
                {
                    foreach (MethodInfo methodInfo in control.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
                    {
                        if (control.Name.StartsWith("Uc") && methodInfo.Name == "Save")
                        {
                            methodInfo.Invoke(control, new object[] { });
                            return;
                        }
                    }

                }
                Cancel(control);
            }
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static Dictionary<T, string> GetEnumDescriptions<T>() where T : Enum
        {
            var enumType = typeof(T);
            return Enum.GetValues(enumType)
                .Cast<T>()
                .ToDictionary(
                    e => e,
                    e => GetDescription(e)
                );
        }

        // 或者获取描述集合（不包含枚举值）
        public static List<string> GetEnumDescriptionList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => GetDescription(e))
                .ToList();
        }
    }
}
