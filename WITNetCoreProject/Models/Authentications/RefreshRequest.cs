using System;
using System.ComponentModel.DataAnnotations;

namespace WITNetCoreProject.Models.Authentications {

    public class RefreshRequest {

        [Required]
        public string RefreshToken { get; set; }
    }
}
