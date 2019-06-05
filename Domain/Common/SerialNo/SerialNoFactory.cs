using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Extends;
using System.Threading;

namespace Domain.Common.SerialNo
{
    public static class SerialNoFactory
    {
        private static Dictionary<string, string> DicDynamicTimesOldCode = new Dictionary<string, string>();

        public static int SLBId = ConfigurationManager.AppSettings["SLBId"].ToInt(1);

        public static string Create(SerialNoType type, string format = "yyMMddHHmmssffff", string suffix = "")
        {
            var key = type.ToDescription();
            return key + SLBId + TimeStampFactory.Create(key, format) + suffix;
        }

        public static string CreateWithoutPrefix(SerialNoType type, string format = "yyMMddHHmmssffff")
        {
            var key = type.ToDescription();
            return SLBId + TimeStampFactory.Create(key, format);
        }

        public static string CreateIncrease(SerialNoType type, int max, int minLenth = 2)
        {
            var result = (max + 1).ToString();
            return type.ToDescription() + result.PadLeft(minLenth, '0');
        }

        public static string CreateConsumerCardNo(string prefix, int num, int no, string postfix)
        {
            var result = no.ToString();
            return prefix + result.PadLeft(num, '0') + postfix;
        }

        public static string CreateConsumerCardNo(int num, int index)
        {
            var result = TimeStampFactory.Create("", "yyMMddHHmmssff");
            string strIndex = index.ToString();
            if (result.Length + strIndex.Length == num)
            {
                return string.Concat(result, strIndex);
            }
            else if (result.Length + strIndex.Length < num)
            {
                strIndex = strIndex.PadLeft(num - result.Length, '0');
                return string.Concat(result, strIndex);
            }
            return string.Concat(result, strIndex).Substring(0, num);
        }

        public static string CreateQueueNo(int queueNo)
        {
            var result = queueNo.ToString();
            return result.PadLeft(6, '0');
        }

        public static string CreateWithInfix(SerialNoType type, string format = "yyMMddHHmmssffff", string infix = "")
        {
            var key = type.ToDescription();
            return key + infix + TimeStampFactory.Create(key, format);
        }

        public static string CreateRandom()
        {
            return Guid.NewGuid().ToString();
        }

        public static string CreateDynamicTimesCardNo(string prefix = "98",int length = 18)
        {
            string result = string.Empty;
            Random rd = new Random();
            while (true)
            {
                result = rd.Next(1000000, int.MaxValue).ToString();
                result = string.Concat(prefix, result);
                
                if (result.Length < length)
                {
                    string bs = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    if (bs.Length + result.Length < length)
                    {
                        result = string.Concat(result, bs).PadRight(18,'0');
                    }
                    else
                    {
                        int cw = length - result.Length;//差位
                        bs = bs.Substring(bs.Length - cw);
                        result = string.Concat(result, bs);
                    }                    
                }
                else if (result.Length > length)
                {
                    result = result.Substring(0, length);
                }

                if (!DicDynamicTimesOldCode.ContainsKey(result))
                {
                    DicDynamicTimesOldCode.Add(result,result);
                    break;
                }
                Thread.Sleep(3);
            }
            return result;
        }

        /// <summary>
        /// 创建下一单序列号(序列号固定5位数)：
        /// 订单类型+前缀+当前时间戳+（序列号+1）
        /// </summary>
        /// <param name="type">订单类型</param>
        /// <param name="infix">前缀</param>
        /// <param name="num">序列号</param>
        /// <param name="format">时间戳格式</param>
        /// <returns></returns>
        public static string CreateNextSeriesNum(SerialNoType type, string infix, int num = 0, string format = "yyMMdd")
        {
            var key = type.ToDescription();
            var date = DateTime.Now.Date.ToString(format);
            var prefix = key + infix + date;

            return prefix + (num + 1).ToString("00000");
        }
    }
}
