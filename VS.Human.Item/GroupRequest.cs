namespace VS.Human.Item
{
    public class GroupRequest : BaseRequest
    {

        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ManagerId { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }



        public GroupRequest() : base()
        {
            Status = 1;
            IsActive = 1;
        }


    }


    public class CommonRequest : BaseRequest
    {

        public string? Code { get; set; }
        public string? Name { get; set; }

        public int? Type { get; set; }

        public int? ApplyFor { get; set; }



        public CommonRequest() : base()
        {
            Type = -1;
            ApplyFor = 2;

        }


    }
}
