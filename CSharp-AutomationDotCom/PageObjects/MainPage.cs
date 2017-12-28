using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows;

namespace AutomationDotCom.PageObjects
{
    class MainPage : Page
    {
        public static bool LoginSuccessful { get; internal set; }

        public static void Login(Credentials credentials)
        {
            LoginSuccessful = false;
            Tuple<string, string> loginInfo = GetCredentials(credentials);

            Driver.Navigate().GoToUrl("http://www.automationpractice.com");

            IWebElement LoginButton = Driver.FindElement(By.ClassName("login"), 5);
            LoginButton.Click();
            IWebElement UserField = Driver.FindElement(By.Id("email"), 4);
            IWebElement PasswordField = Driver.FindElement(By.Id("passwd"));


            UserField.SendKeys(loginInfo.Item1);
            PasswordField.SendKeys(loginInfo.Item2);

            IWebElement SubmitButton = Driver.FindElement(By.Id("SubmitLogin"));
            SubmitButton.Click();

            IWebElement UserElement = null;

            try
            {
                UserElement = Driver.FindElement(By.ClassName("account"));
                if (UserElement != null)
                    LoginSuccessful = true;
            }
            catch (NoSuchElementException)
            {
                LoginSuccessful = false;
            }
        }

        public static void LogOutIfPossible()
        {
            //Sign Out if signed in
            if (LoginSuccessful)
            {
                LogOut();
            }
        }
    }
}
