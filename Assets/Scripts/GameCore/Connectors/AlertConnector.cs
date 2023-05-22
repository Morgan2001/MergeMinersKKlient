using System;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class AlertConnector
    {
        private ReactiveEvent<AlertData> _alertPopupEvent = new();
        public IReactiveSubscription<AlertData> AlertPopupEvent => _alertPopupEvent;
        
        private ReactiveEvent<NoInternetData> _noInternetPopupEvent = new();
        public IReactiveSubscription<NoInternetData> NoInternetPopupEvent => _noInternetPopupEvent;

        public void ShowAlert(string text, string label, Action callback)
        {
            _alertPopupEvent.Trigger(new AlertData(text, label, callback));
        }

        public void ShowNoInternet(Action callback)
        {
            _noInternetPopupEvent.Trigger(new NoInternetData(callback));
        }
    }
    
    public class AlertData
    {
        public string Text { get; }
        public string ButtonLabel { get; }
        public Action ButtonAction { get; }

        public AlertData(string text, string buttonLabel, Action buttonAction)
        {
            Text = text;
            ButtonLabel = buttonLabel;
            ButtonAction = buttonAction;
        }
    }
    
    public class NoInternetData
    {
        public Action ReconnectAction { get; }

        public NoInternetData(Action reconnectAction)
        {
            ReconnectAction = reconnectAction;
        }
    }
}