using System;
using System.ComponentModel.DataAnnotations;

namespace oidc_aspcore.Models {
    public class LoginViewModel {
        [Required, Display(Name = "ユーザーID")]
        public string loginId { get; set; }

        [Required, Display(Name = "パスワード")]
        public string password { get; set; }
    }
}