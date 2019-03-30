using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WebFramework.Filter;

namespace WebFramework.Api
{
    [ApiController]
    [ApiVersion(version: "1")]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")] // api/v1/[controller]
    public class CustomBaseController : ControllerBase
    {
    }
}
