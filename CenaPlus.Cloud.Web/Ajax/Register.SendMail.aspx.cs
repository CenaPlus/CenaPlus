using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CenaPlus.Cloud.Entity;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Register_SendMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RegisterEmail"] != null)
            {
                Response.Write("Failed");
                return;
            }
            var email = Request.Form["Email"].ToString().ToLower();
            var area = (email.Split('@'))[1].ToLower();
            using (Dal.Cloud db = new Dal.Cloud())
            {
                bool IsForbidden = (from f in db.EmailForbiddens
                                    where f.Address.ToLower() == area
                                    select f.ID).Count() == 0 ? false : true;
                if (IsForbidden)
                {
                    Response.Write("WrongAddress");
                    return;
                }
                bool IsExisted = (from u in db.Users
                                  where u.Email.ToLower() == email
                                  select u.ID).Count() == 0 ? false : true;
                if (IsExisted)
                {
                    Response.Write("Existed");
                    return;
                }
            }
            Session["RegisterEmail"] = Request.Form["Email"].ToString();
            Random r = new Random();
            var Code = RandomString(r, 64);
            Session["EmailCode"] = Code;
            Bll.SMTPHelper.Send(Session["RegisterEmail"].ToString(), "Cena+ 用户注册验证", "请将验证码粘贴到注册页面中以继续完成注册！\r\n" + Code);
            Response.Write("OK");
        }
        private string RandomString(Random rand, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int ch = rand.Next(26 + 26 + 10);
                if (ch < 26) sb.Append((char)(ch + 'A'));
                else if (ch < 26 + 26) sb.Append((char)(ch - 26 + 'a'));
                else sb.Append((char)(ch - 26 - 26 + '0'));
            }
            return sb.ToString();
        }
    }
}