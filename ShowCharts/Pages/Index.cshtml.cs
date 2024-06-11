using Highsoft.Web.Mvc.Stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShowCharts.Streaming;

namespace ShowCharts.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BinanceStreaming _binanceStreaming;

    public IndexModel(ILogger<IndexModel> logger, BinanceStreaming binanceStreaming)
    {
        _logger = logger;
        _binanceStreaming = binanceStreaming;
    }

    public void OnGet()
    {
        ViewData["SeriesData"] = GetSeriesData();
    }

    public JsonResult OnGetChartData()
    {
        return new JsonResult(_binanceStreaming.GetSymbols().Select(s => new
        {
            Name = s,
            Prices = _binanceStreaming.GetSymbolPrices(s)
        }));
    }

    private List<Series> GetSeriesData()
    {
        return _binanceStreaming.GetSymbols().Select(symbolName => new LineSeries
        {
            Data = _binanceStreaming.GetSymbolPrices(symbolName).Select(p => new LineSeriesData
            {
                X = p.Timestamp.ToUnixTimeMilliseconds(),
                Y = ((p.Price / p.OpeningPrice) - 1.0) * 100.0,
                Description = p.Price.ToString()
            }).ToList(),
            Name = symbolName,
            TurboThreshold = 10000
        }).ToList<Series>();
    }
}