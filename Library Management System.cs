using System;
using System.Collections.Generic;


public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; }

    public Book(string title, string author, string isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
        IsAvailable = true;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Year: {PublicationYear}, Available: {IsAvailable}");
    }
}


public class Member
{
    public string Name { get; set; }
    public int MemberId { get; set; }
    public List<(Book book, DateTime date)> BorrowedBooks { get; set; }

    public Member(string name, int memberId)
    {
        Name = name;
        MemberId = memberId;
        BorrowedBooks = new List<(Book book, DateTime date)>();
    }

    public void BorrowBook(Book book, DateTime borrowDate)
    {
        if (book.IsAvailable)
        {
            BorrowedBooks.Add((book, borrowDate));
            book.IsAvailable = false;
            Console.WriteLine($"{Name} borrowed {book.Title} on {borrowDate.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine($"{book.Title} is not available.");
        }
    }

    public void ReturnBook(Book book)
    {
        var borrowedBook = BorrowedBooks.Find(bb => bb.book == book);
        if (BorrowedBooks.Remove(borrowedBook))
        {
            book.IsAvailable = true;
            Console.WriteLine($"{Name} returned {book.Title}");
        }
        else
        {
            Console.WriteLine($"{Name} doesn't have {book.Title}.");
        }
    }

    public void DisplayBorrowedBooks()
    {
        Console.WriteLine($"\n{Name}'s Borrowed Books:");
        foreach (var (book, date) in BorrowedBooks)
        {
            Console.WriteLine($"Title: {book.Title}, Borrowed on: {date.ToShortDateString()}");
        }
    }
}

public class Library
{
    public List<Book> Books { get; set; }
    public List<Member> Members { get; set; }

    public Library()
    {
        Books = new List<Book>();
        Members = new List<Member>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public void AddMember(Member member)
    {
        Members.Add(member);
    }

    public Book SearchBookByTitle(string title)
    {
        return Books.Find(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public Member SearchMemberByName(string name)
    {
        return Members.Find(member => member.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public void DisplayAllBorrowedBooks()
    {
        Console.WriteLine("\nAll Borrowed Books:");
        foreach (var member in Members)
        {
            foreach (var (book, date) in member.BorrowedBooks)
            {
                Console.WriteLine($"Title: {book.Title}, Borrowed by: {member.Name}, Borrowed on: {date.ToShortDateString()}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();


        string[] titles = { "The Great Gatsby", "1984", "To Kill a Mockingbird", "The Catcher in the Rye", "The Hobbit", 
                            "Moby Dick", "Pride and Prejudice", "War and Peace", "The Odyssey", "Crime and Punishment",
                            "Harry Potter and the Philosopher's Stone", "The Lord of the Rings", "Jane Eyre", "Brave New World", 
                            "The Adventures of Huckleberry Finn", "Animal Farm", "The Brothers Karamazov", "The Count of Monte Cristo",
                            "Les Misérables", "Great Expectations", "The Grapes of Wrath", "Wuthering Heights", "The Picture of Dorian Gray",
                            "Dracula", "A Tale of Two Cities", "Ulysses", "Madame Bovary", "Fahrenheit 451", "The Scarlet Letter", "Frankenstein"};

        string[] authors = { "F. Scott Fitzgerald", "George Orwell", "Harper Lee", "J.D. Salinger", "J.R.R. Tolkien", 
                             "Herman Melville", "Jane Austen", "Leo Tolstoy", "Homer", "Fyodor Dostoevsky",
                             "J.K. Rowling", "J.R.R. Tolkien", "Charlotte Brontë", "Aldous Huxley", 
                             "Mark Twain", "George Orwell", "Fyodor Dostoevsky", "Alexandre Dumas",
                             "Victor Hugo", "Charles Dickens", "John Steinbeck", "Emily Brontë", "Oscar Wilde",
                             "Bram Stoker", "Charles Dickens", "James Joyce", "Gustave Flaubert", "Ray Bradbury", "Nathaniel Hawthorne", "Mary Shelley"};

        int[] years = { 1925, 1949, 1960, 1951, 1937, 
                        1851, 1813, 1869, -800, 1866,
                        1997, 1954, 1847, 1932, 
                        1884, 1945, 1880, 1844,
                        1862, 1861, 1939, 1847, 1890,
                        1897, 1859, 1922, 1857, 1953, 1850, 1818};

        for (int i = 0; i < titles.Length; i++)
        {
            library.AddBook(new Book(titles[i], authors[i], (i + 1).ToString("D9"), years[i]));
        }

        
        string[] memberNames = { "John Snow", "Aegon Targaryen", "Daenerys Targaryen", "Heisenberg", "Boa Hancock",
                                 "Nico Robin", "Tamayo", "Kakashi Hatake", "Hinata Hyuga", "Light Yagami" };

        for (int i = 0; i < memberNames.Length; i++)
        {
            library.AddMember(new Member(memberNames[i], i + 1));
        }

       
        while (true)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("1. Borrow a book");
            Console.WriteLine("2. Return a book");
            Console.WriteLine("3. Display all borrowed books");
            Console.WriteLine("4. Display borrowed books by a member");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    BorrowBook(library);
                    break;
                case 2:
                    ReturnBook(library);
                    break;
                case 3:
                    library.DisplayAllBorrowedBooks();
                    break;
                case 4:
                    DisplayMemberBorrowedBooks(library);
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void BorrowBook(Library library)
    {
        Console.Write("Enter member name: ");
        string memberName = Console.ReadLine();
        Member member = library.SearchMemberByName(memberName);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            return;
        }

        Console.Write("Enter book title: ");
        string bookTitle = Console.ReadLine();
        Book book = library.SearchBookByTitle(bookTitle);
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        if (book.IsAvailable)
        {
            Console.Write("Enter borrow date (yyyy-mm-dd): ");
            DateTime borrowDate;
            if (DateTime.TryParse(Console.ReadLine(), out borrowDate))
            {
                member.BorrowBook(book, borrowDate);
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
        }
        else
        {
            Console.WriteLine("Book is not available.");
        }
    }

    static void ReturnBook(Library library)
    {
        Console.Write("Enter member name: ");
        string memberName = Console.ReadLine();
        Member member = library.SearchMemberByName(memberName);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            return;
        }

        Console.Write("Enter book title: ");
        string bookTitle = Console.ReadLine();
        Book book = library.SearchBookByTitle(bookTitle);
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        member.ReturnBook(book);
    }

    static void DisplayMemberBorrowedBooks(Library library)
    {
        Console.Write("Enter member name: ");
        string memberName = Console.ReadLine();
        Member member = library.SearchMemberByName(memberName);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            return;
        }

        member.DisplayBorrowedBooks();
    }
}
