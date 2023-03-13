using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class UsersModel : DatabaseConnection
    {
        public static SqlCommand cmd;
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public bool isActive { get; set; }
    }
}
