using System;
using Utils.Reactive;

namespace GameCore.Connectors
{
    public class AlertConnector
    {
        private ReactiveEvent<AlertData> _alertPopupEvent = new();
        public IReactiveSubscription<AlertData> AlertPopupEvent => _alertPopupEvent;
        
        public void ShowAlert(string text, string label, Action callback)
        {
            _alertPopupEvent.Trigger(new AlertData(text, label, callback));
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
}