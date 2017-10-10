using GorinGrain_DAL.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GorinGrain_DAL
{
    public class ShipmentDataAccess
    {
        //set connection path to DB, used by all data call methods
        public string connectionString = @"Server=ADMIN2-PC\SQLEXPRESS;Database = GorinGrainGroup; Trusted_Connection=True;";

        public void InsertShipment(IShipmentInfoDO shipment)
        {
            try
            {
                //create connection to db by way of saved connection string
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command to use for this data session
                using (SqlCommand command = new SqlCommand("ADD_SHIPMENT", connectionToSQL))
                {
                    try
                    {
                        //this command will call a stored proc
                        command.CommandType = CommandType.StoredProcedure;
                        //try for 35 seconds before error is thrown
                        command.CommandTimeout = 35;
                        //stored proc is set to recieve the @values below, object.value is the actual info going to SQL
                        command.Parameters.AddWithValue("@LocationID", shipment.LocationID);
                        command.Parameters.AddWithValue("@Product", shipment.Product);
                        command.Parameters.AddWithValue("@ProducerID", shipment.ProducerID);
                        command.Parameters.AddWithValue("@QuantityInBu", shipment.QuantityInBu);
                        command.Parameters.AddWithValue("@PricePerBushel", shipment.PricePerBushel);

                        //sql commands to open the connection and execute the stored proc, "non query" means no data needed in return
                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        //pass error out and then to presentation layer so that it can be displayed to user
                        throw (e);
                    }
                    finally
                    {
                        //should be handles by using statements, this is back-up
                        connectionToSQL.Close();
                        connectionToSQL.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                //this would be connection error, write to file and pass to presentation layer
                ErrorLogging.LogError(e);
                throw (e);
            }
            finally
            {
                //no connection to dispose, do nothing
            }

        }

        public List<IShipmentInfoDO> ViewAllShipments()
        {
            //create new list to store several objects from db
            List<IShipmentInfoDO> viewList = new List<IShipmentInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("VIEW_ALL_SHIPMENTS", connectionToSQL))
                {
                    try
                    {

                        connectionToSQL.Open();
                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //intantiate new shipment data object to fill from reader
                            IShipmentInfoDO shipment = new ShipmentDO();

                            //shipment.info is set to info from database
                            shipment.ShipmentID = reader.GetInt64(0);
                            shipment.LocationID = reader.GetInt64(1);
                            shipment.Product = reader.GetString(2);
                            shipment.ProducerID = reader.GetInt64(3);
                            shipment.QuantityInBu = reader.GetInt64(4);
                            shipment.PricePerBushel = reader.GetDecimal(5);

                            //add each row, as its own object, to this list
                            viewList.Add(shipment);

                        }
                    }
                    catch (Exception e)
                    {
                        //throw out to next try catch to log, avoid logging same error 2 times
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
                //write error to file and pass to next level up (presentation layer)
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {

            }
            return viewList;
        }

        public void DeleteShipment(long ShipmentID)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("DELETE_SHIPMENT", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30;
                        command.Parameters.AddWithValue("@ShipmentID", ShipmentID);
                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
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
                //write to file and pass to presentation layer
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {
                //do nothing, connection is already disposed
            }


        }

        public void UpdateShipment(IShipmentInfoDO shipment)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("UPDATE_SHIPMENT", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        command.Parameters.AddWithValue("@ShipmentID", shipment.ShipmentID);
                        command.Parameters.AddWithValue("@LocationID", shipment.LocationID);
                        command.Parameters.AddWithValue("@Product", shipment.Product);
                        command.Parameters.AddWithValue("@ProducerID", shipment.ProducerID);
                        command.Parameters.AddWithValue("@QuantityInBu", shipment.QuantityInBu);
                        command.Parameters.AddWithValue("@PricePerBushel", shipment.PricePerBushel);

                        connectionToSQL.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        //error wrote to Log Files in same folder as this project
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

        public IShipmentInfoDO ViewShipmentByID(long ShipmentID)
        {
            IShipmentInfoDO viewshipment = new ShipmentDO();
            try
            {
                //using statements for connection and command
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand("VIEW_SHIPMENT_BY_ID", connectionToSQL))
                {
                    try
                    {
                        command.CommandTimeout = 35;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ShipmentID", ShipmentID);
                        connectionToSQL.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            viewshipment.ShipmentID = reader.GetInt64(0);
                            viewshipment.LocationID = reader.GetInt64(1);
                            viewshipment.Product = reader.GetString(2);
                            viewshipment.ProducerID = reader.GetInt64(3);
                            viewshipment.QuantityInBu = reader.GetInt64(4);
                            viewshipment.PricePerBushel = reader.GetDecimal(5);
                        }
                    }
                    catch (Exception e)
                    {
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
            return viewshipment;

        }

        public List<IShipmentInfoDO> ViewShipmentsByProducer(long ProducerID)
        {
            //create new list to store several objects from db
            List<IShipmentInfoDO> viewList = new List<IShipmentInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command to view shipments by producerID column
                using (SqlCommand command = new SqlCommand("VIEW_SHIPMENTS_BY_PRODUCER", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProducerID", ProducerID);
                        command.CommandTimeout = 35;
                        connectionToSQL.Open();
                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        //as reader gathers data
                        while (reader.Read())
                        {
                            //intantiate new shipment data object to fill from reader
                            IShipmentInfoDO shipment = new ShipmentDO();

                            //shipment.info is set to info from database
                            shipment.ShipmentID = reader.GetInt64(0);
                            shipment.LocationID = reader.GetInt64(1);
                            shipment.Product = reader.GetString(2);
                            shipment.ProducerID = reader.GetInt64(3);
                            shipment.QuantityInBu = reader.GetInt64(4);
                            shipment.PricePerBushel = reader.GetDecimal(5);

                            //add each row, as its own object, to this list
                            viewList.Add(shipment);

                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        //back up closing of connection if "using" statement happens to miss
                        connectionToSQL.Close();
                        connectionToSQL.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                //write error to file and pass to next level up (presentation layer)
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {

            }
            return viewList;
        }

        public List<IShipmentInfoDO> ViewShipmentsByLocation(long LocationID)
        {
            //create new list to store several objects from db
            List<IShipmentInfoDO> viewList = new List<IShipmentInfoDO>();

            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("VIEW_SHIPMENTS_BY_LOCATION", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
                        connectionToSQL.Open();
                        //pass in Location ID to search by that column
                        command.Parameters.AddWithValue("@LocationID", LocationID);

                        //use reader
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            //intantiate new shipment data object to fill from reader
                            IShipmentInfoDO shipment = new ShipmentDO();

                            //shipment.info is set to info from database
                            shipment.ShipmentID = reader.GetInt64(0);
                            shipment.LocationID = reader.GetInt64(1);
                            shipment.Product = reader.GetString(2);
                            shipment.ProducerID = reader.GetInt64(3);
                            shipment.QuantityInBu = reader.GetInt64(4);
                            shipment.PricePerBushel = reader.GetDecimal(5);

                            //add each row, as its own object, to this list
                            viewList.Add(shipment);

                        }
                    }
                    catch (Exception e)
                    {
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
                //write error to file and pass to next level up (presentation layer)
                ErrorLogging.LogError(e);
                throw e;
            }
            finally
            {

            }
            return viewList;
        }



    }
}
