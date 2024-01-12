using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TimeTracker.UI.Shared.Models;

public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged,
    INotifyPropertyChanged
    where TKey : notnull
{

    public ObservableDictionary(Dictionary<TKey, TValue> dict)
    {
        _dictionary = dict;
    }
    
    private readonly Dictionary<TKey, TValue> _dictionary;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
            new KeyValuePair<TKey, TValue>(key, value)));
        OnPropertyChanged(nameof(Count));
    }

    public bool Remove(TKey key)
    {
        if (!_dictionary.TryGetValue(key, out var value)) return false;

        var removedItem = new KeyValuePair<TKey, TValue>(key, value);
        if (!_dictionary.Remove(key)) return false;

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem));
        OnPropertyChanged(nameof(Count));
        return true;
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            if (_dictionary.TryGetValue(key, out var existingValue))
            {
                var changedItem = new KeyValuePair<TKey, TValue>(key, existingValue);
                _dictionary[key] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<TKey, TValue>(key, value), changedItem));
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;
    public bool IsReadOnly => ((IDictionary<TKey, TValue>)_dictionary).IsReadOnly;

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}