using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using BankTransactionAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITransaction, TransactionService>();
builder.Services.AddTransient<IAccount, AccountService>();
builder.Services.AddTransient<ICustomer, CustomerService>();
builder.Services.AddTransient<ITransfer, TransferService>(); 
builder.Services.AddTransient<IRetailCustomer, RetailCustomer>(); 
builder.Services.AddTransient<IDapperDbConnection, DapperDbConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
