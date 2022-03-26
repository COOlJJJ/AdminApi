using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Context.DomainModel
{
    public class UserRole:BaseEntity
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
