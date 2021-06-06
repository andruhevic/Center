using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CenterOfCreativity.Classes
{
    class Transition
    {
        public static string userName;
        public static string userLastName;
        public static string userLogin;
        public static int userRole;

        public static string currentPage = "Авторизация";

        public static bool delLogEntries;

        public static Frame BaseFrame { get; set; }
    }
}
