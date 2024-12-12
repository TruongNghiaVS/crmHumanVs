namespace crmHuman.Model
{
    public class GlobalVar
    {
        private static GlobalVar instance = null;

        public int? TotalUser { get; set; }

        public int? TotalSumOnline { get; set; }

        public List<object> DataUser { get; set; }


        public int TotalCaseSource { get; set; }
        public int TotalCaseSourceCTV { get; set; }

        private GlobalVar()
        {
            TotalCaseSource = 10;
            DataUser = new List<object>();
            TotalSumOnline = 10;
            TotalUser = 25;
            TotalCaseSourceCTV = 0;
        }




        public static GlobalVar GlobalData
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalVar();
                }
                return instance;
            }
        }


    }
}
