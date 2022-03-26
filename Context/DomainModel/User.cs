using System;
using System.Collections.Generic;
using System.Text;

namespace Context.DomainModel
{
    public class User : BaseEntity
    {
        public string Account { get; set; }
        public string UserName { get; set; }

        public string PassWord { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string Mark { get; set; }
    }
}
