using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowReactApp",
//         policy =>
//         {
//             policy.WithOrigins(
//                     "http://localhost:3000",
//                     "http://localhost:3001",
//                     "http://localhost:8080"
//                 )
//                   .AllowAnyMethod()
//                   .AllowAnyHeader()
//                   .AllowCredentials();
//         });
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Use CORS (ADD THIS - MUST be before MapControllers)
// app.UseCors("AllowReactApp");
app.UseCors("AllowAll");


// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();

//     app.UseSwaggerUI();
// }

app.UseSwagger();

app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();