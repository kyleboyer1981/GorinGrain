using GorinGrain_DAL.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GorinGrain_DAL
{
    public class UserDataAccess
    {
        //set connection to db for all calls, individual connections opened and closed in methods.
        public string connectionString = @"Server=ADMIN2-PC\SQLEXPRESS;Database = GorinGrainGroup; Trusted_Connection=True;";


        //comments about procedures in this method
        public UserDO ViewUserByUserName(String iUserName)
        {
            //instantiate new data object that we will fill from sql
            UserDO user = new UserDO();

            try
            {
                //first "try" is for establishing a connection for this method
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //using command to start stored proc for viewing user by login name
                using (SqlCommand command = new SqlCommand("VIEW_USER_BY_USERNAME", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        //pushing the UserName to SQL for query
                        command.Parameters.AddWithValue("@UserName", iUserName);
                        connectionToSQL.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //map to user data object from (column) spot in this user row
                            user.FirstName = reader.GetString(0);
                            user.LastName = reader.GetString(1);
                            user.UserName = reader.GetString(2);
                            user.Password = reader.GetString(3);
                            user.UserLevel = reader.GetInt32(4);
                        }
                    }
                    catch (Exception e)
                    {
                        //write error to file and pass to next level up
                        ErrorLogging.LogError(e);
                        throw e;
                    }
                    finally
                    {//errorr is logged, proceed
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {
                //nothing
            }


            return user;
        }

        public void InsertUser(IuserInfoDO user)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("ADD_USER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Password", user.Password);
                       

                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ErrorLogging.LogError(e);
                        throw (e);
                    }
                    finally
                    {
                        connectionToSQL.Close();
                        connectionToSQL.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogging.LogError(e);
                throw (e);
            }
            finally
            {
                //no connection to dispose, do nothing
            }

        }

        public List<IuserInfoDO> ViewAllUsers()
        {
            List<IuserInfoDO> viewList = new List<IuserInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("VIEW_ALL_USERS", connectionToSQL))
                {
                    try
                    {
                        connectionToSQL.Open();
                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            IuserInfoDO user = new UserDO();

                            user.UserID  = reader.GetInt32(0);
                            user.FirstName = reader.GetString(1);
                            user.LastName  = reader.GetString(2);
                            user.UserName = reader.GetString(3);
                            user.Password  = reader.GetString(4);
                            user.UserLevel = reader.GetInt32(5);

                            viewList.Add(user);
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorLogging.LogError(e);
                        throw e;
                    }
                    finally
                    {
                        connectionToSQL.Close();
                        connectionToSQL.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {
            }
            return viewList;
        }

        public void DeleteUser(int UserID)
        {
            //can not delete admin
            if(UserID != 1)
            {
                try
                {
                    //create connection
                    using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                    //create a command
                    using (SqlCommand command = new SqlCommand("DELETE_USER", connectionToSQL))
                    {
                        try
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 30;
                            command.Parameters.AddWithValue("@UserID", UserID);
                            connectionToSQL.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            ErrorLogging.LogError(e);
                            throw e;
                        }
                        finally
                        {
                            connectionToSQL.Close();
                            connectionToSQL.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogging.LogError(e);
                    throw e;
                }
                finally
                {
                    //do nothing, connection is already disposed
                }
            }
            else
            {
               //program will break at this point, need to have catch on presentation layer
            }
            


        }

        public void UpdateUser(IuserInfoDO user)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("UPDATE_USER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@UserLevel", user.UserLevel);
                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ErrorLogging.LogError(e);
                        throw e;
                    }
                    finally
                    {
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //good to go
            }
        }

        public IuserInfoDO ViewUserByID(int UserID)
        {
            IuserInfoDO viewUser = new UserDO();
            try
            {
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("VIEW_USER_BY_ID", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@UserID", UserID);
                        connectionToSQL.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //map products from reader to interface object
                            viewUser.UserID = reader.GetInt32(0);
                            viewUser.FirstName = reader.GetString(1);
                            viewUser.LastName = reader.GetString(2);
                            viewUser.UserName = reader.GetString(3);
                            viewUser.Password = reader.GetString(4);
                            viewUser.UserLevel = reader.GetInt32(5);
                        }
                    }
                    catch (Exception e)
                    {
                        //write error to file and pass on to next level
                        ErrorLogging.LogError(e);
                        throw e;
                    }
                    finally
                    {
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {
                //nothing
            }
            return viewUser;

        }



    }
}
