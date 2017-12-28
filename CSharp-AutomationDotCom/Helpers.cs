using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationDotCom
{
    public class Browser
    {
        public enum Type
        {
            CHROME,
            FIREFOX,
            IE,
            SAFARI
        }

        public static IWebDriver Driver { get; set; }
        public static Browser.Type TestBrowser { get; set; }

        public static void CloseAll()
        {
            Driver.Quit();
        }

        internal static IWebDriver LaunchBrowser(Type browser)
        {
            switch (browser)
            {
                case Type.CHROME:
                    Driver = new OpenQA.Selenium.Chrome.ChromeDriver();
                    break;
                case Type.FIREFOX:
                    Driver = new OpenQA.Selenium.Firefox.FirefoxDriver();
                    break;
                case Type.IE:
                    Driver = new OpenQA.Selenium.IE.InternetExplorerDriver();
                    break;
                case Type.SAFARI:
                    Driver = new OpenQA.Selenium.Safari.SafariDriver();
                    break;
                default:
                    throw new ApplicationException("No appropriate browser driver was provided.");
            }
            return Driver;
        }
    }

    public enum FindModifier
    {
        IS_VISIBLE
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds, FindModifier modifier)
        {
            switch (modifier) // For Future use cases
            {
                case FindModifier.IS_VISIBLE:
                    {
                        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                        return wait.Until(drv =>
                        {
                            IWebElement waitElem = drv.FindElement(by);
                            if (!waitElem.Displayed)
                                return null;
                            else
                                return waitElem;
                        });
                    }
                default:
                    {
                        // This should never happen...
                        throw new ApplicationException("WebDriverExtension FindElement is missing FindModifier.");
                    }
            }
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElements(by));
            }
            return driver.FindElements(by);
        }
    }
}
