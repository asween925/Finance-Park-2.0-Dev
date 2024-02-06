using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void reset_btn_Click(object sender, EventArgs e)
    {
        firstName_tb.Text = "";
        lastName_tb.Text = "";

    }
}