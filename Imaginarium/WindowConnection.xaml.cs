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
using Imaginarium.ServiceHost;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceGameClient(new System.ServiceModel.InstanceContext(this));
            mas = client.Connect(name);            
            pb.Value = mas[0];
        }
    }
}
