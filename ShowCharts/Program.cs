using Binance.Net.Clients;
using Microsoft.EntityFrameworkCore;
using ShowCharts.DataSchema;
using ShowCharts.Streaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PriceDatabase")));
var streamingBinance = new BinanceStreaming();
builder.Services.AddSingleton(streamingBinance);
   
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var socketClient = new BinanceSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.ExchangeData.SubscribeToTradeUpdatesAsync(
    new List<string>(){
    "ETHUSDT","BTCUSDT","PEPEUSDT","DOGEUSDT","USDCUSDT"}, async (update) =>
    {
        await streamingBinance.OnSymbolTick(update.Data);
    });

    
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();