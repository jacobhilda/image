using image_02.lib;
using image_02.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace image_02.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;

        public HomeController(IConfiguration iconfig)
        {
            configuration = iconfig;
        }
        

        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            try
            {
                /*Configura Sku*/
                Sku _sku = SkuMerget.getSku(id);
                /*Get config Product*/
                Img _img = new Img();
                _img.X = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":x").Value);
                _img.Y = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":y").Value);
                _img.W = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":w").Value);
                _img.H = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":h").Value);
                _img.T = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":t").Value);
                _img.Wr = _sku.Width;

                /*Add Image*/
                List<string> list = new List<string>();
                string s3 = configuration.GetSection("ASW:S3").Value;
                list.Add(s3 + "base/" + _sku.Base + ".jpg");
                list.Add(s3 + "arte/" + _sku.Art + ".png");

                /*Create new Image*/
                Bitmap bitmat = ImageMerger.MergeImages(list, _img);
                MemoryStream ms = new MemoryStream();
                bitmat.Save(ms, ImageFormat.Jpeg);
                /*Export image*/
                return File(ms.ToArray(), "image/jpg");
            }
            catch (Exception ex){
                return Json(ex);
            }


           
        }
        //[HttpGet("{id}")]
        //public IActionResult Index_(string id)
        //{
        //    /*Configura Sku*/
        //    Sku _sku = SkuMerget.getSku(id);
        //    /*Get config Product*/
        //    Img _img = new Img();
        //    _img.X = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":x").Value);
        //    _img.Y = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":y").Value);
        //    _img.W = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":w").Value);
        //    _img.H = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":h").Value);
        //    _img.T = Convert.ToInt32(configuration.GetSection(_sku.Type + ":" + _sku.Product + ":t").Value);
        //    _img.Wr = _sku.Width;

        //    /*Add Image*/
        //    List<string> list = new List<string>();
        //    //string s3 = configuration.GetSection("ASW:S3").Value;
        //    //list.Add(s3 + "base/" + _sku.Base + ".jpg");
        //    //list.Add(s3 + "arte/" + _sku.Art + ".png");
        //    list.Add(@"Imagen\base\" + _sku.Base + ".jpg");
        //    list.Add(@"Imagen\art\" + _sku.Art + ".png");
        //    /*Create new Image*/
        //    Bitmap bitmat = ImageMerger.MergeImages(list, _img);
        //    MemoryStream ms = new MemoryStream();
        //    bitmat.Save(ms, ImageFormat.Jpeg);
        //    /*Export image*/
        //    return File(ms.ToArray(), "image/jpg");
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}