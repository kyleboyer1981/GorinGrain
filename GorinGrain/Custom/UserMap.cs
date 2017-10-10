using GorinGrain.Models;
using GorinGrain_DAL.Interfaces;
using GorinGrain_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GorinGrain.Custom
{
    public class UserMap
    {
        //mapping from data class to presentation class so that we can use this object on different layers
        public static UserPO MapDOtoPO(IuserInfoDO iFrom)
        {
            //instantiate new Presentation object to fill from Data object
            UserPO newuser = new UserPO();

            newuser.UserID = iFrom.UserID;
            newuser.FirstName = iFrom.FirstName;
            newuser.LastName  = iFrom.LastName;
            newuser.UserName  = iFrom.UserName;
            newuser.Password  = iFrom.Password;
            newuser.UserLevel = iFrom.UserLevel;

            return newuser;
        }

        public static IuserInfoDO MapPOtoDO(UserPO iFrom)
        {
            IuserInfoDO newuser = new UserDO();

            newuser.UserID = iFrom.UserID;
            newuser.FirstName = iFrom.FirstName;
            newuser.LastName = iFrom.LastName;
            newuser.UserName = iFrom.UserName;
            newuser.Password = iFrom.Password;
            newuser.UserLevel = iFrom.UserLevel;

            return newuser;
        }

        public static List<UserPO> MapDOtoPO(List<IuserInfoDO> iInfo)
        {
            List<UserPO> vMapInfo = new List<UserPO>();
            foreach (IuserInfoDO info in iInfo)
            {
                UserPO map = MapDOtoPO(info);
                vMapInfo.Add(map);
            }


            return vMapInfo;
        }
    }
}