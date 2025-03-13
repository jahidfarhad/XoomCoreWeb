using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Core.Entities;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.Controllers;

//[MustHaveAuthorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISessionManager _sessionManager;
    //public static List<Employee> employeeList = new List<Employee>();
    //public static List<CreateEmployeeRequest> createEmployeeRequests = new List<CreateEmployeeRequest>();
    //public static List<UpdateEmployeeRequest> updateEmployeeRequests = new List<UpdateEmployeeRequest>();
    public HomeController(ILogger<HomeController> logger, ISessionManager sessionManager)
    {
        _logger = logger;
        _sessionManager = sessionManager;

    }

    [Route("index")]
    //[MustHavePermission(SubMenuKey: "Home", Controller: "Home", Action: "Index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("GetUser")]
    public ActionResult GetUser()
    {
        return Ok(new { error = false, MenuList = _sessionManager.Current.SubMenuAuthorizedList, UserId = _sessionManager.Current.UserId });
    }

    //[Route("GetEmployeeList")]
    //[HttpPost]
    //public async Task<CommonResponse<List<Employee>>> GetEmployeeList()
    //{
    //    return CommonResponse<List<Employee>>.CreateHappyResponse(employeeList);
    //}

    //[Route("CreateEmployeeRequest")]
    //[HttpPost]
    //public async Task<CommonResponse<string>> CreateEmployeeRequest([FromBody] CreateEmployeeRequest createRequest)
    //{
    //    _logger.LogInformation("CreateEmployeeRequest by : {email}", createRequest.Email);
    //    if (!ModelState.IsValid)
    //    {
    //        string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
    //        return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
    //    }
    //    Employee employee = new Employee
    //    {
    //        Id = employeeList.Count + 1,
    //        FirstName = createRequest.FirstName,
    //        LastName = createRequest.LastName,
    //        BirthDate = createRequest.BirthDate,
    //        Email = createRequest.Email,
    //        Phone = createRequest.Phone,
    //        City = createRequest.City,
    //        CreatedAt = DateTime.Now,
    //        UpdatedAt = DateTime.Now
    //    };
    //    employeeList.Add(employee);
    //    return CommonResponse<string>.CreateHappyResponse(message: "Success");
    //}

    //[Route("UpdateEmployeeRequest")]
    //[HttpPost]
    //public async Task<CommonResponse<string>> UpdateEmployeeRequest([FromBody] UpdateEmployeeRequest updateRequest)
    //{
    //    _logger.LogInformation("UpdateEmployeeRequest by : {email}", updateRequest.Email);
    //    if (!ModelState.IsValid)
    //    {
    //        string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
    //        return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
    //    }
    //    Employee employee = employeeList.FirstOrDefault(x => x.Id == updateRequest.Id);
    //    if (employee == null)
    //    {
    //        return CommonResponse<string>.CreateUnhappyResponse(404, "Employee not found");
    //    }
    //    employee.FirstName = updateRequest.FirstName;
    //    employee.LastName = updateRequest.LastName;
    //    employee.Email = updateRequest.Email;
    //    employee.Phone = updateRequest.Phone;
    //    employee.City = updateRequest.City;
    //    employee.UpdatedAt = DateTime.Now;

    //    return CommonResponse<string>.CreateHappyResponse(message: "Success");
    //}

    [Route("GetEmployeeList")]
    [HttpPost]
    public async Task<CommonResponse<List<Employee>>> GetEmployeeList()
    {
        Task.Delay(5000).Wait(); // Simulate delay

        List<Employee> employeeList = new List<Employee>();

        // SQL Select Query to retrieve employees
        string query = "SELECT Id, FirstName, LastName, BirthDate, Email, Phone, City FROM EmployeesTable";

        using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-21494GD;Initial Catalog=EmployeesDB;Integrated Security=True;;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;User Id=sa;password=12345"))
        {
            try
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var employee = new Employee
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                BirthDate = reader.GetString(3),
                                Email = reader.GetString(4),
                                Phone = reader.GetString(5),
                                City = reader.GetString(6)
                            };

                            employeeList.Add(employee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee data");
                // Handle error appropriately here
            }
        }
        return CommonResponse<List<Employee>>.CreateHappyResponse(employeeList);
    }

    [Route("CreateEmployeeRequest")]
    [HttpPost]
    public async Task<CommonResponse<string>> CreateEmployeeRequest([FromBody] CreateEmployeeRequest createRequest)
    {
        Task.Delay(5000).Wait(); // Simulate delay

        _logger.LogInformation("CreateEmployeeRequest by : {email}", createRequest.Email);

        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
        }

        // SQL Insert Query (Note: No need to include 'Id' because it is auto-incremented)
        string query = "INSERT INTO EmployeesTable (FirstName, LastName, BirthDate, Email, Phone, City, CreatedAt, UpdatedAt) " +
                       "VALUES (@FirstName, @LastName, @BirthDate, @Email, @Phone, @City, @CreatedAt, @UpdatedAt)";

        // Use SqlConnection and SqlCommand to execute the query
        using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-21494GD;Initial Catalog=EmployeesDB;Integrated Security=True;;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;User Id=sa;password=12345"))
        {
            try
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", createRequest.FirstName);
                    command.Parameters.AddWithValue("@LastName", createRequest.LastName);
                    command.Parameters.AddWithValue("@BirthDate", createRequest.BirthDate);
                    command.Parameters.AddWithValue("@Email", createRequest.Email);
                    command.Parameters.AddWithValue("@Phone", createRequest.Phone);
                    command.Parameters.AddWithValue("@City", createRequest.City);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return CommonResponse<string>.CreateHappyResponse(message: "Employee created successfully");
                    }
                    else
                    {
                        return CommonResponse<string>.CreateWarningResponse(message: "Employee creation failed");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return CommonResponse<string>.CreateWarningResponse(message: "An error occurred");
            }
        }
    }

    [Route("UpdateEmployeeRequest")]
    [HttpPost]
    public async Task<CommonResponse<string>> UpdateEmployeeRequest([FromBody] UpdateEmployeeRequest updateRequest)
    {
        Task.Delay(5000).Wait(); // Simulate delay

        _logger.LogInformation("UpdateEmployeeRequest by : {email}", updateRequest.Email);

        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
        }

        // SQL Update Query
        string query = "UPDATE EmployeesTable SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
                       "Phone = @Phone, City = @City, UpdatedAt = @UpdatedAt WHERE Id = @Id";

        // Use SqlConnection and SqlCommand to execute the query
        using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-21494GD;Initial Catalog=EmployeesDB;Integrated Security=True;;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;User Id=sa;password=12345"))
        {
            try
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", updateRequest.FirstName);
                    command.Parameters.AddWithValue("@LastName", updateRequest.LastName);
                    command.Parameters.AddWithValue("@Email", updateRequest.Email);
                    command.Parameters.AddWithValue("@Phone", updateRequest.Phone);
                    command.Parameters.AddWithValue("@City", updateRequest.City);
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("@Id", updateRequest.Id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return CommonResponse<string>.CreateHappyResponse(message: "Employee updated successfully");
                    }
                    else
                    {
                        return CommonResponse<string>.CreateUnhappyResponse(404, "Employee not found");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return CommonResponse<string>.CreateWarningResponse(message: "An error occurred");
            }
        }
    }
}