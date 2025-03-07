using Microsoft.AspNetCore.Mvc;
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
    public static List<Employee> employeeList = new List<Employee>();
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
    [Route("GetEmployeeList")]
    [HttpPost]
    public async Task<CommonResponse<List<Employee>>> GetEmployeeList()
    {
        return CommonResponse<List<Employee>>.CreateHappyResponse(employeeList);
    }
    [Route("CreateEmployeeRequest")]
    [HttpPost]
    public async Task<CommonResponse<string>> CreateEmployeeRequest([FromBody] CreateEmployeeRequest createRequest)
    {
        _logger.LogInformation("CreateEmployeeRequest by : {email}", createRequest.Email);
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
        }
        Employee employee = new Employee
        {
            Id = employeeList.Count + 1,
            FirstName = createRequest.FirstName,
            LastName = createRequest.LastName,
            BirthDate = createRequest.BirthDate,
            Email = createRequest.Email,
            Phone = createRequest.Phone,
            City = createRequest.City,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        employeeList.Add(employee);
        return CommonResponse<string>.CreateHappyResponse(message: "Success");
    }

    [Route("UpdateEmployeeRequest")]
    [HttpPost]
    public async Task<CommonResponse<string>> UpdateEmployeeRequest([FromBody] UpdateEmployeeRequest updateRequest)
    {
        _logger.LogInformation("UpdateEmployeeRequest by : {email}", updateRequest.Email);
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
        }
        Employee employee = employeeList.FirstOrDefault(x => x.Id == updateRequest.Id);
        if (employee == null)
        {
            return CommonResponse<string>.CreateUnhappyResponse(404, "Employee not found");
        }
        employee.FirstName = updateRequest.FirstName;
        employee.LastName = updateRequest.LastName;
        employee.Email = updateRequest.Email;
        employee.Phone = updateRequest.Phone;
        employee.City = updateRequest.City;
        employee.UpdatedAt = DateTime.Now;

        return CommonResponse<string>.CreateHappyResponse(message: "Success");
    }
}