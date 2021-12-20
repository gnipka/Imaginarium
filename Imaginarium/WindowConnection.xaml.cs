using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Imaginarium.ServiceReference1;



namespace Imaginarium
{
    /// <summary>
    /// Логика взаимодействия для WindowConnection.xaml
    /// </summary>
    /// 
    [CallbackBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class WindowConnection : Window, IServiceGameCallback
    {

        UserWindow userWindow;
        ServiceGameClient client;
        int[] mas;

        public WindowConnection()
        {
            InitializeComponent();
        }
        public string name { get; set; }
        /// <summary>
        /// true - если вызываем ..., false - если вызываем user.ReturnMessageCallback в методе MsgCallback
        /// </summary>
        public bool signal { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new ServiceGameClient(new System.ServiceModel.InstanceContext(this));
            }
            catch(Exception ex)
            {
                MessageBox.Show($"При подключении к серверу возникла ошибка: {ex.Message}");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            try
            {
                mas = client.Connect(name);
                pb.Value = mas[0];
                if (mas[1] == 1)
                {
                    lbMsg.Items.Add("Происходит подключение, в данный момент к серверу подключены: " + mas[1] + " игрок");

                }
                else
                {
                    lbMsg.Items.Add("Происходит подключение, в данный момент к серверу подключены: " + mas[1] + " игрока");
                }
            }
            catch(System.ServiceModel.EndpointNotFoundException ex)
            {
                MessageBox.Show($"При подключении к серверу возникла ошибка: {ex.Message}");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void pb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(e.NewValue == 5)
            {
                userWindow = new UserWindow();
                userWindow.name = name;
                userWindow.ID = mas[0];
                userWindow.client = client;
                this.Visibility = Visibility.Hidden;
                this.ShowInTaskbar = false;
                userWindow.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                client.Close();
            }
            catch { }
        }

        public void MsgCallback(string msg)
        {
            if (userWindow != null)
            {
                if(msg == "Ready")
                {
                    userWindow.ReturnScoringPlayers();
                }
                else if(msg == "End")
                {
                    userWindow.EndGame();
                }
                else if (signal)
                {
                    userWindow.ReturnImage(msg);
                    signal = false;
                }
                else
                {
                    userWindow.ReturnMsgCallback(msg);
                    signal = true;
                }
            }
            else
            {
                lbMsg.Items.Add(msg);
                pb.Value++;
            }
        }
    }
}
