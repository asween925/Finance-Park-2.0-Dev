using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Budget : System.Web.UI.Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_StudentData Students = new Class_StudentData();
    private Class_Simulation Sim = new Class_Simulation();
    private Class_BusinessData Businesses = new Class_BusinessData();
    private int VisitID;
    private int StudentID;
    private int AcctNum;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get current visit ID and student ID
        VisitID = VisitData.GetVisitID();

        //Check if student id is passed through
        if (Request["b"] != null)
        {
            StudentID = int.Parse(Request["b"]);

            //Get account number
            var Student = Students.StudentLookup(23, StudentID);
            AcctNum = Student.AccountNumber;

            if (!IsPostBack)
            {
                //Load Data
                LoadData(StudentID);
            }

        }
    }

    protected void LoadData(int StudentID)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        int NMI = Convert.ToInt16(Student.NMI);
        int Total = 0;
        int Count = 1;

        //Assign NMI to label
        lblNMI.Text = NMI.ToString("c");

        //Assign textboxes, labels budget amount
        while (Count < 32)
        {
            var BudgetPer = Businesses.GetBudgetPercentages(Count);

            switch (Count)
            {
                case 1:
                    tbBudget1.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer1.Text =  (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer1.Text = "No minimum, can budget at your will.";
                        lblMinDollar1.Text = "";
                    }
                    else
                    {
                        lblMinPer1.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar1.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer1.Text = "No maximum, can budget at your will.";
                        lblMaxDollar1.Text = "";
                    }
                    else
                    {
                        lblMaxPer1.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar1.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }
                                      
                    break;
                case 7:
                    tbBudget7.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer7.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer7.Text = "No minimum, can budget at your will.";
                        lblMinDollar7.Text = "";
                    }
                    else
                    {
                        lblMinPer7.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar7.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer7.Text = "No maximum, can budget at your will.";
                        lblMaxDollar7.Text = "";
                    }
                    else
                    {
                        lblMaxPer7.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar7.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 11:
                    tbBudget11.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer11.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer11.Text = "No minimum, can budget at your will.";
                        lblMinDollar11.Text = "";
                    }
                    else
                    {
                        lblMinPer11.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar11.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer11.Text = "No maximum, can budget at your will.";
                        lblMaxDollar11.Text = "";
                    }
                    else
                    {
                        lblMaxPer11.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar11.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 12:
                    tbBudget12.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer12.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer12.Text = "No minimum, can budget at your will.";
                        lblMinDollar12.Text = "";
                    }
                    else
                    {
                        lblMinPer12.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar12.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer12.Text = "No maximum, can budget at your will.";
                        lblMaxDollar12.Text = "";
                    }
                    else
                    {
                        lblMaxPer12.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar12.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 13:
                    tbBudget13.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer13.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer13.Text = "No minimum, can budget at your will.";
                        lblMinDollar13.Text = "";
                    }
                    else
                    {
                        lblMinPer13.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar13.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer13.Text = "No maximum, can budget at your will.";
                        lblMaxDollar13.Text = "";
                    }
                    else
                    {
                        lblMaxPer13.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar13.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 15:
                    tbBudget15.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer15.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer15.Text = "No minimum, can budget at your will.";
                        lblMinDollar15.Text = "";
                    }
                    else
                    {
                        lblMinPer15.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar15.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer15.Text = "No maximum, can budget at your will.";
                        lblMaxDollar15.Text = "";
                    }
                    else
                    {
                        lblMaxPer15.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar15.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 16:
                    tbBudget16.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer16.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer16.Text = "No minimum, can budget at your will.";
                        lblMinDollar16.Text = "";
                    }
                    else
                    {
                        lblMinPer16.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar16.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer16.Text = "No maximum, can budget at your will.";
                        lblMaxDollar16.Text = "";
                    }
                    else
                    {
                        lblMaxPer16.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar16.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 18:
                    tbBudget18.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer18.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer18.Text = "No minimum, can budget at your will.";
                        lblMinDollar18.Text = "";
                    }
                    else
                    {
                        lblMinPer18.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar18.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer18.Text = "No maximum, can budget at your will.";
                        lblMaxDollar18.Text = "";
                    }
                    else
                    {
                        lblMaxPer18.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar18.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 19:
                    tbBudget19.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer19.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer19.Text = "No minimum, can budget at your will.";
                        lblMinDollar19.Text = "";
                    }
                    else
                    {
                        lblMinPer19.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar19.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer19.Text = "No maximum, can budget at your will.";
                        lblMaxDollar19.Text = "";
                    }
                    else
                    {
                        lblMaxPer19.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar19.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 21:
                    tbBudget21.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer21.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer19.Text = "No minimum, can budget at your will.";
                        lblMinDollar21.Text = "";
                    }
                    else
                    {
                        lblMinPer21.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar21.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer21.Text = "No maximum, can budget at your will.";
                        lblMaxDollar21.Text = "";
                    }
                    else
                    {
                        lblMaxPer21.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar21.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 22:
                    tbBudget22.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer22.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer22.Text = "No minimum, can budget at your will.";
                        lblMinDollar22.Text = "";
                    }
                    else
                    {
                        lblMinPer22.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar22.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer22.Text = "No maximum, can budget at your will.";
                        lblMaxDollar22.Text = "";
                    }
                    else
                    {
                        lblMaxPer22.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar22.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 23:
                    tbBudget23.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer23.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer23.Text = "No minimum, can budget at your will.";
                        lblMinDollar23.Text = "";
                    }
                    else
                    {
                        lblMinPer23.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar23.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer23.Text = "No maximum, can budget at your will.";
                        lblMaxDollar23.Text = "";
                    }
                    else
                    {
                        lblMaxPer23.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar23.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 24:
                    tbBudget24.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer24.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer24.Text = "No minimum, can budget at your will.";
                        lblMinDollar24.Text = "";
                    }
                    else
                    {
                        lblMinPer24.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar24.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer24.Text = "No maximum, can budget at your will.";
                        lblMaxDollar24.Text = "";
                    }
                    else
                    {
                        lblMaxPer24.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar24.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 26:
                    tbBudget26.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer26.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer26.Text = "No minimum, can budget at your will.";
                        lblMinDollar26.Text = "";
                    }
                    else
                    {
                        lblMinPer26.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar26.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer26.Text = "No maximum, can budget at your will.";
                        lblMaxDollar26.Text = "";
                    }
                    else
                    {
                        lblMaxPer26.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar26.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 27:
                    tbBudget27.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer27.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer27.Text = "No minimum, can budget at your will.";
                        lblMinDollar27.Text = "";
                    }
                    else
                    {
                        lblMinPer27.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar27.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer27.Text = "No maximum, can budget at your will.";
                        lblMaxDollar27.Text = "";
                    }
                    else
                    {
                        lblMaxPer27.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar27.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 28:
                    tbBudget28.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer28.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer28.Text = "No minimum, can budget at your will.";
                        lblMinDollar28.Text = "";
                    }
                    else
                    {
                        lblMinPer28.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar28.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer28.Text = "No maximum, can budget at your will.";
                        lblMaxDollar28.Text = "";
                    }
                    else
                    {
                        lblMaxPer28.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar28.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 29:
                    tbBudget29.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer29.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer29.Text = "No minimum, can budget at your will.";
                        lblMinDollar29.Text = "";
                    }
                    else
                    {
                        lblMinPer29.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar29.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer29.Text = "No maximum, can budget at your will.";
                        lblMaxDollar29.Text = "";
                    }
                    else
                    {
                        lblMaxPer29.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar29.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 30:
                    tbBudget30.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer30.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer30.Text = "No minimum, can budget at your will.";
                        lblMinDollar30.Text = "";
                    }
                    else
                    {
                        lblMinPer30.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar30.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer30.Text = "No maximum, can budget at your will.";
                        lblMaxDollar30.Text = "";
                    }
                    else
                    {
                        lblMaxPer30.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar30.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;
                case 31:
                    tbBudget31.Text = Sim.GetBudgetData(23, StudentID, Count).ToString();
                    lblBudgetPer31.Text = (Sim.GetBudgetData(23, StudentID, Count) / Convert.ToDouble(NMI)).ToString("P1");

                    //Check if minimum percentage is 0, change text if so
                    if (BudgetPer.Min == 0)
                    {
                        lblMinPer31.Text = "No minimum, can budget at your will.";
                        lblMinDollar31.Text = "";
                    }
                    else
                    {
                        lblMinPer31.Text = BudgetPer.Min.ToString() + "%";
                        lblMinDollar31.Text = (BudgetPer.Min / 100.00 * NMI).ToString("c");
                    }

                    //Check if maximum percentage is 0, change text if so
                    if (BudgetPer.Max == 0)
                    {
                        lblMaxPer31.Text = "No maximum, can budget at your will.";
                        lblMaxDollar31.Text = "";
                    }
                    else
                    {
                        lblMaxPer31.Text = BudgetPer.Max.ToString() + "%";
                        lblMaxDollar31.Text = (BudgetPer.Max / 100.00 * NMI).ToString("c");
                    }

                    break;

            }
            Count++;
        }

        //Calculate total budgeted amount
        if (Sim.CheckBudgets(23, StudentID) == true)
        {
            Total = Sim.GetTotalBudget(23, StudentID);
        }
            
        //Assign total budgeted and allocate labels
        lblTotal.Text = Total.ToString("c");
        lblAllocate.Text = (NMI - Total).ToString("c");
    }

    protected void Budget(int BusinessID, string Amount)
    {
        //Check if already inserting into DB
        if (Sim.CheckBudgets(23, StudentID) == true)
        {
            Sim.UpdateBudget(23, StudentID, BusinessID, Amount);
        }
        else
        {
            Sim.InsertBudget(23, StudentID, BusinessID, Amount);
        }

        //Load data
        LoadData(StudentID);
    }



    protected void btnNext_Click(object sender, EventArgs e)
    {
        //Check if there is any remaining budget
        if (lblAllocate.Text != "0.00")
        {
            lblError.Text = "Please allocate the remaining budget before moving on.";
            return;
        }

        //Load transition data
        lblPopupText.Text = "Are you sure you want to continue? Please note you will not be able to modify your budget later.";

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {
        //Redirect to sim shopping
        Response.Redirect("Sim_Shopping.aspx?b=" + StudentID);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Automatically closes the popup
    }

    protected void tbBudget1_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget1.Text != "")
        {
            Budget(1, tbBudget1.Text);
            
        }
    }

    protected void tbBudget7_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget7.Text != "")
        {
            Budget(7, tbBudget7.Text);
        }
    }

    protected void tbBudget11_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget11.Text != "")
        {
            Budget(11, tbBudget11.Text);
        }
    }

    protected void tbBudget12_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget12.Text != "")
        {
            Budget(12, tbBudget12.Text);
        }
    }

    protected void tbBudget13_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget13.Text != "")
        {
            Budget(13, tbBudget13.Text);
        }
    }

    protected void tbBudget15_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget15.Text != "")
        {
            Budget(15, tbBudget15.Text);
        }
    }

    protected void tbBudget16_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget16.Text != "")
        {
            Budget(16, tbBudget16.Text);
        }
    }

    protected void tbBudget18_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget18.Text != "")
        {
            Budget(18, tbBudget18.Text);
        }
    }

    protected void tbBudget19_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget19.Text != "")
        {
            Budget(19, tbBudget19.Text);
        }
    }

    protected void tbBudget21_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget21.Text != "")
        {
            Budget(21, tbBudget21.Text);
        }
    }

    protected void tbBudget22_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget22.Text != "")
        {
            Budget(22, tbBudget22.Text);
        }
    }

    protected void tbBudget23_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget23.Text != "")
        {
            Budget(23, tbBudget23.Text);
        }
    }

    protected void tbBudget24_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget24.Text != "")
        {
            Budget(24, tbBudget24.Text);
        }
    }

    protected void tbBudget26_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget26.Text != "")
        {
            Budget(26, tbBudget26.Text  );
        }
    }

    protected void tbBudget27_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget27.Text != "")
        {
            Budget(27, tbBudget27.Text);
        }
    }

    protected void tbBudget28_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget28.Text != "")
        {
            Budget(28, tbBudget28.Text);
        }
    }

    protected void tbBudget29_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget29.Text != "")
        {
            Budget(29, tbBudget29.Text);
        }
    }

    protected void tbBudget30_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget30.Text != "")
        {
            Budget(30, tbBudget30.Text);
        }
    }

    protected void tbBudget31_TextChanged(object sender, EventArgs e)
    {
        if (tbBudget31.Text != "")
        {
            Budget(31, tbBudget31.Text);
        }       
    }
}