using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// @Autor:Robin
    /// @Date:2015-08-19
    /// @Desc:String 对象扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:验证字符串是否为空
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string strValue)
        {
            return String.IsNullOrEmpty(strValue);
        }
    }
}
