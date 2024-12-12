using System.Data;

namespace crmHuman.Model
{
    public class UserActive
    {
        public UserActive()
        {
            DataUsser = new List<UserItem>();

        }

        private static UserActive instance = null;
        private List<UserItem> DataUsser { get; set; }
        public void AddOrUpdate(string UserId, string userName, string fullName)
        {
            var itemUser = new UserItem()
            {
                LastUpdated = DateTime.Now,
                UserId = UserId,
                UserName = userName,
                FullName = fullName

            };

            var itemUserData = DataUsser.Where(x => x.UserId == UserId).FirstOrDefault();
            if (itemUserData == null)
            {
                DataUsser.Add(itemUser);
            }
            else
            {
                itemUserData.LastUpdated = DateTime.Now;
            }
        }

        public static UserActive DataActiveOnline
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserActive();
                }
                return instance;
            }
        }


        public int GetCountUserOnline()
        {
            var datetiemDiff = DateTime.Now.AddMinutes(-3);

            var allUserActive = DataUsser
                                    .Where(x => x.LastUpdated > datetiemDiff)
                                    .Count();

            return allUserActive;
        }

        public List<UserItem> GetListUser(int UserId)
        {
            if (UserId == 17)
                return DataUsser;

            if (UserId == 37)
            {
                var tempCondition = new List<string>
                {
                    "41",
                    "48",
                    "49",
                    "50",
                    "51",
                    "37"
                };
                var dataUserTemp = DataUsser.Where(x => x.StatusOnline == "Online").Where(x => tempCondition.Contains(x.UserId));
                return dataUserTemp.ToList();

            }


            if (UserId == 38)
            {
                var tempCondition = new List<string>
                {
                    "39",
                    "40",
                    "43",
                    "44",
                    "45",
                    "46",
                    "47",
                    "38"
                };
                var dataUserTemp = DataUsser.Where(x => tempCondition.Contains(x.UserId));
                return dataUserTemp.ToList();

            }

            return new List<UserItem>();
        }
    }

    public class UserItem
    {
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string? UserId { get; set; }

        public string StatusOnline
        {
            get
            {
                var datecompare = DateTime.Now.AddMinutes(-3);

                if (LastUpdated >= datecompare)
                {
                    return "Online";
                }
                return "Off";

            }
        }
    }
}