using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DxDataGridExportingWithReports.Models
{
    public class SpDBContext : DbContext, ISpDBContext
    {
        /// <summary>
        /// constructor
        /// </summary>
        public SpDBContext()
        {
        }
        /// <summary>
        /// Inject options.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{ContactContext}"/>
        /// for the context
        /// </param>
        public SpDBContext(DbContextOptions<SpDBContext> options)
           : base(options)
        {
            Console.WriteLine($"{ContextId} context created.");
        }

        //private IConfiguration Configuration { get; }

        /// <summary>
        /// Magic string.
        /// </summary>
        public static readonly string RowVersion = nameof(RowVersion);

        /// <summary>
        /// Magic strings.
        /// </summary>
        public static readonly string ContactsDb = nameof(ContactsDb).ToLower();


        public DbSet<SpModel> SpModels { get; set; }
        public DbSet<SpParamModel> SpParamModels { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"));
        //}
    }
}
