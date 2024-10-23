using CashInn.Model;
using CashInn.Enum;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/create", () =>
{
    var cook = new Cook(2, "Mike Johnson", "Cook", 35000, DateTime.Now.AddYears(-3), DateTime.Now, DateTime.Now.AddHours(8), StatusEmpl.FullTime);
    Employee.SaveEmployee(cook);
    return "Employees created!";
});

// Endpoint to save the employees to file
app.MapGet("/save", () =>
{
    string filepath = "employeesData.bin";
    Employee.SaveExtent(filepath);
    return "Employees saved!";
});

// Endpoint to load employees from file
app.MapGet("/load", () =>
{
    string filepath = "employeesData.bin";
    Employee.LoadExtent(filepath);
    return "Employees loaded!";
});

// Endpoint to retrieve and display all employees
app.MapGet("/employees", () =>
{
    var employees = Employee.GetAllEmployees();
    return employees.Select(emp => new
    {
        emp.Id,
        emp.Name,
        emp.Role,
        emp.Status
    });
});

app.Run();