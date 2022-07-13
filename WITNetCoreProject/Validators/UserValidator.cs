using System;
using FluentValidation;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Validators {

    public class UserValidator : AbstractValidator<Users> {

        public UserValidator() {

            RuleFor(model => model.Username).MaximumLength(20);
            RuleFor(model => model.Password).MaximumLength(10);
            RuleFor(model => model.DisplayName).MaximumLength(50);
            RuleFor(model => model.Phone).Matches(@"^((?:[0-9]\-?){6,14}[0-9])|((?:[0-9]\x20?){6,14}[0-9])$");
            RuleFor(model => model.Email).EmailAddress();
        }
    }
}
