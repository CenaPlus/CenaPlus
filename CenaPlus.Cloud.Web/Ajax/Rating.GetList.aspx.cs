using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CenaPlus.Cloud.Web.Ajax
{
    public partial class Rating_GetList : System.Web.UI.Page
    {
        public string RatingList = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(Request.Form["Page"]);
            using (Dal.Cloud db = new Dal.Cloud())
            {
                var users = (from u in db.Users
                            let s = u.Ratings.Any() ? u.Ratings.Sum(x => x.RatingChange) : 0
                            orderby u.Ratings.Sum(x => x.RatingChange) descending
                            select u).Skip(page * 50).Take(50).ToList();
                foreach (var u in users)
                {
                    RatingList += String.Format("<Rating><Rank>{0}</Rank><NickName>{1}</NickName><Gravatar>{2}</Gravatar><Score>{3}</Score></Rating>",
                        u.NickName,
                        u.GetGravatar(),
                        u.Ratings.Sum(x => x.RatingChange) + 1500);
                }
            }
        }
    }
}