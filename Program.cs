using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserControl.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlite(builder.Configuration.GetConnectionString("db")));

builder.Services.AddAutoMapper(typeof(Program));

//Como vai fazer para validar o token
var key = Encoding.ASCII.GetBytes(builder.Configuration["key"]);

//Adicioando autenticação
builder.Services.AddAuthentication(x =>
{
                //Esquema de autenticação padrão é o JwtBearerDefaults
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Adicionando autenticação JwtBearer
.AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;

                    //Parametros que vai usar para efetuar a validação do token
                    x.TokenValidationParameters = new TokenValidationParameters
        {
                        //Validar a chave
                        ValidateIssuerSigningKey = true,
                        //Passando qual chave ele vai usar
                        IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
