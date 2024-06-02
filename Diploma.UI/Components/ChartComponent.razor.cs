using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Diploma.UI.Components
{
    public partial class ChartComponent : ComponentBase
    {
        [Inject] IJSRuntime JSRuntime { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            var chartData = new
            {
                type = "bar",
                data = new
                {
                    labels = new[] { "Red", "Blue", "Yellow", "Green", "Purple", "Orange" },
                    datasets = new[]
                    {
                        new {
                            label = "# of Votes",
                            data = new[] { 12, 19, 3, 5, 2, 3 },
                            backgroundColor = new[]
                            {
                                "rgba(255, 99, 132, 0.2)",
                                "rgba(54, 162, 235, 0.2)",
                                "rgba(255, 206, 86, 0.2)",
                                "rgba(75, 192, 192, 0.2)",
                                "rgba(153, 102, 255, 0.2)",
                                "rgba(255, 159, 64, 0.2)"
                            },
                            borderColor = new[]
                            {
                                "rgba(255, 99, 132, 1)",
                                "rgba(54, 162, 235, 1)",
                                "rgba(255, 206, 86, 1)",
                                "rgba(75, 192, 192, 1)",
                                "rgba(153, 102, 255, 1)",
                                "rgba(255, 159, 64, 1)"
                            },
                            borderWidth = 1
                        }
                    }
                },
                options = new
                {
                    scales = new
                    {
                        yAxes = new[]
                        {
                            new {
                                ticks = new
                                {
                                    beginAtZero = true
                                }
                            }
                        }
                    }
                }
            };

            var chartDataJson = JsonConvert.SerializeObject(chartData);

            await JSRuntime.InvokeVoidAsync("eval", $"new Chart(document.getElementById('myChart').getContext('2d'), {chartDataJson})");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
