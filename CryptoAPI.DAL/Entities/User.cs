using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DAL.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
