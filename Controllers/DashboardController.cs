using MVC_Invoice_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVC_Invoice_Project.Controllers
{
    public class DashboardController : Controller
    {
        Invoice_DBEntities db;
        public DashboardController()
        { 
            db = new Invoice_DBEntities();
        }
        public ActionResult Index()
        {
            List<tbl_customers> lstcustomer = db.tbl_customers.ToList();
            ViewBag.customer= lstcustomer.Count();

            List<tbl_Products> lstproduct = db.tbl_Products.ToList();
            ViewBag.product= lstproduct.Count();

            List<tbl_invoice_details> lstinvoice = db.tbl_invoice_details.ToList();
            ViewBag.invoices= lstinvoice.Count();

            List<tbl_customers> lstc = db.tbl_customers.ToList();
            ViewData["Customers"]= lstc;

            List<tbl_Products> lstp = db.tbl_Products.ToList();
            ViewData["Products"]=lstp;

            return View();
        }
    }
}