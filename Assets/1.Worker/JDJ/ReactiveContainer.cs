using System;
using System.Collections.Generic;

public enum E_ReactiveType
{
    SlidePopupPos,
}


public static class ReactiveContainer
{
    private static Dictionary<E_ReactiveType, List<IRreceiver>> _table = new();

    public static void RegistProp<T>(RProp<T> m_prop) where T : struct
    {
        if (_table.ContainsKey(m_prop.MyType) == false)
        {
            _table.Add(m_prop.MyType, new List<IRreceiver>());
            return;
        }
    }

    public static void RegistReceiver(E_ReactiveType m_type, IRreceiver m_receiver)
    {
        List<IRreceiver> list;

        if (_table.ContainsKey(m_type) == false)
        {
            list = new List<IRreceiver>();
            _table.Add(m_type, list);
        }

        list = _table[m_type];
        list.Add(m_receiver);
    }

    public static void Update<T>(RProp<T> m_prop) where T : struct
    {
        List<IRreceiver> list = _table[m_prop.MyType];

        for (int i = list.Count-1; i >= 0; i--)
        {
            if (list[i] != null)
            {
                list[i].SendValue(m_prop.Value);
            }
            else
            {
                list.RemoveAt(i);
            }
        }
    }
}

public class RProp<T> where T : struct
{
    public E_ReactiveType MyType { get; private set; }
    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            if(_value.Equals(value) == false)
            {
                this._value = value;
                ReactiveContainer.Update<T>(this);
            }
        }
    }

    public RProp(E_ReactiveType m_type)
    {
        this.MyType = m_type;
        _value = default;

        ReactiveContainer.RegistProp(this);
    }
}

public interface IRreceiver
{ 
    public void SendValue(object m_value);
}