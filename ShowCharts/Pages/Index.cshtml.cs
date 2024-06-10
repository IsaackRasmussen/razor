using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShowCharts.Pages;

public class IndexModel : PageModel
{
    private static Random randGen = new Random();
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        List<double> tokyoValues = new List<double> { 49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4 };
        List<double> nyValues = new List<double> { 83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3 };
        List<double> berlinValues = new List<double> { 42.4, 33.2, 34.5, 39.7, 52.6, 75.5, 57.4, 60.4, 47.6, 39.1, 46.8, 51.1 };
        List<double> londonValues = new List<double> { 48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4, 65.2, 59.3, 51.2 };
        List<ColumnSeriesData> tokyoData = new List<ColumnSeriesData>();
        List<ColumnSeriesData> nyData = new List<ColumnSeriesData>();
        List<ColumnSeriesData> berlinData = new List<ColumnSeriesData>();
        List<ColumnSeriesData> londonData = new List<ColumnSeriesData>();

        tokyoValues.ForEach(p => tokyoData.Add(new ColumnSeriesData {
            Y = p
        }));
        nyValues.ForEach(p => nyData.Add(new ColumnSeriesData {
            Y = p
        }));
        berlinValues.ForEach(p => berlinData.Add(new ColumnSeriesData {
            Y = p
        }));
        londonValues.ForEach(p => londonData.Add(new ColumnSeriesData {
            Y = p
        }));

        ViewData["tokyoData"] = tokyoData;
        ViewData["nyData"] = nyData;
        ViewData["berlinData"] = berlinData;
        ViewData["londonData"] = londonData;
    }

    public JsonResult OnGetChartData()
    {
        return new JsonResult(new object[] { DateTime.Now.ToString(), randGen.NextDouble() });
    }
}