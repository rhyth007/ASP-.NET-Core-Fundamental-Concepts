var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.Map("/route1", async(context) =>
{
    await context.Response.WriteAsync("Inside Route 1");
});



app.Map("/route2", async (context) =>
{
    await context.Response.WriteAsync("Inside Route 2");

});

//Eg : files/sample.txt
app.Map("files/{filename}.{extension}", async (context) =>
{

    string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
    string? extensionName = Convert.ToString(context.Request.RouteValues["extension"]);

    await context.Response.WriteAsync($"Inside Files : For {fileName} with Extension {extensionName}");

});

//Eg employee/profile/john


//Default Routing --> Will send Scott if value not supplied

app.Map("employee/profile/{EmpName=Scott}", async (context) =>
{

    string? empName = Convert.ToString(context.Request.RouteValues["EmpName"]);

    await context.Response.WriteAsync(empName);


});




//Eg : products/details/1

app.Map("products/details/{id=1}", async (context) =>
{

    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    await context.Response.WriteAsync($"Product ID : {id}");

});





app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("For any other route");

});

app.Run();
