using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.DAO.Simple
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-31
    /// @Desc:自定义属性，用于指示如何从DataTable或者DbDataReader中读取类的属性值
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 类属性对应的列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnName">类属性对应的列名</param>
        public ColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
