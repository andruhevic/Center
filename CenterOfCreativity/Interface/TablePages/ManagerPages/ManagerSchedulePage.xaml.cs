using CenterOfCreativity.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using CenterOfCreativity.Classes;
using CenterOfCreativity.Interface.AddEditPages.AddEditManagerPages;

namespace CenterOfCreativity.Interface.ManagerPages
{
    /// <summary>
    /// Логика взаимодействия для ManagerSchedulePage.xaml
    /// </summary>
    public partial class ManagerSchedulePage : Page
    {
        public ManagerSchedulePage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridSchedule.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Schedule.ToList();
                Update();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var scheduleForRemoving = dataGridSchedule.SelectedItems.Cast<Schedule>().ToList();
            MessageBoxResult mesBoxRes = MessageBox.Show($"Вы точно хотите удалить следующие {scheduleForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (mesBoxRes == MessageBoxResult.Yes)
            {
                try
                {
                    CenterOfCreativityBaseEntities.GetContext().Schedule.RemoveRange(scheduleForRemoving);
                    CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    dataGridSchedule.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Schedule.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdManSchedPage(null));
            Transition.currentPage = "Добавление расписания";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdManSchedPage((sender as Button).DataContext as Schedule));
            Transition.currentPage = "Редактирование расписания";
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new PrintPage());
            Transition.currentPage = "Вывод на печать";
        }

        private void comBoxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            List<Schedule> currentSchedule = CenterOfCreativityBaseEntities.GetContext().Schedule.ToList();
            dataGridSchedule.ItemsSource = currentSchedule.Where(p =>
            p.Groups.Name.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Event.Name.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();

            if (comBoxSearch.SelectedIndex == 0)
            {
                dataGridSchedule.Items.SortDescriptions.Add(new SortDescription("Groups.Name", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 1)
            {
                dataGridSchedule.Items.SortDescriptions.Add(new SortDescription("Event.Name", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 2)
            {
                dataGridSchedule.Items.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 3)
            {
                dataGridSchedule.Items.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 4)
            {
                dataGridSchedule.Items.SortDescriptions.Add(new SortDescription("EndTime", ListSortDirection.Descending));
            }
        }

    }
}
