using System.Text;
using Microsoft.IdentityModel.Tokens;

var configuration =
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// *** Add JWT Bearer services
var jwtConfig = configuration.GetSection("Jwt");
var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
var signingKey = new SymmetricSecurityKey(keyByteArray);

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, //是否验证签名,不验证的画可以篡改数据，不安全
            IssuerSigningKey = signingKey, //解密的密钥
            ValidateIssuer = true, //是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
            ValidIssuer = jwtConfig.GetValue<string>("Iss"), //发行人
            ValidateAudience = true, //是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
            ValidAudience = jwtConfig.GetValue<string>("Aud"), //订阅人
            ValidateLifetime = true, //是否验证过期时间，过期了就拒绝访问
            ClockSkew = TimeSpan.Zero, //这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
            RequireExpirationTime = true,
        };
    });
// *** Add JWT Bearer services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();