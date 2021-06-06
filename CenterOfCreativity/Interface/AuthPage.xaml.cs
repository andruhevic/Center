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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CenterOfCreativity.Classes;
using CenterOfCreativity.BaseModel;
using CenterOfCreativity.Interface.ManagerPages;
using CenterOfCreativity.Interface.AdminPages;

namespace CenterOfCreativity.Interface
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int _numOfFail = 0;
        private int _userRoleId = 0;
        private int _userId = 0;
        private string _userLogin = "";
        private string _userPass = "";

        public AuthPage()
        {
            InitializeComponent();
            CheckConnectionAsync();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((textBoxLogin.Text != "") && (passBoxPassword.Password != ""))
                {
                    string logQuery = "SELECT * FROM Users WHERE Login = '" + textBoxLogin.Text + "'";
                    var list = CenterOfCreativityBaseEntities.GetContext().Users.SqlQuery(logQuery).ToArray();
                    if (list.Length != 0 && passBoxPassword.Password == list[0].Password)
                    {
                        var row = list[0];
                        _userId = row.Id;
                        _userRoleId = row.RoleId;
                        _userLogin = row.Login;
                        _userPass = row.Password;
                        Transition.userName = row.FirstName;
                        Transition.userLastName = row.LastName;
                        Transition.userRole = row.RoleId;
                        Transition.userLogin = row.Login;
                        if ((_userRoleId == 1) && (_userPass == passBoxPassword.Password))
                        {
                            MessageBox.Show("Авторизация прошла успешно!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                            Transition.BaseFrame.Navigate(new AdminHistoryPage());
                            RecordLogIn(_userId, true);
                            _numOfFail = 0;
                            Transition.delLogEntries = true;
                            Transition.currentPage = "История входа";
                            return;
                        }
                        else if ((_userRoleId == 2) && (_userPass == passBoxPassword.Password))
                        {
                            MessageBox.Show("Авторизация прошла успешно!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                            Transition.BaseFrame.Navigate(new ManagerSchedulePage());
                            RecordLogIn(_userId, true);
                            _numOfFail = 0;
                            Transition.delLogEntries = true;
                            Transition.currentPage = "Расписание мероприятий";
                            return;
                        }
                    }
                    else
                    {
                        if (_userLogin != "")
                        {
                            RecordLogIn(_userId, false);
                        }
                        MessageBox.Show("Неверный логин или пароль!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                        _numOfFail++;
                    }

                    if (_numOfFail == 3)
                    {
                        MessageBox.Show("Превышено допустимое число попыток входа, подождите 10 секунд и попробуйте снова.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                        BlockAuthAsync();
                    }
                }
                else if ((textBoxLogin.Text == "") && (passBoxPassword.Password != ""))
                {
                    MessageBox.Show("Не заполнено поле логина!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if ((textBoxLogin.Text != "") && (passBoxPassword.Password == ""))
                {
                    MessageBox.Show("Не заполнено поле пароля!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if ((textBoxLogin.Text == "") && (passBoxPassword.Password == ""))
                {
                    MessageBox.Show("Не заполнены поля логина и пароля!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Отсутсвует связь с базой");
            }
        }

        private void btnPass_Click(object sender, RoutedEventArgs e)
        {
            btnPass.Click += PassHidden_Click;
            btnPass.Click -= btnPass_Click;
            textBoxShowPass.Text = passBoxPassword.Password;
            passBoxPassword.Visibility = Visibility.Collapsed;
            textBoxShowPass.Visibility = Visibility.Visible;
        }

        private void PassHidden_Click(object sender, RoutedEventArgs e)
        {
            btnPass.Click += btnPass_Click;
            btnPass.Click -= PassHidden_Click;
            passBoxPassword.Password = textBoxShowPass.Text;
            passBoxPassword.Visibility = Visibility.Visible;
            textBoxShowPass.Visibility = Visibility.Collapsed;
        }

        private void btnRetryCon_Click(object sender, RoutedEventArgs e)
        {
            btnRetryCon.Visibility = Visibility.Collapsed;
            CheckConnectionAsync();
        }

        private async void BlockAuthAsync()
        {
            btnAuth.IsEnabled = false;
            await Task.Run(() => Thread.Sleep(10000));
            await Dispatcher.BeginInvoke(new ThreadStart(delegate { btnAuth.IsEnabled = true; }));
        }

        private void RecordLogIn(int user, bool successful)
        {
            try
            {
                LoginHistory historyEnter = new LoginHistory();
                historyEnter.UserId = user;
                historyEnter.Date = DateTime.Now;
                historyEnter.Successful = successful;
                CenterOfCreativityBaseEntities.GetContext().LoginHistory.Add(historyEnter);
                CenterOfCreativityBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private async void CheckConnectionAsync()
        {
            textBlockCon.Visibility = Visibility.Visible;
            btnAuth.IsEnabled = false;
            btnPass.IsEnabled = false;
            textBoxLogin.IsEnabled = false;
            textBoxShowPass.IsEnabled = false;
            passBoxPassword.IsEnabled = false;
            try
            {
                CenterOfCreativityBaseEntities dataBase = new CenterOfCreativityBaseEntities();
                Task t = Task.Run(() => dataBase.Database.Connection.Open());
                await t;
            }
            catch
            {
                MessageBox.Show("Отсутствует связь с базой данных. Проверьте подключение и повторите попытку.", "Соединение", MessageBoxButton.OK, MessageBoxImage.Error);
                textBlockCon.Visibility = Visibility.Collapsed;
                btnRetryCon.Visibility = Visibility.Visible;
                return;
            }
            textBlockCon.Visibility = Visibility.Collapsed;
            await Dispatcher.BeginInvoke(new ThreadStart(delegate { 
                btnAuth.IsEnabled = true;
                btnPass.IsEnabled = true;
                textBoxLogin.IsEnabled = true; 
                textBoxShowPass.IsEnabled = true; 
                passBoxPassword.IsEnabled = true; }));
        }
    }
}