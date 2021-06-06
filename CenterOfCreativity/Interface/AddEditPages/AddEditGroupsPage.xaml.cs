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

namespace CenterOfCreativity.Interface.AddEditPages
{
    /// <summary>
    /// Логика взаимодействия для AddEditGroupsPage.xaml
    /// </summary>
    public partial class AddEditGroupsPage : Page
    {
        private Groups _currentGroups = new Groups();

        public AddEditGroupsPage(Groups selectedGroups)
        {
            InitializeComponent();
            if (selectedGroups != null)
            {
                _currentGroups = selectedGroups;
            }
            DataContext = _currentGroups;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentGroups.Name))
            {
                errors.AppendLine("Укажите название группы");
            }
            if (string.IsNullOrWhiteSpace(_currentGroups.CountOfMembers.ToString()))
            {
                errors.AppendLine("Укажите количество участников");
            }
            if (_currentGroups.Id == 0)
            {
                string str = "SELECT * FROM Groups WHERE Name = '" + _currentGroups.Name + "'";
                var query = CenterOfCreativityBaseEntities.GetContext().Groups.SqlQuery(str).ToArray();
                if (query.Length == 0)
                {
                    CenterOfCreativityBaseEntities.GetContext().Groups.Add(_currentGroups);
                }
                else
                {
                    MessageBox.Show("Такая группа уже существует", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
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
