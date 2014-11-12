using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Webservice.Controllers
{
    /// <summary>
    /// Version number is used by clients to check whether API specfications have changed
    /// </summary>
    [RoutePrefix("version")]
    public class VersionController : ApiController
    {
        private const int VERSION_NUMBER = 2;

        /// <summary>
        /// Gets the latest version number
        /// </summary>
        /// <returns>The version number.</returns>
        [Route("")]
        [HttpGet]
        public int getVersion()
        {
            return VERSION_NUMBER;
        }
    }
}
