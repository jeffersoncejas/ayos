using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Riss.Devices;

namespace Payroll.Helper.Biometric
{
    public class DeviceComEty
    {
        public DeviceComEty() { }

        public DeviceConnection DeviceConnection { get; set; }

        public Device Device { get; set; }
    }
}