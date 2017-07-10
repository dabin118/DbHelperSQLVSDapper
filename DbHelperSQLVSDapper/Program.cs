using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
namespace DbHelperSQLVSDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //在tempDB库中新建表Customer
            SqlHelper.EnsureDbSetup();
            Console.WriteLine("DbHelperSQL和Dapper性能对比开始......");
            DbHelperSQLTest();
            Console.WriteLine();
            Console.WriteLine();
            DapperTest();
            Console.WriteLine("按回车键退出");
            Console.ReadKey();
        }
        static  void DbHelperSQLTest()
        {
            Hashtable ht = new Hashtable();
            var sw = new Stopwatch();
            sw.Start();
            //插它五百次
            for (int i = 0; i < 500;i++ )
            {
                SqlParameter[] param = { new SqlParameter ("@CustomerId",SqlDbType.NChar,5),
                                     new SqlParameter("@companyName",SqlDbType.NVarChar,40)
                                                                         
                                     };
                param[0].Value = SqlHelper.TestId;
                param[1].Value = SqlHelper.InsertCompanyName;
             
                for (int j = 0; j < i;j++ )
                {
                    SqlHelper.CreateStatement += ";";
                }
                ht.Add(SqlHelper.CreateStatement, param);
            }
           //利用事务批量执行Sql插入操作
            DbHelperSQL.connectionString = SqlHelper.ConnectionString;
            DbHelperSQL.ExecuteSqlTran(ht);
            sw.Stop ();
            Console.WriteLine("DbHelperSQL执行事务插入500条记录耗时(毫秒):  "+sw.ElapsedMilliseconds);
            //测试DbHelperSQL Select top 500耗费时间
            sw.Reset();
            sw.Start();
            DbHelperSQL.Query(SqlHelper.ReadStatement);
          
            sw.Stop();  
            Console.WriteLine("DbHelperSQL选择500条记录耗时(毫秒):  " + sw.ElapsedMilliseconds);
        }
        static void DapperTest()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < 500; i++)
            {

                customers.Add(new Customer { CustomerId = SqlHelper.TestId, CompanyName = SqlHelper.InsertCompanyName });

            }
          SqlMapperUtil.InsertMultiple<Customer>(SqlHelper.CreateStatement, customers, SqlHelper.ConnectionString);
      
            sw.Stop();
            Console.WriteLine("Dapper插入500条记录耗时(毫秒):  " + sw.ElapsedMilliseconds);
            sw.Reset();

            //测试Dapper进行 Select top 500耗费时间
            sw.Start();
           SqlMapperUtil.SqlWithParams<Customer>(SqlHelper.ReadStatement, null,SqlHelper.ConnectionString);
           
            sw.Stop(); 
            Console.WriteLine("Dapper选取前500条记录耗时(毫秒):  " + sw.ElapsedMilliseconds);
        }
    }
}

