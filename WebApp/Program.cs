using Microsoft.AspNetCore.Authorization;
using WebApp.Authorize;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Configurando middleware de altenticação
builder.Services.AddAuthentication().AddCookie("AuthCookie", configureOptions =>
{
    configureOptions.Cookie.Name = "AuthCookie";
    configureOptions.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

//configurando midleware de autorização
builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    configure.AddPolicy("Employee", policy => policy.Requirements.Add(new EmployeePolicyRequirement(3)));
});
builder.Services.AddSingleton<IAuthorizationHandler, EmployeePolicyHandler>();


builder.Services.AddHttpClient("WebApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7283/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
