using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using System.Windows.Shapes;
using Imaginarium.ServiceReference1;

namespace Imaginarium
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    /// 
    // [CallbackBehaviorAttribute(ConcurrencyMode = ConcurrencyMode.Multiple)]

    //public class getMsg : WindowConnection
    //{
    //    UserWindow _w;
    //    public getMsg(UserWindow w) { _w = w; }
    //    public static string Msg { get; set; }
    //    public override void MsgCallback(string msg)
    //    {
    //        _w.tbInstruct.Text = msg;
    //    }
    //}

    public partial class UserWindow
    {


        public static void Callback(string msg)
        {

        }

        public ServiceGameClient client { get; set; }

        /// <summary>
        /// Имя игрока
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ID игрока
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Ассоциация ведущего
        /// </summary>
        public string association { get; set; }
        /// <summary>
        /// ID ведущего
        /// </summary>
        public int leaderID { get; set; }
        /// <summary>
        /// Сообщение для игроков во время загадывания ассоциации
        /// </summary>
        public string instruct { get; set; }
        /// <summary>
        /// Маячок, чтобы игроки после получения ассоциации могли выбрать картинку
        /// </summary>
        public bool signal { get; set; }
        /// <summary>
        /// Маячок, для действий ведущего
        /// </summary>
        public bool signalLeader { get; set; }
        /// <summary>
        /// Маячок выбора карты ведущего
        /// </summary>
        public bool signalRound { get; set; }
        public Dictionary<int, int> scoring;

        public UserWindow()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> nameImg = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                string name = client.ReturnNameImage();
                nameImg.Add(Convert.ToInt32(name));
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
            signal = false;
            signalLeader = true;
            signalRound = true;
            leaderID = Convert.ToInt32(str[1]);
        }
        public void GetMsg(string msg)
        {
            tbInstruct.Text = msg;
        }

        private void Img1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ID == leaderID)
            {

                if (tbInstruct.Text != null && tbInstruct.Text != instruct && signalLeader == true)
                {
                    association = tbInstruct.Text;
                    MessageBoxResult result = MessageBox.Show("Отправляем вашу ассоциацию игрокам?", "Имаджинариум", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Image image = (Image)sender;
                            //отправляем выбранную картинку на сервер
                            client.AddImageInRound(ID, image.Source.ToString());
                            image.Source = new BitmapImage(new Uri("Images/" + client.ReturnNameImage() + ".jpg", UriKind.Relative));
                            //отправляем ассоциацию на сервер
                            client.SendMsg(association, ID);
                            tbInstruct.Text = $"Игроки выбирают карту, которая соответствует Вашей ассоциации - {association}";
                            signalLeader = false;
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Прежде чем выбрать карту, введите ассоциацию.");
                }
            }
            else
            {
                if (signal)
                {
                    MessageBoxResult result = MessageBox.Show("Отправляем карту?", "Имаджинариум", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Image image = (Image)sender;
                            //отправляем выбранную картинку на сервер
                            Image imageTest = new Image() { Source = image.Source };
                            string str = client.ReturnNameImage();
                            //Если пришла картинка с именем null, значит закончились все использованные картинки,тогда игра зканчивается
                            if (str != null)
                            {
                                image.Source = new BitmapImage(new Uri("Images/" + str + ".jpg", UriKind.Relative));
                                client.AddImageInRound(ID, imageTest.Source.ToString());
                                tbInstruct.Text = "Другие игроки еще выбирают карту, ожидайте";
                                signal = false;
                            }
                            else
                            {
                                image.Source = null;
                                client.AddImageInRound(ID, imageTest.Source.ToString());
                                tbInstruct.Text = "Карты закончились. Это последний раунд. Другие игроки еще выбирают карту, ожидайте";
                                signal = false;
                            }
                            //image.Source = new BitmapImage(new Uri("Images/" + client.ReturnNameImage() + ".jpg", UriKind.Relative));
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
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
        /// <summary>
        /// Получение ассоциации с сервера
        /// </summary>
        /// <param name="msg"></param>
        public void ReturnMsgCallback(string msg)
        {
            if (ID != leaderID)
            {
                tbInstruct.Text = msg;
            }
            signal = true;
        }
        public void ReturnScoringPlayers()
        {

            TopPlayers topPlayers = new TopPlayers(client.ReturnPoints());
            TopPlayers.signal = false;
            topPlayers.Show();
            string[] str = new string[2];
            str = client.SendInstruct(ID);
            instruct = str[0];
            tbInstruct.Text = instruct;
            leaderID = Convert.ToInt32(str[1]);
            signalLeader = true;
            signalRound = true;
            Img11.Source = null;
            Img22.Source = null;
            Img33.Source = null;
            Img44.Source = null;
            Img55.Source = null;
            tb1.Text = null;
            tb2.Text = null;
            tb3.Text = null;
            tb4.Text = null;
            tb5.Text = null;
        }
        public void EndGame()
        {
            TopPlayers topPlayers = new TopPlayers(client.ReturnPoints());
            TopPlayers.signal = true;
            topPlayers.Show();
            this.Hide();
        }

        public void ReturnImage(string images)
        {
            if (ID == leaderID)
            {
                tbInstruct.Text = "Игроки угадывают Вашу карту";
            }
            else
            {
                tbInstruct.Text = "Выберите карту, которую загадал ведущий";
            }
            string value = images.Trim();
            int[] arrayImages = value.Split(' ').Select(x => int.Parse(x)).ToArray();
            Img11.Source = new BitmapImage(new Uri("Images/" + arrayImages[0] + ".jpg", UriKind.Relative));
            Img22.Source = new BitmapImage(new Uri("Images/" + arrayImages[1] + ".jpg", UriKind.Relative));
            Img33.Source = new BitmapImage(new Uri("Images/" + arrayImages[2] + ".jpg", UriKind.Relative));
            Img44.Source = new BitmapImage(new Uri("Images/" + arrayImages[3] + ".jpg", UriKind.Relative));
            Img55.Source = new BitmapImage(new Uri("Images/" + arrayImages[4] + ".jpg", UriKind.Relative));
        }

        private void Img11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ID != leaderID && signalRound == true)
            {
                MessageBoxResult result = MessageBox.Show("Отправляем карту?", "Имаджинариум", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Image image = (Image)sender;
                        string nameImage = image.Source.ToString();
                        client.AddAnswer(nameImage, ID);
                        Dictionary<string, int> CardAndName = client.ReturnCardAndName();

                        foreach (var item in CardAndName)
                        {
                            var source = new BitmapImage(new Uri("pack://application:,,,/Imaginarium;component/Images/" + item.Value + ".jpg", UriKind.Absolute));
                            if (Img11.Source.ToString() == source.ToString())
                            {
                                tb1.Text = item.Key;
                            }
                            else if (Img22.Source.ToString() == source.ToString())
                            {
                                tb2.Text = item.Key;
                            }
                            else if (Img33.Source.ToString() == source.ToString())
                            {
                                tb3.Text = item.Key;
                            }
                            else if (Img44.Source.ToString() == source.ToString())
                            {
                                tb4.Text = item.Key;
                            }
                            else if (Img55.Source.ToString() == source.ToString())
                            {
                                tb5.Text = item.Key;
                            }
                        }
                        tbInstruct.Text = "Ваш ответ отправлен. Ожидайте пока все участники выберут карту.";
                        signalRound = false;
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
    }
}
