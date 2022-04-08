using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class RoleInfo_Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AddRole_Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EidtRole_Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
