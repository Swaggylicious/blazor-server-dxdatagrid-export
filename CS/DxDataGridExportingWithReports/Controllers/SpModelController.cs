using DxDataGridExportingWithReports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DxDataGridExportingWithReports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpModelController : ControllerBase
    {
        public SpModelController(SpDBContext myContext, IConfiguration configuration)
        {
            MyContext = myContext;
            Configuration = configuration;
        }

        private SpDBContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpGet()]
        public IActionResult CustomGet()
        {
            try
            {
                string rtn = "No records found.";
                var sp = MyContext.SpModels.ToList();
                if (sp == null || sp.Count == 0)
                {
                    return Problem(rtn);
                }

                return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
        [HttpGet("{spname}")]
        public IActionResult CustomGet(string spname)
        {
            try
            {
                string rtn = "No records found.";
                var sp = MyContext.SpModels.Where<SpModel>(pp => pp.SpName == spname).FirstOrDefault();
                if (sp == null)
                {
                    return Problem(rtn);
                }
                sp.Details = MyContext.SpParamModels.Where<SpParamModel>(pp => pp.SpModel == sp).OrderBy(pp => pp.Seq).ToList();
                //string json = JsonConvert.SerializeObject(sp, Formatting.Indented,
                //                new JsonSerializerSettings
                //                {
                //                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                //                });
                return Ok(JsonConvert.SerializeObject(sp));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
    }
}
