using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class UserList_Model
    {
        public List<User_Model> UserList { get; set; } = new List<User_Model>();

        public int Totalpage { get; set; }
    }

    public class User_Model
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string ReMark { get; set; }
        public string Roles { get; set; }
    }

    public class AddUser_Model
    {
        public string account { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string remark { get; set; }
        public string username { get; set; }
    }

    public class EditUser_Model
    {
        public string account { get; set; }
        public string email { get; set; }
        public int userid { get; set; }
        public string remark { get; set; }
        public string username { get; set; }
    }

    public class GiveRole_Model
    {
        public List<string> newrole { get; set; }

        public int userid { get; set; }

    }
}
