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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window, IServiceGameCallback
    {
        ServiceGameClient client;
        public string name { get; set; }
        public int ID { get; set; }
        public UserWindow()
        {
            InitializeComponent();

        }
        public void MsgCallback(string msg)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = new ServiceGameClient(new System.ServiceModel.InstanceContext(this));
            List<int> nameImg = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                nameImg.Add(client.ReturnNameImage());
            }
            Img1.Source = new BitmapImage(new Uri("Images/" + nameImg[0] + ".jpg", UriKind.Relative));
            Img2.Source = new BitmapImage(new Uri("Images/" + nameImg[1] + ".jpg", UriKind.Relative));
            Img3.Source = new BitmapImage(new Uri("Images/" + nameImg[2] + ".jpg", UriKind.Relative));
            Img4.Source = new BitmapImage(new Uri("Images/" + nameImg[3] + ".jpg", UriKind.Relative));
            Img5.Source = new BitmapImage(new Uri("Images/" + nameImg[4] + ".jpg", UriKind.Relative));
            this.Title = "Пользователь " + name;
            string msg = client.SendInstruct();
            tbInstruct.Text = msg;
        }
    }
}
