using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models.DownloadData
{
    public class DownloadDataViewModel
    {
        //public DateTime FROM { get; set; }
        //public DateTime TO { get; set; }

        public int SN { get; set; }
        public string Type { get; set; }
        public string mode { get; set; }
        public string DN { get; set; }
        public string DIN { get; set; }
        public string Clock { get; set; }
        //public ObservableCollection<DownloadDataViewModel> ListDownloadData { get; } = new ObservableCollection<DownloadDataViewModel>();
    }
}