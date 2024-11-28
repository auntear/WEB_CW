var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// เปิดใช้งาน Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // กำหนดเวลา session หมดอายุ
    options.Cookie.HttpOnly = true;  // ป้องกันการเข้าถึงจาก JavaScript
    options.Cookie.IsEssential = true;  // ทำให้ session สำคัญและไม่สามารถปิดได้
});
builder.Services.AddDistributedMemoryCache();
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.UseSession();
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
