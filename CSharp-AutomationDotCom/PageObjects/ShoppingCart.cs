using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationDotCom.PageObjects
{
    class ShoppingCart : Page
    {
        internal static void AddAllProducts()
        {
            IWebElement ContinueShoppingButton;
            IReadOnlyCollection<IWebElement> AddToCartButtonCollection = Driver.FindElements(By.CssSelector("[title=\"Add to cart\"]"), 3);
            Actions actions = new Actions(Driver);

            foreach (IWebElement AddToCartButton in AddToCartButtonCollection)
            {
                if (!AddToCartButton.Displayed)
                    continue;

                actions.MoveToElement(AddToCartButton);
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
                IWebElement NewAddToCartButton = wait.Until(ExpectedConditions.ElementToBeClickable(AddToCartButton));
                NewAddToCartButton.Click();
                ContinueShoppingButton = Driver.FindElement(By.ClassName("continue"), 2, FindModifier.IS_VISIBLE);
                ContinueShoppingButton.Click();
                Thread.Sleep(300);
            }
        }

        public enum Products
        {
            ALL_PRINTED_DRESS,
            ALL_PRINTED_SUMMER_DRESS,
            FADED_T_SHIRT,
            BLOUSE,
            ALL
        }

        public static int GetCountInCart()
        {
            IWebElement CartCount = Driver.FindElement(By.CssSelector("span.ajax_cart_quantity"), 2);

            int count; //doing it this way because VS2015/C#7 Issues

            Int32.TryParse(CartCount.Text, out count);

            return count;
        }

        internal static void AddProducts(Products products)
        {
            Driver.Navigate().GoToUrl("http://www.automationpractice.com");

            switch (products)
            {
                case Products.ALL:
                    AddAllProducts();
                    break;
                default:
                    AddSpecificProducts(products);
                    break;
            }
        }

        public static void ClearCart()
        {
            LogOut();
        }

        private static void AddSpecificProducts(Products products)
        {
            List<string> ProductNames = GetProductNames(products);
            Actions actions = new Actions(Driver);
            IWebElement AddToCartButton;
            IWebElement ContinueShoppingButton;
            
            ReadOnlyCollection<IWebElement> PotentialProducts = Driver.FindElements(By.ClassName("product-container"), 5);

            foreach (IWebElement product in PotentialProducts)
            {
                if (!product.Displayed)
                    continue;

                IWebElement NameElement = product.FindElement(By.ClassName("product-name"));

                if(ProductNames.Contains(NameElement.Text)) // Ideally, I'd compare a product SKU or UPC# to avoid changes to product names.
                {
                    actions.MoveToElement(product);
                    AddToCartButton = product.FindElement(By.CssSelector("[title=\"Add to cart\"]"));
                    if (!AddToCartButton.Displayed)
                        continue;

                    actions.MoveToElement(AddToCartButton);
                    Thread.Sleep(250);
                    AddToCartButton.Click();
                    ContinueShoppingButton = Driver.FindElement(By.ClassName("continue"), 2, FindModifier.IS_VISIBLE);
                    ContinueShoppingButton.Click();
                }
            }
        }

        internal static object GetTotal()
        {
            IWebElement TotalPriceElement = Driver.FindElement(By.CssSelector(".price.cart_block_total"));
            IWebElement ShippingCostElement = Driver.FindElement(By.CssSelector(".price.cart_block_shipping_cost"));

            decimal total = 0;
            decimal shippingCost = 0;

            decimal.TryParse(TotalPriceElement.GetAttribute("textContent").Replace("$",""), out total);
            decimal.TryParse(ShippingCostElement.GetAttribute("textContent").Replace("$", ""), out shippingCost);

            return Math.Ceiling(total);
        }

        public static decimal GetSumOfCart()
        {
            decimal total = 0;

            ReadOnlyCollection<IWebElement> PriceElement = Driver.FindElements(By.CssSelector(".shopping_cart .products .price"));

            foreach (IWebElement product in PriceElement)   
            {
                decimal price; 
                decimal.TryParse(product.GetAttribute("textContent").Replace("$", ""), out price);
                total += price;
            }

            return Math.Ceiling(total);
        }

        private static List<string> GetProductNames(Products products)
        {
            List<string> ProductList = new List<string>();
            // TODO: Pull this from an Excel sheet instead of hard coding.
            switch (products)
            {
                case Products.ALL_PRINTED_DRESS:
                    ProductList.Add("Printed Dress");
                    ProductList.Add("Printed Dress");
                    break;
                case Products.ALL_PRINTED_SUMMER_DRESS:
                    ProductList.Add("Printed Summer Dress");
                    ProductList.Add("Printed Summer Dress");
                    break;
                case Products.FADED_T_SHIRT:
                    ProductList.Add("Faded Short Sleeve T-shirts");
                    break;
                case Products.BLOUSE:
                    ProductList.Add("Blouse");
                    break;
                default:
                    throw new ApplicationException("GetProductNames: 'Products' enum is not valid for this method.");
            }
            return ProductList;
        }

        public static bool CompareProductsInCartTo(Products products)
        {
            List<string> ProductNames = GetProductNames(products);
            List<string> ProductsInCart = GetProductsInCart(products);
            return !ProductsInCart.Except(ProductNames).Any() && !ProductNames.Except(ProductsInCart).Any();
        }

        private static List<string> GetProductsInCart(Products products)
        {
            List<string> ProductsInCart = new List<string>();

            ReadOnlyCollection<IWebElement> PotentialProducts = Driver.FindElements(By.ClassName("cart_block_product_name"));

            foreach (IWebElement product in PotentialProducts)
                ProductsInCart.Add(product.GetAttribute("title")); // I'd prefer to use textContent here, but the page snips the text and uses ellipses.

            return ProductsInCart;
        }
    }
}
