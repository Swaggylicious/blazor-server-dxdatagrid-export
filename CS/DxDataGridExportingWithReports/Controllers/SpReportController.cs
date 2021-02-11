using Dapper;
using DxDataGridExportingWithReports.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public SpReportController(IConfiguration configuration, SpDBContext spDBContext)
        {
            Configuration = configuration;
            MyContext = spDBContext;

        }
        private SpDBContext MyContext { get; set; }
        private IConfiguration Configuration { get; }

        [HttpPost("{spname}")]
        public IActionResult CustomPost(string spname, [FromBody] SpParamModel[] json)
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


                decimal decvalue = 0;
                DateTime datevalue = DateTime.Now;
                foreach (SpParamModel param in json)
                {
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
                    sp.Details.Where<SpParamModel>(pp => pp.ParamName == param.ParamName).FirstOrDefault().ParamValue = param.ParamValue;
                }

                string sql = sp.SpSql;
                foreach (SpParamModel param in sp.Details)
                {
                    if (sql.Contains("{" + param.ParamName + "}"))
                    {
                        if (param.ParamType == SpParamType.numbertype)
                        {
                            sql = sql.Replace("{" + param.ParamName + "}", param.ParamValue);
                        }
                        else
                        {
                            sql = sql.Replace("{" + param.ParamName + "}", $"'{param.ParamValue}'");
                        }

                        continue;
                    }
                    
                    if (param.ParamType == SpParamType.numbertype)
                    {
                        sql = sql.Replace("{" + param.Seq.ToString() + "}", param.ParamValue);
                    }
                    else
                    {
                        sql = sql.Replace("{" + param.Seq.ToString() + "}", $"'{param.ParamValue}'");
                    }
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
