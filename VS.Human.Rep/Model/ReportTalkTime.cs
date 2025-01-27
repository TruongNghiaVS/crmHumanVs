﻿namespace VS.Human.Rep.Model
{
    public class ReportTalkTime : BaseModel
    {
        public string? LineCode { get; set; }
        public string? NoAgree { get; set; }

        public string? PhoneLog { get; set; }

        public string? FileRecording { get; set; }
        public int? VendorId { get; set; }
        public int? Duration { get; set; }

        public int? CompanyId { get; set; }

        public int? CampangnId { get; set; }

        public DateTime? CallDate { get; set; }

        public DateTime? EventTime { get; set; }

        public string? Linkedid { get; set; }

        public string? Disposition { get; set; }

        public double? DurationReal { get; set; }


        public int DurationBill { get; set; }

        public int? Sourcecall { get; set; }


        public string Lastapp { get; set; }
        public string LastData { get; set; }
        public ReportTalkTime()
        {

        }

    }


}
