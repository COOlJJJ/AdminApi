using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class TokenInfoViewModel
    {
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; } = "Bearer";
    }
}
