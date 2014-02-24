using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CenaPlus.Cloud.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Bll.ContestHelper.Rebuild();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * 60 * 30;
            timer.Elapsed += timer_Elapsed;
        }

        protected void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Bll.ContestHelper.Rebuild();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}