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
    /// Логика взаимодействия для AddEdManSchedPage.xaml
    /// </summary>
    public partial class AddEdManSchedPage : Page
    {
        private Schedule _currentSchedule = new Schedule();

        public AddEdManSchedPage(Schedule selectedSchedule)
        {
            InitializeComponent();
            if (selectedSchedule != null)
            {
                _currentSchedule = selectedSchedule;
            }
            DataContext = _currentSchedule;
            comBoxEvents.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Event.ToList();
            comBoxGroups.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Groups.ToList();
        }

        private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxDate.Text = calDate.SelectedDate.Value.ToString("dd/MM/yyyy");
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            gridCalendar.Visibility = Visibility.Visible;
        }

        private void btnCalClose_Click(object sender, RoutedEventArgs e)
        {
            gridCalendar.Visibility = Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentSchedule.Groups == null)
            {
                errors.AppendLine("Укажите Группу");
            }
            if (_currentSchedule.Event == null)
            {
                errors.AppendLine("Укажите мероприятие");
            }
            if (textBoxDate.Text != "")
            {
                DateTime dateOfBirth = DateTime.Parse(textBoxDate.Text);
                _currentSchedule.Date = DateTime.Parse(dateOfBirth.ToString("yyyy/MM/dd"));
            }
            else
            {
                errors.AppendLine("Укажите Дату");
            }
            if (textBoxStTimeHours.Text.All(char.IsDigit) && textBoxStTimeMin.Text.All(char.IsDigit))
            {
                string stTimeStr = textBoxStTimeHours.Text + ":" + textBoxStTimeMin.Text;
                _currentSchedule.StartTime = stTimeStr;
            }
            else
            {
                errors.AppendLine("Укажите время начала");
            }
            if (textBoxEndTimeHours.Text.All(char.IsDigit) && textBoxEndTimeMin.Text.All(char.IsDigit))
            {
                string endTimeStr = textBoxEndTimeHours.Text + ":" + textBoxEndTimeMin.Text;
                _currentSchedule.EndTime = endTimeStr;
            }
            else
            {
                errors.AppendLine("Укажите время окончания");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            CenterOfCreativityBaseEntities.GetContext().Schedule.Add(_currentSchedule);
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
