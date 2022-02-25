using Microsoft.AspNetCore.Components;
using SharedProject.Requests;
using SharedProject.Response;
using System.Net.Http.Json;

namespace IdentityWasmProject.Pages.Auth
{
    public partial class Login
    {
        private string _errorMessage = string.Empty;
        private bool _isBusy = false;
        private LoginRequest _model = new();
        private async Task HandleSubmit()
        {
            _isBusy = true;
            _errorMessage = string.Empty;
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", _model);
            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<ApiResponse<UserManagerResponse>>();
                await LocalStorageService.SetItemAsStringAsync("token", loginResult.Value.AccessToken);
                await customAuthenticationState.GetAuthenticationStateAsync();
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<UserManagerResponse>>();
                _errorMessage = errorResult.Value.Message;
                await customAuthenticationState.Logout();
            }

            //switch (response.StatusCode)
            //{
            //    case System.Net.HttpStatusCode.OK:
            //        var loginResult = await response.Content.ReadFromJsonAsync<ApiResponse<UserManagerResponse>>();
            //        await LocalStorageService.SetItemAsStringAsync("token", loginResult.Value.AccessToken);
            //        await customAuthenticationState.GetAuthenticationStateAsync();
            //        _navigationManager.NavigateTo("/", true);
            //        break;
            //    case System.Net.HttpStatusCode.BadRequest:
            //        var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<UserManagerResponse>>();
            //        _errorMessage = errorResult.Value.Message;
            //        await customAuthenticationState.Logout();
            //        break;
            //    case System.Net.HttpStatusCode.InternalServerError:
            //        break;
            //}
            _isBusy = false;
        }

        protected void HideError()
        {
            _errorMessage = null;
            StateHasChanged();
        }

    }
}
