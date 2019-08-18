using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectEstimation.Models;
using ProjectEstimation.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjectEstimation.Controllers
{
    public class DashboardController : Controller
    {
        public class projectDetails
        {
            public string ProjectName
            {
                get;
                set;
            }
            public string SQLCount { get; set; }
            public string SQLHours { get; set; }
            public string UICount { get; set; }
            public string UIHours { get; set; }
            public string ControllerCount { get; set; }
            public string ControllerHours { get; set; }
            public string UnitTestCount { get; set; }
            public string UnitTestHours { get; set; }
            public string TechnicalTestCount { get; set; }
            public string TechnicalTestHours { get; set; }
            public string BugCounts { get; set; }
            public string BugHours { get; set; }
        }

        public class projectDashboard
        {
            public string ProjectName { get; set; }
            public string devActivity { get; set; }
        }

        public class projectEstimation
        {
            public int sqlTable { get; set; }
            public int uiScreens { get; set; }
            public bool backendCode { get; set; }
            public bool unitTesting { get; set; }
            public bool technicalTesting { get; set; }
            public bool bugFixing { get; set; }
            public DateTime startDate { get; set; }
            public int developers { get; set; }
        }

        public class estimateHours
        {
            public double value { get; set; }
            public string name { get; set; }
        }

        public class estimatedDuration
        {
            public string name { get; set; }
            public string type { get; set; }
            public List<double> data { get; set; }
        }

        //
        // GET: /Dashboard/
        public ActionResult Dashboard()
        {
            return View();
        }

        public string AddProject(projectDetails projDetails, int? sno)
        {
            projectEstimatorEntities e = new projectEstimatorEntities();
            string message = "Project Details are saved successfully..!";
            if (sno != null)
            {
                projectDetail p = new projectDetail();
                p = (from pd in e.projectDetails where pd.sno == sno select pd).SingleOrDefault();
                p.projectName = projDetails.ProjectName;
                p.devActivityHours = JsonConvert.SerializeObject(projDetails);
                p.Employee = "";
                e.SaveChanges();
                message = "Project Details are updated successfully..!";
            }
            else
            {
                projectDetail p = new projectDetail();
                p.projectName = projDetails.ProjectName;
                p.devActivityHours = JsonConvert.SerializeObject(projDetails);
                p.Employee = "";
                e.projectDetails.Add(p);
                e.SaveChanges();
            }

            return message; // View(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadProject()
        {
            projectEstimatorEntities e = new projectEstimatorEntities();
            List<projectDetail> p = new List<projectDetail>();
            List<projectDashboard> pd = new List<projectDashboard>();
            p = (from proj in e.projectDetails select proj).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public double getAverage(List<projectDetail> pd, string devActivity, string hours)
        {
            double averageHours = 1;
            if (pd.Count >= 1)
            {
                int devHours = 0;
                int devCounts = 0;
                foreach (projectDetail p in pd)
                {
                    var q = JObject.Parse(p.devActivityHours);
                    devHours += int.Parse(q[hours].ToString());
                    devCounts += int.Parse(q[devActivity].ToString());
                }
                averageHours = devHours / devCounts; // Convert.ToInt32(average.Average());
            }
            return averageHours;
        }

        int balanceHours = 0;
        List<double> sqlinput = new List<double>();
        List<double> uiinput = new List<double>();
        List<double> backendinput = new List<double>();
        List<double> unittestinput = new List<double>();
        List<double> techtestinput = new List<double>();
        List<double> bugfixinput = new List<double>();
        List<object> lst = new List<object>();
        List<estimatedDuration> lstED = new List<estimatedDuration>();
        double sqlHours = 0;
        double UIHours = 0;
        double BackendHours = 0;
        double UnitTestHours = 0;
        double TechTestHours = 0;
        double BugFixingHours = 0;

        int usedHours = 0;
        int balanceHourss = 0;
        public void calculationHours(double activityHours, string activityType, double productivity)
        {
            Constants c = new Constants();
            estimatedDuration ed = new estimatedDuration();
            ed.name = activityType;
            ed.type = c.barChart.ToString();
            if(activityHours >= productivity)
            {
                if(usedHours != 0)
                {
                    if(activityType == "SQL")
                    {
                        sqlinput.Add(Convert.ToInt32(balanceHourss));
                    }
                }
            }
        }




        public void calculationHours1(double activityHours, string activityType, double productivity)
        {
            Constants c = new Constants();
            estimatedDuration ed = new estimatedDuration();
            ed.name = activityType;
            ed.type = c.barChart.ToString();
            if (activityHours >= productivity)
            {
                if (usedHours != 0)
                {
                    if (activityType == c.SQLHOURS.ToString()) { 
                        sqlinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                        uiinput.Add(0);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    if (activityType == c.UIHOURS.ToString()) {
                        sqlinput.Add(0);
                        uiinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    if (activityType == c.BackendHOURS.ToString()) {
                        sqlinput.Add(0);
                        uiinput.Add(0); 
                        backendinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    if (activityType == c.UnittestingHOURS.ToString()) {
                        sqlinput.Add(0);
                        uiinput.Add(0);
                        backendinput.Add(0);
                        unittestinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    if (activityType == c.TechnicaltestingHOURS.ToString()) {
                        sqlinput.Add(0);
                        uiinput.Add(0);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                        bugfixinput.Add(0);
                    }
                    if (activityType == c.BugfixingHOURS.ToString()) {
                        sqlinput.Add(0);
                        uiinput.Add(0);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(productivity - usedHours);
                        activityHours = activityHours - (productivity - usedHours);
                    }
                }
            }
            if (activityHours >= productivity)
            {
                int days = Convert.ToInt32(Math.Floor(activityHours / productivity));
                int balanceHours = Convert.ToInt32(activityHours % productivity);
                for (int i = 1; i <= days; i++)
                {
                    if (activityType == c.SQLHOURS.ToString())
                    {
                        sqlinput.Add(productivity);
                        uiinput.Add(0);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    else if (activityType == c.UIHOURS.ToString())
                    {
                        //if (usedHours != 0) { uiinput.Add(productivity - usedHours); }
                        uiinput.Add(productivity);
                        backendinput.Add(0);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    else if (activityType == c.BackendHOURS.ToString())
                    {
                        //if (usedHours != 0) { backendinput.Add(productivity - usedHours); }
                        backendinput.Add(productivity);
                        unittestinput.Add(0);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    else if (activityType == c.UnittestingHOURS.ToString())
                    {
                        //if (usedHours != 0) { unittestinput.Add(productivity - usedHours); }
                        unittestinput.Add(productivity);
                        techtestinput.Add(0);
                        bugfixinput.Add(0);
                    }
                    else if (activityType == c.TechnicaltestingHOURS.ToString())
                    {
                        //if (usedHours != 0) { techtestinput.Add(productivity - usedHours); }
                        techtestinput.Add(productivity);
                        bugfixinput.Add(0);
                    }
                    else if (activityType == c.BugfixingHOURS.ToString())
                    {
                        //if (usedHours != 0) { bugfixinput.Add(productivity - usedHours); }
                        bugfixinput.Add(productivity);
                    }
                }
                if (activityType == c.sqlHours.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - sqlinput.Sum());
                    sqlinput.Add(usedHours);
                }
                if (activityType == c.UIHOURS.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - uiinput.Sum());
                    uiinput.Add(usedHours);
                }
                if (activityType == c.BackendHOURS.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - backendinput.Sum());
                    backendinput.Add(usedHours);
                }
                if (activityType == c.UnittestingHOURS.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - unittestinput.Sum());
                    unittestinput.Add(usedHours);
                }
                if (activityType == c.TechnicaltestingHOURS.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - techtestinput.Sum());
                    techtestinput.Add(usedHours);
                }
                if (activityType == c.BugfixingHOURS.ToString())
                {
                    usedHours = Convert.ToInt32(activityHours - bugfixinput.Sum());
                    bugfixinput.Add(usedHours);
                }
            }
            else
            {
                int balanceHours = Convert.ToInt32(activityHours % productivity);
                if (activityType == c.SQLHOURS.ToString()) { sqlinput.Add(balanceHours); usedHours = balanceHours; }
                if (activityType == c.UIHOURS.ToString()) { uiinput.Add(balanceHours); usedHours = balanceHours; }
                if (activityType == c.BackendHOURS.ToString()) { backendinput.Add(balanceHours); usedHours = balanceHours; }
                if (activityType == c.UnittestingHOURS.ToString()) { unittestinput.Add(balanceHours); usedHours =balanceHours; }
                if (activityType == c.TechnicaltestingHOURS.ToString()) { techtestinput.Add(balanceHours); usedHours = balanceHours; }
                if (activityType == c.BugfixingHOURS.ToString()) { bugfixinput.Add(balanceHours); usedHours = balanceHours; }
            }
            if (activityType == c.SQLHOURS.ToString()) { ed.data = sqlinput; lstED.Add(ed); }
            if (activityType == c.UIHOURS.ToString()) { ed.data = uiinput; lstED.Add(ed); }
            if (activityType == c.BackendHOURS.ToString()) { ed.data = backendinput; lstED.Add(ed); }
            if (activityType == c.UnittestingHOURS.ToString()) { ed.data = unittestinput; lstED.Add(ed); }
            if (activityType == c.TechnicaltestingHOURS.ToString()) { ed.data = techtestinput; lstED.Add(ed); }
            if (activityType == c.BugfixingHOURS.ToString()) { ed.data = sqlinput; lstED.Add(ed); }
        }


        public void calculateDuration(double activityHours, string activityType, double productitvity)
        {
            Constants c = new Constants();
            estimatedDuration ed = new estimatedDuration();
            ed.name = activityType.ToString();
            ed.type = c.barChart.ToString();
            if (productitvity > balanceHours)
            {
                if (activityType == c.UIHOURS.ToString())
                {
                    uiinput.Add(balanceHours);
                    backendinput.Add(0);
                    unittestinput.Add(0);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.BackendHOURS.ToString())
                {
                    backendinput.Add(balanceHours);
                    unittestinput.Add(0);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.UnittestingHOURS.ToString())
                {
                    unittestinput.Add(balanceHours);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.TechnicaltestingHOURS.ToString())
                {
                    techtestinput.Add(balanceHours);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.BugfixingHOURS.ToString())
                {
                    bugfixinput.Add(balanceHours);
                }
            }

            int days = Convert.ToInt32(Math.Floor(activityHours / productitvity));
            balanceHours = Convert.ToInt32(activityHours % productitvity);
            //balanceHours = Convert.ToInt32(productitvity - )
            for (int i = 1; i <= days; i++)
            {
                if (activityType == c.SQLHOURS.ToString())
                {
                    sqlinput.Add(productitvity);
                    uiinput.Add(0);
                    backendinput.Add(0);
                    unittestinput.Add(0);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.UIHOURS.ToString())
                {
                    uiinput.Add(productitvity);
                    backendinput.Add(0);
                    unittestinput.Add(0);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.BackendHOURS.ToString())
                {
                    backendinput.Add(productitvity);
                    unittestinput.Add(0);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.UnittestingHOURS.ToString())
                {
                    unittestinput.Add(productitvity);
                    techtestinput.Add(0);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.TechnicaltestingHOURS.ToString())
                {
                    techtestinput.Add(productitvity);
                    bugfixinput.Add(0);
                }
                else if (activityType == c.BugfixingHOURS.ToString())
                {
                    bugfixinput.Add(productitvity);
                }
            }
            if (activityType == c.SQLHOURS.ToString())
            {
                sqlinput.Add(balanceHours);
                ed.data = sqlinput;
                lstED.Add(ed);
            }
            else if (activityType == c.UIHOURS.ToString())
            {
                int total = Convert.ToInt32(activityHours - uiinput.Sum());
                uiinput.Add(total);
                balanceHours = Convert.ToInt32(productitvity - total);
                ed.data = uiinput;
                lstED.Add(ed);
            }
            else if (activityType == c.BackendHOURS.ToString())
            {
                int total = Convert.ToInt32(activityHours - backendinput.Sum());
                backendinput.Add(total);
                balanceHours = Convert.ToInt32(productitvity - total);
                ed.data = backendinput;
                lstED.Add(ed);
            }
            else if (activityType == c.UnittestingHOURS.ToString())
            {
                int total = Convert.ToInt32(activityHours - unittestinput.Sum());
                unittestinput.Add(total);
                balanceHours = Convert.ToInt32(productitvity - total);
                ed.data = unittestinput;
                lstED.Add(ed);
            }
            else if (activityType == c.TechnicaltestingHOURS.ToString())
            {
                int total = Convert.ToInt32(activityHours - techtestinput.Sum());
                techtestinput.Add(total);
                balanceHours = Convert.ToInt32(productitvity - total);
                ed.data = techtestinput;
                lstED.Add(ed);
            }
            else if (activityType == c.BugfixingHOURS.ToString())
            {
                int total = Convert.ToInt32(activityHours - bugfixinput.Sum());
                bugfixinput.Add(total);
                balanceHours = Convert.ToInt32(productitvity - total);
                ed.data = bugfixinput;
                lstED.Add(ed);
            }
            //balanceHours = Convert.ToInt32(productitvity - balanceHours);
        }


        public ActionResult EstimateProject(projectEstimation pe)
        {
            projectEstimatorEntities e = new projectEstimatorEntities();
            List<projectDetail> p = new List<projectDetail>();
            Constants c = new Constants();
            p = (from proj in e.projectDetails select proj).ToList();

            List<estimateHours> ee = new List<estimateHours>();
            List<string> legend = new List<string>();
            double productity = pe.developers * c.ProductivityHours;

            double sqlAvg = getAverage(p, c.SQLCount, c.sqlHours);
            double uiAvg = getAverage(p, c.UICount, c.UIHours);
            double backendAvg = getAverage(p, c.UICount, c.ControllerHours);
            double unitTestAvg = getAverage(p, c.UICount, c.UnitTestHours);
            double techinicalTestAvg = getAverage(p, c.UICount, c.TechnicalTestHours);
            double bugFixAvg = getAverage(p, c.BugCounts, c.BugHours);

            //SQL Chart
            estimateHours esql = new estimateHours();
            esql.name = c.SQLHOURS.ToString();
            esql.value = pe.sqlTable * int.Parse(sqlAvg.ToString());
            sqlHours = esql.value;
            ee.Add(esql);
            legend.Add(c.SQLHOURS.ToString());

            // UI Chart
            estimateHours eUI = new estimateHours();
            eUI.name = c.UIHOURS.ToString();
            eUI.value = pe.uiScreens * int.Parse(uiAvg.ToString());
            UIHours = eUI.value;
            ee.Add(eUI);
            legend.Add(c.UIHOURS.ToString());

            if (pe.backendCode)
            {
                estimateHours eback = new estimateHours();
                eback.name = c.BackendHOURS.ToString();
                eback.value = backendAvg * pe.uiScreens;
                BackendHours = eback.value;
                ee.Add(eback);
                legend.Add(c.BackendHOURS.ToString());
            }
            if (pe.unitTesting)
            {
                estimateHours eUnit = new estimateHours();
                eUnit.name = c.UnittestingHOURS.ToString();
                eUnit.value = unitTestAvg * pe.uiScreens;
                UnitTestHours = eUnit.value;
                ee.Add(eUnit);
                legend.Add(c.UnittestingHOURS.ToString());
            }
            if (pe.technicalTesting)
            {
                estimateHours etech = new estimateHours();
                etech.name = c.TechnicaltestingHOURS.ToString();
                etech.value = techinicalTestAvg * pe.uiScreens;
                TechTestHours = etech.value;
                ee.Add(etech);
                legend.Add(c.TechnicaltestingHOURS.ToString());
            }
            if (pe.bugFixing)
            {
                estimateHours ebug = new estimateHours();
                ebug.name = c.BugfixingHOURS.ToString();
                ebug.value = bugFixAvg * pe.uiScreens;
                BugFixingHours = ebug.value;
                ee.Add(ebug);
                legend.Add(c.BugfixingHOURS.ToString());
            }
            List<double> lstAvg = new List<double>();
            lstAvg.Add(productity);
            lstAvg.Add(pe.sqlTable * sqlAvg);
            lstAvg.Add(pe.uiScreens * uiAvg);
            lstAvg.Add(backendAvg * pe.uiScreens);
            lstAvg.Add(unitTestAvg * pe.uiScreens);
            lstAvg.Add(techinicalTestAvg * pe.uiScreens);
            lstAvg.Add(bugFixAvg * pe.uiScreens);
            lst.Add(ee);
            lst.Add(lstAvg);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteProject(int projectID)
        {
            projectDetail p = new projectDetail();
            projectEstimatorEntities pe = new projectEstimatorEntities();
            p = (from pp in pe.projectDetails where pp.sno.Equals(projectID) select pp).FirstOrDefault();
            pe.projectDetails.Remove(p);
            pe.SaveChanges();
            return Json("Project Deleted Successfully..!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditProject(int sno)
        {
            projectDetail pd = new projectDetail();
            projectEstimatorEntities pe = new projectEstimatorEntities();
            pd = (from p in pe.projectDetails where p.sno == sno select p).FirstOrDefault();
            return Json(pd, JsonRequestBehavior.AllowGet);
        }

        public void testing()
        {
            //double sqlAvg = getAverage(p, c.SQLCount, c.sqlHours);
            //double uiAvg = getAverage(p, c.UICount, c.UIHours);
            //double backendAvg = getAverage(p, c.UICount, c.ControllerHours);
            //double unitTestAvg = getAverage(p, c.UICount, c.UnitTestHours);
            //double techinicalTestAvg = getAverage(p, c.UICount, c.TechnicalTestHours);
            //double bugFixAvg = getAverage(p, c.BugCounts, c.BugHours);

            //List<estimateHours> ee = new List<estimateHours>();
            //List<string> legend = new List<string>();
            //double productity = pe.developers * c.ProductivityHours;

            ////SQL Chart
            //estimateHours esql = new estimateHours();
            //esql.name = c.SQLHOURS.ToString();
            //esql.value = pe.sqlTable * int.Parse(sqlAvg.ToString());
            //sqlHours = esql.value;
            //ee.Add(esql);
            //legend.Add(c.SQLHOURS.ToString());

            //// UI Chart
            //estimateHours eUI = new estimateHours();
            //eUI.name = c.UIHOURS.ToString();
            //eUI.value = pe.uiScreens * int.Parse(uiAvg.ToString());
            //UIHours = eUI.value;
            //ee.Add(eUI);
            //legend.Add(c.UIHOURS.ToString());

            //if (pe.backendCode)
            //{
            //    estimateHours eback = new estimateHours();
            //    eback.name = c.BackendHOURS.ToString();
            //    eback.value = backendAvg * pe.uiScreens;
            //    BackendHours = eback.value;
            //    ee.Add(eback);
            //    legend.Add(c.BackendHOURS.ToString());
            //}
            //if (pe.unitTesting)
            //{
            //    estimateHours eUnit = new estimateHours();
            //    eUnit.name = c.UnittestingHOURS.ToString();
            //    eUnit.value = unitTestAvg * pe.uiScreens;
            //    UnitTestHours = eUnit.value;
            //    ee.Add(eUnit);
            //    legend.Add(c.UnittestingHOURS.ToString());
            //}
            //if (pe.technicalTesting)
            //{
            //    estimateHours etech = new estimateHours();
            //    etech.name = c.TechnicaltestingHOURS.ToString();
            //    etech.value = unitTestAvg * pe.uiScreens;
            //    TechTestHours = etech.value;
            //    ee.Add(etech);
            //    legend.Add(c.TechnicaltestingHOURS.ToString());
            //}
            //if (pe.bugFixing)
            //{
            //    estimateHours ebug = new estimateHours();
            //    ebug.name = c.BugfixingHOURS.ToString();
            //    ebug.value = bugFixAvg * pe.uiScreens;
            //    BugFixingHours = ebug.value;
            //    ee.Add(ebug);
            //    legend.Add(c.BugfixingHOURS.ToString());
            //}

            //// SQL Line Chart
            //int uHours = 0;
            //int bHours = 0;
            //List<double> sInput = new List<double>();
            //List<double> uInput = new List<double>();
            //List<double> bInput = new List<double>();
            //List<double> unInput = new List<double>();
            //List<double> tInput = new List<double>();
            //List<double> bgInput = new List<double>();
            //estimatedDuration ed = new estimatedDuration();
            ////calculationHours(sqlHours, c.SQLHOURS.ToString(), productity);
            ////sqlHours = 26; UIHours = 22; BackendHours = 30; UnitTestHours = 42; TechTestHours = 20; BugFixingHours = 42;
            //int bHoursss = Convert.ToInt32(sqlHours % productity);
            //if (sqlHours >= productity)
            //{
            //    for (double i = productity; i <= sqlHours; i++)
            //    {
            //        i += Convert.ToInt32(productity);
            //        sInput.Add(productity); uInput.Add(0); bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //    }
            //    if (bHoursss != 0)
            //    {
            //        sInput.Add(bHoursss); //uInput.Add(0); bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //        bHoursss = Convert.ToInt32(productity - bHoursss);
            //    }
            //}
            //else
            //{
            //    sInput.Add(sqlHours); //uInput.Add(0); bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //    bHoursss = Convert.ToInt32(productity - bHoursss);
            //}
            //ed.name = "SQL";
            //ed.data = sInput;
            //ed.type = c.barChart.ToString();
            //lstED.Add(ed);

            ////calculationHours(UIHours, c.UIHOURS.ToString(), productity);
            //if (UIHours >= bHoursss)
            //{
            //    if (bHoursss != 0)
            //    {
            //        uInput.Add(Convert.ToInt32(bHoursss)); bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //        UIHours -= bHoursss;
            //    }
            //    bHoursss = Convert.ToInt32(UIHours % productity);
            //    if (UIHours >= productity)
            //    {

            //        for (double i = productity; i <= UIHours; i++)
            //        {
            //            i += Convert.ToInt32(productity);
            //            uInput.Add(productity); bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //        }
            //        if (bHoursss != 0)
            //        {
            //            uInput.Add(bHoursss); //bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //            bHoursss = Convert.ToInt32(productity - bHoursss);
            //        }
            //    }
            //    else
            //    {
            //        uInput.Add(UIHours); //bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //        bHoursss = Convert.ToInt32(productity - UIHours);
            //    }

            //}
            //else
            //{
            //    uInput.Add(UIHours); //bInput.Add(0); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //    bHoursss += Convert.ToInt32(UIHours);
            //}
            //estimatedDuration ed1 = new estimatedDuration();
            //ed1.name = "UI";
            //ed1.data = uInput;
            //ed1.type = c.barChart.ToString();
            //lstED.Add(ed1);
            //if (pe.backendCode)
            //{
            //    if (BackendHours >= bHoursss)
            //    {
            //        if (bHoursss != 0)
            //        {
            //            bInput.Add(Convert.ToInt32(bHoursss)); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //            BackendHours -= bHoursss;
            //        }
            //        bHoursss = Convert.ToInt32(BackendHours % productity);
            //        if (BackendHours >= productity)
            //        {
            //            for (double i = productity; i <= BackendHours; i++)
            //            {
            //                i += Convert.ToInt32(productity);
            //                bInput.Add(productity); unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //            }
            //            if (bHoursss != 0)
            //            {
            //                bInput.Add(bHoursss); //unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //                bHoursss = Convert.ToInt32(productity - bHoursss);
            //            }
            //        }
            //        else
            //        {
            //            bInput.Add(BackendHours); //unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //            bHoursss = Convert.ToInt32(productity - BackendHours);
            //        }

            //    }
            //    else
            //    {
            //        bInput.Add(BackendHours); //unInput.Add(0); tInput.Add(0); bgInput.Add(0);
            //        bHoursss += Convert.ToInt32(BackendHours);
            //    }
            //    estimatedDuration ed2 = new estimatedDuration();
            //    ed2.name = "BackEnd";
            //    ed2.data = bInput;
            //    ed2.type = c.barChart.ToString();
            //    lstED.Add(ed2);
            //}
            //if (pe.unitTesting)
            //{
            //    if (UnitTestHours >= bHoursss)
            //    {
            //        if (bHoursss != 0)
            //        {
            //            unInput.Add(Convert.ToInt32(bHoursss)); tInput.Add(0); bgInput.Add(0);
            //            UnitTestHours -= bHoursss;
            //        }
            //        bHoursss = Convert.ToInt32(UnitTestHours % productity);
            //        if (UnitTestHours >= productity)
            //        {
            //            for (double i = productity; i <= UnitTestHours; i++)
            //            {
            //                i += Convert.ToInt32(productity);
            //                unInput.Add(productity); tInput.Add(0); bgInput.Add(0);
            //            }
            //            if (bHoursss != 0)
            //            {
            //                unInput.Add(bHoursss); //tInput.Add(0); bgInput.Add(0);
            //                bHoursss = Convert.ToInt32(productity - bHoursss);
            //            }
            //        }
            //        else
            //        {
            //            unInput.Add(UnitTestHours); //tInput.Add(0); bgInput.Add(0);
            //            bHoursss = Convert.ToInt32(productity - UnitTestHours);
            //        }
            //    }
            //    else
            //    {
            //        unInput.Add(UnitTestHours); //tInput.Add(0); bgInput.Add(0);
            //        bHoursss += Convert.ToInt32(UnitTestHours);
            //    }
            //    estimatedDuration ed3 = new estimatedDuration();
            //    ed3.name = "Unit";
            //    ed3.data = unInput;
            //    ed3.type = c.barChart.ToString();
            //    lstED.Add(ed3);
            //    //calculationHours(UnitTestHours, c.UnittestingHOURS.ToString(), productity);
            //}
            //if (pe.technicalTesting)
            //{
            //    if (TechTestHours >= bHoursss)
            //    {
            //        if (bHoursss != 0)
            //        {
            //            tInput.Add(Convert.ToInt32(bHoursss)); bgInput.Add(0);
            //            TechTestHours -= bHoursss;
            //        }
            //        bHoursss = Convert.ToInt32(TechTestHours % productity);
            //        if (TechTestHours >= productity)
            //        {
            //            for (double i = productity; i <= TechTestHours; i++)
            //            {
            //                i += Convert.ToInt32(productity);
            //                tInput.Add(productity); bgInput.Add(0);
            //            }
            //            if (bHoursss != 0)
            //            {
            //                tInput.Add(bHoursss); //bgInput.Add(0);
            //                bHoursss = Convert.ToInt32(productity - bHoursss);
            //            }
            //        }
            //        else
            //        {
            //            tInput.Add(TechTestHours); //bgInput.Add(0);
            //            bHoursss = Convert.ToInt32(productity - TechTestHours);
            //        }
            //    }
            //    else
            //    {
            //        tInput.Add(TechTestHours); //bgInput.Add(0);
            //        bHoursss += Convert.ToInt32(TechTestHours);
            //    }
            //    estimatedDuration ed4 = new estimatedDuration();
            //    ed4.name = "Tech";
            //    ed4.data = tInput;
            //    ed4.type = c.barChart.ToString();
            //    lstED.Add(ed4);
            //    //calculationHours(TechTestHours, c.TechnicaltestingHOURS.ToString(), productity);
            //}
            //if (pe.bugFixing)
            //{
            //    if (BugFixingHours >= bHoursss)
            //    {
            //        if (bHoursss != 0)
            //        {
            //            bgInput.Add(Convert.ToInt32(bHoursss));
            //            BugFixingHours -= bHoursss;
            //        }
            //        bHoursss = Convert.ToInt32(BugFixingHours % productity);
            //        if (BugFixingHours >= productity)
            //        {
            //            for (double i = productity; i <= BugFixingHours; i++)
            //            {
            //                i += Convert.ToInt32(productity);
            //                bgInput.Add(productity);
            //            }
            //            if (bHoursss != 0)
            //            {
            //                bgInput.Add(bHoursss);
            //                bHoursss = Convert.ToInt32(productity - bHoursss);
            //            }
            //        }
            //        else
            //        {
            //            bgInput.Add(BugFixingHours);
            //            bHoursss = Convert.ToInt32(productity - BugFixingHours);
            //        }
            //    }
            //    else
            //    {
            //        bgInput.Add(BugFixingHours);
            //        bHoursss += Convert.ToInt32(BugFixingHours);
            //    }
            //    estimatedDuration ed5 = new estimatedDuration();
            //    ed5.name = "Tech";
            //    ed5.data = bgInput;
            //    ed5.type = c.barChart.ToString();
            //    lstED.Add(ed5);
            //    //calculationHours(BugFixingHours, c.BugfixingHOURS.ToString(), productity);
            //}

            //lst.Add(lstED);
            //lst.Add(ee);
        }
    }
}