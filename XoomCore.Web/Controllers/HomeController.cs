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
    public HomeController(ILogger<HomeController> logger, ISessionManager sessionManager)
    {
        _logger = logger;
        _sessionManager = sessionManager;

    }
    [Route("index")]
    //[MustHavePermission(SubMenuKey: "Home", Controller: "Home", Action: "Index")]
    public IActionResult Index()
    {
        var data = new List<MyModel>
        {
            new MyModel { Id = 1, Name = "John Doe", Age = 30 },
            new MyModel { Id = 2, Name = "Jane Smith", Age = 25 },
            new MyModel { Id = 3, Name = "Sam Johnson", Age = 35 }
        };

        return View(data);
        //return View();
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
    public async Task<CommonResponse<string>> CreateEmployeeRequest([FromBody] CreateEmployeeRequest request)
    {
        _logger.LogInformation("CreateEmployeeRequest by : {email}", request.Email);
        if (!ModelState.IsValid)
        {
            string errorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            return CommonResponse<string>.CreateWarningResponse(message: errorMessage);
        }
        createEmployeeRequests.Add(request);
        return CommonResponse<string>.CreateHappyResponse(message: "Success");
    }

    // Controller Method
    public ActionResult GetData()
    {
        var data = new List<MyModel>
        {
            new MyModel { Id = 1, Name = "John Doe", Age = 30 },
            new MyModel { Id = 2, Name = "Jane Smith", Age = 25 },
            new MyModel { Id = 3, Name = "Sam Johnson", Age = 35 }
        };

        return View(data);
    }

}