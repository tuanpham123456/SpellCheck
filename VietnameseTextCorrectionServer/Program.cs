using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using VietnameseTextCorrectionServer.Interfaces;
using VietnameseTextCorrectionServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Đăng ký dịch vụ AI
builder.Services.AddSingleton<ITextProcessingService, AiTextProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
