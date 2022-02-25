using Microsoft.AspNetCore.Components;
using SharedProject.Requests;
using SharedProject.Response;
using System.Net.Http.Json;

namespace IdentityWasmProject.Pages.Auth
{
    public partial class Register
    {
        private bool _isBusy;
        private bool _showErrorAlert = false;
        private string _errorMessage = String.Empty;
        private readonly RegisterRequest _model = new();

        //private static IEnumerable<string> PasswordStrength(string pw)
        //{
        //    if (string.IsNullOrWhiteSpace(pw))
        //    {
        //        yield return "Password is required!";
        //        yield break;
        //    }
        //    if (pw.Length < 8)
        //        yield return "Password must be at least of length 8";
        //    if (!Regex.IsMatch(pw, @"[A-Z]"))
        //        yield return "Password must contain at least one capital letter";
        //    if (!Regex.IsMatch(pw, @"[a-z]"))
        //        yield return "Password must contain at least one lowercase letter";
        //    if (!Regex.IsMatch(pw, @"[0-9]"))
        //        yield return "Password must contain at least one digit";
        //}
        private async Task HandleSubmit()
        {
            _isBusy = true;
            _showErrorAlert = false;

            var response = await _httpClient.PostAsJsonAsync("api/auth/register/", _model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ResultResponse>>();
                _showErrorAlert = true;
                _navigationManager.NavigateTo("/auth/login"); // TODO just login user in!
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
