using CenaPlus.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace CenaPlus.Server.Bll
{
    public class JudgeNodeInfo
    {
        public IPEndPoint Location { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public bool IsOnline
        {
            get
            {
                try
                {
                    var channel = JudgeNodeChannelFactory.CreateChannel(Location, new JudgeNodeCallback());
                    channel.Close();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return false;
                }
            }
        }

        public bool CheckPassword()
        {
            using (var channel = JudgeNodeChannelFactory.CreateChannel(Location, new JudgeNodeCallback()))
            {
                return channel.Authenticate(Password);
            }
        }

        public IJudgeNodeChannel CreateConnection()
        {
            return JudgeNodeChannelFactory.CreateChannel(Location, new JudgeNodeCallback());
        }
    }
}
