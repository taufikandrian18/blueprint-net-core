using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WITNetCoreProject.Models.Entities {

    [Table("TB_Users")]
    public class Users {

        public Users() {

            RefreshTokens = new HashSet<RefreshTokens>();
        }

        [Key]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(15)]
        public string Username { get; set; }
        [Required]
        [StringLength(15)]
        public string Password { get; set; }
        [Required]
        [StringLength(64)]
        public string DisplayName { get; set; }
        [Required]
        [StringLength(64)]
        public string Phone { get; set; }
        [Required]
        [StringLength(64)]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(64)]
        public string UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }
    }
}
