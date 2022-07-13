using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Diagnostics;

namespace WITNetCoreProject.Models.Exceptions {

    public class MyErrorResponse {

        public IEnumerable<string> ErrorMessages { get; set; }

        public MyErrorResponse() : this(new List<string>()) { }

        public MyErrorResponse(string errorMessage) : this(new List<string>() { errorMessage }) { }

        public MyErrorResponse(IExceptionHandlerFeature context, Exception exception) { }

        public MyErrorResponse(IEnumerable<string> errorMessages) {
            ErrorMessages = errorMessages;
        }
    }
}
