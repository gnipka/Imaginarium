using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_game
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceGame" в коде и файле конфигурации.
    //Атрибут [ServiceContract] говорит о том, как пользователь будет взаимодействовать с сервисом
    [ServiceContract(CallbackContract = typeof(IServerGameCallback))]
    public interface IServiceGame
    {
        [OperationContract]
        int[] Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);

    }

    public interface IServerGameCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}
