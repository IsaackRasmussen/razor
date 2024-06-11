using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot.Socket;

namespace ShowCharts.Streaming;

public class BinanceStreaming
{
    private const int NumberOfLatestPrices = 100;

    public class SymbolPrice
    {
        public double Price { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double OpeningPrice { get; set; }
    }

    private class SymbolOpeningPrice
    {
        public double Price { get; set; }
        public DateOnly Day { get; set; }
    }

    private readonly Dictionary<string, Queue<SymbolPrice>> _prices = new Dictionary<string, Queue<SymbolPrice>>();
    private readonly Dictionary<string, SymbolOpeningPrice> _dayPrices = new Dictionary<string, SymbolOpeningPrice>();
    private readonly BinanceRestClient _binanceRestClient = new BinanceRestClient();

    /// <summary>
    /// Maintain last X number of trade prices for each symbol
    /// </summary>
    /// <param name="trade"></param>
    public async Task OnSymbolTick(BinanceStreamTrade trade)
    {
        var tickSymbolName = trade.Symbol.ToUpper();

        if (!_dayPrices.ContainsKey(tickSymbolName) ||
            _dayPrices[tickSymbolName].Day != DateOnly.FromDateTime(DateTime.Today))
        {
            var dayTicker = await _binanceRestClient.SpotApi.ExchangeData.GetTradingDayTickerAsync(tickSymbolName);
            lock (_dayPrices)
            {
                if (!_dayPrices.ContainsKey(tickSymbolName))
                {
                    _dayPrices.Add(tickSymbolName, new SymbolOpeningPrice());
                }

                _dayPrices[tickSymbolName].Price = Convert.ToDouble(dayTicker.Data.OpenPrice);
                _dayPrices[tickSymbolName].Day = DateOnly.FromDateTime(DateTime.Today);
            }
        }

        lock (_prices)
        {
            if (!_prices.ContainsKey(tickSymbolName))
            {
                _prices.Add(tickSymbolName, new Queue<SymbolPrice>());
            }
        }

        lock (_prices[tickSymbolName])
        {
            if (_prices[tickSymbolName].Count >= NumberOfLatestPrices)
            {
                _prices[tickSymbolName].Dequeue();
            }

            _prices[tickSymbolName].Enqueue(new SymbolPrice
            {
                Price = Convert.ToDouble(trade.Price),
                Timestamp = trade.TradeTime,
                OpeningPrice = _dayPrices[tickSymbolName].Price
            });
        }
    }

    public List<SymbolPrice> GetSymbolPrices(string symbolName)
    {
        symbolName = symbolName.ToUpper();

        return _prices.ContainsKey(symbolName)
            ? NormalizePrices(_prices[symbolName])
            : Enumerable.Empty<SymbolPrice>().ToList();
    }

    public List<String> GetSymbols()
    {
        return _prices.Keys.ToList();
    }

    private List<SymbolPrice> NormalizePrices(IEnumerable<SymbolPrice> prices)
    {
        return prices.Select(p =>
        {
            p.Price = p.Price;
            return p;
        }).ToList();
    }
}