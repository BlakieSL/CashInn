using CashInn.Enum;
using CashInn.Model;
using CashInn.Model.Employee;
using CashInn.Model.MenuItem;
using CashInn.Model.Payment;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/create-branch", () =>
{
    var branch = new Branch(1, "123 Main St", "contact@example.com");
    Branch.SaveExtent();
    return "Branch created with Cook and saved!";
});

app.MapGet("/create-employees", () =>
{
    Branch branch = new Branch(1, "ul. Hermana", "+4857575757");
    var cook = new Cook(2, "John Mikenson", 20000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, true, "Japanese", 2, "Stove", branch, branch);
    
    var chef = new Chef(3, "Mike Johnson", 23000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Mexican", 2, 2, branch);

    var waiter = new Waiter(4, "Jelp Mei", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, 34.10, branch);

    var delivery = new DeliveryEmpl(5, "Goog Mei", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Daytona", "Warsaw", branch);
    
    var flexible = new FlexibleEmpl(6, "Man Moi", 24000, DateTime.Now.AddYears(-2), DateTime.Now, DateTime.Now.AddHours(8), 
        StatusEmpl.FullTime, false, "Daytona", "Warsaw", 103.50, branch);

    AbstractEmployee.SaveExtent();
    return "Employees saved!";
});

// app.MapGet("/create-payments", () =>
// {
//     var paymentCard = new CardPayment(1, 23.60, DateTime.Today.AddDays(1), "123-123-123");
//     var paymentCash = new CashPayment(2, 23.00, 50.00, 27.00, DateTime.Today.AddDays(1));
//     AbstractPayment.SaveExtent();
//     return "Payments created and saved!";
// });

app.MapGet("/create-menuitems", () =>
{
    Category category = new Category(1, "food");
    var item1 = new DefaultItem(1, "burger", 23.00, "Good burger", "no nothin", true, ServingSize.Small, category);
    var item2 = new SpecialItem(1, "burger special", 23.00, "Good burger", "no nothin", true, DateTime.Today.AddDays(1), DateTime.Today.AddDays(10), category);
    AbstractMenuItem.SaveExtent();
    return "Items created and saved!";
});

app.MapGet("/create-categories", () =>
{
    var category1 = new Category(1, "Spicy");
    var category2 = new Category(2, "Sweet");
    Category.SaveExtent();
    return "Categories created and saved!";
});

app.MapGet("/create-customers", () =>
{
    var customer1 = new Customer(1, "Steve", null, "FalseAddress", "falseemail");
    var customer2 = new Customer(2, "John", null, "FalseAddress", "falseemail");
    Customer.SaveExtent();
    return "Customers created and saved!";
});

app.MapGet("/create-ingredients", () =>
{
    var ingredient1 = new Ingredient(1, "Carrot", 100, true);
    var ingredient2 = new Ingredient(2, "Cabbage", 129, true);
    Ingredient.SaveExtent();
    return "Ingredients created and saved!";
});

// WRITE TESTS FOR IT
// app.MapGet("/create-kitchens", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });
//
// app.MapGet("/create-menus", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });
//
// app.MapGet("/create-orders", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });
//
// app.MapGet("/create-reservations", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });
//
// app.MapGet("/create-reviews", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });
//
// app.MapGet("/create-tables", () =>
// {
//     var category1 = new Category(1, "Spicy");
//     var category2 = new Category(2, "Sweet");
//     Category.SaveExtent();
//     return "Categories created and saved!";
// });


app.MapGet("/branches", () =>
{
    if (Branch.GetAll().Count == 0)
    {
        Branch.LoadExtent();
    }
    var branches = Branch.GetAll();
    return branches;
});

app.MapGet("/payments", () =>
{
    if (AbstractPayment.GetAll().Count == 0)
    {
        AbstractPayment.LoadExtent();
    }
    var payments = AbstractPayment.GetAll();
    var result = payments.Select(emp => emp.ToSerializableObject()).ToList();
    return result;
});

app.MapGet("/categories", () =>
{
    if (Category.GetAll().Count == 0)
    {
        Category.LoadExtent();
    }
    var categories = Category.GetAll();
    return categories;
});


app.MapGet("/menuitems", () =>
{
    if (AbstractMenuItem.GetAll().Count == 0)
    {
        AbstractMenuItem.LoadExtent();
    }
    
    var menuItems = AbstractMenuItem.GetAll();
    var result = menuItems.Select(emp => emp.ToSerializableObject()).ToList();

    return result;
});

// Endpoint to load employees from file
app.MapGet("/employees", () =>
{
    if (AbstractEmployee.GetAll().Count == 0)
    {
        AbstractEmployee.LoadExtent();
    }
    var employees = AbstractEmployee.GetAll();
    var resultEmployees = employees.Select(emp => emp.ToSerializableObject()).ToList();
    return resultEmployees;
});

app.Run();
