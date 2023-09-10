using System;
using System.Collections.Generic;
using System.Windows.Input;
using Newtonsoft.Json;

namespace ForegroundAppKiller;

public class KeyboardShortcut : IEquatable<KeyboardShortcut>
{
    public Key Key { get; }
    
    public HashSet<Key> Modifiers { get; }
    
    [JsonConstructor]
    public KeyboardShortcut(Key key, IEnumerable<Key> modifiers)
    {
        Key = key;
        Modifiers = new HashSet<Key>(modifiers);
    }

    public KeyboardShortcut() : this(Key.None, Array.Empty<Key>())
    { }

    public bool Equals(KeyboardShortcut? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Key == other.Key && Modifiers.SetEquals(other.Modifiers);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((KeyboardShortcut)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Key, Modifiers);
    }
}