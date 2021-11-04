using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace WcfHosting
{
    [ServiceContract(CallbackContract = typeof(IServerGameCallback))]
    public interface IServiceGame
    {
        [OperationContract]
        int[] Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);

        [OperationContract]
        int ReturnNameImage();

        [OperationContract]
        string[] SendInstruct();
    }

    public interface IServerGameCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }

    public class ServerUser
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public OperationContext operationContext { get; set; }
    }
    public static class Container
    {
        public static List<int> nameImage = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        public static List<int> nameImagePast = new List<int>();
        public static int nextPlayer { get; set; }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceGame : IServiceGame
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextID = 1;
        
        public int[] Connect(string name)
        {
            int[] mas = new int[2];


            ServerUser user = new ServerUser()
            {
                ID = nextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextID++;

            if (users.Count < 5)
            {
                SendMsg(" К игре подключился " + user.Name + ". Для начала игры не хватает " + (4 - users.Count) + " игроков.", 0);
                Console.WriteLine(" К игре подключился " + user.Name);

            }
            else if (users.Count == 5)
            {
                SendMsg(" К игре подключился " + user.Name + ". Все игроки собраны. Игра скоро начнется.", 0);
                Console.WriteLine(" К игре подключился " + user.Name + ". Все игроки собраны.");
                Console.WriteLine("Игра началась.");
                
            }
            else
            {

            }
            users.Add(user);
            mas[0] = user.ID;
            mas[1] = users.Count;
            return mas;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public int ReturnNameImage()
        {

            Random rnd = new Random();
            int index = rnd.Next(Container.nameImage.Count - 1);

            while(Container.nameImagePast.Contains(Container.nameImage[index]))
            {
                rnd = new Random();
                index = rnd.Next(Container.nameImage.Count - 1);
            }
            Container.nameImagePast.Add(Container.nameImage[index]);
            return Container.nameImage[index];
        }

        public string[] SendInstruct()
        {
            string[] str = new string[2];


            foreach (var item in users)
            {
                if (item.ID == Container.nextPlayer++)
                {
                    str[0] = "Вы ведущий. Введите сюда вашу ассоциацию, затем выберите картинку, к которой вы загадали ассоциацию, кликнув по ней.";
                    str[1] = Container.nextPlayer.ToString();
                }
                else
                {
                    str[0] = "Ведущий выбирает карту.";
                }
            }
            return str;
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString() + " ";
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " загадал ассоциацию: ";
                }

                answer += msg;

                item.operationContext.GetCallbackChannel<IServerGameCallback>().MsgCallback(answer);
            }
        }
    }

    /*
     * Класс реализации запуска WCF-сервиса. 
     * Реализован с использованием шаблона Singleton
     */

    public sealed class WcfService
    {
        private static WcfService _WcfService;
        private ServiceHost _SvcHost;

        public static WcfService Service
        {
            get
            {
                _WcfService = _WcfService ?? new WcfService();
                return _WcfService;
            }
        }

        // Конструктор по умолчанию определяется как private
        private WcfService()
        {
            // Регистрация сервиса и его метаданных
            _SvcHost = new ServiceHost(typeof(ServiceGame));
        }

        public void Start()
        {
            _SvcHost.Open();
        }

        public void Stop()
        {
            _SvcHost.Close();
        }
    }
}
