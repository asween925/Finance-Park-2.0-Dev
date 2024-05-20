using FP2Dev;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Page : System.Web.UI.Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    Class_VisitData VisitData = new Class_VisitData();
    Class_SchoolData SchoolData = new Class_SchoolData();
    Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    ValuesController1 APIs = new ValuesController1();

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

    public Home_Page()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
        Load += Page_Load;
    }

    protected async void Page_Load(object sender, EventArgs e)
    {       
       lblVisitID.Text = VisitData.GetVisitID().ToString();

        //Load header data
        lblTodayDate.Text = DateTime.Now.ToShortDateString();
        lblStudentCount.Text = VisitData.LoadVisitInfoFromDate(DateTime.Now.ToShortDateString(), "studentCount").ToString();
        lblSchoolName.Text = SchoolHeader.GetSchoolHeader().ToString();

        // Populating school header
        lblHeaderSchoolName.Text = SchoolHeader.GetSchoolHeader().ToString();

        //Test api values
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Largo%2C%20FL?unitGroup=metric&key=US8HBUA3APA562EDF8A4K7W2L&contentType=json");
        //var request = new HttpRequestMessage(HttpMethod.Get, "https://www.dnd5eapi.co/api/ability-scores/cha");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode(); //Throw exception if error

        var body = await response.Content.ReadAsStringAsync();

        dynamic weather = JsonConvert.DeserializeObject(body);

        var day = weather.days;

        double MaxTemp = day[0].tempmax;

        //Convert to F
        MaxTemp = (MaxTemp * 9) / 5 + 32;

        lblError.Text = MaxTemp.ToString();
    }
}