# Blueprint Backend .NET Core 3.1

## FluentValidation

FluentValidation is a .NET library for building strongly-typed validation rules.

FluentValidation 11 supports the following platforms:

- .NET Core 3.1
- .NET 5
- .NET 6
- .NET Standard 2.0

For automatic validation with ASP.NET, FluentValidation supports ASP.NET running on .NET Core 3.1, .NET 5 or .NET 6.

### How validator works

FluentValidation works is to be registered first in a class and it will automatically called `Automatic Registration`, Automatic Registration for validators will only work for AbstractValidators implementing a concrete type like List or Array. Implementations with interface types like `IEnumerable` or `IList` may be used, but the validator will need to be specifically registered as a scoped service in your app’s Startup class. This is due to the ASP.NET’s Model-Binding of collection types where interfaces like `IEnumerable` will be converted to a List implementation and a List is the type MVC passes to FluentValidation.

### Using Validatorn a controller

This next example assumes that the `UserValidator` is defined to validate a class called `Users`. Once you’ve configured FluentValidation, ASP.NET will then automatically validate incoming requests using the validator mappings configured in your startup routine.

```
public class UserValidator : AbstractValidator<Users> {

    public UserValidator() {

        RuleFor(model => model.Username).MaximumLength(20);
        RuleFor(model => model.Password).MaximumLength(10);
        RuleFor(model => model.DisplayName).MaximumLength(50);
        RuleFor(model => model.Phone).Matches(@"^((?:[0-9]\-?){6,14}[0-9])|((?:[0-9]\x20?){6,14}[0-9])$");
        RuleFor(model => model.Email).EmailAddress();
    }
}
```

```
var userEntity = _mapper.Map<Users>(request);

var validationResult = new UserValidator().Validate(userEntity);

if(validationResult.IsValid) {

    _repoWrapper.User.CreateUser(userEntity);
    _repoWrapper.Save();
    var createdUser = _mapper.Map<UserDto>(userEntity);
    //return StatusCode(201, createdUser);
    return StatusCode(201, ResponseModel.Created(createdUser));
}
else {

    return BadRequest("Validation failed");
}

```

Finally we already shown how to using FluentValidation in our program.
