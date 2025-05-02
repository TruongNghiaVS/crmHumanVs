using VS.Human.Rep.Model;

namespace crmHuman.DisplayModel
{
    public class EmployeeDisplayEdit : Employee
    {
        public string DayOfBirthDisplay
        {
            get
            {
                if (Dob.HasValue)
                {
                    return Dob.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }


          public string NationalDateDisplay
        {
            get
            {
                if (NationalDate.HasValue)
                {
                    return NationalDate.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }
    }
}
