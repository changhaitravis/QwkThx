using QwkThx.Options;
using QwkThx.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<SlackOptions>(builder.Configuration.GetSection(SlackOptions.SectionName));
builder.Services.Configure<TeamOptions>(builder.Configuration.GetSection(TeamOptions.SectionName));

// Typed HttpClient for the Slack incoming webhook; socket reuse and DNS
// refresh are handled by the factory instead of newing up HttpClient per post.
builder.Services.AddHttpClient<ISlackNotifier, SlackNotifier>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
