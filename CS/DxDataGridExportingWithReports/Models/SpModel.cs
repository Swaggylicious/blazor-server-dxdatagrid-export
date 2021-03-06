﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DxDataGridExportingWithReports.Models
{
    public class SpModel
    {
        public SpModel()
        {
            this.Details = new HashSet<SpParamModel>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string SpName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string SpDesciption { get; set; }
        [Required]
        public string SpSql { get; set; }
        public ICollection<SpParamModel> Details { get; set; }

    }
    public class SpParamModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Seq { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string ParamName { get; set; }
        public SpParamType ParamType { get; set; }
        public string ParamValue { get; set; }
        [IgnoreDataMember]
        public virtual SpModel SpModel { get; set; }
    }

    public enum SpParamType
    {
        stringtype = 0,
        numbertype = 1,
        datetype = 2
    }
}
