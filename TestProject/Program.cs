using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.DAO.Simple;
using System.Data.SqlClient;
using System.Data;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Student> list = GetList();
            Console.WriteLine(list.Count);
            Console.Read();
        }

        private int Add()
        {
            DbUtility dbUtility = new DbUtility("Server=.;uid=sa;pwd=landa;database=ORMTest", DbProviderType.SqlServer);
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@Name",SqlDbType.VarChar,50),
                new SqlParameter("@Sex",SqlDbType.VarChar,10),
                new SqlParameter("@ClassId",SqlDbType.VarChar,50),
                new SqlParameter("@StudentNo",SqlDbType.VarChar,50)
            };
            parms[0].Value = "SimpleTest";
            parms[1].Value = "男";
            parms[2].Value = "09461";
            parms[3].Value = "0946101";
            int result = dbUtility.ExecuteNonQuery("insert into Student(Name,Sex,ClassId,StudentNo)values(@Name,@Sex,@ClassId,@StudentNo)", parms);
            return result;
        }

        public static List<Student> GetList()
        {
            DbUtility dbUtility = new DbUtility("Server=.;uid=sa;pwd=landa;database=ORMTest", DbProviderType.SqlServer);
            return dbUtility.QueryForList<Student>("select * from Student", null);
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string ClassId { get; set; }
        public string StudentNo { get; set; }
    }
}
