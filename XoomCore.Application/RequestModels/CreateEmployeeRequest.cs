namespace XoomCore.Application.RequestModels;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
}

public class MyModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}