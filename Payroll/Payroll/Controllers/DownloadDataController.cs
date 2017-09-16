using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Payroll.Helper.Biometric;
using Payroll.Models;
using Payroll.Models.DownloadData;
using Riss.Devices;
using Payroll.DataAccess.EntityFramework;

namespace Payroll.Controllers
{
    public class DownloadDataController : Controller
    {
        #region Fields
        public Device device;
        private DeviceConnection deviceConnection;
        private DeviceComEty deviceEty;
        #endregion
        public List<DownloadDataViewModel> ListDownloadData { get; set; } = new List<DownloadDataViewModel>();

        // GET: DownloadData
        public ActionResult Index() //this is one page bale localhost:portnum/DownloadData/Index
        {
            return View();
        }

        public ActionResult ConnectBiometric(ConnectViewModel model) //this is one page bale localhost:portnum/DownloadData/ConnectBiometric << pwde kani localhost:portnum/DownloadData/Index
        {
            bool x = false;
            try
            {

                device = new Device();
                device.DN = 1;
                device.Model = "A-C030";
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
                    x = true;
                }
                else
                {
                    TempData["msg"] = "<script>alert('Connection Failed');</script>";
                    x = false;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "<script>alert('ex');</script>";
            }
            
            if (x == true)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        public ActionResult DownloadDataBiometric()
        {
            //List<DownloadDataViewModel> list = new List<DownloadDataViewModel>();

            //------------------------------------------ BINUGO CODE
            device = new Device();
            device.DN = 1;
            device.Model = "A-C030";
            device.ConnectionModel = 5;

            device.IpAddress = "192.168.005.001";
            device.IpPort = 123;
            device.CommunicationType = CommunicationType.Tcp;
            deviceConnection = DeviceConnection.CreateConnection(ref device);
            System.Diagnostics.Debug.WriteLine(deviceConnection.Open());
            deviceEty = new DeviceComEty();
            deviceEty.Device = device;
            deviceEty.DeviceConnection = deviceConnection;
            //-----------------------------------------

            object extraProperty = new object();
            object extraData = new object();
            extraData = Global.DeviceBusy;
            try
            {

                List<DateTime> dtList = new List<DateTime>();
                bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);

                //------------------------------------------ BINUGO CODE
                dtList.Add(Convert.ToDateTime("08/01/2017") );
                dtList.Add(DateTime.Now);
                //-----------------------------------------

                //dtList.Add(model.TO.Date);
                //dtList.Add(model.TO.Date);
                extraProperty = false;
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.AttRecordsCount, extraProperty, ref device,
                    ref extraData);
                if (false == result)
                {
                    TempData["msg"] = "<script>alert('Download Failed');</script>";
                }

                int recordCount = (int)extraData;
                
                if (0 == recordCount)
                {//为0时说明没有新日志记录
                    ListDownloadData.Clear();
                }

                List<bool> boolList = new List<bool>();

                //----------------------------------------- BINUGO CODE
                boolList.Add(false);//所有日志
                boolList.Add(false);//清除新日志标记，false则不清除
                 //-----------------------------------------
                extraProperty = boolList;
                extraData = dtList;
                result = deviceConnection.GetProperty(DeviceProperty.AttRecords, extraProperty, ref device, ref extraData);
                if (result)
                {
                    int i = 1;
                    int y = 0;
                    List<Record> recordList = (List<Record>)extraData;
                    ListDownloadData.Clear();
                    foreach (Record record in recordList)
                    {
                        ListDownloadData.Add(new DownloadDataViewModel{SN = i.ToString(),
                                                           DN = record.DN.ToString(),
                                                           DIN = record.DIN.ToString(),
                                                           Type = ConvertObject.GLogType(record.Verify),
                                                           mode = ConvertObject.IOMode(record.Action),
                                                           Clock = record.Clock.ToString("yyyy-MM-dd HH:mm:ss") });
                        i++;

                        
                    }
                    Session["SessionName"] = ListDownloadData;
                }
                else
                {
                    TempData["msg"] = "<script>alert('Download Failed');</script>";
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "<script>alert('Download Failed');</script>";
            }
            

            return View("ConnectBiometric", ListDownloadData);
        }


        public ActionResult UploadData()
        {
            try
            {
                var ListName = (List<DownloadDataViewModel>)Session["SessionName"];
                PayrollContext db = new PayrollContext();
                foreach (var item in ListName)
                {
                    UploadData up = new UploadData();
                    up.Dn = Convert.ToInt32(item.DN);
                    up.Din = Convert.ToInt32(item.DIN);
                    up.Type = item.Type;
                    up.Mode = item.mode;
                    up.Clock = Convert.ToDateTime(item.Clock) ;
                    up.DateUpload = DateTime.Now;
                    db.UploadDatas.Add(up);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return View("ConnectBiometric");
        }
    }
}