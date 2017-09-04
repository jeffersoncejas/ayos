using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Helper.Biometric;
using Payroll.Models.DownloadData;
using Riss.Devices;

namespace Payroll.Controllers
{
    public class DownloadDataController : Controller
    {
        #region Fields
        private Device device;
        private DeviceConnection deviceConnection;
        private DeviceComEty deviceEty;
        #endregion

        // GET: DownloadData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConnectBiometric(DownloadDataViewModel model)
        {
            device = new Device();
            device.DN = 1;
            device.Model = "A-C071";
            device.ConnectionModel = 5;

            device.IpAddress = model.IpAddress;
            device.IpPort = int.Parse(model.Port);
            device.CommunicationType = CommunicationType.Tcp;
            deviceConnection = DeviceConnection.CreateConnection(ref device);
            System.Diagnostics.Debug.WriteLine(deviceConnection.Open());

            if (deviceConnection.Open() > 0)
            {
                deviceEty = new DeviceComEty();
                deviceEty.Device = device;
                deviceEty.DeviceConnection = deviceConnection;
            }
            else
            {
            }


            return View();
        }
    }
}