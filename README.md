# Oxylium
Problem: Let us assume we're maintaining a WPF application which heavily relies on the property binding mechanism for our controls and the INotifyPropertyChanged interface. Some such applications have numerous transitive dependencies between various controls - and thus by extension - various properties. When adding a new property, the programmer must not only refresh this property, but also others which depend on it, and then all the properties that depend on those properties and so on. Naturally, such an application becomes slow to extend and difficult to maintain.

Solution: Oxylium provides a simple NotifyPropertyChangedMediator. In it, classes that can raise a PropertyChanged can be registered, along with their properties. A notification of a property changing is then reported to all dependent classes, along with the properties that need to be refreshed.

Currently supported .NET version: .NET 6

# Getting started
Oxylium is distributed as a NuGet package.

After installing Oxylium implement INotifyPropertyChanged and IRaisePropertyChanged in the client class - raise the PropertyChanged event in the implemented method.

Then, create a mediator instance and register the property dependencies.

Four registration methods are provided:
```
  RegisterBetween(IRaisePropertyChanged item, string propertyDepends, IRaisePropertyChanged onItem, string onProperty)
  Register(IRaisePropertyChanged item, string propertyDepends, string onProperty)
  RegisterBetweenQ(IRaisePropertyChanged item, object propertyDepends, IRaisePropertyChanged onItem, object onProperty, string propertyDependsExpression = "", string onPropertyExpression = "")
  RegisterQ(IRaisePropertyChanged item, object propertyDepends, IRaisePropertyChanged onItem, object onProperty, string propertyDependsExpression = "", string onPropertyExpression = "")
```
RegisterBetween - Use when the dependency of properties spans two objects
Register - Use when the dependency of properites is contained in the same object. Functionally identical to RegisterBetween(item, propertyDepends, item, onProperty)

The Q variants (Shorthand for Quick) leverage the CallerArgumentExpression attribute to extract the property name via the used expression and are at most capable of extracting the property name when . is used.
For example, this line will work:
```
  mediator.RegisterBetweenQ(other, other.Prop1, this, this.Prop2);
```
Which is shorter than:
```
  mediator.RegisterBetween(other, nameof(other.Prop1), this, nameof(this.Prop2);
```
  
Finally, call ```mediator.OnPropertyChanged(this);``` in your property setters. OnPropertyChanged in turn leverages the CallerMemberName attribute to extract the name of the property it was called from.
  
In the event of errors a simple property checking mechanism has been implemented. If a given type does not contain the given property, then ArgumentException is thrown.
