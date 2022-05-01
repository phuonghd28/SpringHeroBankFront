using SpringHeroBankClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpringHeroBankClient.Controllers
{
    public class BankController : Controller
    {
        Service1Client service;
        public static Account account = new Account();

        public BankController()
        {
            service = new Service1Client();
        }
        // GET: Bank
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            var accountNew = new Account();
            accountNew.AccountNumber = account.AccountNumber;
            accountNew.SecurityCode = account.SecurityCode;
            accountNew.Balance = account.Balance;
            accountNew.Status = account.Status;
            var isRegister = service.Register(account);
            if(isRegister)
            {
                return RedirectToAction("Login"); 
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Login(int AccountNumber, string SecurityCode)
        {
            var token = service.Login(AccountNumber, SecurityCode);
            if(token != null)
            {
                Session["Token"] = token;
                return RedirectToAction("Details");
            }
            return View("Error");
           
        }

        public ActionResult Details()
        {
            var token = Session["Token"].ToString();
            account = service.FindAccount(token);
            return View(account);
        }

        public ActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(double Amount)
        {
            var token = Session["Token"].ToString();
            var deposit = service.Deposit(token, Amount);
            if (deposit == null)
            {
                return View("Error");
            }
            return RedirectToAction("Details");
          
        }

        public ActionResult Withdrawal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(double Amount)
        {
            var token = Session["Token"].ToString();
            var withdraw = service.WithDraw(token, Amount);
            if(withdraw == null)
            {
                return View("Error");
            }
            return RedirectToAction("Details");
        }

        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(int ReceiverAccountNumber, double Amount)
        {
            var token = Session["Token"].ToString();
            var transfer = service.Transfer(token, Amount, ReceiverAccountNumber);
            if ( transfer == null)
            {
                return View("Error");
            }
            else
            {
                return View("Details");
            }
        }
    }
}