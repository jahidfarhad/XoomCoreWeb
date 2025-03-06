using Microsoft.AspNetCore.Mvc;
using XoomCore.Application.RequestModels;
using XoomCore.Application.ResponseModels.Shared;
using XoomCore.Services.SessionControl;

namespace XoomCore.Web.Controllers;

//[MustHaveAuthorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISessionManager _sessionManager;
    public static List<CreateEmployeeRequest> createEmployeeRequests = new List<CreateEmployeeRequest>();
    public static List<UpdateEmployeeRequest> updateEmployeeRequests = new List<UpdateEmployeeRequest>();
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
    public async Task<CommonResponse<List<CreateEmployeeRequest>>> GetEmployeeList()
    {
        return CommonResponse<List<CreateEmployeeRequest>>.CreateHappyResponse(createEmployeeRequests);
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
        createEmployeeRequests.Add(createRequest);
        return CommonResponse<string>.CreateHappyResponse(message: "Success");
    }

    [Route("UpdateEmployeeList")]
    [HttpPost]
    public async Task<CommonResponse<List<UpdateEmployeeRequest>>> UpdateEmployeeList()
    {
        return CommonResponse<List<UpdateEmployeeRequest>>.CreateHappyResponse(updateEmployeeRequests);
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
        updateEmployeeRequests.Add(updateRequest);
        return CommonResponse<string>.CreateHappyResponse(message: "Success");
    }
}