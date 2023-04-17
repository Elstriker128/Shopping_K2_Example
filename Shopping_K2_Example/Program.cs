using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace K2Goods
{
    /// <summary>
    /// Provides properties and implements interfaces for the storing and manipulating of the product data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
    public class Product : IComparable<Product>, IEquatable<string>
    {
        public string StoreName { get; private set; }
        public string ProductName { get; private set; }
        public DateTime ExpirationBegin { get; private set; }
        public int SoldAmount { get; private set; }
        public int RemainingAmount { get; private set; }
        public DateTime ExpirationEnd { get; private set; }
        public decimal SingularPrice { get; private set; }

        public Product(string storeName, string productName, DateTime expirationBegin, int soldAmount, int remainingAmount, DateTime expirationEnd, decimal singularPrice)
        {
            StoreName = storeName;
            ProductName = productName;
            ExpirationBegin = expirationBegin;
            SoldAmount = soldAmount;
            RemainingAmount = remainingAmount;
            ExpirationEnd = expirationEnd;
            SingularPrice = singularPrice;
        }

        public int CompareTo(Product other)
        {
            int currentPeriod = (this.ExpirationEnd - this.ExpirationBegin).Days;
            int otherPeriod = (other.ExpirationEnd - other.ExpirationBegin).Days;

            if ((object)other == null)
                return -1;
            if (currentPeriod.CompareTo(otherPeriod) < 0 && this.RemainingAmount.CompareTo(other.RemainingAmount) > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        public bool Equals(string other)
        {
            if ((object)other == null)
                return false;
            if (this.StoreName == other)
                return true;
            else
                return false;
        }
    }
    /// <summary>
    /// Provides generic container where the data are stored in the linked list.
    /// THE STUDENT SHOULD APPEND CONSTRAINTS ON TYPE PARAMETER <typeparamref name="T"/>
    /// IF THE IMPLEMENTATION OF ANY METHOD REQUIRES IT.
    /// </summary>
    /// <typeparam name="T">The type of the data to be stored in the list. Data 
    /// class should implement some interfaces.</typeparam>
    public class LinkList<T> where T : IComparable<T>
    {
        class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node(T data, Node next)
            {
                Data = data;
                Next = next;
            }
        }

        /// <summary>
        /// All the time should point to the first element of the list.
        /// </summary>
        private Node begin;
        /// <summary>
        /// All the time should point to the last element of the list.
        /// </summary>
        private Node end;
        /// <summary>
        /// Shouldn't be used in any other methods except Begin(), Next(), Exist() and Get().
        /// </summary>
        private Node current;

        /// <summary>
        /// Initializes a new instance of the LinkList class with empty list.
        /// </summary>
        public LinkList()
        {
            begin = current = end = null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the first element of the list.
        /// </summary>
        public void Begin()
        {
            current = begin;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the next element of the list.
        /// </summary>
        public void Next()
        {
            current = current.Next;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to check whether the internal pointer points to the element of the list.
        /// </summary>
        /// <returns>true, if the internal pointer points to some element of the list; otherwise,
        /// false.</returns>
        public bool Exist()
        {
            return current != null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to get the value stored in the node pointed by the internal pointer.
        /// </summary>
        /// <returns>the value of the element that is pointed by the internal pointer.</returns>
        public T Get()
        {
            return current.Data;
        }

        /// <summary>
        /// Method appends new node to the end of the list and saves in it <paramref name="data"/>
        /// passed by the parameter.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="data">The data to be stored in the list.</param>
        public void Add(T data)
        {
            var gotten = new Node(data, null);
            if (begin != null)
            {
                end.Next = gotten;
                end = gotten;
            }
            else
            {
                begin = gotten;
                end = gotten;
            }
        }

        /// <summary>
        /// Removes all the elements that are less than <paramref name="criteria"/> according to
        /// IComparable&lt;T&gt; implementation in <typeparamref name="T"/>.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="criteria">Criteria to test each element for condition.</param>
        /// <returns>The number of elements removed from the list.</returns>
        public int RemoveAll(T criteria)
        {
            int count = 0;
            if (begin == null)
            {
                return count;
            }
            while (begin != null && begin.Data.CompareTo(criteria) > 0)
            {
                begin = begin.Next;
                count++;
            }
            for (Node found = begin; found != null && found.Next != null; found = found.Next)
            {
                if (found.Next.Data.CompareTo(criteria) > 0)
                {
                    found.Next = found.Next.Next;
                    count++;
                }
            }
            return count;
        }
    }

    public static class InOut
    {
        /// <summary>
        /// Creates the list containing data read from the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <returns>List with data from file</returns>
        public static LinkList<Product> ReadFromFile(string fileName)
        {
            LinkList<Product> list = new LinkList<Product>();
            string line;
            using (var file = new StreamReader(fileName, Encoding.UTF8))
            {
                while ((line = file.ReadLine()) != null)
                {
                    var values = Regex.Split(line, "; ");
                    string storeName = values[0];
                    string productName = values[1];
                    DateTime expirationBegin = DateTime.Parse(values[2]);
                    int soldAmount = int.Parse(values[3]);
                    int remainingAmount = int.Parse(values[4]);
                    DateTime expirationEnd = DateTime.Parse(values[5]);
                    decimal singularPrice = decimal.Parse(values[6]);

                    Product current = new Product(storeName, productName, expirationBegin, soldAmount, remainingAmount, expirationEnd, singularPrice);
                    list.Add(current);
                }
            }
            return list;
        }

        /// <summary>
        /// Appends the table, built from data contained in the list and preceded by the header,
        /// to the end of the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <param name="header">The header of the table</param>
        /// <param name="list">The list from which the table to be formed</param>
        public static void PrintToFile(string fileName, string header, LinkList<Product> list)
        {
            using (var file = new StreamWriter(fileName, true))
            {
                file.WriteLine(new string('-', 130));
                file.WriteLine(header);
                file.WriteLine(new string('-', 130));
                file.WriteLine($"| {"Store name",-15} | {"Product name",-15} | {"Day of delivery",-15} | {"Sold amount",-15} | {"Remaining amount",-16} | {"Expiration date",-15} | {"Price for one",-15} |");
                file.WriteLine(new string('-', 130));
                for (list.Begin(); list.Exist(); list.Next())
                {
                    Product current = list.Get();
                    file.WriteLine($"| {current.StoreName,-15} | {current.ProductName,-15} | {current.ExpirationBegin.ToString("MM/dd/yyyy"),15} | {current.SoldAmount,15} | {current.RemainingAmount,16} | {current.ExpirationEnd.Date.ToString("MM/dd/yyyy"),15} | {current.SingularPrice,15} |");
                }
                file.WriteLine(new string('-', 130));
                file.WriteLine();
            }
        }
    }

    public static class Task
    {
        /// <summary>
        /// Calculates the total value of the goods in the list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="list">The list of the goods.</param>
        /// <returns>The total value ot the goods.</returns>
        public static decimal Sum(LinkList<Product> list)
        {
            decimal sum = 0;
            for (list.Begin(); list.Exist(); list.Next())
            {
                Product current = list.Get();
                sum += current.RemainingAmount * current.SingularPrice;
            }
            return sum;
        }

        /// <summary>
        /// Filters a sequence of objects based on <paramref name="criterion"/>.
        /// Method uses the implementation of IEquatable&lt;TCriteria&gt; in <typeparamref name="TData"/>.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// THE STUDENT SHOULDN'T CHANGE THE SIGNATURE OF THE METHOD!
        /// </summary>
        /// <typeparam name="TData">The type of the data objects stored in the list</typeparam>
        /// <typeparam name="TCriteria">The type of a criterion</typeparam>
        /// <param name="source">The source list to filter</param>
        /// <param name="criterion">A criterion to test each element for condition.</param>
        /// <returns>The list that contains elements from the source list that satisfy the condition.</returns>
        public static LinkList<TData> Filter<TData, TCriteria>(LinkList<TData> source, TCriteria criterion) where TData : IComparable<TData>, IEquatable<TCriteria>
        {
            LinkList<TData> list = new LinkList<TData>();
            for (source.Begin(); source.Exist(); source.Next())
            {
                TData current = source.Get();
                if (current.Equals(criterion))
                {
                    list.Add(current);
                }
            }
            return list;
        }

    }

    class Program
    {
        /// <summary>
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        static void Main()
        {
            LinkList<Product> FirstList = InOut.ReadFromFile("Duomenys.txt");
            LinkList<Product> SecondList = new LinkList<Product>();
            LinkList<Product> ThirdList = new LinkList<Product>();

            if (File.Exists("Rezultatai.txt"))
            {
                File.Delete("Rezultatai.txt");
            }
            FirstList.Begin();
            if (FirstList.Exist())
            {
                InOut.PrintToFile("Rezultatai.txt", "First list", FirstList);

                Console.Write("Input first store: ");
                string FirstStore = Console.ReadLine();

                Console.Write("Input second store: ");
                string SecondStore = Console.ReadLine();

                SecondList = Task.Filter<Product, string>(FirstList, FirstStore);
                SecondList.Begin();
                if (SecondList.Exist())
                {
                    decimal secondListSumBR = Task.Sum(SecondList);

                    InOut.PrintToFile("Rezultatai.txt", "Second list before removal", SecondList);

                    Product UsedForSecondRemoval = new Product(null, null, new DateTime(2020, 3, 1), 0, 1000, new DateTime(2020, 3, 31), 0);

                    SecondList.RemoveAll(UsedForSecondRemoval);
                    decimal secondListSumAR = Task.Sum(SecondList);

                    InOut.PrintToFile("Rezultatai.txt", "Second list after removal", SecondList);
                    File.AppendAllText("Rezultatai.txt", $"The sum of all the left products in {FirstStore} before removal: {secondListSumBR}\n\r");
                    File.AppendAllText("Rezultatai.txt", $"The sum of all the left products in {FirstStore} after removal: {secondListSumAR}\n\r");

                }
                else
                {
                    File.AppendAllText("Rezultatai.txt", $"The second list lacks data\n\r");
                }

                ThirdList = Task.Filter<Product, string>(FirstList, SecondStore);
                ThirdList.Begin();
                if (ThirdList.Exist())
                {
                    decimal thirdListSumBR = Task.Sum(ThirdList);

                    InOut.PrintToFile("Rezultatai.txt", "Third list before removal", ThirdList);

                    Product UsedForThirdRemoval = new Product(null, null, new DateTime(2020, 3, 1), 0, 100, new DateTime(2020, 03, 11), 0);

                    ThirdList.RemoveAll(UsedForThirdRemoval);
                    decimal thirdListSumAR = Task.Sum(ThirdList);

                    InOut.PrintToFile("Rezultatai.txt", "Third list after removal", ThirdList);
                    File.AppendAllText("Rezultatai.txt", $"The sum of all the left products in {SecondStore} before removal: {thirdListSumBR}\n\r");
                    File.AppendAllText("Rezultatai.txt", $"The sum of all the left products in {SecondStore} after removal: {thirdListSumAR}\n\r");
                }
                else
                {
                    File.AppendAllText("Rezultatai.txt", $"The third list lacks data\n\r");
                }
            }
            else
            {
                File.AppendAllText("Rezultatai.txt", $"The first list lacks data\n\r");
            }
        }
    }
}
