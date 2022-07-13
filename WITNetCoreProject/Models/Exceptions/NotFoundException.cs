using System;
using System.Net;

namespace WITNetCoreProject.Models.Exceptions {

    public class NotFoundException : Exception {

        public HttpStatusCode Status { get; private set; }

        public NotFoundException(HttpStatusCode status, string msg) : base(msg) {

            Status = status;
        }
    }
}
