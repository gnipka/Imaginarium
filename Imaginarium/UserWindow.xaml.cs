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
        public ServiceGameClient client { get; set; }
        public string name { get; set; }
        public int ID { get; set; }
        public string association { get; set; }
        public int leaderID { get; set; }
        public string instruct { get; set; }

        public UserWindow()
        {
            InitializeComponent();

        }
        public void MsgCallback(string msg)
        {
            tbInstruct.Text = String.Empty;
            tbInstruct.Text = msg;
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
            string[] str = new string[2];
            str = client.SendInstruct(ID);
            instruct = str[0];
            tbInstruct.Text = instruct;
            leaderID = Convert.ToInt32(str[1]);            
        }

        private void Img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ID == leaderID)
            {

                if (tbInstruct.Text != null && tbInstruct.Text != instruct)
                {
                    association = tbInstruct.Text;
                    MessageBoxResult result = MessageBox.Show("Отправляем вашу ассоциацию игрокам?", "Имаджинариум", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            client.SendMsgAssoc(association, ID);
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Прежде чем выбрать картинку, введите ассоциацию.");
                }
            }
        }

        private void tbInstruct_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (leaderID == ID)
            {
                tbInstruct.Clear();
            }
        }

        public void SetAssoc(string msg)
        {
            tbInstruct.Text = String.Empty;
            tbInstruct.Text = msg;
        }

        public void MsgCallbackAssoc(string msg)
        {

        }
    }
}
