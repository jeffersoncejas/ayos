using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models.DownloadData
{
    public class DownloadDataViewModel
    {
        [Required]
        [Display(Name = "IpAddress")]
        public string IpAddress { get; set; }

        [Required]
        [Display(Name = "Port")]
        public string Port { get; set; }
    }
}