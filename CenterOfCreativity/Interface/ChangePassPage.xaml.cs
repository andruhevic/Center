using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CenterOfCreativity.BaseModel;
using CenterOfCreativity.Classes;

namespace CenterOfCreativity.Interface
{
    /// <summary>
    /// Логика взаимодействия для ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (passBoxOldPass.Password != "" && passBoxNewPass.Password != "" 
                && passBoxRePass.Password != "")
            {
                string str = "SELECT * FROM Users WHERE Login = '" + Transition.userLogin + "'";
                var query = CenterOfCreativityBaseEntities.GetContext().Users.SqlQuery(str).ToArray();
                string pass = query[0].Password;
                if (passBoxOldPass.Password == pass && passBoxNewPass.Password 
                    == passBoxRePass.Password)
                {
                    var changePass = new CenterOfCreativityBaseEntities();
                    string updateQuery = "UPDATE Users SET Password = '"
                        + passBoxNewPass.Password + "' WHERE Login = '"
                        + Transition.userLogin + "'";
                    changePass.Database.ExecuteSqlCommand(updateQuery);
                }
            }
        }
    }
}
