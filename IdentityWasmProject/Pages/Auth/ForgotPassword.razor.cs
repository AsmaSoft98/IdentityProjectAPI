using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using SharedProject.Requests;
using SharedProject.Response;
using MudBlazor;

namespace IdentityWasmProject.Pages.Auth
{
    public partial class ForgotPassword 
    {


        [Inject] 
        public ISnackbar _snackbar { get; set; }
  
        private ForgotPasswordRequest _model = new();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;
        private bool _showErrorAlert = false;
        private async Task HandleSubmit()
        { 
            _isBusy = true;
            _errorMessage = string.Empty;
            _showErrorAlert = true;

            var response = await _httpClient.PutAsJsonAsync("api/Auth/forgotpassword", _model);
            if (response.IsSuccessStatusCode)
            {
                _navigationManager.NavigateTo("/", true);
                //_snackbar.Add("Reset password email is sent", Severity.Success);
            }
            else

            {
                _showErrorAlert = true;
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<UserManagerResponse>>();
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
