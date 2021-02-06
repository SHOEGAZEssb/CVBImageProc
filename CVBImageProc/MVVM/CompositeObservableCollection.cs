using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CVBImageProc.MVVM
{
  internal sealed class CompositeObservableCollection<T> : ObservableCollection<T>
  {
    private INotifyCollectionChanged[] _collections;

    public CompositeObservableCollection(params INotifyCollectionChanged[] collections)
    {
      _collections = collections;

      foreach(var col in _collections)
      {
        AddItems((IEnumerable<T>)col);
        col.CollectionChanged += OnSubCollectionChanged;
      }
    }

    private void OnSubCollectionChanged(object source, NotifyCollectionChangedEventArgs args)
    {
      switch (args.Action)
      {
        case NotifyCollectionChangedAction.Add:
          AddItems(args.NewItems.Cast<T>());
          break;
        case NotifyCollectionChangedAction.Remove:
          RemoveItems(args.OldItems.Cast<T>());
          break;
        case NotifyCollectionChangedAction.Reset:
          Clear();
          foreach (var col in _collections.Cast<IEnumerable<T>>())
            AddItems(col);
          break;
        case NotifyCollectionChangedAction.Replace:
          RemoveItems(args.OldItems.Cast<T>());
          AddItems(args.NewItems.Cast<T>());
          break;
        case NotifyCollectionChangedAction.Move:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void AddItems(IEnumerable<T> items)
    {
      foreach (var me in items)
        Add(me);
    }

    private void RemoveItems(IEnumerable<T> items)
    {
      foreach (var me in items)
        Remove(me);
    }
  }
}
