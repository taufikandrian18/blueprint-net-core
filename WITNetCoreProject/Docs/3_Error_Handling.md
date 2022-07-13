# Blueprint Backend .NET Core 3.1

## Error Handling

Unless you’re perfect 100% of the time (who is?), you’ll most likely have errors in your code. If your code doesn’t build due to compilation errors, you can probably correct that by fixing the offending code. But if your application encounters runtime errors while it’s being used, you may not be able to anticipate every possible scenario.

Runtime errors may cause Exceptions, which can be caught and handled in many programming languages. Unit tests will help you write better code, minimize errors and create new features with confidence. In the meantime, there’s the good ol’ try-catch-finally block, which should be familiar to most developers.

### Exceptions with Try-Catch

The simplest form of a try-catch block looks something like this:

```
try
{
   // try something here

} catch (Exception ex)
{
  // catch an exception here
}

```

You can chain multiple catch blocks, starting with more specific exceptions. This allows you to catch more generic exceptions toward the end of your try-catch code. In a string of catch() blocks, only the caught exception (if any) will cause that block of code to run.

```
try
{
   // try something here

} catch (IOException ioex)
{
  // catch specific exception, e.g. IOException

} catch (Exception ex)
{
  // catch generic exception here

}

```

### Try-Catch in NetLearner

In the Best Practice of this blueprint project, the UserController uses a UserRepository from a shared .NET Standard Library to handle database updates. In the overloaded Edit() method, it wraps a call to the Update() method from the service class to catch a possible `NotFoundException` exception. First, it checks to see whether the ModelState is valid or not.

```
public async Task<ActionResult> Update(Guid id, UserForUpdateDto request) {

    try {

        if (ModelState.IsValid)
        {
            ...
        }

        // get data from database
        var userEntity = await _repoWrapper.User.GetUserById(id);

        if (userEntity == null) {

            _logger.LogError($"User with id: {id}, hasn't been found in db.");
            return NotFound();
        }

        // update process through mapper first, after become an user object then update to database
        var validationResult = new UserValidator().Validate(userEntity);
        if(validationResult.IsValid) {

            _mapper.Map(request, userEntity);
            _repoWrapper.User.UpdateUser(userEntity);
            _repoWrapper.Save();
            var createdUser = _mapper.Map<UserDto>(userEntity);
            //return Ok(createdUser);
            return StatusCode(200, ResponseModel.ResponseOk(createdUser, 1));
        }
        else {

            return BadRequest("Validation failed");
        }
    }
    catch (NotFoundException) {

        return NotFound();
    }
}

```

and also you can add a class for custom message error and put on the controller class for example is shown below.

```
public class ResponseModel {

    public string EnumCode { get; set; }
    public Message Msg { get; set; }
    public object Data { get; set; }

    public int Count { get; set; }

    public static ResponseModel ResponseOk(object data) {

        var resp = new ResponseModel {

            EnumCode = "200",
            Msg = new Message {

                En = "Success.",
                Id = "Berhasil."
            },
            Data = data,
        };
        return resp;
    }

    public static ResponseModel ResponseOk(object data, int count) {

        var resp = new ResponseModel {

            EnumCode = "200",
            Msg = new Message {

                En = "Success.",
                Id = "Berhasil."
            },
            Data = data,
            Count = count,
        };
        return resp;
    }

    public static ResponseModel ResponseOk() {

        var resp = new ResponseModel {

            EnumCode = "200",
            Msg = new Message {

                En = "Success.",
                Id = "Berhasil."
            },
            Data = null,

        };
        return resp;
    }

    public static ResponseModel BadRequest(object data, Message msg) {

        var resp = new ResponseModel {

            EnumCode = "400",
            Msg = msg,
            Data = data
        };
        return resp;
    }

    public static ResponseModel UnauthorizedTokenRefresh(object data, Message msg)
    {

        var resp = new ResponseModel
        {

            EnumCode = "401",
            Msg = msg,
            Data = data
        };
        return resp;
    }

    public static ResponseModel BadRequest(object data) {

        var resp = new ResponseModel {

            EnumCode = "400",
            Msg = new Message {

                En = "Invalid data format.",
                Id = "Data tidak valid."
            },
            Data = data
        };
        return resp;
    }

    public static ResponseModel Created(object data) {

        var resp = new ResponseModel {

            EnumCode = "201",
            Msg = new Message {

                En = "Data has been created.",
                Id = "Data telah berhasil ditambahkan."
            },
            Data = data
        };
        return resp;
    }

    public static ResponseModel Error(object data, Exception ex) {

        var resp = new ResponseModel {

            EnumCode = "500",
            Msg = new Message {

                En = "Internal server error.",
                Id = "Terjadi kesalahan pada server."
            },
            Data = ex.Message
        };
        return resp;
    }
}

public class Message {

    public string En { get; set; }
    public string Id { get; set; }
}
```

```
return StatusCode(201, ResponseModel.Created(createdUser));

```

Then, it tries to update user-submitted data by passing a `userEntity` object. If a `NotFoundException` exception occurs, there is a check to verify whether the current `userEntity` even exists or not, so that a 404 (NotFound) can be returned if necessary.

### Error Handling for MVC

In ASP .NET Core MVC web apps, unhandled exceptions are typically handled in different ways, depending on whatever environment the app is running in. The default template uses the `DeveloperExceptionPage` middleware in a development environment but redirects to a shared Error view in non-development scenarios. This logic is implemented in the `Configure()` method of the `Startup.cs` class.

```
if (env.IsDevelopment()) {

    app.UseDeveloperExceptionPage();
}
else {

    app.UseExceptionHandler("/error"); //front-end have to provide /error page for this
}

```

### Logging Errors

To make use of error logging (in addition to other types of logging) in your web app, you may call the necessary methods in your controller’s action methods or equivalent. Here, you can log various levels of information, warnings and errors at various severity levels.

As seen in the snippet below, you have to do the following in your MVC Controller that you want to add Logging to:

- Add using statement for Logging namespace
- Add a private readonly variable for an ILogger object
- Inject an `ILogger<model>` object into the constructor
- Assign the private variable to the injected variable
- Call various log logger methods as needed.

```
public class MyController: Controller
{
   ...
   private readonly ILogger _logger;

   public MyController(..., ILogger<MyController> logger)
   {
      ...
      _logger = logger;
   }

   public IActionResult MyAction(...)
   {
      _logger.LogTrace("log trace");
      _logger.LogDebug("log debug");
      _logger.LogInformation("log info");
      _logger.LogWarning("log warning");
      _logger.LogError("log error");
      _logger.LogCritical("log critical");
   }
}

```

and also you can check summary of all activities in `WITNetCoreProject/Services/Logger/date.txt` file to help you solve all this programs problem.
