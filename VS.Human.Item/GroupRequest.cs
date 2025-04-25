﻿namespace VS.Human.Item
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


    public class MasterRequestInput : BaseRequest
    {

        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? Type { get; set; }

        public int? ApplyFor { get; set; }




        public MasterRequestInput() : base()
        {
            Type = "hdld";

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


     public class ScheduleInterviewRquest : BaseRequest
    {

        public int? RelId { get; set; }
        public string? RelCode { get; set; }

        public int? Type { get; set; }
        
        public ScheduleInterviewRquest() : base()
        {

        }

    }

    public class DocumentDataRquest : BaseRequest
    {
        public int? RelId { get; set; }
        public string? RelCode { get; set; }

        public DocumentDataRquest() : base()
        {

        }

    }
}
