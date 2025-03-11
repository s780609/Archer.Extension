using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Archer.Extension.Models
{
    public class TokenModel
    {
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string[] Roles { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;
        public DateTime NotValidBefore { get; set; } = DateTime.Now;
        public DateTime ExpirationTime { get; set; } = DateTime.Now;
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
