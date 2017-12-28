using NUnit.Framework;
using AutomationDotCom.PageObjects;

namespace AutomationDotCom.Tests
{
    [SetUpFixture]
    class TestConfigAndTeardown
    {
        [OneTimeSetUp]
        public void InitialSetup()
        {
            Page.Driver = Browser.LaunchBrowser(Browser.Type.CHROME);
        }

        [OneTimeTearDown]
        public void FinalTearDown()
        {
            Page.Driver.Quit();
        }
    }

    [TestFixture]
    class HomePage_LoginTests
    {
        [TearDown]
        public void TearDown()
        {
            MainPage.LogOutIfPossible();
        }

        [Test]
        public void Login_With_Good_Credentials_Should_Pass()
        {
            MainPage.Login(MainPage.Credentials.GOOD);
            Assert.IsTrue(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_Bad_Credentials_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.BAD);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_Bad_Password_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.BAD_PASSWORD);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_Bad_Email_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.BAD_EMAIL);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_No_Password_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.EMPTY_PASSWORD);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_No_Email_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.EMPTY_EMAIL);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }

        [Test]
        public void Login_With_No_Credentials_Should_Fail()
        {
            MainPage.Login(MainPage.Credentials.EMPTY);
            Assert.IsFalse(MainPage.LoginSuccessful);
        }
    }

    [TestFixture]
    class ShoppingCartTests
    {
        [SetUp]
        public void ShoppingCartSetUp()
        {
            MainPage.Login(Page.Credentials.GOOD);
        }

        [TearDown]
        public void ShoppingCartTearDown()
        {
            ShoppingCart.ClearCart();
        }

        [Test]
        public void Add_All_Products_To_Shopping_Cart()
        {
            ShoppingCart.AddProducts(ShoppingCart.Products.ALL);
            Assert.AreEqual(7, ShoppingCart.GetCountInCart());
        }

        [Test]
        public void Add_Blouse_To_Shopping_Cart()
        {
            ShoppingCart.AddProducts(ShoppingCart.Products.BLOUSE);
            Assert.IsTrue(ShoppingCart.CompareProductsInCartTo(ShoppingCart.Products.BLOUSE));
        }

        [Test]
        public void Add_All_Printed_Dresses_To_Shopping_Cart()
        {
            ShoppingCart.AddProducts(ShoppingCart.Products.ALL_PRINTED_DRESS);
            Assert.IsTrue(ShoppingCart.CompareProductsInCartTo(ShoppingCart.Products.ALL_PRINTED_DRESS));
        }

        [Test]
        public void Cart_Prices_Add_Up_To_Total_Price()
        {
            ShoppingCart.AddProducts(ShoppingCart.Products.ALL);
            Assert.AreEqual(ShoppingCart.GetSumOfCart(), ShoppingCart.GetTotal());
        }
    }
}
