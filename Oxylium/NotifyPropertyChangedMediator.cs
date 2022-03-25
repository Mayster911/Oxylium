using System.Runtime.CompilerServices;

namespace Oxylium
{
    public class NotifyPropertyChangedMediator
    {
        private class NotificationItem
        {
            public IRaisePropertyChanged Item;
            public string PropertyName;

            public NotificationItem(IRaisePropertyChanged item, string propertyName)
            {
                Item = item;
                PropertyName = propertyName;
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) 
                    return false;

                if (obj is not NotificationItem other)
                    return false;

                return ReferenceEquals(Item, other.Item) && PropertyName == other.PropertyName;
            }

            public override int GetHashCode()
            {
                return Item.GetHashCode() ^ PropertyName.GetHashCode();
            }
        }

        private readonly Dictionary<NotificationItem, List<NotificationItem>> dictionary = new();

        public void RegisterQ(
            IRaisePropertyChanged item, 
            object propertyDepends, 
            object onProperty, 
            [CallerArgumentExpression("propertyDepends")] string propertyName = "", 
            [CallerArgumentExpression("onProperty")] string onPropertyName = "")
        {
            Register(item, propertyName, onPropertyName);
        }

        public void Register(IRaisePropertyChanged item, string propertyDepends, string onProperty)
        {
            var source = new NotificationItem(item, onProperty);
            var target = new NotificationItem(item, propertyDepends);

            if (!dictionary.ContainsKey(source))
            {
                dictionary.Add(source, new List<NotificationItem>());
            }

            dictionary[source].Add(target);
        }

        public void RegisterQ(
            IRaisePropertyChanged item, 
            object propertyDepends, 
            IRaisePropertyChanged onItem, 
            object onProperty, 
            [CallerArgumentExpression("propertyDepends")] string propertyName = "", 
            [CallerArgumentExpression("onProperty")] string onPropertyName = "")
        {
            Register(item, propertyName, onItem, onPropertyName);
        }

        public void Register(IRaisePropertyChanged item, string propertyDepends, IRaisePropertyChanged onItem, string onProperty)
        {
            var source = new NotificationItem(onItem, onProperty);
            var target = new NotificationItem(item, propertyDepends);

            if (!dictionary.ContainsKey(source))
            {
                dictionary.Add(source, new List<NotificationItem>());
            }

            dictionary[source].Add(target);
        }

        public void OnPropertyChanged(IRaisePropertyChanged item, [CallerMemberName] string propertyName = "")
        {
            var source = new NotificationItem(item, propertyName);

            var set = new HashSet<NotificationItem>
            {
                source
            };

            CollectNotifications(source, set);

            foreach (var notificationItem in set)
            {
                notificationItem.Item.RaisePropertyChanged(notificationItem.PropertyName);
            }
        }

        private void CollectNotifications(NotificationItem source, HashSet<NotificationItem> set)
        {
            if (!dictionary.ContainsKey(source))
                return;

            foreach (var notificationItem in dictionary[source])
            {
                if (set.Add(notificationItem))
                {
                    CollectNotifications(notificationItem, set);
                }
            }
        }
    }
}