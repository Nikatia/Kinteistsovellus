using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kiinteistosovellus.Controllers
{
    public class ImagesController : BaseController
    {


        ImageService imageService = new ImageService();


        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                var imageUrl = await imageService.UploadImageAsync(photo);
                TempData["LatestImage"] = imageUrl.ToString();
                return RedirectToAction("LatestImage");
            }
            else
            {
                ViewBag.error = "Valitse kuva!";
                return View();
            }
        }

        public ActionResult LatestImage()
        {
            var latestImage = string.Empty;
            if (TempData["LatestImage"] != null)
            {
                ViewBag.LatestImage = Convert.ToString(TempData["LatestImage"]);
            }

            return View();
        }
    }
}
