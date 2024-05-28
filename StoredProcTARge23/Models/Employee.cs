using System.ComponentModel.DataAnnotations;

namespace StoredProcTARge23.Models
{
    public class Employee
    {
        [Key]

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Salary { get; set; }
    }
}
