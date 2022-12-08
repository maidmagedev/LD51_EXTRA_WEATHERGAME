using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom Priority Queue based on Java PQueue implementation made for UNH - CS 515 LAB ASSIGNMENT 5
// I couldn't wrap my head around using generics, so these are all built off of AStarGridCell.cs.
public class PrioQueue
{

    int _size;
    AStarGridCell[] _array;

    public PrioQueue(int capacity)
    {
        this._array = new AStarGridCell[capacity];
        this._size = 0;
    }

    public void insert(AStarGridCell cell)
    {
        bool notInserted = true;
        int n = -1;
        while (notInserted)
        {
            n++;
            if (n < _array.Length && _array[n] == null)
            {
                _array[n] = cell;
                notInserted = false;
                _size++;
            }
        }
    }

    public AStarGridCell findMin()
    {
        AStarGridCell min;
        if (_array.Length > 0)
        {
            min = _array[0];
        } else
        {
            return null;
        }

        foreach (AStarGridCell item in _array)
        {
            if (item != null && min != null && min.compareTo(item) > 0)
            {
                // New Min Found
                min = item;
            } else if (min == null && item != null)
            {
                // there was no min (null)
                min = item;
            }
        }
        return min;
    }

    public void deleteMin()
    {
        AStarGridCell min = findMin();
        bool removed = false;
        if (!isEmpty())
        {
            int index = -1;
            foreach (AStarGridCell item in _array)
            {
                index++;
                if (!removed && item != null && min.compareTo(item) == 0)
                {
                    _array[index] = null;
                    _size--;
                    removed = true;
                }
            }
        }
    }

    public void delete(AStarGridCell desired)
    {
        bool removed = false;
        if (!isEmpty())
        {
            int index = -1;
            foreach (AStarGridCell item in _array)
            {
                index++;
                if (!removed && item != null && desired.compareTo(item) == 0)
                {
                    _array[index] = null;
                    _size--;
                    removed = true;
                }
            }
        }
    }

    public bool isEmpty()
    {
        return _size == 0;
    }

    public int size()
    {
        return _size;
    }
    

}
