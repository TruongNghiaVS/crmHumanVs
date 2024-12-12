namespace crmHuman.Model
{



    public class GlobalGroupMember
    {
        private static GlobalGroupMember instance = null;

        public List<SelectDisplay> GroupEm { get; set; }

        public List<SelectDisplay> MemberGroups { get; set; }
        private GlobalGroupMember()
        {
            GroupEm = new List<SelectDisplay>();
            MemberGroups = new List<SelectDisplay>();
        }
        public void AddGroup(SelectDisplay item)
        {
            var temp = GroupEm.Where(x => x.LinkId == item.LinkId).FirstOrDefault();
            if (temp != null)
            {
                temp = item;
                return;
            }

            GroupEm.Add(item);
        }

        public void AddMember(SelectDisplay item)
        {
            var temp = MemberGroups.Where(x => x.LinkId == item.LinkId).FirstOrDefault();
            if (temp != null)
            {
                temp = item;
                return;
            }

            MemberGroups.Add(item);
        }

        public List<SelectDisplay> GetAllMemberByGroupId(int groupId)
        {
            if (groupId == -1)
            {
                return MemberGroups;
            }
            return MemberGroups.Where(x => x.RelId == groupId).ToList();
        }

        public List<SelectDisplay> GetAllGroup()
        {
            return GroupEm;
        }
        public static GlobalGroupMember GlobalData
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalGroupMember();
                }
                return instance;
            }
        }


    }
}
