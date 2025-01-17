﻿namespace VS.Human.Business.Model
{
    public class ReportQuerryTaltimeIndex
    {
        public string? LineCode { get; set; }
        public string? NoAgree { get; set; }

        public string? PhoneLog { get; set; }

        public string? FileRecording { get; set; }

        public int? Duration { get; set; }

        public int? CompanyId { get; set; }

        public int? CampangnId { get; set; }

        public DateTime? CallDate { get; set; }

        public DateTime? EventTime { get; set; }

        public string Linkedid { get; set; }

        public string Disposition { get; set; }

        public int? SourceCall { get; set; }

        public int DurationBill { get; set; }
        public double DurationReal { get; set; }
        public string? Lastapp { get; set; }
        public string? Lastdata { get; set; }
    }


    public class ReportQuerryRecordingFileIndex
    {
        public string File { get; set; }
        public int? Duration { get; set; }

        public int updated { get; set; }


    }
}
