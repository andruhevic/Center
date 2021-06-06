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
    /// Логика взаимодействия для AddEdAdmEvPage.xaml
    /// </summary>
    public partial class AddEdAdmEvPage : Page
    {
        private Event _currentEvent = new Event();

        public AddEdAdmEvPage(Event selectedEvent)
        {
            InitializeComponent();
            if (selectedEvent != null)
            {
                _currentEvent = selectedEvent;
            }
            DataContext = _currentEvent;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentEvent.Name))
            {
                errors.AppendLine("Укажите название группы");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentEvent.Id == 0)
            {
                string str = "SELECT * FROM Event WHERE Name = '" + _currentEvent.Name + "'";
                var query = CenterOfCreativityBaseEntities.GetContext().Event.SqlQuery(str).ToArray();
                if (query.Length == 0)
                {
                    CenterOfCreativityBaseEntities.GetContext().Event.Add(_currentEvent);
                }
                else
                {
                    MessageBox.Show("Такое мероприятие уже существует", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
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
