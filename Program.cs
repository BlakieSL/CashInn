using CashInn.Enum;
using CashInn.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/create-branch-with-cook", () =>
{
    var employees = new List<Employee>();
    var menu = new Menu();
    
    var branch = new Branch(1, "123 Main St", "contact@example.com", employees, menu);
    
    var cook = new Cook(2, "Mike Johnson", "Cook", 35000, DateTime.Now.AddYears(-3), DateTime.Now, DateTime.Now.AddHours(8), 
                        StatusEmpl.FullTime, false);
    
    branch.Employees.Add(cook);
    Branch.SaveBranch(branch);

    return "Branch created with Cook and saved!";
});

// Endpoint to save branches to file
app.MapGet("/save-branches", () =>
{
    string filepath = "branchesData.bin";
    Branch.SaveExtent(filepath);
    return "Branches saved!";
});

// Endpoint to load branches from file
app.MapGet("/load-branches", () =>
{
    string filepath = "branchesData.bin";
    Branch.LoadExtent(filepath);
    return "Branches loaded!";
});

// Endpoint to retrieve and display all branches
app.MapGet("/branches", () =>
{
    var branches = Branch.GetAllBranches();
    return branches.Select(branch => new
    {
        branch.Id,
        branch.Location,
        branch.ContactInfo,
    });
});

app.MapGet("/create-cook", () =>
{
    var branch = Branch.GetAllBranches().FirstOrDefault(); // Assuming you have at least one branch

    if (branch == null)
    {
        return "No branches available. Create a branch first!";
    }

    var cook = new Cook(3, "John Doe", "Cook", 32000, DateTime.Now.AddYears(-1), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false);
    
    Employee.SaveEmployee(cook);
    branch.Employees.Add(cook); // Add the cook to the branch's employees collection

    return "Cook created and added to the branch!";
});

// Endpoint to save employees to file
app.MapGet("/save-employees", () =>
{
    string filepath = "employeesData.bin";
    Employee.SaveExtent(filepath);
    return "Employees saved!";
});

// Endpoint to load employees from file
app.MapGet("/load-employees", () =>
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
