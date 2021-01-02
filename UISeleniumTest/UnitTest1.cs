using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UISeleniumTest
{
    //HUSK at hente nugetpacket: Selenium.WebDriver, Selenium.Support og Selenium.WebDriver.ChromeDriver
    //Dette er en MSTest

    [TestClass]
    public class UnitTest1
    {
        private const string URL = "http://localhost:3000/";
        IWebDriver driver = new ChromeDriver();

        [TestInitialize]
        public void Setup()
        {
            driver.Navigate().GoToUrl(URL);

        }

        //her henter jeg data'en gennem min API og gemmer data'en i en liste
        private async Task<IList<Plante>> getPlanteList()
        {
            using (HttpClient client = new HttpClient())
            {
                string ProductEndPoint = "http://studymock1restservice.azurewebsites.net/planter";
                string content = await client.GetStringAsync(ProductEndPoint);
                IList<Plante> list = JsonConvert.DeserializeObject<IList<Plante>>(content);

                return list;
            }
        }

        [TestMethod]
        public void TestGet()
        {
            //her trykker jeg på hent planter i appen
            IWebElement buttonElement = driver.FindElement(By.Id("HentPlanter"));
            buttonElement.Click();
            Thread.Sleep(3000);


            IWebElement planteList = driver.FindElement(By.Id("planteListeOutput"));
            Assert.AreEqual(true, planteList.Displayed);

        }

        [TestMethod]
        public void TestGetById()
        {
            IWebElement inputDeletePlante = driver.FindElement(By.Id("planteByIdInput"));
            inputDeletePlante.Clear();
            inputDeletePlante.SendKeys("2");

            IWebElement FindIdButton = driver.FindElement(By.Id("findPlanteId"));
            FindIdButton.Click();

            Thread.Sleep(3000);

            IWebElement planteByIdList = driver.FindElement(By.Id("planteIdOutput"));
            String ExpectedText = "Plante ID: 2 PlanteType: Busk Plante Navn: Aronia Pris: 200 Max Højde: 169";

            Assert.AreEqual(ExpectedText, planteByIdList.Text);

        }

        [TestMethod]
        public void TestGetByType()
        {
            Thread.Sleep(3000);
            IWebElement inputDeletePlante = driver.FindElement(By.Id("planteByTypeInput"));
            inputDeletePlante.Clear();
            inputDeletePlante.SendKeys("rhododendron");

            IWebElement FindTypeButton = driver.FindElement(By.Id("findPlanteType"));
            FindTypeButton.Click();

            Thread.Sleep(3000);

            IWebElement planteByTypeList = driver.FindElement(By.Id("planteTypeListe"));
            String ExpectedText = "Plante ID: 4 PlanteType: Rhododendron Plante Navn: Astrid Pris: 40 Max Højde: 269";

            Assert.AreEqual(ExpectedText, planteByTypeList.Text);

        }

        [TestMethod]
        public void TestPost()
        {
            //Henter Liste med alle planter der er oprettet
            IList<Plante> listBefore = getPlanteList().Result;

            //Indtaster alle værdierne og trykker på opret
            IWebElement inputPlanteType = driver.FindElement(By.Id("planteTypeInput"));
            inputPlanteType.Clear();
            inputPlanteType.SendKeys("Test");

            IWebElement inputPlanteNavn = driver.FindElement(By.Id("planteNavnInput"));
            inputPlanteNavn.Clear();
            inputPlanteNavn.SendKeys("Test");

            IWebElement InputPlantePris = driver.FindElement(By.Id("plantePrisInput"));
            InputPlantePris.Clear();
            InputPlantePris.SendKeys("100");

            IWebElement InputMaksHoejde = driver.FindElement(By.Id("planteHoejdeInput"));
            InputMaksHoejde.Clear();
            InputMaksHoejde.SendKeys("100");


            Thread.Sleep(2000);

            IWebElement AddButton = driver.FindElement(By.Id("tilfoejPlante"));
            AddButton.Click();

            Thread.Sleep(5000);

            //henter liste efter en ny plante er oprettet
            IList<Plante> listAfter = getPlanteList().Result;

            Thread.Sleep(5000);

            //---------------------------------OBS   Denne passer ikke da det virker som om AddButton bliver trykket 2 gange - og der er -prevent på html'en
            Assert.AreEqual(listBefore.Count + 1, listAfter.Count);
        }

        [TestMethod]
        public void TestDelete()
        {
            //Henter Liste med alle planter der er oprettet
            IList<Plante> listBefore = getPlanteList().Result;

            //finder tekstfelt og indtaster værdi 
            IWebElement inputPlanteType = driver.FindElement(By.Id("planteDeleteIdInput"));
            inputPlanteType.Clear();
            inputPlanteType.SendKeys("8");

            //finder slet knap og trykker på den
            IWebElement DeleteButton = driver.FindElement(By.Id("deletePlanteId"));
            DeleteButton.Click();

            Thread.Sleep(3000);

            //henter liste efter en plante er slettet
            IList<Plante> listAfter = getPlanteList().Result;


            Assert.AreEqual(listBefore.Count, listAfter.Count + 1);
        }

        [TestCleanup]
        public void TestTearDown()
        {
            driver.Quit();
        }
    }
}
