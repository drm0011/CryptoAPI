using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.Core.DTOs
{
    public class UserDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
