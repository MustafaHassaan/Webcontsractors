using Microsoft.EntityFrameworkCore;
using Repo.Unitofwork;
using Rotativa.AspNetCore;
using Webcontsractors.Domain.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromDays(30);//You can set Time   
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ConstractorsContext>
(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnection")));
builder.Services.AddTransient<IUnitofwork, Unitofwork>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sign}/{action=Signin}/{id?}");
app.Run();
