using MVC_Invoice_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Invoice_Project.Controllers
{
    public class ProductController : Controller
    {
        Invoice_DBEntities db;
        public ProductController()
        { 
            db = new Invoice_DBEntities();
        }
        public ActionResult Index()
        {
            tbl_Products p = new tbl_Products();
            ViewBag.Product = db.tbl_Products.ToList();
            return View(p);
        }
        [HttpPost]
        public ActionResult Index(tbl_Products tp)
        {
            db.tbl_Products.Add(tp);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.msg = "Data Added Successfully..!";
            tbl_Products p = new tbl_Products();
            ViewBag.Product = db.tbl_Products.ToList();
            return View(p);
        }
    }
}