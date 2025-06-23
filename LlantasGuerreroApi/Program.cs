using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using LlantasGuerreroApi.Repositorio;
using Microsoft.EntityFrameworkCore;
using LlantasGuerreroApi.LlantasGuerreroMapper;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

RegistryKey conexionSql = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\LlantasGuerrero");
string cadenaConexionSql = conexionSql.GetValue("ConexionSql")?.ToString(); 

// Add services to the container.
builder.Services.AddDbContext<ContextoAplicacionBD>(opciones =>
        opciones.UseSqlServer(cadenaConexionSql));

//Agregamos los Repositorios
builder.Services.AddScoped<IArticuloRepositorio, ArticuloRepositorio>();
//builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
builder.Services.AddScoped<ICatalogoRepositorio, CatalogoRepositorio>();
//Agregar filtro personalizado validar rol
//builder.Services.AddScoped<AtritutoValidarRolJwtFiltro>(); //NO FUNCIONAL POR EL MOMENTO

//OBTENER LLAVE TOKEN
//var llave = builder.Configuration.GetValue<string>("ApiSettings:Secreta");
RegistryKey llaveApi = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\LlantasGuerrero");
string cadenaLlaveApi = llaveApi.GetValue("Secreta")?.ToString();
var llave = cadenaLlaveApi;

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(LlantasGuerreroMapper));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando el esquema Bearer. \r\n\r\n" +
        "Ingresa la palabra 'Bearer' seguido de un [espacio] y después su token en el campo de abajo. \r\n\r\n" +
        "Ejemplo: \"Bearer asfdasfki1234oaks23\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
    });
});

//AQUÍ SE CONFIGURA LA AUTENTICACIÓN
builder.Services.AddAuthentication(
        x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    ).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false; //EN UNA API PRODUCTIVA, ESTO SE CAMBIA POR 'true'
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(llave)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Soporte para CORS
//Se pueden habilitar: 1-Un dominio, 2-multiples dominios,
//3-cualquier dominio (Tener en cuenta seguridad)
//Usamos de ejemplo el dominio: http://localhost:7008, se debe cambiar por el correcto
//Se usa (*) para todos los dominios
builder.Services.AddCors(p => p.AddPolicy("PoliticaCors", build =>
{
    build.WithOrigins("http://localhost").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("https://localhost").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("http://10.166.1.75").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("https://10.166.1.75").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("https://alexservidor.softether.net").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("http://alexservidor.softether.net").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("http://localhost:7008").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("http://llantasguerrero.com.mx").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    build.WithOrigins("https://llantasguerrero.com.mx").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccesoPermitido", policy =>
        policy.RequireAssertion(context =>
        {
            var idRol = context.User.FindFirst("IdRol")?.Value;

            return idRol == "1" || idRol == "2" || idRol == "3"; // Aquí defines los roles permitidos
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//PRUEBA
//app.UseSwaggerUI();
//app.UseSwagger();

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PoliticaCors");

//EL USE AUTHENTICATION ES EL SOPORTE PARA AUTENTICACIÓN
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
