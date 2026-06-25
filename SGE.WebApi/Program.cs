using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using SGE.WebApi.Usuarios;              
using SGE.Aplicacion.Interfaces;   
using SGE.Aplicacion.Usuarios;     
using SGE.Aplicacion.Tramites;     
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Servicios;

using SGE.Infraestructura; 
using SGE.Infraestructura.SQLite; 

using SGE.WebApi;                  
using SGE.WebApi.Middlewares;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SgeContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), sqliteOptions =>
    {
        sqliteOptions.CommandTimeout(30);
    });
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "ClaveSuperSecretaDeRespaldoDe32Bytes!");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddExceptionHandler<ManejadorExcepciones>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IHashService, ServicioHash>();
builder.Services.AddScoped<ITokenService, JwtTokenProvider>();
    
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<EliminarUsuarioUseCase>();
builder.Services.AddScoped<ModificarMisDatosUseCase>();
builder.Services.AddScoped<ModificarPermisosUsuarioUseCase>();

builder.Services.AddScoped<ListarTramitesPorExpedienteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SgeContext>();
    InicializadorBD.Inicializar(context);
    context.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;"); 
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/api/usuarios/login", (
    LoginApiRequest body, 
    LoginUseCase useCase, 
    ITokenService tokenService) => 
{
    var request = new LoginUsuarioRequest(body.Correo, body.Contrasena); 

    var response = useCase.Ejecutar(request);

    var tokenStr = tokenService.GenerarToken(response.Id); 

    return Results.Ok(new { Token = tokenStr });
}).AllowAnonymous().WithTags("Usuarios");

app.Run();
