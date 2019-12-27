using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBBarcode;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

namespace WebBarcode.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        // GET: Barcode
        public ActionResult QRBarcode(string num)
        {
            Bitmap qrCodeImage2 = PostBarcode.PrintQRCode(num);

            MemoryStream ms = new MemoryStream();
            qrCodeImage2.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            ms.Position = 0;
            return File(ms, "image/gif");
        }
        // GET: Barcode
        public ActionResult Barcode(string num)
        {
            Bitmap qrCodeImage = PostBarcode.Print2of5Interleaved(num);
         
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms,System.Drawing.Imaging.ImageFormat.Gif);
            ms.Position = 0;
            return File(ms, "image/gif");
        }
        // GET: Barcode
        public ActionResult Postcode(string index)
        {
            Bitmap postCode = PostBarcode.PrintPostcode(index);
            MemoryStream ms = new MemoryStream();
            postCode.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            ms.Position = 0;
            return File(ms, "image/gif");
        }

        public ActionResult getpristav()
        {
            long curent = long.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["initalPRistav"]);
            curent++;
            System.Web.Configuration.WebConfigurationManager.AppSettings["initalPRistav"] = curent.ToString();
            /*if (curent % 8 == 0)
            {*/
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["initalPRistav"].Value = curent.ToString();
                config.Save();
            /*}*/
            return Content(curent.ToString());
        }

    }


}