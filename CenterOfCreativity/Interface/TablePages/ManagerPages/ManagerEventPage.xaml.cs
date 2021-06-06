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

namespace CenterOfCreativity.Interface.ManagerPages
{
    /// <summary>
    /// Логика взаимодействия для ManagerEventPage.xaml
    /// </summary>
    public partial class ManagerEventPage : Page
    {
        public ManagerEventPage()
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
