using ProyectoRestaurante.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioMesas,RepositorioMesas>();
builder.Services.AddTransient<IRepositorioTiposPlatos, RepositorioTiposPlatos>();
builder.Services.AddTransient<IRepositorioCarta, RepositorioCarta>();
builder.Services.AddTransient<IRepositorioEmpleados, RepositorioEmpleados>();
builder.Services.AddTransient<IRepositorioPedidos, RepositorioPedidos>();
builder.Services.AddTransient<IRepositorioDetallePedido, RepositorioDetallePedido>();
builder.Services.AddAutoMapper(typeof(Program));

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
