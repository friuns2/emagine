﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.41108.0
// 
namespace EmagineClient.DuplexDrawService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DuplexDrawService.IDuplexDrawService", CallbackContract=typeof(EmagineClient.DuplexDrawService.IDuplexDrawServiceCallback))]
    public interface IDuplexDrawService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, AsyncPattern=true, Action="http://tempuri.org/IDuplexDrawService/Register")]
        System.IAsyncResult BeginRegister(string name, System.AsyncCallback callback, object asyncState);
        
        void EndRegister(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, AsyncPattern=true, Action="http://tempuri.org/IDuplexDrawService/Draw")]
        System.IAsyncResult BeginDraw(string data, System.AsyncCallback callback, object asyncState);
        
        void EndDraw(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDuplexDrawServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://amazedsaint.net/SilverPaint/draw")]
        void Notify(System.ServiceModel.Channels.Message request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDuplexDrawServiceChannel : EmagineClient.DuplexDrawService.IDuplexDrawService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DuplexDrawServiceClient : System.ServiceModel.DuplexClientBase<EmagineClient.DuplexDrawService.IDuplexDrawService>, EmagineClient.DuplexDrawService.IDuplexDrawService {
        
        private BeginOperationDelegate onBeginRegisterDelegate;
        
        private EndOperationDelegate onEndRegisterDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterCompletedDelegate;
        
        private BeginOperationDelegate onBeginDrawDelegate;
        
        private EndOperationDelegate onEndDrawDelegate;
        
        private System.Threading.SendOrPostCallback onDrawCompletedDelegate;
        
        private bool useGeneratedCallback;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public DuplexDrawServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public DuplexDrawServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                this(new DuplexDrawServiceClientCallback(), binding, remoteAddress) {
        }
        
        private DuplexDrawServiceClient(DuplexDrawServiceClientCallback callbackImpl, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                this(new System.ServiceModel.InstanceContext(callbackImpl), binding, remoteAddress) {
            useGeneratedCallback = true;
            callbackImpl.Initialize(this);
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> RegisterCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DrawCompleted;
        
        public event System.EventHandler<NotifyReceivedEventArgs> NotifyReceived;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult EmagineClient.DuplexDrawService.IDuplexDrawService.BeginRegister(string name, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegister(name, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void EmagineClient.DuplexDrawService.IDuplexDrawService.EndRegister(System.IAsyncResult result) {
            base.Channel.EndRegister(result);
        }
        
        private System.IAsyncResult OnBeginRegister(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string name = ((string)(inValues[0]));
            return ((EmagineClient.DuplexDrawService.IDuplexDrawService)(this)).BeginRegister(name, callback, asyncState);
        }
        
        private object[] OnEndRegister(System.IAsyncResult result) {
            ((EmagineClient.DuplexDrawService.IDuplexDrawService)(this)).EndRegister(result);
            return null;
        }
        
        private void OnRegisterCompleted(object state) {
            if ((this.RegisterCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterAsync(string name) {
            this.RegisterAsync(name, null);
        }
        
        public void RegisterAsync(string name, object userState) {
            if ((this.onBeginRegisterDelegate == null)) {
                this.onBeginRegisterDelegate = new BeginOperationDelegate(this.OnBeginRegister);
            }
            if ((this.onEndRegisterDelegate == null)) {
                this.onEndRegisterDelegate = new EndOperationDelegate(this.OnEndRegister);
            }
            if ((this.onRegisterCompletedDelegate == null)) {
                this.onRegisterCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterDelegate, new object[] {
                        name}, this.onEndRegisterDelegate, this.onRegisterCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult EmagineClient.DuplexDrawService.IDuplexDrawService.BeginDraw(string data, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDraw(data, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void EmagineClient.DuplexDrawService.IDuplexDrawService.EndDraw(System.IAsyncResult result) {
            base.Channel.EndDraw(result);
        }
        
        private System.IAsyncResult OnBeginDraw(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string data = ((string)(inValues[0]));
            return ((EmagineClient.DuplexDrawService.IDuplexDrawService)(this)).BeginDraw(data, callback, asyncState);
        }
        
        private object[] OnEndDraw(System.IAsyncResult result) {
            ((EmagineClient.DuplexDrawService.IDuplexDrawService)(this)).EndDraw(result);
            return null;
        }
        
        private void OnDrawCompleted(object state) {
            if ((this.DrawCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DrawCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DrawAsync(string data) {
            this.DrawAsync(data, null);
        }
        
        public void DrawAsync(string data, object userState) {
            if ((this.onBeginDrawDelegate == null)) {
                this.onBeginDrawDelegate = new BeginOperationDelegate(this.OnBeginDraw);
            }
            if ((this.onEndDrawDelegate == null)) {
                this.onEndDrawDelegate = new EndOperationDelegate(this.OnEndDraw);
            }
            if ((this.onDrawCompletedDelegate == null)) {
                this.onDrawCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDrawCompleted);
            }
            base.InvokeAsync(this.onBeginDrawDelegate, new object[] {
                        data}, this.onEndDrawDelegate, this.onDrawCompletedDelegate, userState);
        }
        
        private void OnNotifyReceived(object state) {
            if ((this.NotifyReceived != null)) {
                object[] results = ((object[])(state));
                this.NotifyReceived(this, new NotifyReceivedEventArgs(results, null, false, null));
            }
        }
        
        private void VerifyCallbackEvents() {
            if (((this.useGeneratedCallback != true) 
                        && (this.NotifyReceived != null))) {
                throw new System.InvalidOperationException("Callback events cannot be used when the callback InstanceContext is specified. Pl" +
                        "ease choose between specifying the callback InstanceContext or subscribing to th" +
                        "e callback events.");
            }
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            this.VerifyCallbackEvents();
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override EmagineClient.DuplexDrawService.IDuplexDrawService CreateChannel() {
            return new DuplexDrawServiceClientChannel(this);
        }
        
        private class DuplexDrawServiceClientCallback : object, IDuplexDrawServiceCallback {
            
            private DuplexDrawServiceClient proxy;
            
            public void Initialize(DuplexDrawServiceClient proxy) {
                this.proxy = proxy;
            }
            
            public void Notify(System.ServiceModel.Channels.Message request) {
                this.proxy.OnNotifyReceived(new object[] {
                            request});
            }
        }
        
        private class DuplexDrawServiceClientChannel : ChannelBase<EmagineClient.DuplexDrawService.IDuplexDrawService>, EmagineClient.DuplexDrawService.IDuplexDrawService {
            
            public DuplexDrawServiceClientChannel(System.ServiceModel.DuplexClientBase<EmagineClient.DuplexDrawService.IDuplexDrawService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginRegister(string name, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = name;
                System.IAsyncResult _result = base.BeginInvoke("Register", _args, callback, asyncState);
                return _result;
            }
            
            public void EndRegister(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("Register", _args, result);
            }
            
            public System.IAsyncResult BeginDraw(string data, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = data;
                System.IAsyncResult _result = base.BeginInvoke("Draw", _args, callback, asyncState);
                return _result;
            }
            
            public void EndDraw(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("Draw", _args, result);
            }
        }
    }
    
    public class NotifyReceivedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public NotifyReceivedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.ServiceModel.Channels.Message request {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.ServiceModel.Channels.Message)(this.results[0]));
            }
        }
    }
}