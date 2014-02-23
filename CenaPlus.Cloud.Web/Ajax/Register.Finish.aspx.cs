using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Register_Finish : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmailOK"] == null)
            {
                Response.Write("Error");
                return;
            }

            var username = Request.Form["Username"];
            var password = Request.Form["Password"];
            
            var email = Session["RegisterEmail"].ToString();
            using (Dal.Cloud db = new Dal.Cloud())
            {
                var IsExisted = (from u in db.Users
                                 where u.Name == username
                                 select u.ID).Count() == 0 ? false : true;
                if (IsExisted)
                {
                    Response.Write("Existed");
                    return;
                }
                else
                {
                    db.Users.Add(new Entity.User()
                    {
                        Email = email,
                        Gravatar = email,
                        Password = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password)),
                        Name = username,
                        NickName = username,
                        Role = Entity.UserRole.User
                    });
                    db.SaveChanges();
                    Response.Write("OK");
                }
            }
        }
    }
}