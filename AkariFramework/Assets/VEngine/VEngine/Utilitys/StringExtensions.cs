using System;

namespace VEngine
{
    /// <summary>
    ///     字符串扩张类，封装了常用的字符串转换函数
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static int[] IntArrayValue(this string s, string split = ",")
        {
            var items = s.Split(new[]
            {
                split
            }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length > 0)
            {
                return Array.ConvertAll(items, int.Parse);
            }

            return new int[0];
        }

        /// <summary>
        ///     将输入的字符串 s 转换成 ulong 数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ulong ULongValue(this string s)
        {
            ulong.TryParse(s, out var value);
            return value;
        }

        /// <summary>
        ///     将输入的字符串 s 转换成 int 数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int IntValue(this string s)
        {
            int.TryParse(s, out var value);
            return value;
        }

        /// <summary>
        ///     将输入的字符串 s 转换成 int 数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte ByteValue(this string s)
        {
            byte.TryParse(s, out var value);
            return value;
        }

        /// <summary>
        ///     将输入的字符串 s 转换成 uint 数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint UIntValue(this string s)
        {
            uint.TryParse(s, out var value);
            return value;
        }

        /// <summary>
        ///     将制定的 array 转换成用 separator 连接字符串输出
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Join<T>(string separator, T[] array)
        {
            var value = new string[array.Length];
            for (var index = 0; index < array.Length; index++)
            {
                var a = array[index];
                value[index] = a.ToString();
            }

            return string.Join(separator, value);
        }
    }
}