using RMSExternalApi.Businesses;
using RMSExternalApi.Commons;
using RMSExternalApi.Models;
using RMSExternalApi.Models.RMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMSExternalApi.Controllers
{
    public class RMSConfigDataController : RMSAPIBaseController
    {


        /// <summary>
        /// Get eduction level list
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public CusResponse1<List<ConfigData>> GetEducationLevelLs()
        {
            return RMSConfigDataBusiness.Instance.GetEducationLevelLs();

        }

        /// <summary>
        /// Get factory list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<List<ConfigData>> GetFactoryLs()
        {
            return RMSConfigDataBusiness.Instance.GetFactoryLs();
        }





        /// <summary>
        /// Get gender list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public CusResponse1<List<ConfigData>> GetGenderLs()
        {
            return RMSConfigDataBusiness.Instance.GetGenderLs();

        }



    }
}
