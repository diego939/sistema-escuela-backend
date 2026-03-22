using SistemaEscuela.IOC;
using SistemaEscuela.DAL.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.InyectarDependencias(builder.Configuration);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngular",
		policy =>
		{
			policy.WithOrigins("http://localhost:4200") // 🔥 Front-end Angular
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
	var key = builder.Configuration["Jwt:Key"];
	if (string.IsNullOrEmpty(key))
		throw new InvalidOperationException("JWT Key is not configured");

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,

		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],

		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(key)
		)
	};
});

var app = builder.Build();

// Verificar conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<SistemaEscolarContext>();
	try
	{
		if (dbContext.Database.CanConnect())
		{
			Console.WriteLine("✓ Conexión exitosa a la base de datos");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"✗ Error de conexión: {ex.Message}");
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Escuela API V1");
		options.RoutePrefix = string.Empty; // Swagger en la raíz
	});
}

app.UseAuthorization();

app.UseCors("AllowAngular");

app.MapControllers();

app.Run();
