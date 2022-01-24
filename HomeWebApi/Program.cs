using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeWebApi.Data;
using HomeWebApi.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HomeWebApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HomeWebApiContext")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IConstomJWTService, HSJWTService>();
// Configure the JWTTokenOptions 
builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));


JWTTokenOptions tokenOptions = new JWTTokenOptions();
builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
//���ü�Ȩ����
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(optsion =>
{
    optsion.TokenValidationParameters = new TokenValidationParameters
    {
        //�Ƿ���֤��������
        ValidateIssuer = true,  //�Ƿ���֤Issuer
        ValidateAudience = true,//�Ƿ���֤Audience
        ValidateLifetime = true,//�Ƿ���֤tokenʧЧʱ��
        ValidateIssuerSigningKey = true, //�Ƿ���֤SigningKey
        ValidAudience = tokenOptions.Audience,
        ValidIssuer = tokenOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))

    };
});
//(CORS) ����
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); //֧�ּ�Ȩ��֤
app.UseAuthorization();
app.UseCors("AllowCors");
app.MapControllers();

app.Run();
