# Priority Queue

Priority queue based on a max [heap](http://en.wikipedia.org/wiki/Heap_(data_structure). A comparer can be injected to control the priority function.
```csharp
var queue = new PriorityQueue<int>();
queue.Push(2);
queue.Push(100);

queue.Top(); // returns 100
queue.Pop(); // return 100 and removes it from queue
```

The underlying heap structure and a derived heapsort algorithm are also available in the library.

# Reversible dictionary
Provides `IReversibleDictionary<TKey,TValue>` an interface for reversible dictionaries. A reversible dictionary maintains a bijective association between keys and values (aka reverse keys). Two implementations are provided in the library: 
* `SortedReversibleDictionary<K,V>` based on [<code>SortedDictionary</code>](http://msdn.microsoft.com/en-us/library/f7fta44c.aspx).
* `ReversibleDictionary<T,V>` based on [<code>Dictionary</code>](http://msdn.microsoft.com/en-us/library/xfhwa508.aspx).

# Dynamic Array
`DynamicArray<T>` is a multidimensional generic array based list. Just like [<code>List<T></code>](http://msdn.microsoft.com/en-us/library/6sh2ey19.aspx) but with configurable rank ([see array rank](http://msdn.microsoft.com/en-us/library/system.array.rank.aspx)).

```csharp
var array = new DynamicArray<int>(2);
for (int i = 0; i < 1000; i++)
    for (int j = 0; j < 1000; i++)
        array[i, j] = i + j;
```

You can set an element at any position, the array will grow automatically:
```csharp
var array = new DynamicArray<string>(3);
array[100,999,29] = "hello";
```

The dynamic array can be resized up or down, globally or per dimension using `Resize` and `ResizeDim`

A regular array can be extracted using the `ToArray` method or by type conversion.
```csharp
var darray = new DynamicArray<string>(2);
var array = (string[,])darray;
```
An array can be inserted into the dynamic array at any given position, puhsing elements along any given dimension using the `Insert` method.

# License
Copyright 2009-2013 [Sinbadsoft](http://www.sinbadsoft.com).

Licensed under the GNU Library General Public License (LGPL) [version 2.1](http://www.gnu.org/licenses/lgpl-2.1-standalone.html).
