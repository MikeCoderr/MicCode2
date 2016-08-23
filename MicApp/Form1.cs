using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.IE;


namespace MicApp
{
    public partial class Form1 : Form
    {
        static string con = "";
        static IWebDriver driver = null;
        static IWebElement element = null;
        static IWebElement finalElement = null;
        static WebDriverWait wait = null;
        static Int16 loopCount = 0;

        //Reading values from app.config file

        static string url = ConfigurationManager.AppSettings["URL"].ToString();
        static string uname = ConfigurationManager.AppSettings["userName"].ToString();
        static string pass = ConfigurationManager.AppSettings["passWord"].ToString();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                loopCount = Convert.ToInt16(textBox1.Text);
                if (loopCount.Equals(null) || loopCount.Equals("") || loopCount.Equals(0))
                {
                    MessageBox.Show("Please enter valid count!");
                    return;
                }
                this.WindowState = FormWindowState.Minimized;
                if (!isDriverExist())
                {
                    createDriver(); // Common funtion for driver
                    loginFuntion(); // Common funtion for login
                }
                if (isSessionExpire())
                {
                    loginFuntion(); // Common funtion for login
                }
                else
                {
                    gotoHomePage();
                    createTemplate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was some error." + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (driver == null)
            {
                MessageBox.Show("Good bye");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Good bye");
                Application.Exit();
                driver.Close();
                driver.Quit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            runFunding();

        }


        //Template creation funtion
        public static void createTemplate()
        {
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
            Thread.Sleep(15000);

            Actions a = new Actions(driver);
            for (int i = 0; i <= 100; i++)
            {
                try
                {
                    driver.SwitchTo().DefaultContent();
                    driver.SwitchTo().Frame(0);

                    finalElement = wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("manageClients"))));

                    if (finalElement != null)
                    {
                        Thread.Sleep(2000);
                        a.MoveToElement(driver.FindElement(By.XPath("//*[@id='manageClients']/a"))).Perform();
                        Thread.Sleep(2000);
                        break;

                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception ex)
                {

                    Thread.Sleep(1000);
                    finalElement = null;
                }

            }

            for (int i = 0; i <= 100; i++)
            {
                try
                {
                    a.MoveToElement(driver.FindElement(By.XPath("//*[@id='manageClients']/a"))).Perform();
                    Thread.Sleep(2000);
                    finalElement = driver.FindElement(By.XPath("//*[@id='clientFees']/a"));
                    if (finalElement != null)
                    {
                        Thread.Sleep(2000);
                        element = driver.FindElement(By.Id("clientFees"));
                        element.FindElements(By.TagName("a"))[0].Click();
                        Thread.Sleep(2000);
                        element.FindElements(By.TagName("a"))[0].Click();
                        Thread.Sleep(2000);
                        element.FindElements(By.TagName("a"))[0].Click();

                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {

                    Thread.Sleep(1000);
                    finalElement = null;
                }

            }

            for (int i = 0; i <= 100; i++)
            {
                try
                {

                    finalElement = driver.FindElement(By.Id("feeTemplate"));
                    if (finalElement != null)
                    {
                        element = driver.FindElement(By.Id("feeTemplate"));
                        element.FindElements(By.TagName("a"))[0].Click();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {

                    Thread.Sleep(1000);
                    finalElement = null;
                }
            }
            driver.SwitchTo().DefaultContent();
            Int16 x = Convert.ToInt16(driver.FindElements(By.TagName("frame")).Count());

            IList<IWebElement> elements = driver.FindElements(By.TagName("frame"));

            for (int i = 0; i <= x; i++)
            {
                String value = elements[i].GetAttribute("id");
                if (value == "content")
                {
                    driver.SwitchTo().Frame(i);
                    String name = driver.FindElement(By.Id("newTemplate")).GetAttribute("title");
                    break;
                }

            }


            String templates = "";
            for (int i = 1; i <= loopCount; i++)
            {
                Thread.Sleep(3000);
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("newTemplate")))).Click();
                String st = GenerateRandomAlphaNumericCode(10);
                Thread.Sleep(1000);
                //file template
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Name("templateName")))).SendKeys(st);
                //expand stock
                Thread.Sleep(2000);
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("pspan_STK")))).Click();

                Thread.Sleep(2000);
                //enter data
                driver.FindElement(By.Name("USD/STK_TICKET_CHARGE")).SendKeys("1");
                driver.FindElement(By.Name("USD/STK_MU_ABS")).SendKeys("0.07");
                //click on continue id
                Thread.Sleep(2000);
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("continueID")))).Click();
                Thread.Sleep(7000);
                wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("continueID")))).Click();
                templates = templates + "," + st;
            }

            MessageBox.Show("Total template created:" + loopCount + "Names were:" + templates);
        }

        //Create Funding main funtion
        public static void runFunding()
        {
            if (!isDriverExist())
            {
                createDriver(); // Common funtion for driver
                loginFuntion(); // Common funtion for login
            }
            else if (isSessionExpire())
            {
                loginFuntion(); // Common funtion for login
            }
            else
            {
                gotoHomePage();
                createFunding();
            }

        }

        //Go to Home page common funtion
        public static void gotoHomePage()
        {

        }

        //Creating funding called by Funding main funtion
        public static void createFunding()
        {
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.XPath("//html/body//div/ul//li[@id='funding']")))).Click();
            Thread.Sleep(10000);
            wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.XPath("//html/body//div/ul/li[@id='fundTrans']")))).Click();
            Thread.Sleep(10000);
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);
            driver.SwitchTo().Frame(2);
            SelectElement tranType = new SelectElement(driver.FindElement(By.Id("transaction")));
            tranType.SelectByText("Internal Transfers");
            Thread.Sleep(10000);
            SelectElement accNum = new SelectElement(driver.FindElement(By.Id("fromAcct")));
            accNum.SelectByText("I1641010");
            Thread.Sleep(10000);
            driver.FindElement(By.XPath("//*[@id='mainscreen']//table/tbody//tr[@id='amountRow']//td/input")).SendKeys("0.01");
            Thread.Sleep(10000);
            SelectElement currency = new SelectElement(driver.FindElement(By.Id("currency")));
            currency.SelectByText("RUB");
            Thread.Sleep(10000);
            SelectElement destAccNum = new SelectElement(driver.FindElement(By.Id("currency")));
            currency.SelectByText("F1845979");
            Thread.Sleep(10000);

        }

        //Verifying if driver exist
        public static Boolean isDriverExist()
        {

            if (driver == null) //Checking if driver already exist or not.
            {
                return false;
            }
            else
                return true;
        }

        //Verifying if session was expired
        public static Boolean isSessionExpire()
        {


            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);
            wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("home")))).Click();
            Thread.Sleep(10000);
            finalElement = wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("manageClients"))));
            if (finalElement == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Generating unique number
        public static string GenerateRandomAlphaNumericCode(int length)
        {
            string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomCode = new string(Enumerable.Repeat(characterSet, length).Select(set => set[random.Next(set.Length)]).ToArray());
            return randomCode;
        }

        //Creating driver
        public static void createDriver()
        {
            driver = new FirefoxDriver();
        }

        //Common login funtion
        public static void loginFuntion()
        {


            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Manage().Window.Maximize();
            driver.Url = url;
            driver.FindElement(By.Id("user_name")).SendKeys(uname);
            Thread.Sleep(100);
            driver.FindElement(By.Id("password")).SendKeys(pass);
            Thread.Sleep(100);
            driver.FindElement(By.Id("submitForm")).Click();
        }


    }
}
