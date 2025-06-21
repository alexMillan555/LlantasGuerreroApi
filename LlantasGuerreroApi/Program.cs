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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContextoAplicacionBD>(opciones =>
        opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

//Agregamos los Repositorios
builder.Services.AddScoped<IArticuloRepositorio, ArticuloRepositorio>();
//builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
//Agregar filtro personalizado validar rol
//builder.Services.AddScoped<AtritutoValidarRolJwtFiltro>(); //NO FUNCIONAL POR EL MOMENTO

//OBTENER LLAVE TOKEN
var llave = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

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
    build.WithOrigins("http://localhost:7008").AllowAnyMethod().AllowAnyHeader();
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

app.UseHttpsRedirection();

//Soporte para CORS
app.UseCors("PoliticaCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
