using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DxDataGridExportingWithReports.Models
{
    public class SpParamModel
    {
        public string ParamName { get; set; }
        public SpParamType ParamType { get; set; }
        public string ParamValue { get; set; }
    }

    public enum SpParamType
    {
        stringtype = 0,
        numbertype = 1,
        datetype = 2
    }
}
