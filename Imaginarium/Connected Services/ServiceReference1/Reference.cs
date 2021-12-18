﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Imaginarium.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IServiceGame", CallbackContract=typeof(Imaginarium.ServiceReference1.IServiceGameCallback))]
    public interface IServiceGame {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/Connect", ReplyAction="http://tempuri.org/IServiceGame/ConnectResponse")]
        int[] Connect(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/Connect", ReplyAction="http://tempuri.org/IServiceGame/ConnectResponse")]
        System.Threading.Tasks.Task<int[]> ConnectAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/Disconnect", ReplyAction="http://tempuri.org/IServiceGame/DisconnectResponse")]
        void Disconnect(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/Disconnect", ReplyAction="http://tempuri.org/IServiceGame/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/SendMsg")]
        void SendMsg(string msg, int id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/SendMsg")]
        System.Threading.Tasks.Task SendMsgAsync(string msg, int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnNameImage", ReplyAction="http://tempuri.org/IServiceGame/ReturnNameImageResponse")]
        string ReturnNameImage();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnNameImage", ReplyAction="http://tempuri.org/IServiceGame/ReturnNameImageResponse")]
        System.Threading.Tasks.Task<string> ReturnNameImageAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/SendInstruct", ReplyAction="http://tempuri.org/IServiceGame/SendInstructResponse")]
        string[] SendInstruct(int ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/SendInstruct", ReplyAction="http://tempuri.org/IServiceGame/SendInstructResponse")]
        System.Threading.Tasks.Task<string[]> SendInstructAsync(int ID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/AddImageInRound")]
        void AddImageInRound(int ID, string nameImage);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/AddImageInRound")]
        System.Threading.Tasks.Task AddImageInRoundAsync(int ID, string nameImage);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/AddAnswer")]
        void AddAnswer(string nameImage, int ID);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/AddAnswer")]
        System.Threading.Tasks.Task AddAnswerAsync(string nameImage, int ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnPoints", ReplyAction="http://tempuri.org/IServiceGame/ReturnPointsResponse")]
        System.Collections.Generic.Dictionary<string, int> ReturnPoints();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnPoints", ReplyAction="http://tempuri.org/IServiceGame/ReturnPointsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> ReturnPointsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnCardAndName", ReplyAction="http://tempuri.org/IServiceGame/ReturnCardAndNameResponse")]
        System.Collections.Generic.Dictionary<string, int> ReturnCardAndName();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGame/ReturnCardAndName", ReplyAction="http://tempuri.org/IServiceGame/ReturnCardAndNameResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> ReturnCardAndNameAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceGameCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServiceGame/MsgCallback")]
        void MsgCallback(string msg);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceGameChannel : Imaginarium.ServiceReference1.IServiceGame, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceGameClient : System.ServiceModel.DuplexClientBase<Imaginarium.ServiceReference1.IServiceGame>, Imaginarium.ServiceReference1.IServiceGame {
        
        public ServiceGameClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServiceGameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServiceGameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceGameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceGameClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public int[] Connect(string name) {
            return base.Channel.Connect(name);
        }
        
        public System.Threading.Tasks.Task<int[]> ConnectAsync(string name) {
            return base.Channel.ConnectAsync(name);
        }
        
        public void Disconnect(int id) {
            base.Channel.Disconnect(id);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(int id) {
            return base.Channel.DisconnectAsync(id);
        }
        
        public void SendMsg(string msg, int id) {
            base.Channel.SendMsg(msg, id);
        }
        
        public System.Threading.Tasks.Task SendMsgAsync(string msg, int id) {
            return base.Channel.SendMsgAsync(msg, id);
        }
        
        public string ReturnNameImage() {
            return base.Channel.ReturnNameImage();
        }
        
        public System.Threading.Tasks.Task<string> ReturnNameImageAsync() {
            return base.Channel.ReturnNameImageAsync();
        }
        
        public string[] SendInstruct(int ID) {
            return base.Channel.SendInstruct(ID);
        }
        
        public System.Threading.Tasks.Task<string[]> SendInstructAsync(int ID) {
            return base.Channel.SendInstructAsync(ID);
        }
        
        public void AddImageInRound(int ID, string nameImage) {
            base.Channel.AddImageInRound(ID, nameImage);
        }
        
        public System.Threading.Tasks.Task AddImageInRoundAsync(int ID, string nameImage) {
            return base.Channel.AddImageInRoundAsync(ID, nameImage);
        }
        
        public void AddAnswer(string nameImage, int ID) {
            base.Channel.AddAnswer(nameImage, ID);
        }
        
        public System.Threading.Tasks.Task AddAnswerAsync(string nameImage, int ID) {
            return base.Channel.AddAnswerAsync(nameImage, ID);
        }
        
        public System.Collections.Generic.Dictionary<string, int> ReturnPoints() {
            return base.Channel.ReturnPoints();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> ReturnPointsAsync() {
            return base.Channel.ReturnPointsAsync();
        }
        
        public System.Collections.Generic.Dictionary<string, int> ReturnCardAndName() {
            return base.Channel.ReturnCardAndName();
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> ReturnCardAndNameAsync() {
            return base.Channel.ReturnCardAndNameAsync();
        }
    }
}
