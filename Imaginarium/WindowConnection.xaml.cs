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
using System.Windows.Shapes;
using Imaginarium.ServiceReference1;



namespace Imaginarium
{
    /// <summary>
    /// Логика взаимодействия для WindowConnection.xaml
    /// </summary>
    public partial class WindowConnection : Window, IServiceGameCallback
    {
        ServiceGameClient client;
        int[] mas;

        public WindowConnection()
        {
            InitializeComponent();
        }
        public string name { get; set; }


        public void MsgCallback(string msg)
        {
            lbMsg.Items.Add(msg);
            pb.Value++;
        }

        public void MsgCallbackAssoc(string msg)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.SetAssoc(msg);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceGameClient(new System.ServiceModel.InstanceContext(this));
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

        private void pb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(e.NewValue == 3)
            {
                UserWindow userWindow = new UserWindow();
                userWindow.name = name;
                userWindow.ID = mas[0];
                userWindow.Show();
                this.Hide();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.client = client;
        }
    }
}
