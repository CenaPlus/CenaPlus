using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Client.Bll
{
    public static class StandingsCache
    {
        public static Hashtable Standings = new System.Collections.Hashtable();
        public static void UpdateSingleUser(int contest_id, Entity.StandingItem si)
        {
            var userindex = (Standings[contest_id] as List<Entity.StandingItem>).FindIndex(x => x.UserID == si.UserID);
            if (userindex == -1)
            {
                (Standings[contest_id] as List<Entity.StandingItem>).Add(si);
            }
            else
            {
                (Standings[contest_id] as List<Entity.StandingItem>)[userindex] = si;
            }
        }
    }
}
