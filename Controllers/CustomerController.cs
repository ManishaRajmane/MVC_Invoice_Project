using MVC_Invoice_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Invoice_Project.Controllers
{
    public class CustomerController : Controller
    {
        Invoice_DBEntities db;
        public CustomerController()
        { 
            db = new Invoice_DBEntities();
        }
        public ActionResult Index()
        {
            tbl_customers t = new tbl_customers();
            ViewBag.customer = db.tbl_customers.ToList();
            return View(t);
        }
        [HttpPost]
        public ActionResult Index(tbl_customers tc)
        { 
            db.tbl_customers.Add(tc);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.msg = "Data Added Successfully...!";
            tbl_customers t = new tbl_customers();
            ViewBag.customer = db.tbl_customers.ToList();
            return View(t);
        }
    }
}