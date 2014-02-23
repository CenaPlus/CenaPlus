using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Register_EmailAuthentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var code = Request.Form["Code"].ToString();
            if (code == Session["EmailCode"].ToString())
            {
                Session["EmailOK"] = true;
                Response.Write("OK");
            }
            else
            {
                Response.Write("Failed");
            }
        }
    }
}