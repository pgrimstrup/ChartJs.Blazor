using ChartJs.Blazor.Common;
using ChartJs.Blazor.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace ChartJs.Blazor
{
    /// <summary>
    /// Represents a Chart.js chart.
    /// </summary>
    public partial class Chart
    {
        private ConfigBase _config;

        /// <summary>
        /// This event is fired when the chart has been setup through interop and
        /// the JavaScript chart object is available. Use this callback if you need to setup
        /// custom JavaScript options or register plugins.
        /// </summary>
        [Parameter]
        public EventCallback SetupCompletedCallback { get; set; }

        /// <summary>
        /// Gets the injected <see cref="IJSRuntime"/> for the current Blazor application.
        /// </summary>
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Gets or sets the configuration of the chart.
        /// </summary>
        [Parameter]
        public ConfigBase Config
        {
            get => _config;
            set
            {
                // Need to synchronize the Config.CanvasId with Chart.ChartId
                if (_config != null) _config.CanvasId = null;
                _config = value;
                if (_config != null) _config.CanvasId = ChartId;
            }
        }

        /// <summary>
        /// Gets or sets the width of the canvas HTML element.
        /// </summary>
        [Parameter]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the canvas HTML element. Use <see langword="null"/> when using <see cref="BaseConfigOptions.AspectRatio"/>.
        /// </summary>
        [Parameter]
        public int? Height { get; set; }

        /// <summary>
        /// Gets the id for the html canvas element associated with this chart.
        /// This property is initialized to a random GUID-string upon creation.
        /// </summary>
        public string ChartId { get; } = Guid.NewGuid().ToString();

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.SetupChart(Config);
                await SetupCompletedCallback.InvokeAsync(this);
            }
            else
            {
                await JsRuntime.UpdateChart(Config);
            }
        }

        /// <summary>
        /// Updates the chart.
        /// <para>
        /// Call this method after you've updated the <see cref="Config"/>.
        /// </para>
        /// </summary>
        public Task Update()
        {
            return JsRuntime.UpdateChart(Config).AsTask();
        }
    }
}
