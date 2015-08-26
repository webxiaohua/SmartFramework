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
    /// @Desc:文件操作类
    /// </summary>
    public class FileHelper
    {
        private static Object lockObj = new object();//同步标志

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:检测指定文件是否存在
        /// </summary>
        /// <param name="filePath">文件目录</param>
        /// <returns>是否存在文件</returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #region 获取所有文件
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取目录下的所有文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            if (DirectoryHelper.IsExistDirectory(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }
            else
            {
                return Directory.GetFiles(directoryPath);
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">制定目录物理路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0个或者N个字符，"?"代表1个字符，示例：log*.xml 表示搜索所有以log开头的xml文件</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //目录不存在
            if (DirectoryHelper.IsExistDirectory(directoryPath))
            {
                throw new DirectoryNotFoundException();
            }
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);//包含子目录
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);//不包含子目录
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:创建一个文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);
                    //如果文件目录不存在，则创建目录
                    DirectoryHelper.CreateDirectory(directoryPath);
                    lock (lockObj)
                    {
                        //创建文件                    
                        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate)) { }
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
        /// @Date:2015-08-18
        /// @Desc:创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">待写入的字符串数据</param>
        public static void CreateFile(string filePath, string text)
        {
            CreateFile(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        /// <param name="encoding">字符编码</param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);
                    //如果文件的目录不存在，则创建目录
                    DirectoryHelper.CreateDirectory(directoryPath);
                    //创建文件
                    FileInfo file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            //写入字符串     
                            writer.Write(text);
                            //输出
                            writer.Flush();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 读取文件

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns>文件内容</returns>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>文件内容</returns>
        public static string FileToString(string filePath, Encoding encoding)
        {
            if (IsExistFile(filePath))
            {
                //创建流读取器
                StreamReader reader = new StreamReader(filePath, encoding);
                try
                {
                    //读取流
                    return reader.ReadToEnd();
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 写入文件
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="text">写入内容</param>
        /// <param name="encoding">文本编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            //向文件写入内容
            File.WriteAllText(filePath, text, encoding);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        public static void AppendText(string filePath, string text)
        {
            try
            {
                lock (lockObj)
                {
                    //创建流写入器
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(text);
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
        /// @Date:2015-08-18
        /// @Desc:将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="targetFilePath">目标文件的绝对路径</param>
        public static void CopyTo(string sourceFilePath, string targetFilePath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }
            try
            {
                //检测目标文件的目录是否存在，不存在则创建
                string destDirectoryPath = GetDirectoryFromFilePath(targetFilePath);
                DirectoryHelper.CreateDirectory(destDirectoryPath);

                //复制文件
                FileInfo file = new FileInfo(sourceFilePath);
                file.CopyTo(targetFilePath, true);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:写文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strValue">写入内容</param>
        public static void WriteFile(string strFilePath, string strValue)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(strFilePath);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                if (!fileInfo.Exists)
                {
                    fileInfo.Create().Close();
                }
                using (StreamWriter streamWriter = new StreamWriter(strFilePath, false, Encoding.UTF8))
                {
                    streamWriter.Write(strValue);
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
        /// @Desc:写文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strValue">写入内容</param>
        /// <param name="charset">编码格式</param>
        public static void WriteFile(string strFilePath, string strValue, Encoding encoding)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(strFilePath);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                if (!fileInfo.Exists)
                {
                    fileInfo.Create().Close();
                }
                using (StreamWriter streamWriter = new StreamWriter(strFilePath, false, encoding))
                {
                    streamWriter.Write(strValue);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:删除指定文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:清空文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void ClearFile(string filePath)
        {
            try
            {
                //删除文件
                File.Delete(filePath);
                //重新创建该文件
                CreateFile(filePath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 移动文件
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:将文件移动到指定目录，并指定新的文件名( 剪切并改名 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descFilePath">目标文件的绝对路径</param>
        public static void Move(string sourceFilePath, string targetFilePath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }
            try
            {
                //获取目标文件目录
                string targetDirectoryPath = GetDirectoryFromFilePath(targetFilePath);
                //创建目标目录
                DirectoryHelper.CreateDirectory(targetDirectoryPath);
                //将文件移动到指定目录
                File.Move(sourceFilePath, targetFilePath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:将文件移动到指定目录( 剪切 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="targetFilePath">移动到的目录的绝对路径</param>
        public static void MoveToDirectory(string sourceFilePath, string targetFilePath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }
            try
            {
                //获取源文件的名称
                string sourceFileName = GetFileName(sourceFilePath);
                //如果目标目录不存在则创建
                DirectoryHelper.CreateDirectory(targetFilePath);
                //如果目标中存在同名文件,则删除
                if (IsExistFile(targetFilePath + "\\" + sourceFileName))
                {
                    DeleteFile(targetFilePath + "\\" + sourceFileName);
                }
                //目标文件路径
                string descFilePath;
                if (!targetFilePath.EndsWith(@"\"))
                {
                    descFilePath = targetFilePath + "\\" + sourceFileName;
                }
                else
                {
                    descFilePath = targetFilePath + sourceFileName;
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descFilePath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取文件属性
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Name;
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fileInfo = new FileInfo(filePath);
            if (!filePath.IsNullOrEmpty())
            {
                return fileInfo.Name.Split('.')[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static int GetLineCount(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    //行数
                    int i = 0;
                    while (true)
                    {
                        //如果读取到内容就把行数加1
                        if (reader.ReadLine() != null)
                        {
                            i++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return i;
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
        /// @Desc:获取文本文件的大小 单位：byte 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return (int)fileInfo.Length;
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取文本文件的大小 单位：KB 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static double GetFileSizeByKB(string filePath)
        {
            //创建一个文件对象
            FileInfo fileInfo = new FileInfo(filePath);
            //获取文件的大小
            return Convert.ToDouble(Convert.ToDouble(fileInfo.Length) / 1024);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取文本文件的大小 单位：MB 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static double GetFileSizeByMB(string filePath)
        {
            //创建一个文件对象
            FileInfo fileInfo = new FileInfo(filePath);
            //获取文件的大小
            return Convert.ToDouble(Convert.ToDouble(fileInfo.Length) / 1024 / 1024);
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取文件扩展名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件扩展名 带'.'</returns>
        public static string GetFileExtensionByPath(string filePath)
        {
            if (IsExistFile(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Extension;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:获取文件扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtensionByName(string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                if (fileName.Contains('.'))
                {
                    int tmpPosition = fileName.LastIndexOf('.');
                    return fileName.Substring(tmpPosition, fileName.Length - tmpPosition);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region 文件流操作
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:根据指定路径获取文件流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件流</returns>
        public static byte[] GetFileSream(string filePath)
        {
            try
            {
                byte[] buffer = null;
                using (FileStream stream = new FileInfo(filePath).OpenRead())
                {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                }
                return buffer;
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 检测文件是否存在于某个目录下面
        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-19
        /// @Desc:检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>是否包含某个文件</returns>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);
                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
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
        /// @Desc:检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。</param>
        /// <returns>是否包含某个文件</returns>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);
                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// @Autor:Robin
        /// @Date:2015-08-18
        /// @Desc:从文件绝对路径中获取目录路径
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <returns></returns>
        public static string GetDirectoryFromFilePath(string filePath)
        {
            //实例化文件
            FileInfo file = new FileInfo(filePath);
            //获取目录信息
            DirectoryInfo directory = file.Directory;
            //返回目录路径
            return directory.FullName;
        }

    }
}
