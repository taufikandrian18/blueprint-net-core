using System;
using FluentValidation;
using WITNetCoreProject.Models.Authentications;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Validators {

    public class LoginValidator : AbstractValidator<ChangePasswordDto> {

        public LoginValidator() {

            RuleFor(model => model.ConfirmPassword).MaximumLength(10);
            RuleFor(model => model.Password).MaximumLength(10);
            RuleFor(model => model.Email).EmailAddress();
        }
    }
}
