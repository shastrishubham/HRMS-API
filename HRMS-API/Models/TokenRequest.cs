using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS_API.Models
{
    public class TokenRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GoogleId { get; set; }
        public Guid CompId { get; set; }
    }
}