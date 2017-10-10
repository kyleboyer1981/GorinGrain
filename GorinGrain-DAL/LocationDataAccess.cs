using GorinGrain_DAL.Custom;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GorinGrain_DAL
{
    public class LocationDataAccess
    {
        //set connection path to DB
        public string connectionString = @"Server=ADMIN2-PC\SQLEXPRESS;Database = GorinGrainGroup; Trusted_Connection=True;";

        public void InsertLocation(ILocationInfoDO location)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                {
                    //create a command for adding location
                    using (SqlCommand command = new SqlCommand("ADD_LOCATION", connectionToSQL))
                    {
                        try
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 35;
                            //SQL needs @params, pass these values as @values
                            command.Parameters.AddWithValue("@LocationName", location.LocationName);
                            command.Parameters.AddWithValue("@Address", location.Address);
                            command.Parameters.AddWithValue("@Phone", location.Phone);
                            command.Parameters.AddWithValue("@MaxCapacity", location.MaxCapacity);

                            connectionToSQL.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            //write data error to file, pass to presentation layer
                            ErrorLogging.LogError(e);
                            throw (e);
                        }
                        finally
                        {
                            // using statement should close connection, this is just a double check
                            connectionToSQL.Close();
                            connectionToSQL.Dispose();
                        }
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

        public List<ILocationInfoDO> ViewAllLocations()
        {
            List<ILocationInfoDO> viewList = new List<ILocationInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("VIEW_ALL_LOCATIONS", connectionToSQL))
                {
                    try
                    {

                        connectionToSQL.Open();
                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ILocationInfoDO location = new LocationDO();

                            location.LocationID = reader.GetInt64(0);
                            location.LocationName = reader.GetString(1);
                            location.Address = reader.GetString(2);
                            location.Phone = reader.GetString(3);
                            location.MaxCapacity = reader.GetInt64(4);

                            viewList.Add(location);

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

        public void DeleteLocation(long LocationID)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("DELETE_LOCATION", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30;
                        command.Parameters.AddWithValue("@LocationID", LocationID);
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

        public void UpdateLocation(ILocationInfoDO location)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("UPDATE_LOCATION", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@LocationID", location.LocationID);
                        command.Parameters.AddWithValue("@LocationName", location.LocationName);
                        command.Parameters.AddWithValue("@Address", location.Address);
                        command.Parameters.AddWithValue("@Phone", location.Phone);
                        command.Parameters.AddWithValue("@MaxCapacity", location.MaxCapacity);

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

        public ILocationInfoDO ViewLocationByID(long LocationID)
        {
            ILocationInfoDO viewlocation = new LocationDO();
            try
            {
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("VIEW_LOCATION_BY_ID", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@LocationID", LocationID);
                        connectionToSQL.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            viewlocation.LocationID = reader.GetInt64(0);
                            viewlocation.LocationName = reader.GetString(1);
                            viewlocation.Address = reader.GetString(2);
                            viewlocation.Phone = reader.GetString(3);
                            viewlocation.MaxCapacity = reader.GetInt64(4);
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
            return viewlocation;

        }
    }

}