using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace GoodForecast
{
    class Program
    {
        /*
Можно использовать лямбда-выражения или SQL-нотацию, на ваш выбор.
        */
        class Group
        {
            public int Id;
            public string Name;
        }

        // Товар
        class Product
        {
            public int Id;
            public string Name;
            public int GroupId; // Идентификатор группы товаров (внешний ключ)
            public Group Group; // Navigation property
        }

        static void Main()
        {
            while (true)
            {
                try
                {
                    Group group1 = new Group { Id = 1, Name = "First Group" };
                    Group group2 = new Group { Id = 2, Name = "Second Group" };
                    Group group3 = new Group { Id = 3, Name = "Third Group" };

                    IEnumerable<Group> groups = new Group[] { group1, group2, group3 }.AsEnumerable();

                    Product product1 = new Product { Id = 1, Name = "First Product", GroupId = 1, Group = groups.FirstOrDefault(g => g.Id == 1) };
                    Product product2 = new Product { Id = 2, Name = "Second Product", GroupId = 2, Group = groups.FirstOrDefault(g => g.Id == 2) };
                    Product product3 = new Product { Id = 3, Name = "Third Product", GroupId = 3, Group = groups.FirstOrDefault(g => g.Id == 3) };
                    Product product4 = new Product { Id = 4, Name = "Fourth Product", GroupId = 1, Group = groups.FirstOrDefault(g => g.Id == 1) };
                    Product product5 = new Product { Id = 5, Name = "Fifth Product", GroupId = 2, Group = groups.FirstOrDefault(g => g.Id == 2) };

                    IEnumerable<Product> products = new Product[] { product1, product2, product3, product4, product5 }.AsEnumerable();

                    List<int> productIds = new List<int> { 2, 3, 5 };

                    /*
                    2.1. Пусть в переменной List<int> productIds задан список идентификаторов товаров; 
                            IEnumerable<Product> products – коллекция товаров. 
                            Из указанной коллекции выбрать список записей вида: 
                            { ProductName /* Имя товара /, GroupName /* Имя группы. /}, для всех товаров, идентификаторы которых присутствуют в списке productIds.
                     */

                    var select = from p in products
                                 where productIds.Contains(p.Id)
                                 select new
                                 {
                                     ProductName = p.Name,
                                     GroupName = p.Group.Name
                                 };

                    int i = 0;
                    foreach (var s in select)
                    {
                        i++;
                        Console.WriteLine($"{i}:\tProductName: {s.ProductName}\tGroupName: {s.GroupName}");
                    }
                    Console.WriteLine();
                    /*
                    2.2. Добавим условие, что в таблице Products нет внешнего ключа на Groups, 
                            т.е.свойства Group в классе нет совсем, а для значения GroupId может отсутствовать соответствующая запись в таблице Groups.
                            Пусть IEnumerable<Product> products, IEnumerable<Group> groups – коллекции товаров и групп, соответственно.
                            Из указанных коллекций выбрать список записей вида: 
                            { ProductName /* Имя товара /, GroupName /* Имя группы, если она существует, или “No Group”, если такой группы нет. /  }.
                     */

                    Product product6 = new Product { Id = 6, Name = "Sixth Product" };
                    productIds.Add(6);
                    IEnumerable<Product> products2 = products.Append(product6);

                    var select2 = from p in products2
                                  where productIds.Contains(p.Id)
                                  select new
                                  {
                                      ProductName = p.Name,
                                      GroupName = p.Group is null ? "No Group" : p.Group.Name
                                  };

                    i = 0;
                    foreach (var s in select2)
                    {
                        i++;
                        Console.WriteLine($"{i}:\tProductName: {s.ProductName}\tGroupName: {s.GroupName}");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"!!!EXCEPTION: {ex.Message}");
                }
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
