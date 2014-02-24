using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (Dal.Cloud db = new Dal.Cloud())
            { 
                var user = (from u in db.Users
                                where u.Name == Request.Form["Username"]
                                && u.Password == SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(Request.Form["Password"]))
                                select u).FirstOrDefault();
                if(user == null)
                {
                    Response.Write("Failed");
                    return;
                }
                else
                {
                    Session["User"] = user;
                    Response.Write("OK");
                }
            }
        }
    }
}