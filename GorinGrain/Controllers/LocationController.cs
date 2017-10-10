using GorinGrain.Custom;
using GorinGrain.ViewModels;
using GorinGrain_DAL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GorinGrain.Controllers
{
    public class LocationController : Controller
    {
        LocationDataAccess _LocationAccess = new LocationDataAccess();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddLocation()
        {
            ActionResult oResponse = null;
            if (Session["Username"] == null || (int)Session["UserLevel"] != 1)
            {
                oResponse = RedirectToAction("Login", "User");
                return oResponse;

            }
            else
            {
                LocationVM newVM = new LocationVM();
                oResponse = View(newVM);
                return oResponse;
            }
        }

        [HttpPost]
        public ActionResult AddLocation(LocationVM viewModel)
        {
            ActionResult oResponse = null;
            if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        ILocationInfoDO locationform = LocationMap.MapPOtoDO(viewModel.Location);
                        _LocationAccess.InsertLocation(locationform);
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("duplicate"))
                        {
                            viewModel.ErrorMessage = String.Format("There is already a {0} in the database.", viewModel.Location.LocationName);
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
                        oResponse = RedirectToAction("ViewLocations", "Location");
                    }
                    else
                    {
                        oResponse = View(viewModel);
                    }
                }
                else
                {
                    oResponse = View(viewModel);
                }
            }
            else
            {
                oResponse = RedirectToAction("Login", "User");
            }
            return oResponse;
        }

        [HttpGet]
        public ActionResult ViewLocations()
        {
            LocationVM newVM = new LocationVM();


            try
            {
                List<ILocationInfoDO> locationData = _LocationAccess.ViewAllLocations();
                newVM.LocationList = LocationMap.MapDOtoPO(locationData);

            }
            catch (Exception)
            {
                newVM.ErrorMessage = "There was an issue obtaining the list";

            }
            finally
            {
                ///nothing else needs to happen
            }

            return View(newVM);
        }

        [HttpGet]
        public ActionResult DeleteLocation(long LocationID)
        {
            ActionResult oResponse = null;
            LocationVM removeLocation = new LocationVM();
            if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
            {
                try
                {
                    _LocationAccess.DeleteLocation(LocationID);

                }
                catch (Exception)
                {
                    removeLocation.ErrorMessage = "There was a problem deleting the record from the database, please try again later";
                }
                finally
                {
                    //error is kicked to user, return must be outside this scope.
                }

                oResponse = RedirectToAction("ViewLocations", "Location");
               
            }
            else
            {
               oResponse = RedirectToAction("Login", "User");
                
            }

            return oResponse;
        }

        [HttpGet]
        public ActionResult UpdateLocation(long LocationID)
        {
            LocationVM updateVM = new LocationVM();
            ILocationInfoDO location = _LocationAccess.ViewLocationByID(LocationID);

            updateVM.Location = LocationMap.MapDOtoPO(location);

            return View(updateVM);
        }

        [HttpPost]
        public ActionResult UpdateLocation(LocationVM changelocation)
        {
            ActionResult oResult = null;

            if (ModelState.IsValid)
            {
                try
                {
                    ILocationInfoDO updateLoc = LocationMap.MapPOtoDO(changelocation.Location);
                    _LocationAccess.UpdateLocation(updateLoc);
                }
                catch (Exception e)
                {
                    //what to do with exception what to write

                    if (e.Message.Contains("duplicate"))
                    {
                        changelocation.ErrorMessage = "Unable to process request, duplicate record already exists";
                    }
                    else
                    {
                        changelocation.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                    }
                }
                finally
                {
                }

                if (changelocation.ErrorMessage == null)
                {
                    oResult = RedirectToAction("ViewLocations", "Location");
                }
                else
                {
                    oResult = RedirectToAction("UpdateLocation", "Location");
                }
            }
            else
            {
                oResult = View(changelocation);
            }
            return oResult;

        }
    }
}