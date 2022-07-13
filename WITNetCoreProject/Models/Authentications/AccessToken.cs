using System;
using WITNetCoreProject.Models.Dtos;

namespace WITNetCoreProject.Models.Authentications {

    public class AccessToken {

        public string Value { get; set; }
        public DateTime ExpirationTime { get; set; }
    }

    public static class Constants {

        public const string Issuer = Audiance;
        public const string Audiance = "https://localhost:5001";
        public const string Secret = "not_too_short_secret_otherwise_it_might_error";
    }

    public class AuthenticatedUserResponse {

        public string AccessToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserProjectLogin {

        public UserDto Users { get; set; }
        public AuthenticatedUserResponse Token { get; set; }
    }

    public class RegisterDto {

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class ChangePasswordDto {

        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginDto {

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
