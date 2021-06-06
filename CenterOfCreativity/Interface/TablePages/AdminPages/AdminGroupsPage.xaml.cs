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
using CenterOfCreativity.Interface.AddEditPages;

namespace CenterOfCreativity.Interface.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AdminGroupsPage.xaml
    /// </summary>
    public partial class AdminGroupsPage : Page
    {
        public AdminGroupsPage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridGroups.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Groups.ToList();
                Update();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEditGroupsPage((sender as Button).DataContext as Groups));
            Transition.currentPage = "Редактирование группы";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.BaseFrame.Navigate(new AddEditGroupsPage(null));
            Transition.currentPage = "Добавление группы";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var groupsForRemoving = dataGridGroups.SelectedItems.Cast<Groups>().ToList();
            MessageBoxResult mesBoxRes = MessageBox.Show($"Вы точно хотите удалить следующие {groupsForRemoving.Count()} элементов?", "Внимание", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);
            if (mesBoxRes == MessageBoxResult.Yes)
            {
                try 
                {
                    CenterOfCreativityBaseEntities.GetContext().Groups.RemoveRange(groupsForRemoving);
                    CenterOfCreativityBaseEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    dataGridGroups.ItemsSource = CenterOfCreativityBaseEntities.GetContext().Groups.ToList();
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
            List<Groups> currentGroups = CenterOfCreativityBaseEntities.GetContext().Groups.ToList();
            dataGridGroups.ItemsSource = currentGroups.Where(p =>
            p.Name.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.CountOfMembers.ToString().ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();
        }
    }
}
