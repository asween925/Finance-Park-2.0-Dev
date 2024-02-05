using FP2Dev;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Home_Page : System.Web.UI.Page
{
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    string connection_string;
    SqlDataReader dr;
    Class_VisitData VisitData = new Class_VisitData();
    
    //Dim SchoolData As New Class_SchoolData
    //Dim SQL As New Class_SQLCommands
    //Dim StudentData As New Class_StudentData
    //Dim VisitData As New Class_VisitData
    //Dim VisitID As Integer = VisitData.GetVisitID
    //Dim SchoolHeader As New Class_SchoolHeader
    //Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
    //var MondayVisitDate = DateTime.Today.AddDays(-1 * dayOfWeek + 1);
    //Dim TuesdayVisitDate = DateTime.Today.AddDays(2 - dayOfWeek);
    //Dim WednesdayVisitDate = DateTime.Today.AddDays(3 - dayOfWeek);
    //Dim ThursdayVisitDate = DateTime.Today.AddDays(4 - dayOfWeek);
    //Dim FridayVisitDate = DateTime.Today.AddDays(5 - dayOfWeek);

    protected void Page_Load(object sender, EventArgs e)
    {       
       visitID_lbl.Text = (VisitData.GetVisitID()).ToString();
    }
}