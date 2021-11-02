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

namespace Imaginarium
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            tbUserName.Focus();
        }



        private void Button_Click_StartGame(object sender, RoutedEventArgs e)
        {
            if (tbUserName.Text != null)
            {
                WindowConnection windowConnection = new WindowConnection();
                windowConnection.name = tbUserName.Text;
                windowConnection.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Заполните поле для ввода имени");
            }
        }
    }
}
