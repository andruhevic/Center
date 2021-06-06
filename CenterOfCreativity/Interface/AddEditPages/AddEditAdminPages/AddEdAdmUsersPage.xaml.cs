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

namespace CenterOfCreativity.Interface.AddEditPages.AddEditAdminPages
{
    /// <summary>
    /// Логика взаимодействия для AddEdAdmUsersPage.xaml
    /// </summary>
    public partial class AddEdAdmUsersPage : Page
    {
        private Users _currentUsers = new Users();

        public AddEdAdmUsersPage(Users selectedUsers)
        {
            InitializeComponent();
            if (selectedUsers != null)
            {
                _currentUsers = selectedUsers;
            }
            DataContext = _currentUsers;
            comBoxRoles.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Roles.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentUsers.FirstName))
            {
                errors.AppendLine("Укажите имя");
            }
            if (string.IsNullOrWhiteSpace(_currentUsers.LastName))
            {
                errors.AppendLine("Укажите фамилию");
            }
            if (string.IsNullOrWhiteSpace(_currentUsers.Login))
            {
                errors.AppendLine("Укажите логин");
            }
            if (string.IsNullOrWhiteSpace(_currentUsers.Password))
            {
                errors.AppendLine("Укажите пароль");
            }
            if (_currentUsers.Roles == null)
            {
                errors.AppendLine("Укажите роль");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentUsers.Id == 0)
            {
                string str = "SELECT * FROM Users WHERE Login = '" + _currentUsers.Login + "'";
                var query = CenterOfCreativityBaseEntities.GetContext().Users.SqlQuery(str).ToArray();
                if (query.Length == 0)
                {
                    CenterOfCreativityBaseEntities.GetContext().Users.Add(_currentUsers);
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            try
            {
                CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Transition.BaseFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
