using Microsoft.EntityFrameworkCore;

namespace DxDataGridExportingWithReports.Models
{
    public interface ISpDBContext
    {
        DbSet<SpModel> SpModels { get; set; }
        DbSet<SpParamModel> SpParamModels { get; set; }
    }
}