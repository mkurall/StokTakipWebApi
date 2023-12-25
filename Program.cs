

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StokTakipWebApi.Protocol;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Bu metot swagger ekranýnda authorize iþlemi için eklendi
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//---------------------------------------------------------------------------------



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Bu bölüm kimlik doðrulamasý için eklendi
builder.Services.AddAuthentication((options) =>
{
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
.AddJwtBearer("Bearer", options =>
{
    var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.
        GetBytes(builder.Configuration["JWT:key"]));

    options.TokenValidationParameters = new
    TokenValidationParameters
    {
        IssuerSigningKey = serverSecret,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"]
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            context.Response.WriteAsync(context.Exception.ToString()).Wait();
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            // Call this to skip the default logic and avoid using the default response
            context.HandleResponse();

            context.Response.Headers.Append("my-custom-header", "custom-value");
            ApiCevap cevap = new ApiCevap() { BasariliMi = false, HataMesaji = "Bu metodu çalýþtýrmak için kimlik doðrulamasý gerekli." };

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(cevap));
        }
    };
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = "stoktakip.WebApi.Cookie.Basics";
    options.LoginPath = "/Home/Index";
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    // runs on each request
    options.ForwardDefaultSelector = context =>
    {
        // filter by auth type
        string authorization = context.Request.Headers[HeaderNames.Authorization];

        if (authorization == null) return "Bearer";

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            return "Bearer";

        // otherwise always check for cookie auth
        return "Cookies";
    };
}); 

//--------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//Kimlik dogrulamasý için eklendi

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();



app.Run();
