﻿namespace XoomCore.Application.RequestModels;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    //public string Gender { get; set; }
}