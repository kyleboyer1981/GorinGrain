using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GorinGrain_DAL;
using GorinGrain.ViewModels;
using GorinGrain_DAL.Interfaces;
using GorinGrain.Custom;
using GorinGrain.Models;
using GorinGrain.Interfaces;
using GorinGrain_BLL;
using GorinGrain_BLL.Interfaces;

namespace GorinGrain.Controllers
{

    public class ShipmentController : Controller
    {
        //access to dta layers required to fill dropdown lists
        ShipmentDataAccess _ShipmentAccess = new ShipmentDataAccess();
        LocationDataAccess _LocationAccess = new LocationDataAccess();
        ProducerDataAccess _ProducerAccess = new ProducerDataAccess();
        ShipmentLogicLayer _ShipmentLogic = new ShipmentLogicLayer();

        // GET: Shipment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddShipment()
        {
            ActionResult oResponse = null;

            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                ShipmentVM newVM = new ShipmentVM();

                newVM.Shipment.ProducerDDL = PopulateProducersDDL();
                newVM.Shipment.LocationDDL = PopulateLocationsDDL();



                oResponse = View(newVM);
            }
            else
            {
                oResponse = RedirectToAction("Login", "User");
            }

            return oResponse;
        }

        [HttpPost]
        public ActionResult AddShipment(ShipmentVM viewModel)
        {
            ActionResult oResponse = null;
            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IShipmentInfoDO shipmentform = ShipmentMap.MapPOtoDO(viewModel.Shipment);
                        //call to method in DAL to view shipment by producerID
                        _ShipmentAccess.InsertShipment(shipmentform);
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("duplicate"))
                        {
                            viewModel.ErrorMessage = String.Format("There is already a {0} in the database.", viewModel.Shipment.ShipmentID);
                        }
                        else
                        {
                            viewModel.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                        }
                    }
                    finally
                    {
                        //nothing to do here
                    }
                    if (viewModel.ErrorMessage == null)
                    {
                        oResponse = RedirectToAction("ViewShipments", "Shipment");
                    }
                    else
                    {
                        //Data call here to re-get the locations and producers.
                        viewModel.Shipment.ProducerDDL = PopulateProducersDDL();
                        viewModel.Shipment.LocationDDL = PopulateLocationsDDL();

                        oResponse = View(viewModel);
                    }
                }
                else
                {
                    //Data call here to re-get the locations and producers.
                    //placed into method

                    viewModel.Shipment.ProducerDDL = PopulateProducersDDL();
                    viewModel.Shipment.LocationDDL = PopulateLocationsDDL();

                    oResponse = View(viewModel);
                }
            }
            else
            {
                oResponse = RedirectToAction("Login", "User");
            }
            return oResponse;
        }

        private SelectList PopulateProducersDDL()
        {
            //for entereing shipments, need to know it is coming from a farmer in the db!
            List<IProducerInfoDO> databaseProducers = _ProducerAccess.ViewAllProducers();

            //map
            List<ProducerPO> mappedProducers = ProducerMap.MapDOtoPO(databaseProducers);
            List<ProducerPO> actualList = new List<ProducerPO>();

            //add dummy location for "default" on dropdown, otherwise it sets to ACTUAL location
            actualList.Add(new ProducerPO() { ProducerID = 0, CompanyName = " -- Please choose a Producer --" });

            //after dummy is in top stop, add range from db
            actualList.AddRange(mappedProducers);

            //producer id what is listed, user will see label as Company Name
            return new SelectList(actualList, "ProducerID", "CompanyName");
        }

        private SelectList PopulateLocationsDDL()
        {
            List<ILocationInfoDO> databaseLocations = _LocationAccess.ViewAllLocations();
            List<LocationPO> mappedLocations = LocationMap.MapDOtoPO(databaseLocations);
            List<LocationPO> locationList = new List<LocationPO>();
            //add dummy location for "default" on dropdown, otherwise it sets to ACTUAL location
            locationList.Add(new LocationPO() { LocationID = 0, LocationName = " -- Please choose a Location --" });
            locationList.AddRange(mappedLocations);
            return new SelectList(locationList, "LocationID", "LocationName");

        }

        [HttpGet]
        public ActionResult ViewShipments()
        {
            ActionResult oResponse = null;
            ShipmentVM newVM = new ShipmentVM();
            if (Session["UserName"] != null && ((int)Session["UserLevel"] == 2 || (int)Session["UserLevel"] == 1))
            {
                try
                {
                    //call to db to obtain all shipments, then map list data to presentation list obj.
                    List<IShipmentInfoDO> shipmentData = _ShipmentAccess.ViewAllShipments();
                    newVM.ShipmentList = ShipmentMap.MapDOtoPO(shipmentData);
                }
                catch (Exception)
                {
                    newVM.ErrorMessage = "There was an issue obtaining the list";
                                    }
                finally
                {
                    oResponse = View(newVM);
                }
            }
            else
            {
               oResponse = RedirectToAction("Login", "User");
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult DeleteShipment(long ShipmentID)
        {
            if ((int)Session["UserLevel"] <= 2)
            {
                ShipmentVM removeShipment = new ShipmentVM();

                try
                {
                    _ShipmentAccess.DeleteShipment(ShipmentID);

                }
                catch (Exception)
                {
                    removeShipment.ErrorMessage = "There was a problem deleting the record from the database, please try again later";
                }
                finally
                {
                    //error is kicked to user, return must be outside this scope.
                }
            }
            else
            {
                RedirectToAction("Login", "User");
            }

            return RedirectToAction("ViewShipments", "Shipment");
        }

        [HttpGet]
        public ActionResult UpdateShipment(long ShipmentID)
        {
            ShipmentVM updateVM = new ShipmentVM();

            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                IShipmentInfoDO shipment = _ShipmentAccess.ViewShipmentByID(ShipmentID);

                updateVM.Shipment = ShipmentMap.MapDOtoPO(shipment);
                //call to data layer method to get list of producers for drop down
                updateVM.Shipment.LocationDDL = PopulateLocationsDDL();
                updateVM.Shipment.ProducerDDL = PopulateProducersDDL();
            }
            else
            {
                RedirectToAction("Login", "User");
            }

            return View(updateVM);
        }

        [HttpPost]
        public ActionResult UpdateShipment(ShipmentVM changeshipment)
        {
            ActionResult oResult = null;
            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IShipmentInfoDO updateShipment = ShipmentMap.MapPOtoDO(changeshipment.Shipment);
                        //call method from DAL to change the info in db
                        _ShipmentAccess.UpdateShipment(updateShipment);
                    }
                    catch (Exception e)
                    {
                        //what to do with exception what to write

                        if (e.Message.Contains("duplicate"))
                        {
                            changeshipment.ErrorMessage = "Unable to process request, duplicate record already exists";
                        }
                        else
                        {
                            changeshipment.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                        }
                    }
                    finally
                    {
                    }

                    if (changeshipment.ErrorMessage == null)
                    {
                        //if no errors with update, return to list of shipments to see if info was changed
                        oResult = RedirectToAction("ViewShipments", "Shipment");
                    }
                    else
                    {
                        oResult = RedirectToAction("UpdateShipment", "Shipment");
                    }
                }
                else
                {
                    oResult = View(changeshipment);
                }
            }
            else
            {
                oResult = RedirectToAction("Login", "User");
            }
            return oResult;

        }

        [HttpGet]
        public ActionResult ViewShipmentsByLocation(long LocationID)
        {
            ShipmentVM newVM = new ShipmentVM();

            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                try
                {
                    //call to db to get ships by loc, then map list data to presentation list obj.
                    List<IShipmentInfoDO> shipmentData = _ShipmentAccess.ViewShipmentsByLocation(LocationID);
                    newVM.ShipmentList = ShipmentMap.MapDOtoPO(shipmentData);

                }
                catch (Exception)
                {
                    newVM.ErrorMessage = "There was an issue obtaining the list";

                }
                finally
                {
                    ///nothing else needs to happen
                }
            }
            else
            {
                RedirectToAction("Login", "User");
            }

            return View(newVM);
        }

        [HttpGet]
        public ActionResult ViewShipmentsByProducer(long ProducerID)
        {
            ShipmentVM newVM = new ShipmentVM();
            if (Session["UserName"] != null && (int)Session["UserLevel"] <= 2)
            {
                try
                {
                    //call to db to get ships by loc, then map list data to presentation list obj.
                    List<IShipmentInfoDO> shipmentData = _ShipmentAccess.ViewShipmentsByProducer(ProducerID);
                    newVM.ShipmentList = ShipmentMap.MapDOtoPO(shipmentData);

                }
                catch (Exception)
                {
                    newVM.ErrorMessage = "There was an issue obtaining the list";

                }
                finally
                {
                    ///nothing else needs to happen
                }
            }
            else
            {
                RedirectToAction("Login", "User");
            }
            return View(newVM);
        }
    }
}