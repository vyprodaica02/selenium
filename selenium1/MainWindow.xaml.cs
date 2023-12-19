using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Windows;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using SeleniumExtras.WaitHelpers;

namespace selenium1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private IWebElement WaitForElementClickable(ChromeDriver driver, By by, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element = null;

            for (int i = 0; i < timeoutInSeconds; i++)
            {
                try
                {
                    element = wait.Until(drv => drv.FindElement(by));
                    if (element != null && element.Enabled)
                    {
                        return element;
                    }
                }
                catch (StaleElementReferenceException) { }

                Thread.Sleep(1000); // Chờ 1 giây và thử lại
            }

            throw new TimeoutException($"Element {by} was not clickable within {timeoutInSeconds} seconds.");
        }

        private IWebElement WaitForElementClickables(ChromeDriver driver, By by, int timeoutInSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        private void button_click(object sender, RoutedEventArgs e)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-data-dir=C:\\Users\\Duc\\AppData\\Local\\Google\\Chrome\\User Data");
            options.AddArgument("--profile-directory=Default");
            ChromeDriver chrome = new ChromeDriver(options);
            chrome.Url = "https://www.tiktok.com/vi-VN/";

            // Chờ cho trang web hoàn tất tải
            WaitForPageToLoad(chrome);

            // Tìm đối tượng theo ô tìm kiếm
            var searchBox = WaitForElementClickable( chrome, By.Name("q"), 10);

            // Gửi ký tự tìm kiếm vào ô tìm kiếm trên YouTube
            searchBox.SendKeys("ntan21.02");

            // Nhấn Enter để thực hiện tìm kiếm
             searchBox.SendKeys(Keys.Enter);

            WaitForPageToLoad(chrome);

            var userAvatarLink = WaitForElementClickables(chrome, By.CssSelector("[data-e2e='search-user-avatar']"), 60);
            userAvatarLink.Click();

            // Chờ cho trang kết quả tìm kiếm hoàn tất tải
            WaitForPageToLoad(chrome);
        }

        private void WaitForPageToLoad(ChromeDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(drv => ((IJavaScriptExecutor)drv).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
