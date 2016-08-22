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
        static IWebDriver driver;
        static IWebElement element = null;
        static IWebElement finalElement = null;
        static String value1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //driver = new InternetExplorerDriver();
            driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {

                driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30000));
                this.WindowState = FormWindowState.Minimized;
                Int16 loopCount = Convert.ToInt16(textBox1.Text);
                // con = conString();
                driver.Manage().Window.Maximize();
                driver.Url = "https://www.interactivebrokers.com.hk/sso/Login?SERVICE=AM.LOGIN";
                driver.FindElement(By.Id("user_name")).SendKeys("agora105");
                Thread.Sleep(100);
                driver.FindElement(By.Id("password")).SendKeys("98a4ZDf");
                Thread.Sleep(100);
                driver.FindElement(By.Id("submitForm")).Click();
                driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30000));

                Thread.Sleep(15000);

                Actions a = new Actions(driver);
                for (int i = 0; i <= 100; i++)
                {
                    try
                    {
                        driver.SwitchTo().DefaultContent();
                        driver.SwitchTo().Frame(0);


                        finalElement = driver.FindElement(By.Id("manageClients"));
                        if (finalElement != null)
                        {

                            //element = driver.FindElement(By.Id("manageClients"));

                            //a.MoveToElement(element.FindElements(By.TagName("adf"))[0]);
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




            catch (Exception ex)
            {
                MessageBox.Show("There was some error." + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Good bye");
            Application.Exit();
            driver.Close();
            driver.Quit();
        }
        public static string GenerateRandomAlphaNumericCode(int length)
        {
            string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string randomCode = new string(Enumerable.Repeat(characterSet, length).Select(set => set[random.Next(set.Length)]).ToArray());
            return randomCode;
        }
    }
}
