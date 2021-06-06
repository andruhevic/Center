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

namespace CenterOfCreativity.Interface.AdminPages
{
    /// <summary>
    /// Логика взаимодействия для AdminHistoryPage.xaml
    /// </summary>
    public partial class AdminHistoryPage : Page
    {
        public AdminHistoryPage()
        {
            InitializeComponent();
            Update();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                CenterOfCreativityBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dataGridLogHistory.ItemsSource = CenterOfCreativityBaseEntities.GetContext().LoginHistory.ToList();
                Update();
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
            List<LoginHistory> currentHistory = CenterOfCreativityBaseEntities.GetContext().LoginHistory.ToList();
            dataGridLogHistory.ItemsSource = currentHistory.Where(p =>
            p.Users.FirstName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Users.LastName.ToLower().Contains(textBoxSearch.Text.ToLower()) ||
            p.Users.Login.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();

            if (comBoxSearch.SelectedIndex == 0)
            {
                dataGridLogHistory.Items.SortDescriptions.Add(new SortDescription("Users.FirstName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 1)
            {
                dataGridLogHistory.Items.SortDescriptions.Add(new SortDescription("Users.LastName", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 2)
            {
                dataGridLogHistory.Items.SortDescriptions.Add(new SortDescription("Users.Login", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 3)
            {
                dataGridLogHistory.Items.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            }
            if (comBoxSearch.SelectedIndex == 4)
            {
                dataGridLogHistory.Items.SortDescriptions.Add(new SortDescription("Successful", ListSortDirection.Descending));
            }
        }
    }
}
