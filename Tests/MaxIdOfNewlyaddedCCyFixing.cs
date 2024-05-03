﻿using AventStack.ExtentReports;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SwaggerWebAPI.Libs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SwaggerWebAPI
{
    class MaxIdOfNewlyaddedCCyFixing
    {
        IWebDriver Driver;
        ExcelAPI InputExcelAPI, ValidationExcelAPI, OutputExcelAPI;
        ExtentReports ExtReport;
        ExtentTest test;
        WebDriverWait WebDriverWait;
        string username = User.getEncodedUserName();
        SqlConnection con = DBConnection.GetConnection();

        static string lattise_url = "//*[@id='DatabaseTopic_DatabaseTopic_GetMaxIdofNewlyAddedCCyFixingIndexs']/div[1]/h3/span[2]/a",
                         lattise_Try = "//*[@id='DatabaseTopic_DatabaseTopic_GetMaxIdofNewlyAddedCCyFixingIndexs_content']/form/div[2]/input",
                         lattise_code = "//*[@id='DatabaseTopic_DatabaseTopic_GetMaxIdofNewlyAddedCCyFixingIndexs_content']/div[2]/div[4]/pre",
                         userName = "//*[@id='DatabaseTopic_DatabaseTopic_GetMaxIdofNewlyAddedCCyFixingIndexs_content']/form/table/tbody/tr/td/input[@name='Username']",
                        lattise_body = "//*[@id='DatabaseTopic_DatabaseTopic_GetMaxIdofNewlyAddedCCyFixingIndexs_content']/div[2]/div[3]/pre/code";


        public MaxIdOfNewlyaddedCCyFixing(IWebDriver driver, Dictionary<string, ExcelAPI> ExcelApiList, ExtentReports ExtReport)
        {
            this.Driver = driver;
            this.InputExcelAPI = ExcelApiList["InputExcelApi"];
            this.ValidationExcelAPI = ExcelApiList["ValidationExcelApi"];
            this.OutputExcelAPI = ExcelApiList["OutputExcelApi"];
            this.ExtReport = ExtReport;
            this.WebDriverWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
        }

        IWebElement url_lattise => Driver.FindElement(By.XPath(lattise_url));
        IWebElement button_lattise => Driver.FindElement(By.XPath(lattise_Try));
        IWebElement body_lattise => Driver.FindElement(By.XPath(lattise_body));
        IWebElement un => Driver.FindElement(By.XPath(userName));
        IWebElement res_Code => Driver.FindElement(By.XPath(lattise_code));


        public void GetMaxIdOfNewlyaddedCCyFixing()
        {

            test = ExtReport.CreateTest("Get_NewlyaddedCCyFixing").Info("Test Started");

            //SqlCommand cmd = new SqlCommand("select max(Id), code from CcyPair", con);
            SqlCommand cmd = new SqlCommand("select * from CcyPair where Id=(select max(Id) from CcyPair)", con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string maxId;
            string code;

            reader.Read();
            
            
            maxId = Convert.ToString(reader[0]);
            code = Convert.ToString(reader[1]);

            reader.Close();
            con.Close();

            /*string path = @"D:\Users\j_gamage\source\repos\Swagger\Swagger\Test.xlsx";

            XSSFWorkbook wb = new XSSFWorkbook(File.Open(path, FileMode.Open));

            var sheet = wb.GetSheetAt(5);
            var row = sheet.GetRow(61);
            var value = row.GetCell(3).NumericCellValue.ToString();*/
            //var val2 = row.GetCell(4).StringCellValue;

            WebDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(lattise_url)));

            url_lattise.Click();
            test.Log(Status.Info, "GetNewlyaddedCCyFixing selected");

            WebDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(userName)));

            string input = string.Format(username);
            un.SendKeys(input);

            WebDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(lattise_Try)));

            button_lattise.Click();
            test.Log(Status.Info, "Try it Now Button Clicked");


            WebDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(lattise_body)));

            Actions action = new Actions(Driver);
            action.MoveToElement(body_lattise);
            action.Perform();

            string resBody = body_lattise.Text;
            string resCode = res_Code.Text;

            test.Log(Status.Info, "Verifying Value....");


            if (resCode == "200")
            {
                test.Log(Status.Pass, "GetNewlyaddedCCyFixing Response is " + resCode);

                if (resBody.Contains(maxId))
                {
                    test.Log(Status.Pass, "GetNewlyaddedCCyFixing Response body contains Latest ID " + maxId+" & last added CcyPair "+code );
                    test.Log(Status.Pass, "Test is Pass");
                }
                else
                {
                    test.Log(Status.Fail, "GetNewlyaddedCCyFixing Response is Failed! It contains "+resBody);
                    Assert.Fail("GetNewlyaddedCCyFixing Response is " + resBody);
                }
            }
            else
            {
                test.Log(Status.Fail, "GetNewlyaddedCCyFixing Response is " + resCode);
                Assert.Fail("GetNewlyaddedCCyFixing Response is " + resCode);
            }

            url_lattise.Click();
           // Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

        }
    }
}
