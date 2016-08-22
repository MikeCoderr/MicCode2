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
            driver = new InternetExplorerDriver();

            // driver = new FirefoxDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));



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
            string title = driver.Title;

           element = wait.Until<IWebElement>((d) =>
           {
               return d.FindElement(By.Id("manageClients"));
           });


            //wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("manageClients")));
                


            if (title.Equals("Accountant Management"))
                MessageBox.Show("Done");
            else
                MessageBox.Show("Not found");






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
