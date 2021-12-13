using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
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

        //[OperationContract(IsOneWay = true)]
        //void SendMsgAssoc(string msg, int id);
        [OperationContract]
        int ReturnNameImage();

        [OperationContract]
        string[] SendInstruct(int ID);

        [OperationContract(IsOneWay = true)]
        void AddImageInRound(int ID, string nameImage);


        //[OperationContract]
        //void SendAssoc(string assoc, int ID);
    }


    public interface IServerGameCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }

    public class ServerUser
    {
        /// <summary>
        /// ID игрока
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Имя игрока
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Количество игроков
        /// </summary>
        public int Count { get; set; }
        public OperationContext operationContext { get; set; }
    }
    public static class Container
    {
        /// <summary>
        /// Список всех картинок
        /// </summary>
        public static List<int> nameImage = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41 };
        /// <summary>
        /// Список использующихся картинок
        /// </summary>
        public static List<int> nameImagePast = new List<int>();
        /// <summary>
        /// Список картинок участвующих в одном раунде, первой число - ID игрока, через пробел второе число - номер картинки
        /// </summary>
        public static Dictionary<int, int> nameImageRound = new Dictionary<int, int>();
        /// <summary>
        /// Указатель ведущего
        /// </summary>
        public static int nextPlayer { get; set; } = 1;
        public static List<int> rnd = new List<int>();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    
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
        /// <summary>
        /// Добавляет карту в список карт участвующих в данном раунде
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="nameImage"></param>
        public void AddImageInRound(int ID, string nameImage)
        {
            int number;
            int.TryParse(string.Join("", nameImage.Where(c => char.IsDigit(c))), out number);
            Container.nameImageRound.Add(ID, number);
            if (ID == Container.nextPlayer)
            {
                Random random = new Random();
                int value = random.Next(1, 6);
                Container.rnd.Add(value);
                while (Container.rnd.Count != 5)
                {
                    value = random.Next(1, 6);
                    if (!Container.rnd.Contains(value))
                    {
                        Container.rnd.Add(value);
                    }
                }
            }
            string strReturn = string.Empty;
            if (Container.nameImageRound.Count == 5)
            {
                foreach (var item in Container.rnd)
                {
                    strReturn += Container.nameImageRound.ElementAt(item - 1).Value + " ";
                }
                SendMsg(strReturn, ID);
            }
        }

        /// <summary>
        /// отсоединяет пользователя
        /// </summary>
        /// <param name="id"></param>
        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
            }
        }
        /// <summary>
        /// Отправляет карту игрокам
        /// </summary>
        /// <returns></returns>
        public int ReturnNameImage()
        {

            Random rnd = new Random();
            int index = rnd.Next(1, Container.nameImage.Count - 1);

            while (Container.nameImagePast.Contains(Container.nameImage[index]))
            {
                rnd = new Random();
                index = rnd.Next(Container.nameImage.Count - 1);
            }
            Container.nameImagePast.Add(Container.nameImage[index]);
            return Container.nameImage[index];
        }
        /// <summary>
        /// Отправляет сообщение пользователям во время ожидания выбора ведущего
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string[] SendInstruct(int ID)
        {
            string[] str = new string[2];

            if (ID == Container.nextPlayer)
            {
                str[0] = "Вы ведущий. Введите сюда вашу ассоциацию, затем выберите картинку, к которой вы загадали ассоциацию, кликнув по ней.";
                str[1] = Container.nextPlayer.ToString();
            }
            else
            {
                str[0] = "Ведущий выбирает карту.";
                str[1] = Container.nextPlayer.ToString();
            }
            return str;
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = string.Empty;
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    if (Container.nameImageRound.Count != 5)
                    {
                        answer = $"{user.Name} +  загадал ассоциацию: {msg}. Выберите подходящую картинку к этой ассоциации, нажав на нее.";
                    }
                    else
                    {
                        answer = msg;
                    }
                }
                else
                {
                    answer += DateTime.Now.ToShortTimeString() + " " + msg;
                }
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
