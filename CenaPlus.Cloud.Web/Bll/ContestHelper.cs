using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace CenaPlus.Cloud.Web.Bll
{
    public static class ContestHelper
    {
        public static List<Entity.Contest> List = new List<Entity.Contest>();
        public static List<int> ServerList = new List<int>();
        public static DateTime LastRefreshTime = DateTime.Now;
        public static void Rebuild()
        {
            LastRefreshTime = DateTime.Now;
            using(Dal.Cloud db = new Dal.Cloud())
            {
                var servertmplist = (from s in db.CloudServers
                                  where s.StatusAsInt == (int)Entity.CloudServerStatus.Normal
                                  select s).ToList();
                foreach (var s in servertmplist)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        if (CheckOnline(s.Address, s.Port))
                        {
                            ServerList.Add(s.ID);
                        }
                    });
                }
                List.Clear();
                List = (from c in db.Contests
                                where ServerList.Contains(c.CloudServerID)
                                select c).ToList();
                List.Sort((x, y) => { return x.Status - y.Status; });
            }
        }
        public static bool CheckOnline(string Address, int Port)
        {
            TcpClient tcp = new TcpClient();
            try
            {
                tcp.Connect(Address, Port);
                if (tcp.Connected)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
    }
}