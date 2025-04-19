using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Invoice_Project.Models;

namespace MVC_Invoice_Project.Controllers
{
    public class InvoiceController : Controller
    {
        Invoice_DBEntities db;
        public InvoiceController()
        { 
            db = new Invoice_DBEntities();
        }
        public ActionResult Index()
        {
            ViewBag.customer = new SelectList(db.tbl_customers, "Customer_id", "Customer_Name");
            ViewBag.Product = new SelectList(db.tbl_Products, "Product_Id", "Product_Name");
            return View();
        }
        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            tbl_Products p = db.tbl_Products.Find(id);

            tbl_Products pr = new tbl_Products() 
            {
              Product_Id=p.Product_Id,
              Product_Name=p.Product_Name,
              GST=p.GST,
              Rate=p.Rate,
              Stock_Quantity=p.Stock_Quantity
            };

            return Json(pr, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string GenerateInvoice(tbl_invoice_details d)
        {
            db.tbl_invoice_details.Add(d);
            db.SaveChanges();
            return "Invoice Generated Successfully......";
        }
        public ActionResult AllInvoices()
        {
            List<InvoiceModel>lst= GetAllInvoices();
            return View(lst);
        }
        public List<InvoiceModel> GetAllInvoices()
        { 
            List<InvoiceModel> lst = new List<InvoiceModel>();

            foreach (tbl_invoice_details d in db.tbl_invoice_details.ToList())
            {
                tbl_customers c = db.tbl_customers.Find(d.Customer_Id);

                float total_amount = (float)d.Total_Amount;
                float paid_amount = 0,remaining_amount = 0;
                string status = "";

                List<tbl_invoice_payments> payments = db.tbl_invoice_payments.ToList().Where(e=>e.Invoice_Id.Equals(d.Invoice_Id)).ToList();

                if (payments != null)
                {
                    paid_amount = (float)payments.Sum(e => e.Payment_Amount);
                }
                remaining_amount = total_amount - paid_amount;


                if (paid_amount == 0)
                {
                    status = "UnPaid";
                }
                else if (paid_amount > 0 && paid_amount < total_amount)
                {
                    status = "Partial Paid";
                }
                else
                {
                    status = "Paid";
                }

                ViewBag.amount = remaining_amount;

                InvoiceModel m = new InvoiceModel()
                {
                    invoice_id = d.Invoice_Id,
                    customer_id = (int)d.Customer_Id,
                    customer_name = c.Customer_Name,
                    invoice_date = (DateTime)d.Invoice_Date,
                    total_amount =total_amount,
                    paid_amount=paid_amount,
                    remaining_amount=remaining_amount,
                    status=status
                };
                lst.Add(m);
            }

            return lst;
        }
        public ActionResult PayNow(int id)
        {
            InvoiceModel m = GetAllInvoices().FirstOrDefault(e => e.invoice_id.Equals(id));

            tbl_invoice_payments p = new tbl_invoice_payments()
            {
                Invoice_Id = id,
                Payment_Amount = m.remaining_amount
            };

            ViewBag.invoice = m;

            return View(p);
        }
        [HttpPost]
        public ActionResult PayNow(tbl_invoice_payments p)
        { 
            db.tbl_invoice_payments.Add(p);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.msg = "Payment Done Successfully....!";
            return RedirectToAction("AllInvoices");
        }
        public ActionResult ViewInvoice(int id)
        {
            tbl_invoice_details d = db.tbl_invoice_details.Find(id);

            InvoiceModel m = GetAllInvoices().FirstOrDefault(e => e.invoice_id.Equals(id));
            ViewBag.invoice = m;

            return View(d);
        }
    }   
}