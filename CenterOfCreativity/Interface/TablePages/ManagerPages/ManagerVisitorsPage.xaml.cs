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
    /// Логика взаимодействия для ManagerVisitorsPage.xaml
    /// </summary>
    public partial class ManagerVisitorsPage : Page
    {
        public ManagerVisitorsPage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridVisitors.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Visitors.ToList();
                Update();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdManVisPage((sender as Button).DataContext as Visitors));
            Transition.currentPage = "Редактирование данных о посетителе";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdManVisPage(null));
            Transition.currentPage = "Добавление данных о посетителе";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var visitorsForRemoving = dataGridVisitors.SelectedItems.Cast<Visitors>().ToList();
            MessageBoxResult mesBoxRes = MessageBox.Show($"Вы точно хотите удалить следующие {visitorsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (mesBoxRes == MessageBoxResult.Yes)
            {
                try
                {
                    CenterOfCreativityBaseEntities.GetContext().Visitors.RemoveRange(visitorsForRemoving);
                    CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    dataGridVisitors.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Visitors.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void comBoxSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            List<Visitors> currentVisitors = CenterOfCreativityBaseEntities.GetContext().Visitors.ToList();
            dataGridVisitors.ItemsSource = currentVisitors.Where(p =>
            p.FirstName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.LastName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Patronymic.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Groups.Name.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();

            if (comBoxSearch.SelectedIndex == 0)
            {
                dataGridVisitors.Items.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 1)
            {
                dataGridVisitors.Items.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 2)
            {
                dataGridVisitors.Items.SortDescriptions.Add(new SortDescription("Patronymic", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 3)
            {
                dataGridVisitors.Items.SortDescriptions.Add(new SortDescription("DateOfBirth", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 4)
            {
                dataGridVisitors.Items.SortDescriptions.Add(new SortDescription("Groups.Name", ListSortDirection.Descending));
            }
        }
    }
}
