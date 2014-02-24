using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Contests_GetList : System.Web.UI.Page
    {
        public string ContestsList = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/XML";
            int page = Convert.ToInt32(Request.Form["Page"]);
            var contest_list = (from c in Bll.ContestHelper.List
                                select c).Skip(page * 20).Take(20);
            foreach (var c in contest_list)
            {
                ContestsList += String.Format("<Contest><Status>{0}</Status><Title>{1}</Title><BeginTime>{2}</BeginTime><Length>{3}</Length><Type>{4}</Type><Server>{5}</Server></Contest>",
                    c.Status.ToString(),
                    HttpUtility.HtmlEncode(c.Title),
                    c.BeginTime,
                    (c.EndTime - c.BeginTime).TotalHours.ToString("0.00"),
                    c.Type.ToString(),
                    HttpUtility.HtmlEncode(c.CloudServer.Name) + "(" + HttpUtility.HtmlEncode(c.CloudServer.Address) + ":" + c.CloudServer.Port + ")");
            }
        }
    }
}