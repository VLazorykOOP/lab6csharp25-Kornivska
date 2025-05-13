using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    // === Завдання 1 ===
    interface IPrintable
    {
        void Show();
    }

    interface IEntity : IPrintable, ICloneable
    {
        string Title { get; set; }
    }

    abstract class PrintedEdition : IEntity
    {
        public string Title { get; set; }
        public string Publisher { get; set; }

        public PrintedEdition(string title, string publisher)
        {
            Title = title;
            Publisher = publisher;
        }

        public abstract void Show();
        public abstract object Clone();
    }

    class Book : PrintedEdition
    {
        public string Author { get; set; }

        public Book(string title, string publisher, string author) : base(title, publisher)
        {
            Author = author;
        }

        public override void Show()
        {
            Console.WriteLine($"[Книга] Назва: {Title}, Автор: {Author}, Видавництво: {Publisher}");
        }

        public override object Clone() => new Book(Title, Publisher, Author);
    }

    class Journal : PrintedEdition
    {
        public int Issue { get; set; }

        public Journal(string title, string publisher, int issue) : base(title, publisher)
        {
            Issue = issue;
        }

        public override void Show()
        {
            Console.WriteLine($"[Журнал] Назва: {Title}, Номер: {Issue}, Видавництво: {Publisher}");
        }

        public override object Clone() => new Journal(Title, Publisher, Issue);
    }

    class Textbook : Book
    {
        public string Subject { get; set; }

        public Textbook(string title, string publisher, string author, string subject)
            : base(title, publisher, author)
        {
            Subject = subject;
        }

        public override void Show()
        {
            Console.WriteLine($"[Підручник] Назва: {Title}, Автор: {Author}, Предмет: {Subject}, Видавництво: {Publisher}");
        }

        public override object Clone() => new Textbook(Title, Publisher, Author, Subject);
    }

    // === Завдання 2 ===
    interface IProduct
    {
        string Name { get; set; }
        decimal Price { get; set; }
        void Show();
        bool IsExpired();
    }

    class Product : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int ShelfLifeDays { get; set; }

        public Product(string name, decimal price, DateTime date, int days)
        {
            Name = name;
            Price = price;
            ManufactureDate = date;
            ShelfLifeDays = days;
        }

        public virtual void Show()
        {
            Console.WriteLine($"Продукт: {Name}, Ціна: {Price}, Виготовлено: {ManufactureDate.ToShortDateString()}, Строк: {ShelfLifeDays} днів");
        }

        public virtual bool IsExpired()
        {
            return DateTime.Now > ManufactureDate.AddDays(ShelfLifeDays);
        }
    }

    class Batch : Product
    {
        public int Quantity { get; set; }

        public Batch(string name, decimal price, int quantity, DateTime date, int days)
            : base(name, price, date, days)
        {
            Quantity = quantity;
        }

        public override void Show()
        {
            base.Show();
            Console.WriteLine($"Кількість: {Quantity}");
        }
    }

    class Kit : IProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Product> Items { get; set; }

        public Kit(string name, decimal price, List<Product> items)
        {
            Name = name;
            Price = price;
            Items = items;
        }

        public void Show()
        {
            Console.WriteLine($"Комплект: {Name}, Ціна: {Price}");
            foreach (var item in Items)
                item.Show();
        }

        public bool IsExpired()
        {
            return Items.Any(p => p.IsExpired());
        }
    }

    // === Завдання 3 ===
    class MyOverflowException : Exception
    {
        public MyOverflowException(string message) : base(message) { }
    }

    static class OverflowDemo
    {
        public static void Run()
        {
            try
            {
                checked
                {
                    int a = int.MaxValue;
                    int b = a + 1;
                }
            }
            catch (OverflowException ex)
            {
                throw new MyOverflowException("Переповнення при обчисленні: " + ex.Message);
            }
        }
    }

    // === Завдання 4 ===
    class Student : IEnumerable<int>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Class { get; set; }
        public string Phone { get; set; }
        public int[] Grades { get; set; }

        public Student(string last, string first, string middle, string cls, string phone, int[] grades)
        {
            LastName = last;
            FirstName = first;
            MiddleName = middle;
            Class = cls;
            Phone = phone;
            Grades = grades;
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (int g in Grades) yield return g;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Show()
        {
            Console.WriteLine($"{LastName} {FirstName} {MiddleName}, Клас: {Class}, Тел: {Phone}, Оцінки: {string.Join(", ", Grades)}");
        }

        public bool HasTwo()
        {
            return Grades.Any(g => g == 2);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\nОберіть завдання (1-4) або 0 для виходу:");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var pub = new Journal("Наука і техніка", "Науковець", 3);
                        var book = new Book("Мартин Боруля", "Освіта", "Іван Карпенко-Карий");
                        var text = new Textbook("Алгебра", "Шкільна книга", "Сидоренко", "Математика");
                        pub.Show(); book.Show(); text.Show();
                        break;
                    case "2":
                        IProduct[] products =
                        {
                            new Product("Молоко", 35.5m, DateTime.Now.AddDays(-3), 2),
                            new Batch("Печиво", 80m, 10, DateTime.Now.AddDays(-1), 5),
                            new Kit("Набір сніданку", 120m, new List<Product>
                            {
                                new Product("Сік", 25m, DateTime.Now.AddDays(-2), 7),
                                new Product("Батончик", 20m, DateTime.Now.AddDays(-10), 5)
                            })
                        };
                        foreach (var p in products)
                        {
                            p.Show();
                            Console.WriteLine(p.IsExpired() ? "Прострочено!" : "Придатний.");
                        }
                        break;
                    case "3":
                        try
                        {
                            OverflowDemo.Run();
                        }
                        catch (MyOverflowException ex)
                        {
                            Console.WriteLine("Помилка: " + ex.Message);
                        }
                        break;
                    case "4":
                        var students = new List<Student>
                        {
                            new Student("Іванов", "Іван", "Іванович", "10-А", "0987654321", new[] { 5, 3, 2, 4 }),
                            new Student("Петров", "Петро", "Петрович", "9-Б", "0631234567", new[] { 4, 4, 5, 5 })
                        };
                        students.RemoveAll(s => s.HasTwo());
                        students.Insert(0, new Student("Новий", "Учень", "Іванович", "11-В", "0991234567", new[] { 5, 5, 5, 5 }));
                        foreach (var s in students) s.Show();
                        break;
                    case "0": return;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
        }
    }
}
