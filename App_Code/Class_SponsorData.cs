﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public class Class_SponsorData
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

    public Class_SponsorData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    //Load sponsor name ddl
    public object LoadSponsorNamesDDL(DropDownList sponsorNameDDL)
    {
        // Clear out teacher and school DDLs
        sponsorNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT sponsorName FROM sponsorsFP ORDER BY sponsorName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                sponsorNameDDL.Items.Add(dr[0].ToString());
            sponsorNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            
        }

        return sponsorNameDDL.Items;
    }


    //Gets sponsor name
    public string GetSponsorName(int SponsorID)
    {
        string SponsorName = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT sponsorName FROM sponsorsFP WHERE id='" + SponsorID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            SponsorName = dr["sponsorName"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return SponsorName;
    }


    //Get sponsor ID from the name
    public int GetSponsorID(string SponsorName)
    {
        int SponsorID = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT id FROM sponsorsFP WHERE sponsorName='" + SponsorName + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            SponsorID = int.Parse(dr["id"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return SponsorID;
    }


    //Get the IDs os all sponsors for a business ID
    public List<int> GetSponsorIDs(int BusinessID)
    {
        List<int> SponsorIDs = new List<int>();       

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT id FROM sponsorsFP WHERE businessID='" + BusinessID + "' OR businessID2='" + BusinessID + "' OR businessID3='" + BusinessID + "' OR businessID4='" + BusinessID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            SponsorIDs.Add(int.Parse(dr[0].ToString()));
        }

        cmd.Dispose();
        con.Close();

        return SponsorIDs;
    }


    //Get logo path from sponsor ID
    public string GetSponsorLogoFromID (int SponsorID)
    {
        string Logo = "";
        
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT logoPath FROM sponsorsFP WHERE id='" + SponsorID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Logo = dr["logoPath"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return Logo;
    }

}