using Diploma.UI.Entities.Models;
using Diploma.UI.Services.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Diploma.UI.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] StatisticHttpService StatisticHttpService { get; set; }
        private List<CounterSnapshot> Statistic { get; set; }
        private Dictionary<string, List<CounterSnapshot>> groupedData = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Statistic = (await StatisticHttpService.GetAllStatistic()).ToList();

                groupedData = Statistic
                .GroupBy(s => s.CreatedAt.ToString("yyyy-MM-dd"))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderBy(s => s.CreatedAt).ToList());

                await InvokeAsync(StateHasChanged); // Переконайтеся, що рендеринг завершений

                foreach (var day in groupedData)
                {
                    var labels = day.Value.Select(s => s.CreatedAt.ToString("HH:mm")).ToArray();
                    var currentValueData = day.Value.Select(s => s.CurrentValue).ToArray();
                    var differenceValueData = day.Value.Select(s => s.DifferenceWithPreviousValue).ToArray();

                    var chartData = new
                    {
                        type = "line",
                        data = new
                        {
                            labels,
                            datasets = new object[]
                            {
                                new {
                                    label = "Актуальне значення",
                                    data = currentValueData,
                                    borderColor = "rgba(75, 192, 192, 1)",
                                    backgroundColor = "rgba(75, 192, 192, 0.2)",
                                    fill = false,
                                    yAxisID = "y-axis-1"
                                },
                                new {
                                    label = "Фактично спожито",
                                    data = differenceValueData,
                                    borderColor = "rgba(255, 99, 132, 1)",
                                    backgroundColor = "rgba(255, 99, 132, 0.2)",
                                    fill = false,
                                    yAxisID = "y-axis-2"
                                }
                            }
                        },
                        options = new
                        {
                            scales = new
                            {
                                xAxes = new object[]
                                {
                                    new {
                                        type = "time",
                                        time = new
                                        {
                                            unit = "hour",
                                            displayFormats = new
                                            {
                                                hour = "HH:mm"
                                            },
                                            tooltipFormat = "HH:mm",
                                            stepSize = 1,
                                            minUnit = "hour"
                                        },
                                        ticks = new
                                        {
                                            source = "labels",
                                            maxRotation = 0,
                                            autoSkip = true
                                        }
                                    }
                                },
                                yAxes = new object[]
                                {
                                    new {
                                        id = "y-axis-1",
                                        type = "linear",
                                        position = "left",
                                        scaleLabel = new
                                        {
                                            display = true,
                                            labelString = "Актуальне значення (кубічні метри води)"
                                        }
                                    },
                                    new {
                                        id = "y-axis-2",
                                        type = "linear",
                                        position = "right",
                                        scaleLabel = new
                                        {
                                            display = true,
                                            labelString = "Фактично спожито (кубічні метри води)"
                                        },
                                        gridLines = new
                                        {
                                            drawOnChartArea = false
                                        },
                                        ticks = new
                                        {
                                            beginAtZero = true
                                        }
                                    }
                                }
                            },
                            title = new
                            {
                                display = true,
                                text = "Споживання води (кубічні метри)"
                            }
                        }
                    };

                    var chartDataJson = JsonConvert.SerializeObject(chartData);

                    // Перевірка наявності елемента перед виконанням JavaScript коду
                    var canvasElementExists = await JSRuntime.InvokeAsync<bool>("eval", $"document.getElementById('chart-{day.Key}') !== null");
                    if (canvasElementExists)
                    {
                        await JSRuntime.InvokeVoidAsync("eval", $"new Chart(document.getElementById('chart-{day.Key}').getContext('2d'), {chartDataJson})");
                    }
                    else
                    {
                        Console.WriteLine($"Canvas element with ID 'chart-{day.Key}' not found.");
                    }
                }
            }
        }
    }
}
