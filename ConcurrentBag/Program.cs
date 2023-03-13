// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Collections.Concurrent;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        /**********************************************************************************************************************************************************************************
          In C#, there are many classes that we can use to represent a group of objects that we may manipulate concurrently using multiple threads. One such class is the ConcurrentBag<T>.
          The ConcurrentBag<T> is a generic collection type in the System.Collection.Generic namespace that represents an unordered collection of objects.
          It implements the IProducerConsumerCollection<T> interface.
         **********************************************************************************************************************************************************************************
         */

        ConcurrentBag<int> myNumbers = new();// Create a new ConcurrentBag , this is an empty ConcurrentBag

        var myConcurrentBag = new ConcurrentBag<int>() { 2, 4, 6, 8, 10 };// Create and Initialze a new ConcurrentBag

        //We initialize a ConcurrentBag that doesn’t have a specified capacity.
        //Its size can increase or decrease dynamically depending on the operations that we perform on it.
        //Also, we can iterate through this collection using a foreach loop.

        //It is important to note that the data type of the collection we pass in, must be the same as the type of ConcurrentBag.

        //Working With a ConcurrentBag in a Multithreaded Environment


        //Create a method to populate a ConcurrentBag using Parallel

        var mynumBag = CreateAndAddToConcurrentBagConcurrently();
        //To get the number of elements in the returned collection, we can invoke the built-in Count property:

        Console.WriteLine($"Number of elements in mynumBag = {mynumBag.Count}"); // This prints 100
        //Remove Elements From a ConcurrentBag
        //var removedList = RemoveFromConcurrentBag(mynumBag);
        RemoveFromConcurrentBag(mynumBag);
        Console.WriteLine($"After removing , new Count = {mynumBag.Count}");//This prints 99


        //Now, let’s see what happens when we try to remove items from a ConcurrentBag concurrently:
        RemoveFromConcurrentBagConcurrently(mynumBag);
        Console.WriteLine($"After removing Concurrently 20 items, new Count = {mynumBag.Count}");//This prints 79


        //To check the bag is empty , use IsEmpty

        if (myConcurrentBag.IsEmpty == false)
            Console.WriteLine("myConcurrentBag is not Empty");

        //Access an Element in a ConcurrentBag , we use TryPeek , this will not remove the item from ConcurrentBag

        IList<int> myList = AccessItemFromAConcurrentBag(myConcurrentBag);

        if (myList.Count > 0)
        {
            foreach (int num in myList)
                Console.WriteLine($"mynum {num}");
        }


        //Now Access Elemenent cuncurrently

        IList<int>  myListconcurrent = AccessItemFromAConcurrentBagConcurrently(myConcurrentBag);

        if (myListconcurrent.Count > 0)
        {
            foreach (int num in myListconcurrent)
                Console.WriteLine($"concurrent element accessed: {num}");
        }

        Console.ReadLine();


    }

    public static ConcurrentBag<int> CreateAndAddToConcurrentBagConcurrently()
    {
        ConcurrentBag<int> numbersBag = new ConcurrentBag<int>();// or just new()
        //we invoke the Parallel.For method to add items to our bag. We can use this method to execute a loop in parallel, with each iteration running on a separate thread.
        Parallel.For(0, 100, i =>
        {
            numbersBag.Add(i);
        });
        return numbersBag;
    }

    public static void RemoveFromConcurrentBag(ConcurrentBag<int> numbersBag)
    {
        numbersBag.TryTake(out int number);
        //var result = new List<int>();

        //if(numbersBag.TryTake(out int number))
        //{
        //    result.Add(number);
        //}
        //return result;
    }

    public static void RemoveFromConcurrentBagConcurrently(ConcurrentBag<int> numbersBag)
    {
        Parallel.For(0, 20, i =>
        {
            numbersBag.TryTake(out int number);
        });
    }

    public static IList<int> AccessItemFromAConcurrentBag(ConcurrentBag<int> bag)
    {
        var result = new List<int>();
        if (bag.TryPeek(out int number))
        {
            result.Add(number);
        }
        return result;
    }

    public static IList<int> AccessItemFromAConcurrentBagConcurrently(ConcurrentBag<int> bag)
    {
        var result = new List<int>();
        Parallel.For(1, 20, i =>
        {
            if (bag.TryPeek(out int number))
            {
                result.Add(number);
            }

        });
        return result;
    }


}