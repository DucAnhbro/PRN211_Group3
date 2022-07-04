﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment1.BusinessObject;
using DataAccess;
using Microsoft.Data.SqlClient;
using System.Data;
namespace Assignment1.DataAccess
{
    public class MemberDAO : BaseDAL
    {

        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        public MemberDAO() { }
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<MemberObject> GetMemberList()
        {
            IDataReader? dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, Companyname, City, Country, Password FROM Member";
            var mem = new List<MemberObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    mem.Add(new MemberObject
                    {
                        ID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                        City = dataReader.GetString(2),
                        Country = dataReader.GetString(3),
                        Email = dataReader.GetString(4),
                        Password = dataReader.GetString(5)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return mem;
        }

        public MemberObject GetMemberByID(int memID)
        {
            MemberObject? mem = null;
            IDataReader? dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, Companyname, City, Country, Password FROM Member WHERE MemberId = @MemberId";
            try
            {
                var param = dataProvider.CreateParameter("@MemberId", 4, memID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    mem = new MemberObject
                    {
                        ID = dataReader.GetInt32(0),
                        Name = dataReader.GetString(1),
                        City = dataReader.GetString(2),
                        Country = dataReader.GetString(3),
                        Email = dataReader.GetString(4),
                        Password = dataReader.GetString(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return mem;
        }

        public bool Check(string email)
        {
            bool check = false;
            IDataReader? dataReader = null;
            try
            {
                string SQLSelect = "SELECT MemberId, Email, Companyname, City, Country, Password FROM Member WHERE Email = @Email";
                var mail = dataProvider.CreateParameter("@Email", 50, email, DbType.String);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, mail);
                if (dataReader.Read())
                {
                    check = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("This email does not exits");
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return check;
        }
        public MemberObject CheckLogin(string email, string pass)
        {
            MemberObject? mem = null;
            IDataReader? reader = null;
            string SQL = "SELECT [MemberId],[Email],[CompanyName],[City],[Country],[Password] FROM [FStore_Ass2].[dbo].[tbl_Member] WHERE [Email] like @Email AND [Password] like @Password";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@Email", 50, email, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Password", 50, pass, DbType.String));
                reader = dataProvider.GetDataReader(SQL, CommandType.Text, out connection, parameters.ToArray());
                if (reader.Read())
                {
                    mem = new MemberObject
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        City = reader.GetString(2),
                        Country = reader.GetString(3),
                        Email = reader.GetString(4),
                        Password = reader.GetString(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                reader.Close();
                CloseConnection();
            }
            return mem;
        }

        public void AddNew(MemberObject mem)
        {
            try
            {
                MemberObject member = GetMemberByID(mem.ID);
                if (member.ID == null)
                {
                    string SQLInsert = "INSERT Member values(@MemberId, @Email, @Companyname, @City, @Country, @Password)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 4, mem.ID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Email", 4, mem.Name, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Companyname", 50, mem.City, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@City", 50, mem.Country, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Country", 50, mem.Email, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Password", 4, mem.Password, DbType.String));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("This Member is already exits.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Update(MemberObject mem)
        {
            try
            {
                MemberObject member = GetMemberByID(mem.ID);
                if (member != null)
                {
                    string SQLUpdate = "UPDATE Member set Email = @Email,Companyname = @Companyname,City = @City, Country = @Country, Password = @Password WHERE MemberId = @MemberId";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 4, mem.ID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Email", 4, mem.Name, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Companyname", 50, mem.City, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@City", 50, mem.Country, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Country", 50, mem.Email, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Password", 4, mem.Password, DbType.String));
                    dataProvider.Insert(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Member does not already exits.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void Remove(int memID)
        {
            try
            {
                MemberObject member = GetMemberByID(memID);
                if (member != null)
                {
                    string SQLDelete = "DELETE Member WHERE MemberId = @MemberId";
                    var param = dataProvider.CreateParameter("@MemberId", 4, memID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Member does not already exits.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
