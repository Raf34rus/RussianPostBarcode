using RBBarcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBarcodeOld.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        // GET: Barcode
        public ActionResult Barcode(string num)
        {
            Bitmap qrCodeImage = PostBarcode.Print2of5Interleaved(num);

            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
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
    }

}