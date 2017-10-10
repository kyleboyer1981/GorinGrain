using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GorinGrain_DAL;
using GorinGrain.ViewModels;
using GorinGrain_DAL.Models;
using GorinGrain.Custom;
using GorinGrain_DAL.Interfaces;
using GorinGrain.Models;
using System.Web;

namespace GorinGrain.Controllers
{
    public class UserController : Controller
    {
        private UserDataAccess _UserAccess = new UserDataAccess();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            UserVM VM = new UserVM();

            HttpCookie cookie = Request.Cookies["UserInfo"];

            if (TempData.ContainsKey("UserName") && TempData.ContainsKey("Password"))
            {
                VM.User.UserName = TempData["UserName"].ToString();
                VM.User.Password = TempData["Password"].ToString();
            }
            else
            {
                if (cookie != null)
                {
                    VM.User.UserName = cookie["UserName"];
                    VM.User.Password = cookie["Password"];
                    VM.RememberMe = true;
                }
                else
                {
                    //blank username and blank password fields
                }
            }

            return View(VM);
        }

        [HttpPost]
        public ActionResult Login(UserVM iForm)
        {
            ActionResult oResponse = null;
            //if data annotation requirements are met
            if (ModelState.IsValid)
            {
                //check db to see if user input matches that user db info
                UserPO user = UserMap.MapDOtoPO(_UserAccess.ViewUserByUserName(iForm.User.UserName));

                if (user != null && iForm.User.Password.Equals(user.Password))
                {
                    //set session for current user
                    Session["UserName"] = user.UserName;
                    Session["UserLevel"] = user.UserLevel;
                    Session.Timeout = 10;

                    if (iForm.RememberMe == true)
                    {
                        //save cookie with user info IF they checked the "Remember me" box on log in page
                        HttpCookie cookie = new HttpCookie("UserInfo");
                        cookie["UserName"] = user.UserName;
                        cookie["Password"] = user.Password;
                        Response.Cookies.Add(cookie);
                    }

                    else
                    {
                        //if box is not checked, do not keep cookie
                        Response.Cookies["UserInfo"].Expires = DateTime.Now.AddDays(-1);
                    }

                    oResponse = RedirectToAction("Index", "Home");
                }
                else
                {
                    //if password is incorrect, return to screen and gice error message
                    ModelState.AddModelError("User.Password", "The Login Info is Invalid");
                    oResponse = View(iForm);
                }
            }
            else
            {
                oResponse = View(iForm);
            }

            return oResponse;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            UserVM newVM = new UserVM();
            return View(newVM);
        }

        [HttpPost]
        public ActionResult AddUser(UserVM viewModel)
        {
            ActionResult oResponse = null;
            if (ModelState.IsValid)
            {
                try
                {
                    IuserInfoDO userform = UserMap.MapPOtoDO(viewModel.User);
                    //call to User DAL for method
                    _UserAccess.InsertUser(userform);

                    //set tempdata to hold registered info for login screen
                    TempData["UserName"] = userform.UserName;
                    TempData["Password"] = userform.Password;



                }
                catch (Exception e)
                {
                    if (e.Message.Contains("duplicate"))
                    {
                        //if duplicate user name is found, give user this msg
                        viewModel.ErrorMessage = String.Format("There is already a {0} in the database.", viewModel.User.UserName);
                    }
                    else
                    {
                        //all other errors return this msg to user
                        viewModel.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                    }
                }
                finally
                {
                    //nothing to do here
                }

                if (viewModel.ErrorMessage == null)
                {
                    oResponse = RedirectToAction("Login", "User");
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

            return oResponse;


        }

        [HttpGet]
        public ActionResult ViewUsers()
        {
            UserVM newVM = new UserVM();

            if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
            {
                try
                {
                    //create new list
                    List<IuserInfoDO> userData = _UserAccess.ViewAllUsers();
                    //map info from data to presentation obj
                    newVM.UserList = UserMap.MapDOtoPO(userData);

                }
                catch (Exception)
                {
                    //pass error to user here in presentation layer, error has already been logged
                    newVM.ErrorMessage = "There was an issue obtaining the list";

                }
                finally
                {
                    ///nothing else needs to happen
                }

                return View(newVM);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public ActionResult DeleteUser(int UserID)
        {
            UserVM removeUser = new UserVM();

            //data call to pull up user info and get that UserLevel
            UserPO deleteuser = UserMap.MapDOtoPO(_UserAccess.ViewUserByID(UserID));

            //if user is not an admin
            if (deleteuser.UserLevel != 1)
            {

                //session check to make sure address isnt just typed in
                if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
                {
                    try
                    {
                        //call to method from data layer to execute delete procedure
                        _UserAccess.DeleteUser(UserID);

                    }
                    catch (Exception)
                    {
                        removeUser.ErrorMessage = "There was a problem deleting the record from the database, please try again later";
                    }
                    finally
                    {
                        //error is kicked to user, return must be outside this scope.
                    }

                    //take user back to list of users to see if delete went through
                    return RedirectToAction("ViewUsers", "User");
                }
                else
                {
                    
                    return RedirectToAction("Login", "User");
                }
            }

            else
            {
                //set temp data with message to return to View Users screen
                TempData["ErrorMessage"] = "You are not authorized to delete an Admin!";
                return RedirectToAction("ViewUsers", "User");
            }






        }

        [HttpGet]
        public ActionResult UpdateUser(int UserID)
        {
            if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
            {
                UserVM updateVM = new UserVM();
                IuserInfoDO user = _UserAccess.ViewUserByID(UserID);

                updateVM.User = UserMap.MapDOtoPO(user);

                return View(updateVM);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(UserVM changeUser)
        {
            ActionResult oResult = null;
            if (Session["UserName"] != null && (int)Session["UserLevel"] == 1)
            {

                //if the info was entered as required
                if (ModelState.IsValid)
                {
                    try
                    {
                        IuserInfoDO updateUser = UserMap.MapPOtoDO(changeUser.User);
                        //call to producer data access method
                        _UserAccess.UpdateUser(updateUser);
                    }
                    catch (Exception e)
                    {
                        //what to do with exception, what to write

                        if (e.Message.Contains("duplicate"))
                        {
                            changeUser.ErrorMessage = "Unable to process request, duplicate record already exists";
                        }
                        else
                        {
                            changeUser.ErrorMessage = "We apologize but we were unable to handle your request at this time.";
                        }
                    }
                    finally
                    {
                    }

                    if (changeUser.ErrorMessage == null)
                    {
                        //if no errors take them to producer list to see if their submission was added
                        oResult = RedirectToAction("ViewUsers", "User");
                    }
                    else
                    {
                        //if any errors caught then redirect to screen to change producer info, same as they came from 
                        oResult = View(changeUser);
                    }
                }
                else
                {
                    oResult = View(changeUser);
                }

            }
            else
            {
                oResult = RedirectToAction("Login", "User");
            }

            return oResult;
        }

    }
}
