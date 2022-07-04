using NLog;

Logger logger = LogManager.GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);


//ŠÂ‹«•Ï”‚Ì“Ç‚Ýo‚µ
var user = Environment.GetEnvironmentVariable("BASIC_AUTH_USER_NAME");
var pass = Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD");
var googleCredential = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
var jobName = Environment.GetEnvironmentVariable("AWS_JOB_QUEUE_NAME");
var jobRoleARN = Environment.GetEnvironmentVariable("AWS_JOB_EXECUTE_ROLE_ARN");

if (user == null || user == "" ||
   pass == null || pass == "" ||
   googleCredential == null || googleCredential == "" ||
   jobName == null || jobName == "" ||
   jobRoleARN == null || jobRoleARN == "")
{
    logger.Error("ŠÂ‹«•Ï”‚ª“KØ‚ÉÝ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");

    logger.Error("BASIC_AUTH_USER_NAME:{0}", user);
    logger.Error("BASIC_AUTH_PASSWORD:{0}", pass);
    logger.Error("GOOGLE_APPLICATION_CREDENTIALS:{0}", googleCredential);
    logger.Error("AWS_JOB_QUEUE_NAME:{0}", jobName);
    logger.Error("AWS_JOB_EXECUTE_ROLE_ARN:{0}", jobRoleARN);

    //Environment.Exit(0);
}


// Add services to the container.

var app = builder.Build();


app.MapGet("/slow_api/{wait}", async context =>
{
    int wait;
    try
    {
        wait = Convert.ToInt32(context.GetRouteValue("wait"));
    }
    catch 
    {
        wait = 0;
    }

    var result = await slowFunction(wait);
    await context.Response.WriteAsync($"result:{result}");
});

app.Run();

async Task<int> slowFunction(int wait)
{
    var rand = new  System.Random();
    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(wait));
    return rand.Next();
}


