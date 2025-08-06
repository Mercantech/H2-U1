using Microsoft.AspNetCore.Components;

namespace BlazorWASM.Components;

public partial class BackendStatus : ComponentBase
{
    [Inject]
    private BlazorWASM.Services.APIService ApiService { get; set; } = default!;

    private BlazorWASM.Services.BackendStatus? backendStatus;
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await RefreshStatus();
    }

    private async Task RefreshStatus()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            backendStatus = await ApiService.GetBackendStatusAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved opdatering af status: {ex.Message}");
            backendStatus = null;
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private string GetStatusClass(string service)
    {
        return service.ToLower() switch
        {
            "server" => IsServiceRunning(backendStatus?.Server?.Status) ? "running" : "error",
            "mongodb" => IsServiceRunning(backendStatus?.MongoDB?.Status) && !backendStatus.MongoDB.IsError ? "running" : "error",
            "postgresql" => IsServiceRunning(backendStatus?.PostgreSQL?.Status) && !backendStatus.PostgreSQL.IsError ? "running" : "error",
            _ => ""
        };
    }

    private string GetStatusIcon(string service)
    {
        return service.ToLower() switch
        {
            "server" => IsServiceRunning(backendStatus?.Server?.Status) ? "fa-check-circle" : "fa-times-circle",
            "mongodb" => IsServiceRunning(backendStatus?.MongoDB?.Status) && !backendStatus.MongoDB.IsError ? "fa-check-circle" : "fa-times-circle",
            "postgresql" => IsServiceRunning(backendStatus?.PostgreSQL?.Status) && !backendStatus.PostgreSQL.IsError ? "fa-check-circle" : "fa-times-circle",
            _ => "fa-question-circle"
        };
    }

    private bool IsServiceRunning(string? status)
    {
        return !string.IsNullOrEmpty(status) && status.Contains("running", StringComparison.OrdinalIgnoreCase);
    }
}
