using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CenterOfCreativity.Classes;
using CenterOfCreativity.BaseModel;
using CenterOfCreativity.Interface;
using CenterOfCreativity.Interface.ManagerPages;
using CenterOfCreativity.Interface.AdminPages;

namespace CenterOfCreativity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {

        private string _role = "";
        private int _index = 0;

        public BaseWindow()
        {
            InitializeComponent();
            baseFrame.Navigate(new Interface.AuthPage());
            Transition.BaseFrame = baseFrame;
        }

        private void baseFrame_ContentRendered(object sender, EventArgs e)
        {
            textBlockPage.Text = Transition.currentPage;
            if (Transition.userRole == 1)
            {
                _role = "Администратор";
            }
            else if (Transition.userRole == 2)
            {
                _role = "Менеджер";
            }
            if (_role == "Менеджер")
            {
                menuItemVisitors.Visibility = Visibility.Visible;
                menuItemSchedule.Visibility = Visibility.Visible;
                menuItemGroupsManager.Visibility = Visibility.Visible;
                menuItemEventManager.Visibility = Visibility.Visible;
            }
            else if (_role == "Администратор")
            {
                menuItemHistory.Visibility = Visibility.Visible;
                menuItemEventAdmin.Visibility = Visibility.Visible;
                menuItemGroupsAdmin.Visibility = Visibility.Visible;
                menuItemUsers.Visibility = Visibility.Visible;
            }
            if (baseFrame.Content.GetType().Name == "AuthPage" )
            {
                while (baseFrame.CanGoBack)
                {
                    baseFrame.NavigationService.RemoveBackEntry();
                }
                textBlockUser.Text = "";
                textBlockUser.Visibility = Visibility.Collapsed;
                btnMenu.Visibility = Visibility.Collapsed;
                menuMain.Visibility = Visibility.Collapsed;
                gridMenu.Visibility = Visibility.Collapsed;

                menuItemVisitors.Visibility = Visibility.Collapsed;
                menuItemSchedule.Visibility = Visibility.Collapsed;
                menuItemGroupsManager.Visibility = Visibility.Collapsed;
                menuItemEventManager.Visibility = Visibility.Collapsed;

                menuItemHistory.Visibility = Visibility.Collapsed;
                menuItemEventAdmin.Visibility = Visibility.Collapsed;
                menuItemGroupsAdmin.Visibility = Visibility.Collapsed;
                menuItemUsers.Visibility = Visibility.Collapsed;

                btnMenu.Click -= HiddenMenu_Click;
                btnMenu.Click -= btnMenu_Click;
                btnMenu.Click += btnMenu_Click;

                ResizeMode = ResizeMode.NoResize;
                MinWidth = 800;
                MinHeight = 520;
                Width = MinWidth;
                Height = MinHeight;
                WindowState = WindowState.Normal;
            }
            else
            {
                textBlockUser.Text = _role + " " + Transition.userName + " " + Transition.userLastName;

                textBlockUser.Visibility = Visibility.Visible;
                btnMenu.Visibility = Visibility.Visible;

                ResizeMode = ResizeMode.CanResize;

                if (Transition.delLogEntries == true)
                {
                    MinWidth = 1000;
                    MinHeight = 700;
                    Width = MinWidth;
                    Height = MinHeight;
                }
            }
            if (Transition.delLogEntries == true)
            {
                baseFrame.NavigationService.RemoveBackEntry();
                Transition.delLogEntries = false;
            }
            if (baseFrame.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Collapsed;
            }

        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            menuMain.Visibility = Visibility.Visible;
            DoubleAnimation showMenu = new DoubleAnimation();
            showMenu.From = 1;
            showMenu.To = 200;
            showMenu.Duration = TimeSpan.FromSeconds(0.15);
            menuMain.BeginAnimation(WidthProperty, showMenu);
            gridMenu.Visibility = Visibility.Visible;
            btnMenu.Click += HiddenMenu_Click;
            btnMenu.Click -= btnMenu_Click;
        }

        private void HiddenMenu_Click(object sender, EventArgs e)
        {
            HiddenMenuAnim();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _index++;
            baseFrame.GoBack();
        }

        private void menuItemClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgRes = MessageBox.Show("Завершить работу в приложении?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dlgRes == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dlgRes = MessageBox.Show("Выйти из учётной записи?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dlgRes == MessageBoxResult.Yes)
            {
                baseFrame.Navigate(new AuthPage());
                while (baseFrame.CanGoBack)
                {
                    baseFrame.NavigationService.RemoveBackEntry();
                }
            }
        }

        private void menuItemUsers_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new AdminUsersPage());
            Transition.currentPage = "Список пользователей";
            HiddenMenuAnim();
        }

        private void menuItemHistory_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new AdminHistoryPage());
            Transition.currentPage = "История входа";
            HiddenMenuAnim();
        }

        private void menuItemGroupsAdmin_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new AdminGroupsPage());
            Transition.currentPage = "Список групп";
            HiddenMenuAnim();
        }

        private void menuItemEventAdmin_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new AdminEventPage());
            Transition.currentPage = "Список мероприятий";
            HiddenMenuAnim();
        }

        private void menuItemVisitors_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new ManagerVisitorsPage());
            Transition.currentPage = "Список посетителей";
            HiddenMenuAnim();
        }

        private void menuItemSchedule_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new ManagerSchedulePage());
            Transition.currentPage = "Расписание мероприятий";
            HiddenMenuAnim();
        }

        private void menuItemGroupsManager_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new ManagerGroupsPage());
            Transition.currentPage = "Список групп";
            HiddenMenuAnim();
        }

        private void menuItemEventManager_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new ManagerEventPage());
            Transition.currentPage = "Список мероприятий";
            HiddenMenuAnim();
        }

        private void menuItemChangePass_Click(object sender, RoutedEventArgs e)
        {
            baseFrame.Navigate(new ChangePassPage());
            Transition.currentPage = "Смена пароля";
            HiddenMenuAnim();
        }

        private async void HiddenMenuAnimAsync()
        {
            await Task.Run(() => Thread.Sleep(160));
            await Dispatcher.BeginInvoke(new ThreadStart(delegate { menuMain.Visibility = Visibility.Collapsed; }));
        }

        private void HiddenMenuAnim()
        {
            DoubleAnimation hiddenMenu = new DoubleAnimation();
            hiddenMenu.From = 200;
            hiddenMenu.To = 1;
            hiddenMenu.Duration = TimeSpan.FromSeconds(0.15);
            menuMain.BeginAnimation(WidthProperty, hiddenMenu);
            gridMenu.Visibility = Visibility.Collapsed;
            btnMenu.Click += btnMenu_Click;
            btnMenu.Click -= HiddenMenu_Click;
            HiddenMenuAnimAsync();
        }
    }
}