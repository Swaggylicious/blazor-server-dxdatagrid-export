using Dapper;
using DxDataGridExportingWithReports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DxDataGridExportingWithReports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpReportController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        public SpReportController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpPost("{spname}")]
        public IActionResult CustomPost(string spname, [FromBody] SpParamModel[] json)
        {
            try
            {
                SpDBContext spdb = new SpDBContext();
                var sp = spdb.SpModels.Where<SpModel>(pp => pp.SpName == spname).FirstOrDefault();

                string rtn = "No records found.";
                string sql = string.Format("exec {0} ", spname);
                decimal decvalue = 0;
                DateTime datevalue = DateTime.Now;
                int paramcnt = 0;
                foreach (SpParamModel param in json)
                {
                    paramcnt++;
                    if (paramcnt > 1)
                    {
                        sql += ",";
                    }

                    switch (param.ParamType)
                    {
                        case SpParamType.stringtype:
                            break;
                        case SpParamType.numbertype:
                            if (!decimal.TryParse(param.ParamValue, out decvalue))
                            {
                                rtn = string.Format("{0} is not a number", param.ParamName);
                                return Problem(rtn);
                            }
                            break;
                        case SpParamType.datetype:
                            if (!DateTime.TryParse(param.ParamValue, out datevalue))
                            {
                                rtn = string.Format("{0} is not a date", param.ParamName);
                                return Problem(rtn);
                            }
                            break;
                    }
                    if (param.ParamType == SpParamType.numbertype)
                        sql += string.Format("{0}", param.ParamValue);
                    else
                        sql += string.Format("'{0}'", param.ParamValue);


                }


                using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("Default")))
                {
                    var list = conn.Query<dynamic>(sql).ToArray();
                    if (list.Length > 0)
                    {
                        //WriteLog("Log", portaluserid, _sapdoctype);
                        //string jsonstr = JsonConvert.SerializeObject(list);
                        //JsonToXML(jsonstr, _sapdoctype, true, "", "::List");
                        return Ok(list);
                    }
                }
                //temp = string.Format("{0} No record found.", spname);
                //WriteLog("Not Found", portaluserid, temp);
                return Problem(rtn);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message.Contains("Exception caught") ? ex.Message : ("Exception caught : " + ex.Message));
            }
        }
    }
}
