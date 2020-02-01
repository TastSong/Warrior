using UnityEngine;
using System.Collections.Generic;


public class Queue<T>
{
    private uint _count = 0;
    private Node<T> _head = null;
    private Node<T> _tail = null;
    private Node<T> _temp = null;

    public bool IsEmpty { get { return (_count == 0); } }

    public uint Count { get { return _count; } }

    public T Peek()
    {
        if (IsEmpty) return default(T); //throw new Exception("The queue is empty");

        return _head.Value;
    }

    public void Enqueue(T obj)
    {
        if (_count == 0)
        {
            _head = _tail = new Node<T>(obj, _head);
        }
        else
        {
            _tail.Next = new Node<T>(obj, _tail.Next);
            _tail = _tail.Next;
        }
        _count++;
    }

    public T Dequeue()
    {
        if (IsEmpty) return default(T);// throw new Exception("The queue is empty");
        _temp = _head;
        _head = _head.Next;
        _count--;
        return _temp.Value;
    }
        
    public void Clear()
    {
        _head = _tail = _temp = null;
        _count = 0;
    }

    public override string ToString()
    {
        string result = "(";
        _temp = _head;
        while (_temp != null)
        {
            result += _temp.Value.ToString()
               + (_temp.Next != null ? ", " : "");
            _temp = _temp.Next;
        }
        result += ")";
        return result;
    }
}


class Node<T>
{
    public Node<T> Next;
    public T Value;
    public Node(T value, Node<T> next)
    {
        Next = next; Value = value;
    }
}

public class Queue
{
}