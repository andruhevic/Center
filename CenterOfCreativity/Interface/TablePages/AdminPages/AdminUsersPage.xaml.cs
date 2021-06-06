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
    /// Логика взаимодействия для AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        public AdminUsersPage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridUsers.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Users.ToList();
                Update();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdAdmUsersPage(null));
            Transition.currentPage = "Добавление пользователя";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEdAdmUsersPage((sender as Button).DataContext as Users));
            Transition.currentPage = "Редактирование пользователя";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = dataGridUsers.SelectedItems.Cast<Users>().ToList();
            MessageBoxResult mesBoxRes = MessageBox.Show($"Вы точно хотите удалить следующие {usersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (mesBoxRes == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var user in usersForRemoving)
                    {
                        if (user.Login == Transition.userLogin)
                        {
                            MessageBox.Show("Нельзя удалить пользователя под которым вы находитесь!", "Удаление", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    CenterOfCreativityBaseEntities.GetContext().Users.RemoveRange(usersForRemoving);
                    CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    dataGridUsers.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Users.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
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
            List<Users> currentUsers = CenterOfCreativityBaseEntities.GetContext().Users.ToList();
            dataGridUsers.ItemsSource = currentUsers.Where(p =>
            p.FirstName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.LastName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Patronymic.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Login.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Roles.Name.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();

            if (comBoxSearch.SelectedIndex == 0)
            {
                dataGridUsers.Items.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 1)
            {
                dataGridUsers.Items.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 2)
            {
                dataGridUsers.Items.SortDescriptions.Add(new SortDescription("Patronymic", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 3)
            {
                dataGridUsers.Items.SortDescriptions.Add(new SortDescription("Login", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 4)
            {
                dataGridUsers.Items.SortDescriptions.Add(new SortDescription("Roles.Name", ListSortDirection.Descending));
            }
        }
    }
}
