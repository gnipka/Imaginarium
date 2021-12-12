using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Imaginarium.ServiceReference1;

namespace Imaginarium
{



    class MyServiceClient : DuplexClientBase<IServiceGame>, IServiceGame
    {
        public MyServiceClient(InstanceContext callbackCntx)
            : base(callbackCntx)
        {
        }

        public int[] Connect(string name)
        {
            return base.Channel.Connect(name);
        }

        public Task<int[]> ConnectAsync(string name)
        {
            return base.Channel.ConnectAsync(name);
        }

        public void Disconnect(int id)
        {
             base.Channel.Disconnect(id);
        }

        public Task DisconnectAsync(int id)
        {
            return base.Channel.DisconnectAsync(id);
        }

        public int ReturnNameImage()
        {
            return base.Channel.ReturnNameImage();
        }

        public Task<int> ReturnNameImageAsync()
        {
            return base.Channel.ReturnNameImageAsync();
        }

        public void SendAssoc(string assoc, int ID)
        {
            base.Channel.SendAssoc(assoc, ID);
        }

        public Task SendAssocAsync(string assoc, int ID)
        {
            return base.Channel.SendAssocAsync(assoc, ID);
        }

        public string[] SendInstruct(int ID)
        {
            return base.Channel.SendInstruct(ID);
        }

        public Task<string[]> SendInstructAsync(int ID)
        {
            return base.Channel.SendInstructAsync(ID);
        }

        public void SendMsg(string msg, int id)
        {
            base.Channel.SendMsg(msg, id);
        }

        public void SendMsgAssoc(string msg, int id)
        {
            base.Channel.SendMsgAssoc(msg, id);
        }

        public Task SendMsgAssocAsync(string msg, int id)
        {
            return base.Channel.SendMsgAssocAsync(msg, id);
        }

        public Task SendMsgAsync(string msg, int id)
        {
            return base.Channel.SendMsgAsync(msg, id);
        }
    }

    class MyCallBack : IServiceGameCallback
    {
        public void MsgCallback(string msg)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.tbInstruct.Text = msg;
        }

        public void MsgCallbackCon(string msg)
        {
            WindowConnection windowConnection = new WindowConnection();
            windowConnection.lbMsg.Items.Add(msg);
            windowConnection.pb.Value++;
        }
    }

}
