using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class UserInfo_Model
    {
        public int userid { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
        public string name { get; set; } = "Dong Jie";

        public string avatar { get; set; } = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif";
        public string introduction { get; set; } = "A siemens User";
    }
}
