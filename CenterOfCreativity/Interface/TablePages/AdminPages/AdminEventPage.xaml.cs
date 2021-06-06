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
using CenterOfCreativity.BaseModel;
using CenterOfCreativity.Classes;
using CenterOfCreativity.Interface.AddEditPages.AddEditAdminPages;

namespace CenterOfCreativity.Interface.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AdminEventPage.xaml
    /// </summary>
    public partial class AdminEventPage : Page
    {
        public AdminEventPage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                dataGridEvents.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridEvents.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Event.ToList();
                Update();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdAdmEvPage(null));
            Transition.currentPage = "Добавление мероприятия";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdAdmEvPage((sender as Button).DataContext as Event));
            Transition.currentPage = "Редактирование мероприятия";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var eventForRemoving = dataGridEvents.SelectedItems.Cast<Event>().ToList();
            MessageBoxResult mesBoxRes = MessageBox.Show($"Вы точно хотите удалить следующие {eventForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (mesBoxRes == MessageBoxResult.Yes)
            {
                try
                {
                    CenterOfCreativityBaseEntities.GetContext().Event.RemoveRange(eventForRemoving);
                    CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    dataGridEvents.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Event.ToList();
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

        private void Update()
        {
            List<Event> currentEvent = CenterOfCreativityBaseEntities.GetContext().Event.ToList();
            dataGridEvents.ItemsSource = currentEvent.Where(p =>
            p.Name.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Description.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();
        }
    }
}
