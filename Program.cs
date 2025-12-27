using Routing.CustomRouteConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months",typeof(MonthCustomConstraint));
});

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

app.Map("employee/profile/{EmpName:length(4,10):alpha=Scott}", async (context) =>
{

    string? empName = Convert.ToString(context.Request.RouteValues["EmpName"]);

    await context.Response.WriteAsync(empName);


});

/*//Eg : products/details/1
app.Map("products/details/{id=1}", async (context) =>
{

    int id = Convert.ToInt32(context.Request.RouteValues["id"]);
    await context.Response.WriteAsync($"Product ID : {id}");

});

*/
//Eg : products/details/1  
//Make paramter optional by adding ? for null
app.Map("products/details/{id?}", async (context) =>
{
    if (context.Request.RouteValues.ContainsKey("id"))
    {

        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product ID : {id}");

    }
    else
    {
        await context.Response.WriteAsync($"Product ID Not Passed ");
    }


});



//Route Constraints to accept only datetime
app.Map("daily-digest-report/{reportdate:datetime}", async(context) =>
{

   DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
    await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
});



//Eg cities/{cityid}

app.Map("cities/{cityid:guid}", async (context) =>
{

 Guid cityID = Guid.Parse((Convert.ToString(context.Request.RouteValues["cityid"]))!);
    await context.Response.WriteAsync($"City Information - {cityID}");
});


//Eg: sales-report/2030/apr
//Regular Expression Constrains

app.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async (context) =>
{

    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
    string? month = Convert.ToString(context.Request.RouteValues["month"]);


    await context.Response.WriteAsync($"sales report - {year} - {month}");

});


//Sales Report using Custom Constaint
app.Map("sales-report-custom/{year:int:min(1900)}/{month:months}", async (context) =>
{

    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
    string? month = Convert.ToString(context.Request.RouteValues["month"]);
    await context.Response.WriteAsync($"sales report - {year} - {month}");

});





app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("For any other route");

});

app.Run();
