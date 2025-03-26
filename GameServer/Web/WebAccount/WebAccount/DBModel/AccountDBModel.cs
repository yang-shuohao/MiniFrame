using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebAccount.Entity;
using WebAccount.YSHFrame.Singleton;

namespace WebAccount.DBModel
{
    public class AccountDBModel : Singleton<AccountDBModel>
    {
        private const string connStr = "Data Source=.;Initial Catalog=DBAccount;User Id=sa;Password=123456YSHabc;";
        public AccountEntity Get(int id)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Account_Get", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    if(dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AccountEntity entity = new AccountEntity();

                            entity.Id = dr["Id"] is DBNull ? 0: Convert.ToInt32(dr["Id"]);
                            entity.UserName = dr["UserName"] is DBNull ? string.Empty : dr["UserName"].ToString();
                            entity.Pwd = dr["Pwd"] is DBNull ? string.Empty : dr["Pwd"].ToString();
                            entity.YuanBao = dr["YuanBao"] is DBNull ? 0 : Convert.ToInt32(dr["YuanBao"]);
                            entity.LastServerId = dr["LastServerId"] is DBNull ? 0 : Convert.ToInt32(dr["LastServerId"]);
                            entity.LastServerName = dr["LastServerName"] is DBNull ? string.Empty : dr["LastServerName"].ToString();
                            entity.CreateTime = dr["CreateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["CreateTime"]);
                            entity.UpdateTime = dr["UpdateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["UpdateTime"]);

                            return entity;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int Register(string userName, string pwd)
        {
            int userId = -1;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Account_Register", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                cmd.Parameters.Add(new SqlParameter("@Pwd", pwd));

                userId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return userId;
        }
    }
}