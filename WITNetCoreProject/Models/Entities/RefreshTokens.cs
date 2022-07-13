using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WITNetCoreProject.Models.Entities {

    [Table("TB_Refresh_Tokens")]
    public class RefreshTokens {

        public RefreshTokens() {
        }

        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [StringLength(int.MaxValue)]
        public string Token { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("RefreshTokens")]
        public virtual Users User { get; set; }
    }
}
