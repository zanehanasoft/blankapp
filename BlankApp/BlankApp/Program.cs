using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = "Server=localhost; Port=33060; uid=admin; pwd=password123; database=Users;";
builder.Services.AddDbContext<UserDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/getuserslastnamelist", (UserDbContext context) =>
{
    var allUsers = context.Users.ToList();
    return allUsers.Select(u => u.LastName).ToList();
});

app.MapPost("/createuser", (User user, UserDbContext context) =>
{
    try
    {
        if (user.LastName == null || user.LastName == "")
        {
            throw new Exception("Last name is required");
        }
        context.Users.Add(user);
    }
    catch (Exception)
    {
        return Results.BadRequest();
    }
    return Results.Ok();
});

app.UseHttpsRedirection();

app.Run();

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DOB { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}