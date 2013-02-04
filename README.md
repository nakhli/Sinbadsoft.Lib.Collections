# PriorityQueue

Priority queue based on a max [heap](http://en.wikipedia.org/wiki/Heap_(data_structure)). A comparer can be injected to control the priority function.
```csharp
var queue = new PriorityQueue<int>();
queue.Push(2);
queue.Push(100);

queue.Top(); // returns 100
queue.Pop(); // return 100 and removes it from queue
```

The underlying heap structure and a derived heapsort algorithm are also available in the library.

# IReversibleDictionary<TKey,TValue>
A reversible dictionaries maintains a bijective association between keys and values. Two implementations are provided: one based on SortedDictionary<T,V> and the second on Dictionary<T,V>.

# DynamicArray
TODO

# License
Copyright 2009-2013 [Sinbadsoft](http://www.sinbadsoft.com).

Licensed under the GNU Library General Public License (LGPL) [version 2.1](http://www.gnu.org/licenses/lgpl-2.1-standalone.html).
