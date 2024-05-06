using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_BusinessData
{
    private Class_VisitData VisitID = new Class_VisitData();
    private int Visit;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string ConnectionString;

    public Class_BusinessData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object GetBusinessID(string businessName)
    {
        string returnBusinessID = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT ID FROM businessInfoFP WHERE businessName = '" + businessName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnBusinessID = dr["ID"].ToString();
            cmd.Dispose();
            con.Close();
            return returnBusinessID;
        }

        cmd.Dispose();
        con.Close();

        return returnBusinessID;
    }



    //Get business name from ID
    public string GetBusinessName(int BusinessID)
    {
        string BusinessName = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT businessName FROM businessInfoFP WHERE id='" + BusinessID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            BusinessName = dr["businessName"].ToString();
        }

        con.Close();
        cmd.Dispose();

        return BusinessName;
    }



    //Load businesses into DDL
    public object LoadBusinessNamesDDL(DropDownList businessNameDDL)
    {
        string errorString;
        // Dim businessNameDDL As DropDownList

        // Clear out teacher and school DDLs
        businessNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT businessName FROM businessInfoFP ORDER BY businessName";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                businessNameDDL.Items.Add(dr[0].ToString());
            businessNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitDate. Could not get school names.";
            return errorString;
        }

        return businessNameDDL.Items;
    }



    // Update image and title based on business ID
    public (string ImagePath, string BColor, string BusinessName) GetBusinessLogos(string BusinessID)
    {
        string I = "";
        string C = "";
        string B = "";
        string logoRoot = "~/media/Logos/";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT logoPath, businessColor, businessName FROM businessinfo WHERE ID='" + BusinessID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            I = logoRoot + dr[0].ToString();
            C = dr[1].ToString();
            B = dr[2].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (I, C, B);

    }



    // Get business address
    public object GetBusinessAddress(string BusinessName)
    {
        string returnAddress = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT address FROM businessInfo WHERE businessName = '" + BusinessName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnAddress = dr["address"].ToString();
            cmd.Dispose();
            con.Close();
            return returnAddress;
        }

        cmd.Dispose();
        con.Close();

        return returnAddress;
    }



    // Get job ID from job title
    public object GetJobID(string JobTitle)
    {
        var ReturnID = default(string);

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM jobs WHERE jobTitle='" + JobTitle + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            ReturnID = dr["id"].ToString();

        cmd.Dispose();
        con.Close();

        return ReturnID;

    }



    // Gets a string of the closed businesses in EV
    public object GetClosedBusinesses(string VisitDate)
    {
        var ClosedBusinesses = default(string);

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = @"Declare @val varchar(MAX)
                               SELECT @val = COALESCE(@val + ', ' + b.businessName, b.businessName)
	                                FROM businessInfo b
                                INNER JOIN onlineBanking o
                                ON b.id = o.businessID
                                WHERE o.openstatus=0 AND o.visitDate='" + VisitDate + @"'
                                ORDER BY b.businessName
	                                SELECT @val as names";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            ClosedBusinesses = dr["names"].ToString();

        cmd.Dispose();
        con.Close();

        return ClosedBusinesses;
    }


    //Get the business name from the ID
    public object GetBusinessNameFromID(int ID)
    {
        string ReturnID = "";

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT businessName FROM jobsFP WHERE id='" + ID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnID = dr["businessName"].ToString();
        }
            
        cmd.Dispose();
        con.Close();

        return ReturnID;
    }



    //Get total number of businesses
    public object GetTotalBusinesses()
    {
        string ReturnCount = "";

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT COUNT(businessName) FROM businessInfoFP";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnCount = dr["businessName"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return ReturnCount;
    }



    //Load sponsor DDL
    public object LoadSponsorDDL(DropDownList ddlSponsor)
    {
        // Clear out teacher and school DDLs
        ddlSponsor.Items.Clear();

        // Populate school DDL from entered visit date
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT sponsorName FROM sponsorsFP ORDER BY sponsorName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                ddlSponsor.Items.Add(dr[0].ToString());
            ddlSponsor.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();

        return ddlSponsor.Items;
    }



    //Get sponsor ID from name
    public int GetSponsorID(string SponsorName)
    {
        int SponsorID = 0;

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM sponsorsFP WHERE sponsorName='" + SponsorName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            SponsorID = int.Parse(dr["id"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return SponsorID;
    }



    //Get script data from business ID
    public (string Kiosk, string Popup, string Main, string Loan, string Shopping) GetBusinessScripts (int BusinessID)
    {
        string Kiosk = "";
        string Popup = "";
        string Main = "";
        string Loan = "";
        string Shopping = "";
        string SQL = "SELECT kioskScript, researchPopupScript, researchMainScript, loanAppScript, shoppingRule FROM businessInfoFP WHERE id='" + BusinessID + "'";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = SQL;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Kiosk = dr["kioskScript"].ToString();
            Popup = dr["researchPopupScript"].ToString();
            Main = dr["researchMainScript"].ToString();
            Loan = dr["loanAppScript"].ToString();
            Shopping = dr["shoppingRule"].ToString();
        }

        cmd.Dispose();
        con.Close();
        
        return (Kiosk, Popup, Main, Loan, Shopping);
    }


    
    //Get other action business data
    public (int Action, bool Loan, bool Table, bool Retire) GetBusinessActions (int BusinessID)
    {
        int Action = 0;
        bool Loan = false;
        bool Table = false;
        bool Retire = false;

        string SQL = "SELECT hasAction, hasLoan, hasTable, hasRetire FROM businessInfoFP WHERE id='" + BusinessID + "'";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = SQL;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Action = int.Parse(dr["hasAction"].ToString());
            Loan = bool.Parse(dr["hasLoan"].ToString());
            Table = bool.Parse(dr["hasTable"].ToString());
            Retire = bool.Parse(dr["hasRetire"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return (Action, Loan, Table, Retire);
    }



    //Get action button data
    public (string Btn1Text, string BtnText2, string BtnText3, string Btn1A, string Btn2A, string Btn3A) GetActionButtons (int BusinessID)
    {
        string Btn1Text = "";
        string Btn2Text = "";
        string Btn3Text = "";
        string Btn1A = "";
        string Btn2A = "";
        string Btn3A = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT actionBtnText, actionBtnText2, actionBtnText3, actionBtn1Action, actionBtn2Action, actionBtn3Action FROM businessInfoFP WHERE id='" + BusinessID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Btn1Text = dr["actionBtnText"].ToString();
            Btn2Text = dr["actionBtnText2"].ToString();
            Btn3Text = dr["actionBtnText3"].ToString();
            Btn1A = dr["actionBtn1Action"].ToString();
            Btn2A = dr["actionBtn2Action"].ToString();
            Btn3A = dr["actionBtn3Action"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (Btn1Text, Btn2Text, Btn3Text, Btn1A, Btn2A, Btn3A);
    }



    //Get business table data
    public (string Cat1, string Cat2, string Cat3, string Cat4, string Cat5, string Cat6, string Cat1D1, string Cat1D2, string Cat1D3, string Cat1D4, string Cat2D1, string Cat2D2, string Cat2D3, string Cat2D4, string Cat3D1, string Cat3D2, string Cat3D3, string Cat3D4, string Cat4D1, string Cat4D2, string Cat4D3, string Cat4D4, string Cat5D1, string Cat5D2, string Cat5D3, string Cat5D4, string Cat6D1, string Cat6D2, string Cat6D3, string Cat6D4) GetBusinessTableData (int BusinessID)
    {
        string Cat1 = "";
        string Cat2 = "";
        string Cat3 = "";
        string Cat4 = "";
        string Cat5 = "";
        string Cat6 = "";
        string Cat1D1 = "";
        string Cat1D2 = "";
        string Cat1D3 = "";
        string Cat1D4 = "";
        string Cat2D1 = "";
        string Cat2D2 = "";
        string Cat2D3 = "";
        string Cat2D4 = "";
        string Cat3D1 = "";
        string Cat3D2 = "";
        string Cat3D3 = "";
        string Cat3D4 = "";
        string Cat4D1 = "";
        string Cat4D2 = "";
        string Cat4D3 = "";
        string Cat4D4 = "";
        string Cat5D1 = "";
        string Cat5D2 = "";
        string Cat5D3 = "";
        string Cat5D4 = "";
        string Cat6D1 = "";
        string Cat6D2 = "";
        string Cat6D3 = "";
        string Cat6D4 = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM businessInfoFP WHERE id='" + BusinessID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Cat1 = dr["tblCat1"].ToString();
            Cat2 = dr["tblCat2"].ToString();
            Cat3 = dr["tblCat3"].ToString();
            Cat4 = dr["tblCat4"].ToString();
            Cat5 = dr["tblCat5"].ToString();
            Cat6 = dr["tblCat6"].ToString();
            Cat1D1 = dr["tblCat1Data1"].ToString();
            Cat1D2 = dr["tblCat1Data2"].ToString();
            Cat1D3 = dr["tblCat1Data3"].ToString();
            Cat1D4 = dr["tblCat1Data4"].ToString();
            Cat2D1 = dr["tblCat2Data1"].ToString();
            Cat2D2 = dr["tblCat2Data2"].ToString();
            Cat2D3 = dr["tblCat2Data3"].ToString();
            Cat2D4 = dr["tblCat2Data4"].ToString();
            Cat3D1 = dr["tblCat3Data1"].ToString();
            Cat3D2 = dr["tblCat3Data2"].ToString();
            Cat3D3 = dr["tblCat3Data3"].ToString();
            Cat3D4 = dr["tblCat3Data4"].ToString();
            Cat4D1 = dr["tblCat4Data1"].ToString();
            Cat4D2 = dr["tblCat4Data2"].ToString();
            Cat4D3 = dr["tblCat4Data3"].ToString();
            Cat4D4 = dr["tblCat4Data4"].ToString();
            Cat5D1 = dr["tblCat5Data1"].ToString();
            Cat5D2 = dr["tblCat5Data2"].ToString();
            Cat5D3 = dr["tblCat5Data3"].ToString();
            Cat5D4 = dr["tblCat5Data4"].ToString();
            Cat6D1 = dr["tblCat6Data1"].ToString();
            Cat6D2 = dr["tblCat6Data2"].ToString();
            Cat6D3 = dr["tblCat6Data3"].ToString();
            Cat6D4 = dr["tblCat6Data4"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (Cat1, Cat2, Cat3, Cat4, Cat5, Cat6, Cat1D1, Cat1D2, Cat1D3, Cat1D4, Cat2D1, Cat2D2, Cat2D3, Cat2D4, Cat3D1, Cat3D2, Cat3D3, Cat3D4, Cat4D1, Cat4D2, Cat4D3, Cat4D4, Cat5D1, Cat5D2, Cat5D3, Cat5D4, Cat6D1, Cat6D2, Cat6D3, Cat6D4);

    }

}