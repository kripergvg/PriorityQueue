using System;
using System.Collections.Generic;

namespace PriorityQueue
{
    public class PriorityQueue<TPriority, TValue>
        where TPriority : IComparable<TPriority>
    {
        private readonly List<Element> _elements;

        public PriorityQueue(int capacity = 8)
        {
            _elements = new List<Element>(capacity);
        }

        public void Enqueue(TPriority priority, TValue value)
        {
            var newElement = new Element(priority, value);
            
            var searchIndex = _elements.Count;
            bool sorted = false;
            while (!sorted)
            {
                // value lower than root
                if (searchIndex == 0)
                {
                    SetElement(searchIndex, newElement);
                    sorted = true;
                }
                else
                {
                    var parentIndex = GetParentIndex(searchIndex);
                    var parentElement = _elements[parentIndex];

                    if (parentElement.Priority.CompareTo(priority) <= 0)
                    {
                        SetElement(searchIndex, newElement);
                        // new value is bigger or equal
                        sorted = true;
                    }
                    else
                    {
                        // new value is lower or equal
                        // continue searching
                        SetElement(searchIndex, parentElement);
                        searchIndex = parentIndex;
                    }
                }
            }
        }

        public bool TryDequeue(out TValue value)
        {
            if (_elements.Count == 0)
            {
                value = default;
                return false;
            }

            value = _elements[0].Value;
            
            var lastElement = _elements[^1];
            _elements.RemoveAt(_elements.Count - 1);

            if (_elements.Count == 0)
            {
                return true;
            }
            
            bool sorted = false;
            var searchIndex = 0;
            while (!sorted)
            {
                var leftChildIndex = GetLeftChildIndex(searchIndex);
                var leftChildExists = leftChildIndex < _elements.Count;

                var rightChildIndex = GetRightChildIndex(searchIndex);
                var rightChildExists = rightChildIndex < _elements.Count;

                if (leftChildExists && rightChildExists)
                {
                    var leftChild = _elements[leftChildIndex];
                    var rightChild = _elements[rightChildIndex];

                    var leftBigger = leftChild.Priority.CompareTo(lastElement.Priority) > 0;
                    var rightBigger = rightChild.Priority.CompareTo(lastElement.Priority) > 0;
                    if (leftBigger && rightBigger)
                    {
                        // we found our place
                        _elements[searchIndex] = lastElement;
                        sorted = true;
                    }
                    else
                    {
                        // raise lowest element
                        var lowestElementIndex = leftChild.Priority.CompareTo(rightChild.Priority) > 0 ? rightChildIndex : leftChildIndex;
                        _elements[searchIndex] = _elements[lowestElementIndex];
                        searchIndex = lowestElementIndex;
                    }
                }
                else if (leftChildExists)
                {
                    var leftChild = _elements[leftChildIndex];
                    if (leftChild.Priority.CompareTo(lastElement.Priority) > 0)
                    {
                        // we found our place
                        _elements[searchIndex] = lastElement;
                    }
                    else
                    {
                        _elements[searchIndex] = _elements[leftChildIndex];
                        _elements[leftChildIndex] = lastElement;
                    }
                        
                    sorted = true;
                }
                else
                {
                    // we found our place because element doesn't have children
                    _elements[searchIndex] = lastElement;
                    sorted = true;
                }
            }

            return true;
        }

        private void SetElement(int searchIndex, Element newElement)
        {
            if (_elements.Count == searchIndex)
            {
                _elements.Add(newElement);
            }
            else
            {
                _elements[searchIndex] = newElement;
            }
        }

        private int GetParentIndex(int childIndex)
        {
            if (childIndex % 2 == 0)
            {
                return childIndex / 2 - 1;
            }
            else
            {
                return (childIndex + 1) / 2 - 1;
            }
        }

        private int GetLeftChildIndex(int parentIndex)
        {
            return (parentIndex + 1) * 2 - 1;
        }
        
        private int GetRightChildIndex(int parentIndex)
        {
            return (parentIndex + 1) * 2;
        }

        private readonly struct Element
        {
            public Element(TPriority priority, TValue value)
            {
                Priority = priority;
                Value = value;
            }

            public TPriority Priority { get; }
            public TValue Value { get; }
        }

    }
}