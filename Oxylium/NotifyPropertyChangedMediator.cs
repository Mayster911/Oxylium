using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "propertyDepends and onProperty are dummies")]
        public void RegisterQ(
            IRaisePropertyChanged item, 
            object propertyDepends, 
            object onProperty, 
            [CallerArgumentExpression("propertyDepends")] string propertyDependsExpression = "", 
            [CallerArgumentExpression("onProperty")] string onPropertyExpression = "")
        {
            RegisterBetween(item, GetProperty(propertyDependsExpression), item, GetProperty(onPropertyExpression));
        }

        public void Register(IRaisePropertyChanged item, string propertyDepends, string onProperty)
        {
            RegisterBetween(item, propertyDepends, item, onProperty);
        }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "propertyDepends and onProperty are dummies")]
        public void RegisterBetweenQ(
            IRaisePropertyChanged item, 
            object propertyDepends, 
            IRaisePropertyChanged onItem, 
            object onProperty, 
            [CallerArgumentExpression("propertyDepends")] string propertyDependsExpression = "", 
            [CallerArgumentExpression("onProperty")] string onPropertyExpression = "")
        {
            RegisterBetween(item, GetProperty(propertyDependsExpression), onItem, GetProperty(onPropertyExpression));
        }

        public void RegisterBetween(IRaisePropertyChanged item, string propertyDepends, IRaisePropertyChanged onItem, string onProperty)
        {
            ThrowIfPropertyDoesNotExist(item.GetType(), propertyDepends);
            ThrowIfPropertyDoesNotExist(onItem.GetType(), onProperty);

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

        private static string GetProperty(string callerArgumentExpression)
        {
            return callerArgumentExpression.Split(".", StringSplitOptions.RemoveEmptyEntries).Last();
        }

        private static void ThrowIfPropertyDoesNotExist(Type type, string propertyName)
        {
            var properties = type.GetProperties(BindingFlags.Public);
            if (!properties.Any(p => p.Name == propertyName))
                throw new ArgumentException($"A public property named \"{propertyName}\" does not exist on type \"{type.Name}\"", nameof(propertyName));
        }
    }
}