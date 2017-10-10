using GorinGrain_DAL.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorinGrain_DAL
{
    class ShipmentsDataAccess
    {
        //set connection path to DB
        public string connectionString = @"Server=ADMIN2-PC\SQLEXPRESS;Database = GorinGrainGroup; Trusted_Connection=True;";

        public void InsertShipment(IShipmentInfoDO shipment)
        {
            try
            {
                //create connection
                using (SqlConnection connectionToSQL = new SqlConnection(connectionString))
                //create a command
                using (SqlCommand command = new SqlCommand("ADD_SHIPMENT", connectionToSQL))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 35;
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
                        ErrorLogging.logError(e);
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
                ErrorLogging.logError(e);
                throw (e);
            }
            finally
            {
                //no connection to dispose, do nothing
            }

        }

        public List<IShipmentInfoDO> ViewAllShipments()
        {
            //create new list to store several interface objects
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
                            IShipmentInfoDO location = new ShipmentDO();

                            location.LocationID = reader.GetInt64(0);
                            location.Product = reader.GetString(1);
                            location.ProducerID = reader.GetInt64(2);
                            location.QuantityInBu = reader.GetInt64(3);
                            location.PricePerBushel = reader.GetDecimal(4);

                            viewList.Add(location);

                        }
                    }
                    catch (Exception e)
                    {
                        ErrorLogging.logError(e);
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
                ErrorLogging.logError(e);
                throw e;
            }
            finally
            {

            }
            return viewList;
        }

    }
}
