using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationDotCom
{
    public class Page
    {
        public static IWebDriver Driver { get; set; }

        public static void Back()
        {
            Driver.Navigate().Back();
        }

        public static void Forward()
        {
            Driver.Navigate().Forward();
        }

        public static void LogOut()
        {
            IWebElement SignOutButton = Driver.FindElement(By.ClassName("logout"), 3);
            SignOutButton.Click();
        }

        public static Tuple<string, string> GetCredentials(Credentials credentials)
        {
            // TODO: Reading from a config file is the most flexible way to do this.

            string user = "";
            string password = "";
            switch (credentials)
            {
                case Credentials.GOOD:
                    user = "createarealaccount@automationpractice.com";
                    password = "password";
                    break;
                case Credentials.BAD:
                    user = "BLLOPO@BLOOP1.BLLOP";
                    password = "IMABADPASSWORD";
                    break;
                case Credentials.BAD_PASSWORD:
                    user = "createarealaccount@automationpractice.com";
                    password = "password";
                    break;
                case Credentials.BAD_EMAIL:
                    user = "BLLOPO@BLOOP1.BLLOP";
                    password = "password";
                    break;
                case Credentials.EMPTY:
                    //KEEP ORIGINAL
                    break;
                case Credentials.EMPTY_PASSWORD:
                    user = "createarealaccount@automationpractice.com";
                    //keep password
                    break;
                case Credentials.EMPTY_EMAIL:
                    //keep user
                    password = "password";
                    break;
                default:
                    break;
            }
            return Tuple.Create<string, string>(user, password);
        }

        public enum Credentials
        {
            GOOD,
            BAD,
            BAD_PASSWORD,
            BAD_EMAIL,
            EMPTY,
            EMPTY_PASSWORD,
            EMPTY_EMAIL
        }
    }
}