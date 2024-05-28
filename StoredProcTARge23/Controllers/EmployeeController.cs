using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcTARge23.Data;
using StoredProcTARge23.Models;

namespace StoredProcTARge23.Controllers
{
    public class EmployeeController : Controller
    {
        public StoredProcDbContext _context;
        public IConfiguration _config;

        public EmployeeController
            (
                StoredProcDbContext context,
                IConfiguration config
            )
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<Employee> SearchResult()
        {
            var result = _context.Employees
                .FromSqlRaw<Employee>("spSearchEmployees")
                .ToList();

            return result;
        }

        [HttpGet]
        public IActionResult DynamicSQL()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> models = new List<Employee>();
                while (sdr.Read()) 
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    models.Add( details );
                }
                return View(models);
            }
        }
    }
}
