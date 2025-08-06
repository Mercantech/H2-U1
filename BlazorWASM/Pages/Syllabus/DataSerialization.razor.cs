using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Pages.Syllabus;

public partial class DataSerialization
{
    [Inject] IJSRuntime JS { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("PrismHighlight");
        }
    }
} 