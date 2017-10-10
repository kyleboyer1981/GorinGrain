using GorinGrain_DAL.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GorinGrain_DAL
{
   public class ProducerDataAccess
    {
        //set connection path to DB
        public string connectionString = @"Server=ADMIN2-PC\SQLEXPRESS;Database = GorinGrainGroup; Trusted_Connection=True;";

        public void InsertProducer(IProducerInfoDO producer)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("ADD_PRODUCER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        //values from form get pushed to fill SQL
                        command.Parameters.AddWithValue("@CompanyName", producer.CompanyName);
                        command.Parameters.AddWithValue("@ContactName", producer.ContactName);
                        command.Parameters.AddWithValue("@Phone", producer.Phone);
                        command.Parameters.AddWithValue("@Address", producer.Address);

                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        //error wrote to file
                        ErrorLogging.LogError(e);
                        throw (e);
                    }
                    finally
                    {
                        //should close with using statement, finally is just to be sure
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

        public List<IProducerInfoDO> ViewAllProducers()
        {
            //create new list for producers VIEW ALL
            List<IProducerInfoDO> viewList = new List<IProducerInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("VIEW_ALL_PRODUCERS", connectionToSQL))
                {
                    try
                    {

                        connectionToSQL.Open();
                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        //reader pulls each row 
                        while (reader.Read())
                        {
                            IProducerInfoDO producer = new ProducerDO();

                            producer.ProducerID = reader.GetInt64(0);
                            producer.CompanyName = reader.GetString(1);
                            producer.ContactName = reader.GetString(2);
                            producer.Phone = reader.GetString(3);
                            producer.Address = reader.GetString(4);

                            //after data is pulled from row, that object is added to list
                            viewList.Add(producer);

                        }
                    }
                    catch (Exception e)
                    {
                        //write error, if any, to file
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
                //throw up to presentation layer, then push to user
                throw e;
            }
            finally
            {

            }
            return viewList;
        }

        public void DeleteProducer(long ProducerID)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command to process DELETE PRODUCER STORED PROC
                using (SqlCommand command = new SqlCommand("DELETE_PRODUCER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30;
                        command.Parameters.AddWithValue("@ProducerID", ProducerID);
                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        //save and write error to file
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

        public void UpdateProducer(IProducerInfoDO producer)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command to run Stored Proc
                using (SqlCommand command = new SqlCommand("UPDATE_PRODUCER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        //fill new SQL row with this data
                        command.Parameters.AddWithValue("@ProducerID", producer.ProducerID);
                        command.Parameters.AddWithValue("@CompanyName", producer.CompanyName);
                        command.Parameters.AddWithValue("@ContactName", producer.ContactName);
                        command.Parameters.AddWithValue("@Phone", producer.Phone);
                        command.Parameters.AddWithValue("@Address", producer.Address);
                        //open SQL and run process this request
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
                //nothing needs to happen on this finally, connections are closed and errors are logged and passed up the chain
            }
        }

        public IProducerInfoDO ViewProducerByID(long ProducerID)
        {
            IProducerInfoDO viewProducer = new ProducerDO();
            try
            {
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("VIEW_PRODUCER_BY_ID", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@ProducerID", ProducerID);
                        connectionToSQL.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //map products from reader to interface object
                            viewProducer.ProducerID = reader.GetInt64(0);
                            viewProducer.CompanyName = reader.GetString(1);
                            viewProducer.ContactName = reader.GetString(2);
                            viewProducer.Phone = reader.GetString(3);
                            viewProducer.Address = reader.GetString(4);
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
            return viewProducer;

        }


    }
}
