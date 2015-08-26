using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Smart.FileFunction
{
    /// <summary>
    /// @Autor:Robin
    /// @Date:2015-08-18
    /// @Desc:目录操作类
    /// </summary>
    public class DirectoryHelper
    {
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:检测目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>        
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">制定目录</param>
        /// <returns>子目录列表</returns>
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">源目录</param>
        /// <param name="searchPattern">检索表达式</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>目录列表</returns>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            try
            {
                if (IsExistDirectory(directoryPath))
                {
                    //删除目录中所有的文件
                    string[] fileNames = FileHelper.GetFileNames(directoryPath);
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        FileHelper.DeleteFile(fileNames[i]);
                    }
                    //删除目录中所有的子目录
                    string[] directoryNames = GetDirectories(directoryPath);
                    for (int i = 0; i < directoryNames.Length; i++)
                    {
                        DeleteDirectory(directoryNames[i]);
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = FileHelper.GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }
                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
    }
}
