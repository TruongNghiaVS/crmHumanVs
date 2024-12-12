namespace crmHuman.Model
{
    public class UserDataView
    {
        public UserDataView()
        {

        }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName
        {
            get
            {
                if (RoleCode == "1")
                {
                    return "Admin";
                }
                else if (RoleCode == "2")
                {
                    return "TC";
                }
                else if (RoleCode == "3")
                {
                    return "Team lead";
                }
                else if (RoleCode == "4")
                {
                    return "Marketing";
                }
                else if (RoleCode == "6")
                {
                    return "CTV";
                }
                return "Không rõ vai trò";

            }
        }
        public int UserId { get; set; }
        public string RoleCode { get; set; }

        public string LineCode { get; set; }
    }
}
