using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GorinGrain.ViewModels;
using GorinGrain_DAL;
using GorinGrain.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain_BLL.Interfaces;
using GorinGrain_BLL;
using GorinGrain.Models;
using GorinGrain.Interfaces;

namespace GorinGrain.Controllers
{
    public class ProducerController : Controller
    {
        ProducerDataAccess _ProducerAccess = new ProducerDataAccess();
        ShipmentDataAccess _ShipmentAccess = new ShipmentDataAccess();
        ShipmentLogicLayer _ShipmentLogic = new ShipmentLogicLayer();
        // GET: Producer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddProducer()
        {
            if (Session["UserName"] == null || (int)Session["UserLevel"] > 2)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                ProducerVM newVM = new ProducerVM();
                return View(newVM);
            }
        }

        [HttpPost]
        public ActionResult AddProducer(ProducerVM viewModel)
        {
            ActionResult oResponse = null;
            if (Session["UserName"] == null || (int)Session["UserLevel"] > 2)
            {
                oResponse = RedirectToAction("Login", "User");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IProducerInfoDO locationform = ProducerMap.MapPOtoDO(viewModel.Producer);
                        _ProducerAccess.InsertProducer(locationform);
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("duplicate"))
                        {
                            //if that Company name is already in the DB, push this message to user
                            viewModel.ErrorMessage = String.Format("There is already a {0} in the database.", viewModel.Producer.CompanyName);
                        }
                        else
                        {
                            //other errors thatn duplicate
                            viewModel.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                        }

                    }
                    finally
                    {
                        //nothing to do here
                    }
                    if (viewModel.ErrorMessage == null)
                    {
                        oResponse = RedirectToAction("ViewProducers", "Producer");
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
            return oResponse;


        }

        [HttpGet]
        public ActionResult ViewProducers()
        {
            //instantiate new viewmodel obj.
            ProducerVM newVM = new ProducerVM();

            //make sure user is authorized to add producer
            if (Session["UserName"] == null || (int)Session["UserLevel"] == 3 )
            {
                RedirectToAction("Login", "User");
            }
            else
            {

                //instantiated new viewmodel, fill below
                try
                {
                    //call to DAL for method, reader fills list in method
                    List<IProducerInfoDO> ProducerData = _ProducerAccess.ViewAllProducers();
                    //call to method from mapping class
                    newVM.ProducerList = ProducerMap.MapDOtoPO(ProducerData);


                    //get a fresh list of shipments
                    List<IShipmentInfoDO> mylist = _ShipmentAccess.ViewAllShipments();
                    //Map each from DO to a BO
                    List<IShipmentInfoBO> viewTop = ShipmentMap.MapDOtoBO(mylist);
                    //call to the business logic, pass the information.
                    long topProducerID = _ShipmentLogic.GetTopProducer(viewTop);
                    //turn id into an object by calling down to the database, map in line.
                    newVM.Producer = ProducerMap.MapDOtoPO(_ProducerAccess.ViewProducerByID(topProducerID));
                    //now top producer is in PO form, can display using producerVM


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


            return View(newVM);
        }

        [HttpGet]
        public ActionResult DeleteProducer(long ProducerID)
        {
            ProducerVM removeProducer = new ProducerVM();
            if (Session["UserName"] == null || (int)Session["UserLevel"] > 2)
            {
                RedirectToAction("Login", "User");
            }
            else
            {
                try
                {
                    //call to method from data layer to execute delete procedure
                    _ProducerAccess.DeleteProducer(ProducerID);

                }
                catch (Exception)
                {
                    removeProducer.ErrorMessage = "There was a problem deleting the record from the database, please try again later";
                }
                finally
                {
                    //error is kicked to user, return must be outside this scope.
                }

            }

            return RedirectToAction("ViewProducers", "Producer");
        }

        [HttpGet]
        public ActionResult UpdateProducer(long ProducerID)
        {
            ProducerVM updateVM = new ProducerVM();
            IProducerInfoDO producer = _ProducerAccess.ViewProducerByID(ProducerID);

            updateVM.Producer = ProducerMap.MapDOtoPO(producer);

            return View(updateVM);
        }

        [HttpPost]
        public ActionResult UpdateProducer(ProducerVM changeproducer)
        {
            ActionResult oResult = null;
            if (Session["UserName"] == null || ((int)Session["UserLevel"] > 2))
            {
                RedirectToAction("Login", "User");
            }
            else
            {
                //if the info was entered as required
                if (ModelState.IsValid)
                {
                    try
                    {
                        IProducerInfoDO updateProducer = ProducerMap.MapPOtoDO(changeproducer.Producer);
                        //call to producer data access method
                        _ProducerAccess.UpdateProducer(updateProducer);
                    }
                    catch (Exception e)
                    {
                        //what to do with exception, what to write

                        if (e.Message.Contains("duplicate"))
                        {
                            changeproducer.ErrorMessage = "Unable to process request, duplicate record already exists";
                        }
                        else
                        {
                            changeproducer.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                        }
                    }
                    finally
                    {
                    }

                    if (changeproducer.ErrorMessage == null)
                    {
                        //if no errors take them to producer list to see if their submission was added
                        oResult = RedirectToAction("ViewProducers", "Producer");
                    }
                    else
                    {
                        //if any errors caught then redirect to screen to change producer info, same as they came from 
                        oResult = View(changeproducer);
                    }
                }
                else
                {
                    oResult = View(changeproducer);
                }
            }
            return oResult;
        }
    }
}