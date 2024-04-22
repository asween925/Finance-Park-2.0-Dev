using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Antlr.Runtime.Tree.TreeWizard;

public partial class School_Visit_Checklist : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_SQLCommands SQL = new Class_SQLCommands();
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_TeacherData TeacherData = new Class_TeacherData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_SVC SVC = new Class_SVC();
    private int VisitID;

    public School_Visit_Checklist()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";      
        Load += Page_Load;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check if user is logged in
        if (HttpContext.Current.Session["LoggedIn"] == null)
        {
            Response.Redirect("../../Default.aspx");
        }

        if (!IsPostBack)
        {
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                hfCurrentVID.Value = VisitID.ToString();
            }

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(VisitDate.ToString()).ToString());
        string SchoolName = ddlSchoolName.SelectedValue;
        int SchoolID = int.Parse(SchoolData.GetSchoolID(SchoolName).ToString());
        
        //Make sections visible
        divS1.Visible = true;
        divS2.Visible = true;
        divS3.Visible = true;
        divS4.Visible = true;
        divS5.Visible = true;

        //Reset and clear all labels
        lblError.Text = "";

        lblS1LastEdit.Text = "";
        lblS1Comp.Visible = false;
        lblS1SchoolName.Text = "";
        lblS1VisitDate.Text = "";
        lblS1ContactTeacher.Text = "";
        lblS1NumStudents.Text = "";
        lblS1AdminEmail.Text = "";
        ddlS1SchoolType.SelectedIndex = 0;
        tbS1StudentForm.Text = "";

        lblS2LastEdit.Text = "";
        lblS2Comp.Visible = false;
        chkS2Invoice.Checked = false;
        chkS2Director.Checked = false;

        lblS3LastEdit.Text = "";
        lblS3Comp.Visible = false;
        tbS3Contract.Text = "";
        tbS3Invoice.Text = "";
        tbS3Notes.Text = "";
        ddlS3Delivery.SelectedIndex = 0;

        lblS4LastEdit.Text = "";
        lblS4Comp.Visible = false;
        ddlS4NumKits.SelectedIndex = 0;
        ShowKits();
        tbS4Workbooks.Text = "";

        lblS5LastEdit.Text = "";
        lblS5Comp.Visible = false;
        tbS5DelAcc.Text = "";
        tbS5Position.Text = "";
        tbS5DateAcc.Text = "";

        //Step 1
        try
        {
            //Load initial school, teacher, and visit data
            try
            {
                lblS1SchoolName.Text = SchoolName;
                lblS1VisitDate.Text = VisitDate.ToString("d");
                lblS1ContactTeacher.Text = TeacherData.GetContactTeacher(SchoolID.ToString()).ToString();
                lblS1NumStudents.Text = VisitData.LoadVisitInfoFromDate(VisitDate.ToString(), "studentCount").ToString();
                lblS1AdminEmail.Text = SchoolData.LoadSchoolInfoFromSchool(SchoolName, "administratorEmail").ToString();
            }
            catch
            {
                lblError.Text = "Error in LoadData(). Unable to load base data for Step 1.";
                return;
            }

            //Check if Step 1 is completed
            try
            {
                if (SVC.LoadTable(VisitID, SchoolID, "visitID").ToString() != "")
                {
                    //Load completed data
                    lblS1LastEdit.Text = SVC.LoadTable(VisitID, SchoolID, "lastEditS1").ToString();
                    ddlS1SchoolType.SelectedValue = SVC.LoadTable(VisitID, SchoolID, "schoolTypeS1").ToString();
                    tbS1StudentForm.Text = DateTime.Parse(SVC.LoadTable(VisitID, SchoolID, "formReceivedS1").ToString()).ToString("yyyy-MM-dd");

                    //Make completed label visible
                    lblS1Comp.Visible = true;

                    //Change S1 button to update
                    btnS1Submit.Text = "Update";
                }
                else
                {
                    btnS1Submit.Text = "Submit";
                    return;
                }
            }
            catch
            {
                lblError.Text = "Error in LoadData(). Could not load completed data for Step 1.";
                return;
            }
            
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Could not load inital data or completion data for Step 1.";
            return;
        }

        //Step 2
        try
        {
            //Check if Step 2 is completed
            if (SVC.LoadTable(VisitID, SchoolID, "completeS2").ToString() != "False")
            {
                lblS2LastEdit.Text = SVC.LoadTable(VisitID, SchoolID, "lastEditS2").ToString();
                chkS2Invoice.Checked = bool.Parse(SVC.LoadTable(VisitID, SchoolID, "invoiceIssuedS2").ToString());
                chkS2Director.Checked = bool.Parse(SVC.LoadTable(VisitID, SchoolID, "directorSignS2").ToString());

                //Make completed label visible
                lblS2Comp.Visible = true;

                //Change S2 button to update
                btnS2Submit.Text = "Update";
            }
            else
            {
                //Change S2 button to submit
                btnS2Submit.Text = "Submit";
            }
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Could not load data for Step 2.";
            return;
        }

        //Step 3
        try
        {
            //Check if Step 3 is completed
            if (SVC.LoadTable(VisitID, SchoolID, "completeS3").ToString() != "False")
            {
                lblS3LastEdit.Text = SVC.LoadTable(VisitID, SchoolID, "lastEditS3").ToString();
                tbS3Contract.Text = DateTime.Parse(SVC.LoadTable(VisitID, SchoolID, "contractReceivedS3").ToString()).ToString("yyyy-MM-dd");
                tbS3Invoice.Text = SVC.LoadTable(VisitID, SchoolID, "invoiceS3").ToString();
                ddlS3Delivery.SelectedValue = SVC.LoadTable(VisitID, SchoolID, "deliveryMethodS3").ToString();
                tbS3Notes.Text = SVC.LoadTable(VisitID, SchoolID, "notesS3").ToString();

                //Make completed label visible
                lblS3Comp.Visible = true;

                //Change S3 button to update
                btnS3Submit.Text = "Update";
            }
            else
            {
                //Change S3 button to submit
                btnS3Submit.Text = "Submit";
            }
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Could not load data for Step 3.";
            return;
        }

        //Step 4
        try
        {
            //Check if Step 4 is completed
            if (SVC.LoadTable(VisitID, SchoolID, "completeS4").ToString() != "False")
            {
                lblS4LastEdit.Text = SVC.LoadTable(VisitID, SchoolID, "lastEditS4").ToString();
                ddlS4NumKits.SelectedValue = SVC.GetKitNumbers(VisitID, SchoolID).ToString();
                ShowKits();
                AssignKits(VisitID, SchoolID);
                tbS4Workbooks.Text = SVC.GetWorkbooks(VisitID, SchoolID).ToString();

                //Make completed label visible
                lblS4Comp.Visible = true;

                //Change S4 button to update
                btnS4Submit.Text = "Update";
            }
            else
            {
                //Change S4 button to submit
                btnS4Submit.Text = "Submit";
            }
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Could not load data for Step 4.";
            return;
        }

        //Step 5
        try
        {
            //Check if Step 5 is completed
            if (SVC.LoadTable(VisitID, SchoolID, "completeS5").ToString() != "False" )
            {
                lblS5LastEdit.Text = SVC.LoadTable(VisitID, SchoolID, "lastEditS5").ToString();
                tbS5DelAcc.Text = SVC.LoadTable(VisitID, SchoolID, "deliveryByS5").ToString();
                tbS5Position.Text = SVC.LoadTable(VisitID, SchoolID, "positionS5").ToString();
                tbS5DateAcc.Text = DateTime.Parse(SVC.LoadTable(VisitID, SchoolID, "dateByS5").ToString()).ToString("yyyy-MM-dd");

                //Make completed label visible
                lblS5Comp.Visible = true;

                //Change S5 button to update
                btnS5Submit.Text = "Update";
            }
        }
        catch
        {
            //Change S5 button to submit
            btnS5Submit.Text = "Submit";
        }
        
    }

    public void ShowKits()
    {
        // Make kit textboxes invisible
        tbS4Kit1.Visible = false;
        tbS4Kit2.Visible = false;
        tbS4Kit3.Visible = false;
        tbS4Kit4.Visible = false;
        tbS4Kit5.Visible = false;
        tbS4Kit6.Visible = false;
        tbS4Kit7.Visible = false;
        tbS4Kit8.Visible = false;
        tbS4Kit9.Visible = false;
        tbS4Kit10.Visible = false;

        // Make textboxes visible
        switch (ddlS4NumKits.SelectedValue)
        {
            case "0":
                {
                    tbS4Kit1.Visible = false;
                    tbS4Kit2.Visible = false;
                    tbS4Kit3.Visible = false;
                    tbS4Kit4.Visible = false;
                    tbS4Kit5.Visible = false;
                    tbS4Kit6.Visible = false;
                    tbS4Kit7.Visible = false;
                    tbS4Kit8.Visible = false;
                    tbS4Kit9.Visible = false;
                    tbS4Kit10.Visible = false;
                    break;
                }
            case "1":
                {
                    tbS4Kit1.Visible = true;
                    break;
                }
            case "2":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    break;
                }
            case "3":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    break;
                }
            case "4":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    break;
                }
            case "5":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    break;
                }
            case "6":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    tbS4Kit6.Visible = true;
                    break;
                }
            case "7":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    tbS4Kit6.Visible = true;
                    tbS4Kit7.Visible = true;
                    break;
                }
            case "8":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    tbS4Kit6.Visible = true;
                    tbS4Kit7.Visible = true;
                    tbS4Kit8.Visible = true;
                    break;
                }
            case "9":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    tbS4Kit6.Visible = true;
                    tbS4Kit7.Visible = true;
                    tbS4Kit8.Visible = true;
                    tbS4Kit9.Visible = true;
                    break;
                }
            case "10":
                {
                    tbS4Kit1.Visible = true;
                    tbS4Kit2.Visible = true;
                    tbS4Kit3.Visible = true;
                    tbS4Kit4.Visible = true;
                    tbS4Kit5.Visible = true;
                    tbS4Kit6.Visible = true;
                    tbS4Kit7.Visible = true;
                    tbS4Kit8.Visible = true;
                    tbS4Kit9.Visible = true;
                    tbS4Kit10.Visible = true;
                    break;
                }
            }
        
    }

    public void AssignKits(int VisitID, int SchoolID)
    {
        switch (ddlS4NumKits.SelectedValue)
        {
            case "1":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    break;
                }
            case "2":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    break;
                }
            case "3":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    break;
                }
            case "4":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    break;
                }
            case "5":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    break;
                }
            case "6":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    tbS4Kit6.Text = SVC.GetKitNumber(VisitID, SchoolID, 6).ToString();
                    break;
                }
            case "7":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    tbS4Kit6.Text = SVC.GetKitNumber(VisitID, SchoolID, 6).ToString();
                    tbS4Kit7.Text = SVC.GetKitNumber(VisitID, SchoolID, 7).ToString();
                    break;
                }
            case "8":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    tbS4Kit6.Text = SVC.GetKitNumber(VisitID, SchoolID, 6).ToString();
                    tbS4Kit7.Text = SVC.GetKitNumber(VisitID, SchoolID, 7).ToString();
                    tbS4Kit8.Text = SVC.GetKitNumber(VisitID, SchoolID, 8).ToString();
                    break;
                }
            case "9":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    tbS4Kit6.Text = SVC.GetKitNumber(VisitID, SchoolID, 6).ToString();
                    tbS4Kit7.Text = SVC.GetKitNumber(VisitID, SchoolID, 7).ToString();
                    tbS4Kit8.Text = SVC.GetKitNumber(VisitID, SchoolID, 8).ToString();
                    tbS4Kit9.Text = SVC.GetKitNumber(VisitID, SchoolID, 9).ToString();
                    break;
                }
            case "10":
                {
                    tbS4Kit1.Text = SVC.GetKitNumber(VisitID, SchoolID, 1).ToString();
                    tbS4Kit2.Text = SVC.GetKitNumber(VisitID, SchoolID, 2).ToString();
                    tbS4Kit3.Text = SVC.GetKitNumber(VisitID, SchoolID, 3).ToString();
                    tbS4Kit4.Text = SVC.GetKitNumber(VisitID, SchoolID, 4).ToString();
                    tbS4Kit5.Text = SVC.GetKitNumber(VisitID, SchoolID, 5).ToString();
                    tbS4Kit6.Text = SVC.GetKitNumber(VisitID, SchoolID, 6).ToString();
                    tbS4Kit7.Text = SVC.GetKitNumber(VisitID, SchoolID, 7).ToString();
                    tbS4Kit8.Text = SVC.GetKitNumber(VisitID, SchoolID, 8).ToString();
                    tbS4Kit9.Text = SVC.GetKitNumber(VisitID, SchoolID, 9).ToString();
                    tbS4Kit10.Text = SVC.GetKitNumber(VisitID, SchoolID, 10).ToString();
                    break;
                }        
        }
    }

    public void Submit(int Step)
    {
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(VisitDate.ToString()).ToString());
        string SchoolName = ddlSchoolName.SelectedValue;
        int SchoolID = int.Parse(SchoolData.GetSchoolID(SchoolName).ToString());
        string User = Session["username"].ToString();
        string SQLStatement = "";
        string LastEdit = "Last edited by: " + User + " on " + DateTime.Now.ToString("g");
        
        //Select which block to run based on Step number
        switch (Step)
        {
            //Step 1
            case 1:
                {
                    //Check if school type DDLhas been filled out
                    if (ddlS1SchoolType.SelectedIndex == 0)
                    {
                        lblError.Text = "Please select a school type from the STEP 1: TEACHER ONLY section before submitting.";
                        return;
                    }
                    else
                    {
                        //Check if row for visit ID is already in SVCFP table
                        if (SVC.LoadTable(VisitID, SchoolID, "visitID").ToString() == "")
                        {
                            SQLStatement = @"INSERT INTO schoolVisitChecklistFP (visitID, schoolID, lastEditS1, completeS1, schoolTypeS1, completeS2, completeS3, completeS4, completeS5)
                                            VALUES ('" + VisitID + "', '" + SchoolID + "', '" + LastEdit + "', 1, '" + ddlS1SchoolType.SelectedValue + "', 0, 0, 0, 0)";

                            //Insert into kits table
                            SQL.ExecuteSQL("INSERT INTO kitsFP (visitID, schoolID) VALUES ('" + VisitID + "', '" + SchoolID + "')");
                        }
                        else
                        {
                            SQLStatement = "UPDATE schoolVisitChecklistFP SET lastEditS1='" + LastEdit + "', completeS1=1, schoolTypeS1='" + ddlS1SchoolType.SelectedValue + "', formReceivedS1='" + tbS1StudentForm.Text + "' WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";                           
                        }
                        
                    }

                    break;
                }

            //Step 2
            case 2:
                {
                    //Check if step 1 is completed
                    if (SVC.LoadTable(VisitID, SchoolID, "completeS1").ToString() == "True")
                    {
                        //Check if invoice or directors checkbox is checked
                        if (chkS2Invoice.Checked == false || chkS2Director.Checked == false)
                        {
                            lblError.Text = "Please mark off the invoice signature or the director's signature before submitting.";
                            return;
                        }
                        else
                        {
                            SQLStatement = "UPDATE schoolVisitChecklistFP SET invoiceIssuedS2='" + chkS2Invoice.Checked + "', directorSignS2='" + chkS2Director.Checked + "', lastEditS2='" + LastEdit + "', completeS2=1 WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";
                        }
                    }
                    else
                    {
                        lblError.Text = "Step 1 is not yet completed. Please inform an FP teacher that their step is not completed before completing Step 2.";
                        return;
                    }
                    break;
                }

            //Step 3
            case 3:
                {
                    //Check if step 2 is completed
                    if (SVC.LoadTable(VisitID, SchoolID, "completeS2").ToString() == "True")
                    {
                        //Check if step 3 fields are blank
                        if (tbS3Contract.Text == "" || tbS3Invoice.Text == "" || ddlS3Delivery.SelectedIndex == 0)
                        {
                            lblError.Text = "Please enter the date the contract was received on, the invoice number, and delivery method before submitting.";
                            return;
                        }
                        else
                        {
                            SQLStatement = "UPDATE schoolVisitChecklistFP SET contractReceivedS3='" + tbS3Contract.Text + "', invoiceS3='" + tbS3Invoice.Text + "', deliveryMethodS3='" + ddlS3Delivery.SelectedValue + "', notesS3='" + tbS3Notes.Text + "', lastEditS3='" + LastEdit + "', completeS3=1 WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";
                        }
                    }
                    else
                    {
                        lblError.Text = "Step 2 is not yet completed. Please inform the bookkeeper that their step is not completed before completing Step 3.";
                        return;
                    }
                    break;
                }

            //Step 4
            case 4:
                {
                    //Check if step 3 is completed
                    if (SVC.LoadTable(VisitID, SchoolID, "completeS3").ToString() == "True")
                    {
                        //Check if step 4 kit 1 or workbook textboxes are blank
                        if (tbS4Kit1.Text == "" || tbS4Workbooks.Text == "")
                        {
                            lblError.Text = "Please enter a kit number and materials included number before submitting.";
                            return;
                        }
                        else
                        {
                            SQLStatement = "UPDATE schoolVisitChecklistFP SET lastEditS4='" + LastEdit + "', completeS4=1 WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";

                            //Update kits table
                            SQL.ExecuteSQL("UPDATE kitsFP SET workbooks='" + tbS4Workbooks.Text + "', kitTotal='" + ddlS4NumKits.SelectedValue + "', kit1='" + tbS4Kit1.Text + "', kit2='" + tbS4Kit2.Text + "', kit3='" + tbS4Kit3.Text + "', kit4='" + tbS4Kit4.Text + "', kit5='" + tbS4Kit5.Text + "', kit6='" + tbS4Kit6.Text + "', kit7='" + tbS4Kit7.Text + "', kit8='" + tbS4Kit8.Text + "', kit9='" + tbS4Kit9.Text + "', kit10='" + tbS4Kit10.Text + "' WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'");

                        }
                    }
                    else
                    {
                        lblError.Text = "Step 3 is not yet completed. Please inform the front office that their step is not completed before completing Step 4.";
                        return;
                    }
                    break;
                }

            //Step 5
            case 5:
                {
                    //Check if step 4 is completed
                    if (SVC.LoadTable(VisitID, SchoolID, "completeS4").ToString() == "True")
                    {
                        //Check if step 3 fields are blank
                        if (tbS5DelAcc.Text == "" || tbS5Position.Text == "" || tbS5DateAcc.Text == "")
                        {
                            lblError.Text = "Please enter who accepted the delivery, a position, and the date accepted before submitting.";
                            return;
                        }
                        else
                        {
                            SQLStatement = "UPDATE schoolVisitChecklistFP SET deliveryByS5='" + tbS5DelAcc.Text + "', dateByS5='" + tbS5DateAcc.Text + "', positionS5='" + tbS5Position.Text + "', lastEditS5='" + LastEdit + "', completeS5=1 WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";
                        }
                    }
                    else
                    {
                        lblError.Text = "Step 4 is not yet completed. Please inform a TA that their step is not completed before completing Step 5.";
                        return;
                    }
                    break;
                }

        }

        //Submit new entry
        SQL.ExecuteSQL(SQLStatement);

        //Show success message
        lblError.Text = "Submission successful!";

        //Refresh page
    }
    


    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        //Check if visit date is entered
        if (tbVisitDate.Text != "")
        {
            //Check if visit date is a valid visit date
            if (int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString()) != 0)
            {
                //Clear label
                lblError.Text = "";
                
                //Show and load visiting schools DDL
                SchoolData.LoadVisitDateSchoolsDDL(tbVisitDate.Text, ddlSchoolName);
                divSchoolName.Visible = true;
            }
            else
            {
                divSchoolName.Visible = false;
                lblError.Text = "No visit scheduled for selected date.";
                return;
            }
        }
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Check if school name is selected
        if (ddlSchoolName.SelectedIndex != 0) 
        {
            //Make buttons and checklist visible
            divButtons.Visible = true;
            divChecklist.Visible = true;

            //Load data
            LoadData();
        }
        else
        {
            divButtons.Visible = false;
            divChecklist.Visible = false;
        }
    }

    protected void ddlS4NumKits_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowKits();
    }



    protected void btnPrintFull_Click(object sender, EventArgs e)
    {
        //Make divs and labels invisible for ticket, make FP logo visible at bottom
        divTitlePrint.Visible = true;
        lblS1Comp.Visible = false;
        lblS1LastEdit.Visible = false;
        lblS2Comp.Visible = false;
        lblS2LastEdit.Visible = false;
        lblS3Comp.Visible = false;
        lblS3LastEdit.Visible = false;
        lblS4Comp.Visible = false;
        lblS4LastEdit.Visible = false;
        lblS5Comp.Visible = false;
        lblS5LastEdit.Visible = false;

        imgStavrosLogo.Visible = true;

        //Print
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);
    }

    protected void btnPrintTicket_Click(object sender, EventArgs e)
    {
        string VisitID = VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString();
        string SchoolID = SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString();

        //Redirect to Delivery Ticket and include the school ID and visit ID in the url
        Response.Redirect("../Forms/Delivery_Ticket.aspx?b=" + VisitID + "?c=" + SchoolID);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("School_Visit_Checklist.aspx");
    }

    protected void btnS1Submit_Click(object sender, EventArgs e)
    {
            Submit(1);

    }

    protected void btnS2Submit_Click(object sender, EventArgs e)
    {
            Submit(2);

    }

    protected void btnS3Submit_Click(object sender, EventArgs e)
    {
            Submit(3);
    }

    protected void btnS4Submit_Click(object sender, EventArgs e)
    {
            Submit(4);
    }

    protected void btnS5Submit_Click(object sender, EventArgs e)
    {
            Submit(5);
    }
}