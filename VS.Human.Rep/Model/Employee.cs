namespace VS.Human.Rep.Model
{
    public class Employee : BaseModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? LineCode { get; set; }

        public string? ColorCode { get; set; }

        public DateTime? Onboard { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Noted { get; set; }
        public string? RoleCode { get; set; }
        public string? Pass { get; set; }
        public DateTime? Dob { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }
        public string? AvatarFile { get; set; }
    }
}
