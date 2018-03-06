using HelloWorld.Dal;
using HelloWorld.Models;
using HelloWorld.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HelloWorld.Controllers
{
    public class CustomerBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string custCode = objContext.Request.Form["txtCustomerCode"];
            string custName = objContext.Request.Form["txtCustomerName"];
            Customer obj = new Customer()
            {
                CustomerCode = custCode,
                CustomerName = custName
            };
            return obj;
        }
    }

    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Load()
        {
            Customer objCustomer = new Customer {
                CustomerCode = "1001",
                CustomerName = "Shiv"
            };
            return View("Customer", objCustomer);
        }

        public JsonResult LoadJson()
        {
            Customer objCustomer = new Customer
            {
                CustomerCode = "1001",
                CustomerName = "Shiv"
            };
            return Json(objCustomer, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Enter()
        {
            CustomerViewModel obj = new CustomerViewModel();
            obj.customer = new Customer();
            //CustomerDal dal = new CustomerDal();
            //List<Customer> customersColl = dal.Customers.ToList<Customer>();
            //obj.customers = customersColl;
            //Thread.Sleep(10000);
            return View("EnterCustomer", obj);
        }

        public ActionResult GetCustomers()
        {
            CustomerDal dal = new CustomerDal();
            List<Customer> customersColl = dal.Customers.ToList<Customer>();
            Thread.Sleep(10000);
            return Json(customersColl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnterSearch()
        {
            CustomerViewModel objVM = new CustomerViewModel();
            objVM.customers = new List<Customer>();
            return View("SearchCustomer", objVM);
        }

        public ActionResult SearchCustomer()
        {
            string searchString = Request.Form["txtCustomerName"].ToString();
            CustomerDal dal = new CustomerDal();
            List<Customer> customersColl = (from x in dal.Customers
                                            where x.CustomerName == searchString
                                            select x).ToList<Customer>();

            CustomerViewModel objCustomerVM = new CustomerViewModel();
            objCustomerVM.customers = customersColl;
            return View("SearchCustomer", objCustomerVM);
        }

        public ActionResult Submit()
        {
            Customer obj = new Customer();
            obj.CustomerCode = Request.Form["Customer.CustomerCode"];
            obj.CustomerName = Request.Form["Customer.CustomerName"];
            CustomerViewModel objCustomerVM = new CustomerViewModel();
            objCustomerVM.customer = obj;
            if (ModelState.IsValid)
            {
                CustomerDal Dal = new CustomerDal();
                Dal.Customers.Add(objCustomerVM.customer);
                Dal.SaveChanges();
            }
            else
            {
                objCustomerVM.customer = obj;
            }
            CustomerDal dal = new CustomerDal();
            List<Customer> customersColl = dal.Customers.ToList<Customer>();
            //return View("EnterCustomer", objCustomerVM);
            return Json(customersColl, JsonRequestBehavior.AllowGet);
        }
    }
}