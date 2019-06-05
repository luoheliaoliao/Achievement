using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure.Extends
{
    /// <summary>
    /// 枚举说明属性
    /// </summary>
    public class EnumDescriptionAttribute : Attribute
    {
        public string Text { get; private set; }

        public EnumDescriptionAttribute(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// Enum对象扩展
    /// </summary>
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((EnumDescriptionAttribute)attrs[0]).Text;
            }
            return enumeration.ToString();
        }

        public static List<EnumData> ToEnumData(Type Enum1)
        {
            if (!Enum1.IsEnum)
            {
                throw new Exception("只有枚举才能使用这个方法！");
            }
            var data = new List<EnumData>();
            foreach (var enumeration in Enum.GetValues(Enum1))
            {

                var type = enumeration.GetType();
                MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

                if (memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                    if (attrs.Length > 0)
                    {
                        data.Add(new EnumData() { Key = (int)enumeration, Value = ((EnumDescriptionAttribute)attrs[0]).Text });
                        continue;
                    }
                }
                data.Add(new EnumData() { Key = (int)enumeration, Value = enumeration.ToString() });
            }
            return data;
        }

        public static int? GetValueByDesc<T>(this string desc)
        {
            if (!(typeof(T).BaseType == typeof(Enum)))
                return null;

            foreach (var key in Enum.GetNames(typeof(T)))
            {
                var type = typeof(T);
                MemberInfo[] memInfo = type.GetMember(key);

                if (memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                    if (attrs.Length > 0 && ((EnumDescriptionAttribute)attrs[0]).Text == desc)
                    {
                        return (int)Enum.Parse(typeof(T), key);
                    }
                }

            }
            return null;

        }

        ///// <summary>
        ///// 字符串枚举名称转枚举
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="name"></param>
        ///// <param name="defaultEnum">转换失败默认</param>
        ///// <returns></returns>
        //public static T StringToEnum<T>(this T enumModel, string name, T defaultEnum)
        //{
        //    object o = null;
        //    var MyEnum = enumModel.GetType();
        //    var sn = Enum.GetNames(MyEnum);
        //    name = name.ToLower();
        //    foreach (var statusName in sn)
        //    {
        //        if (statusName.ToLower() == name)
        //        {
        //            o = Enum.Parse(MyEnum, statusName);
        //            break;
        //        }
        //    }
        //    if (o == null)
        //        return defaultEnum;
        //    return (T)o;
        //}
        ///// <summary>
        ///// 字符串枚举名称转枚举
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="MyEnum"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static T StringToEnum<T>(this T enumModel,string name)
        //{
        //    Type MyEnum = enumModel.GetType();
        //    object o = null;
        //    var sn = Enum.GetNames(MyEnum);
        //    name = name.ToLower();
        //    foreach (var statusName in sn)
        //    {
        //        if (statusName.ToLower() == name)
        //        {
        //            o = Enum.Parse(MyEnum, statusName);
        //            break;
        //        }
        //    }
        //    if (o == null)
        //        o = default(T);// o = Enum.Parse(MyEnum, "None", true);
        //    return (T)o;
        //}
        //public static T StringToEnum<T>(string name)
        //{
        //    Type MyEnum = typeof(T);
        //    object o = null;
        //    var sn = Enum.GetNames(MyEnum);
        //    name = name.ToLower();
        //    foreach (var statusName in sn)
        //    {
        //        if (statusName.ToLower() == name)
        //        {
        //            o = Enum.Parse(MyEnum, statusName);
        //            break;
        //        }
        //    }
        //    if (o == null)
        //        o = default(T);// o = Enum.Parse(MyEnum, "None", true);
        //    return (T)o;
        //}
        //public static T StringToEnum<T>(string name,T defaultEnum)
        //{
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        return defaultEnum;
        //    }
        //    Type MyEnum = typeof(T);
        //    object o = null;
        //    var sn = Enum.GetNames(MyEnum);
        //    name = name.ToLower();
        //    foreach (var statusName in sn)
        //    {
        //        if (statusName.ToLower() == name)
        //        {
        //            o = Enum.Parse(MyEnum, statusName);
        //            break;
        //        }
        //    }
        //    if (o == null)
        //        o = defaultEnum;// o = Enum.Parse(MyEnum, "None", true);
        //    return (T)o;
        //}
    }

    public class EnumData
    {
        public int Key { get; set; }

        public string Value { get; set; }
    }
}
