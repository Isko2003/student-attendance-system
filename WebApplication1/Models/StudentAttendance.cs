namespace WebApplication1.Models
{
    public class StudentAttendance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public bool IsPresent => Status == "Present";
    }
}
