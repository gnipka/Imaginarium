using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_game
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceGame" в коде и файле конфигурации.
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
            }
            else if (users.Count == 5)
            {
                SendMsg(" К игре подключился " + user.Name + ". Все игроки собраны. Игра скоро начнется.", 0);
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
            if(user != null)
            {
                users.Remove(user);
            }
        }
        public void SendMsg(string msg, int id)
        {
            foreach(var item in users)
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
}
