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

namespace CenterOfCreativity.Interface.AddEditPages.AddEditManagerPages
{
    /// <summary>
    /// Логика взаимодействия для AddEdManVisPage.xaml
    /// </summary>
    public partial class AddEdManVisPage : Page
    {
        private Visitors _currentVisitors = new Visitors();

        public AddEdManVisPage(Visitors selectedVisitors)
        {
            InitializeComponent();
            if (selectedVisitors != null)
            {
                _currentVisitors = selectedVisitors;
            }
            DataContext = _currentVisitors;
            comBoxGroups.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Groups.ToList();
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            gridCalendar.Visibility = Visibility.Visible;
        }

        private void btnCalClose_Click(object sender, RoutedEventArgs e)
        {
            gridCalendar.Visibility = Visibility.Collapsed;
        }

        private void calDateOfBirth_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxDateOfBirth.Text = calDateOfBirth.SelectedDate.Value.ToString("dd/MM/yyyy");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentVisitors.FirstName))
            {
                errors.AppendLine("Укажите имя");
            }
            if (string.IsNullOrWhiteSpace(_currentVisitors.LastName))
            {
                errors.AppendLine("Укажите фамилию");
            }
            if (textBoxDateOfBirth.Text != "")
            {
                DateTime dateOfBirth = DateTime.Parse(textBoxDateOfBirth.Text);
                _currentVisitors.DateOfBirth = DateTime.Parse(dateOfBirth.ToString("yyyy/MM/dd"));
            }
            else
            {
                errors.AppendLine("Укажите дату рождения");
            }
            if (_currentVisitors.Groups == null)
            {
                errors.AppendLine("Укажите Группу");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentVisitors.Id == 0)
            {
                CenterOfCreativityBaseEntities.GetContext().Visitors.Add(_currentVisitors);
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
