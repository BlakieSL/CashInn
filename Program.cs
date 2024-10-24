using CashInn.Enum;
using CashInn.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/create-branch-with-cook", () =>
{
    var employees = new List<AbstractEmployee>();
    var menu = new Menu();
    
    var branch = new Branch(1, "123 Main St", "contact@example.com", employees, menu);
    
    var cook = new Cook(2, "Mike Johnson", 35000, DateTime.Now.AddYears(-3), DateTime.Now, DateTime.Now.AddHours(8), 
                        StatusEmpl.FullTime, false, "mexican", 3, "grill");
    
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
    var cook = new Cook(2, "John Mikenson", 20000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Japanese", 2, "Stove");
    
    var chef = new Chef(3, "Mike Johnson", 23000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Mexican", 2, 2);

    var waiter = new Waiter(4, "Jelp Mei", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, 34.10);

    var delivery = new DeliveryEmpl(5, "Goog Mei", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Daytona", "Warsaw");
    
    var flexible = new FlexibleEmpl(6, "Man Moi", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Daytona", "Warsaw", 103.50);
    
    AbstractEmployee.SaveEmployee(cook);
    AbstractEmployee.SaveEmployee(chef);
    AbstractEmployee.SaveEmployee(waiter);
    AbstractEmployee.SaveEmployee(delivery);
    AbstractEmployee.SaveEmployee(flexible);

    return "Cook created and added to the branch!";
});

// Endpoint to save employees to file
app.MapGet("/save-employees", () =>
{
    string filepath = "employeesData.bin";
    AbstractEmployee.SaveExtent(filepath);
    return "Employees saved!";
});

// Endpoint to load employees from file
app.MapGet("/load-employees", () =>
{
    string filepath = "employeesData.bin";
    AbstractEmployee.LoadExtent(filepath);
    return "Employees loaded!";
});

// Endpoint to retrieve and display all employees
app.MapGet("/employees", () =>
{
    var employees = AbstractEmployee.GetAllEmployees();
    return employees.Select(emp => new
    {
        emp.Id,
        emp.Name,
        emp.Salary,
        emp.HireDate,
        emp.LayoffDate,
        emp.ShiftStart,
        emp.ShiftEnd,
        emp.Status,
        emp.IsBranchManager,
    });
});

app.Run();
