var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// �Դ��ҹ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // ��˹����� session �������
    options.Cookie.HttpOnly = true;  // ��ͧ�ѹ�����Ҷ֧�ҡ JavaScript
    options.Cookie.IsEssential = true;  // ����� session �Ӥѭ����������ö�Դ��
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
