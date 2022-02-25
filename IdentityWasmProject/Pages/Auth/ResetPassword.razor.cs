using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SharedProject.Requests;
using SharedProject.Response;
using System.Net.Http.Json;

namespace IdentityWasmProject.Pages.Auth
{
    public partial class ResetPassword  
    {
        private ResetPasswordRequest _model = new();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;
        private async Task HandleSubmit()
        {
            _isBusy = true;
            _errorMessage = string.Empty;
            var response = await _httpClient.PutAsJsonAsync("api/auth/changepassword", _model);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadFromJsonAsync<ApiResponse<ResultResponse>>();
                await LocalStorageService.RemoveItemAsync("access_token");
                await LocalStorageService.RemoveItemAsync("expiry_date");
                await customAuthenticationState.GetAuthenticationStateAsync();
                _navigationManager.NavigateTo("/auth/login");
            }
            else
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<ResultResponse>>();
                _errorMessage = errorResult.Value.Message;
            }

            _isBusy = false;
        }
        protected void HideError()
        {
            _errorMessage = null;
            StateHasChanged();
        }
    }
}
