using LanchesMac.Areas.Admin.Servicos;
using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repository.Interfaces;
using LanchesMac.Repository;
using LanchesMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using FastReport.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

builder.Services.Configure<ConfigurationImagens>(builder.Configuration.GetSection("ConfigurationPastaImagens"));
//AJUSTE PARA CRIAR UMA SENHA:

builder.Services.Configure<IdentityOptions>(options =>
{
	//Default Password settings.
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 8;
	options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddTransient<ILancheRepository, LancheRepository>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<RelatorioVendasService>();
builder.Services.AddScoped<GraficoVendasService>();
builder.Services.AddScoped<RelatorioLanchesService>();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin",
		politica =>
		{
			politica.RequireRole("Admin");
		});
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));


builder.Services.AddControllersWithViews();

builder.Services.AddPaging(options =>
{
	options.ViewName = "Bootstrap4";
	options.PageParameterName = "pageindex";
});

builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseFastReport();

app.UseRouting();

CriarPerfisUsuarios(app);
////Cria os perfis
//seedUserRoleInitial.SeedRoles();
////cria os usuários e atribui ao perfil
//seedUserRoleInitial.SeedUsers();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "areas",
		pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

	endpoints.MapControllerRoute(
		name: "categoriaFiltro",
		pattern: "Lanche/{action}/{categoria?}",
		defaults: new { controller = "Lanche", action = "List" });

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
});




app.Run();

void CriarPerfisUsuarios(WebApplication app)
{
	var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
	using (var scope = scopedFactory.CreateScope())
	{
		var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
		service.SeedUsers();
		service.SeedRoles();
	}	
}