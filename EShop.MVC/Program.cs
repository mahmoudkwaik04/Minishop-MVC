using EShop.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => { options.Cookie.IsEssential = true; });
builder.Services.AddScoped<EShop.MVC.Services.ICartService, EShop.MVC.Services.CartService>(provider => new EShop.MVC.Services.CartService(provider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>(), provider.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>()));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// إضافة Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // إنشاء قاعدة البيانات إذا لم تكن موجودة
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(new[]
        {
            new EShop.MVC.Models.Product 
            { 
                Name = "حاسوب محمول Dell XPS 13", 
                Description = "حاسوب محمول عالي الأداء مع معالج Intel Core i7 وذاكرة 16GB", 
                Price = 3500, 
                ImageUrl = "https://via.placeholder.com/300x250/667eea/ffffff?text=Dell+XPS+13",
                Stock = 15,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EShop.MVC.Models.Product 
            { 
                Name = "هاتف Samsung Galaxy S24", 
                Description = "هاتف ذكي بمواصفات ممتازة وكاميرا عالية الدقة", 
                Price = 2200, 
                ImageUrl = "https://via.placeholder.com/300x250/764ba2/ffffff?text=Galaxy+S24",
                Stock = 25,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EShop.MVC.Models.Product 
            { 
                Name = "سماعات AirPods Pro", 
                Description = "سماعات لاسلكية مع خاصية إلغاء الضوضاء", 
                Price = 899, 
                ImageUrl = "https://via.placeholder.com/300x250/56ab2f/ffffff?text=AirPods+Pro",
                Stock = 30,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EShop.MVC.Models.Product 
            { 
                Name = "ساعة Apple Watch Series 9", 
                Description = "ساعة ذكية مع مراقب صحي متقدم", 
                Price = 1599, 
                ImageUrl = "https://via.placeholder.com/300x250/f093fb/ffffff?text=Apple+Watch",
                Stock = 5,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EShop.MVC.Models.Product 
            { 
                Name = "كيبورد ميكانيكي RGB", 
                Description = "لوحة مفاتيح ميكانيكية للألعاب مع إضاءة RGB", 
                Price = 299, 
                ImageUrl = "https://via.placeholder.com/300x250/667eea/ffffff?text=Gaming+Keyboard",
                Stock = 20,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new EShop.MVC.Models.Product 
            { 
                Name = "ماوس لاسلكي Logitech", 
                Description = "ماوس لاسلكي عالي الدقة للعمل والألعاب", 
                Price = 199, 
                ImageUrl = "https://via.placeholder.com/300x250/764ba2/ffffff?text=Wireless+Mouse",
                Stock = 35,
                Category = "إلكترونيات",
                IsAvailable = true,
                IsFeatured = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
