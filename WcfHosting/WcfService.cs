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
        /// <summary>
        /// Подключение игрока
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        int[] Connect(string name);
        /// <summary>
        /// Отключение игрока
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void Disconnect(int id);
        /// <summary>
        /// Рассылка сообщения всем игрокам
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);
        /// <summary>
        /// Рандомный выбор карты
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string ReturnNameImage();
        /// <summary>
        /// Выбор ведущего
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [OperationContract]
        string[] SendInstruct(int ID);
        /// <summary>
        /// Добавление карты и ID игрока (карта, которая подходит под ассоциацию ведущего) в список (nameImageRound)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="nameImage"></param>
        [OperationContract(IsOneWay = true)]
        void AddImageInRound(int ID, string nameImage);
        /// <summary>
        /// Подсчет очков, добавление их в список (Top)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, int> ScoringPoints();
        /// <summary>
        /// Добавление карты, которую игрок считает картой ведущего, в список (ChoicePlayers)
        /// </summary>
        /// <param name="nameImage"></param>
        /// <param name="ID"></param>
        [OperationContract]
        void AddAnswer(string nameImage, int ID);
        /// <summary>
        /// Возвращает имя игрока и номер карты, чтобы отобразить после окончания раунда
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, int> ReturnCardAndName();
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
        public const int CountPlayers = 5;
        /// <summary>
        /// Позиции, на которых будут размещаться карты, после выбора всех участников
        /// </summary>
        public static List<int> rnd = new List<int>();
        /// <summary>
        /// Рейтинг участников, ID игрока и количество очков
        /// </summary>

        /// <summary>
        /// Карты, которые выбрали игроки, считая, что они подходят под ассоциацию ведущего
        /// </summary>
        public static Dictionary<int, int> ChoicePlayers = new Dictionary<int, int>();


    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]

    public class ServiceGame : IServiceGame
    {
        /// <summary>
        /// Список участников
        /// </summary>
        List<ServerUser> users = new List<ServerUser>();
        /// <summary>
        /// Рейтинг участников, ID игрока и количество очков
        /// </summary>
        Dictionary<int, int> Top = new Dictionary<int, int>();
        int nextID = 1;

        public int[] Connect(string name)
        {
            int[] mas = new int[2];

            //Создаем новый экземпляр класса ServerUser, добавляем нового игрока
            ServerUser user = new ServerUser()
            {
                ID = nextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            //Добавляем нового игрока в рейтинг для подсчета очков
            Top.Add(nextID, 0);
            nextID++;
            if (users.Count < 4)
            {
                SendMsg(" К игре подключился " + user.Name + ". Для начала игры не хватает " + (4 - users.Count) + " игроков.", 0);
                Console.WriteLine(" К игре подключился " + user.Name);

            }
            else if (users.Count == 4)
            {
                SendMsg(" К игре подключился " + user.Name + ". Все игроки собраны. Игра скоро начнется.", 0);
                Console.WriteLine(" К игре подключился " + user.Name + ". Все игроки собраны.");
                Console.WriteLine("Игра началась.");
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
        public string ReturnNameImage()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, Container.nameImage.Count - 1);

            while (Container.nameImagePast.Contains(Container.nameImage[index]))
            {
                rnd = new Random();
                index = rnd.Next(Container.nameImage.Count - 1);
            }
            Container.nameImagePast.Add(Container.nameImage[index]);
            return Container.nameImage[index].ToString();
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
                str[0] = $"Ведущий выбирает карту. Ведущий: {users.FirstOrDefault(x => x.ID == Container.nextPlayer).Name}";
                str[1] = Container.nextPlayer.ToString();
            }
            return str;
        }
        /// <summary>
        /// Добавляем в список карту, выбранную игроком, которую он считает, что загадал ведущий
        /// </summary>
        /// <param name="nameImage"></param>
        /// <param name="ID"></param>
        public void AddAnswer(string nameImage, int ID)
        {
            int number = 0;
            int.TryParse(string.Join("", nameImage.Where(c => char.IsDigit(c))), out number);
            Container.ChoicePlayers.Add(ID, number);
            if(Container.ChoicePlayers.Count == Container.CountPlayers - 1)
            {
                string answer = "Ready";
                foreach (var item in users)
                {
                    item.operationContext.GetCallbackChannel<IServerGameCallback>().MsgCallback(answer);
                }
            }
        }
        /// <summary>
        /// Подсчет очков
        /// </summary>
        /// <param name="nameImage"></param>
        /// <param name="ID"></param>
        public Dictionary<int, int> ScoringPoints()
        {
            int count = 0;
            foreach (var item in Container.ChoicePlayers)
            {
                if (item.Value == Container.nameImageRound.FirstOrDefault(x => x.Key == Container.nextPlayer).Value)
                {
                    Top[item.Key] += 1;
                    Top[Container.nextPlayer] += 1;
                    count++;
                }
                else if (Container.nameImageRound.Values.Contains(item.Value))
                {
                    //Добавляем очко игроку, карту которого угадали
                    Top[Container.nameImageRound.FirstOrDefault(x => x.Value == item.Value).Key] += 1;
                }
            }
            if (count == 4)
            {
                // отнимаем у ведущего все баллы которые он получил от игроков + 3 балла
                Top[Container.nextPlayer] -= 7;
            }
            else
            {
                foreach (var item in Container.ChoicePlayers)
                {
                    if (item.Value == Container.nameImageRound.FirstOrDefault(x => x.Key == Container.nextPlayer).Value)
                    {
                        Top[item.Key] += 2;
                    }
                }
                Top[Container.nextPlayer] += 3;
            }
            Top = Top.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return Top;
        }
        public Dictionary<string, int> ReturnCardAndName()
        {
            Dictionary<string, int> CardAndName = new Dictionary<string, int>();
            foreach (var item in Container.nameImageRound)
            {
                CardAndName.Add(users.FirstOrDefault(x => x.ID == item.Key).Name, item.Value);
            }
            return CardAndName;
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
