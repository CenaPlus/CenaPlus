using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Server_ContestRefresh : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var client_secret = Request.Form["ClientSecret"].ToString();
            using (Dal.Cloud db = new Dal.Cloud())
            {
                var server = (from s in db.CloudServers
                              where s.ClientSecret == client_secret
                              select s).FirstOrDefault();
                if (server != null)
                {
                    Response.Write("Access denied");
                    return;
                }
                bool IsExisted = (from c in db.Contests
                                  where c.CloudServerID == server.ID
                                  && DateTime.Now < c.BeginTime
                                  select c.ID).Count() > 0 ? true : false;
                if (!IsExisted)
                {
                    db.Contests.Add(new Entity.Contest()
                    {
                        BeginTime = Convert.ToDateTime(Request.Form["BeginTime"]),
                        EndTime = Convert.ToDateTime(Request.Form["EndTime"]),
                        Type = (Entity.ContestType)(Convert.ToInt32(Request.Form["Type"])),
                        Title = Request.Form["Title"].ToString(),
                        CloudServerID = server.ID
                    });
                }
                else
                {
                    var contest = db.Contests.Find((from c in db.Contests
                                                    where c.CloudServerID == server.ID
                                                    && DateTime.Now < c.BeginTime
                                                    select c.ID).FirstOrDefault());
                    contest.Title = Request.Form["Title"].ToString();
                    contest.BeginTime = Convert.ToDateTime(Request.Form["BeginTime"]);
                    contest.EndTime = Convert.ToDateTime(Request.Form["EndTime"]);
                    contest.Type = (Entity.ContestType)(Convert.ToInt32(Request.Form["Type"]));
                }
                db.SaveChanges();
            }
        }
    }
}