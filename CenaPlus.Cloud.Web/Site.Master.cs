using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public string LoginStr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                LoginStr = "<li id=\"navUserCenter\"><a href=\"/UserCenter\" accesskey=\"6\" title=\"\">用户中心</a></li>";
                LoginStr += "<li id=\"navLogout\"><a href=\"/UserCenter\" accesskey=\"7\" title=\"\">注销</a></li>";
            }
            else
            {
                LoginStr = "<li id=\"navLogin\"><a href=\"/UserCenter\" accesskey=\"6\" title=\"\">登录</a></li>\r\n";
                LoginStr += "<li id=\"navRegister\"><a href=\"/UserCenter\" accesskey=\"7\" title=\"\">注册</a></li>";
            }
        }
    }
}