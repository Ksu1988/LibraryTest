using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyLibrary.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<LibraryContext>>()))
            {

                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }

                var book1 = new Book
                {
                    Name = "When Harry Met Sally",
                    PadgesCount = 400,
                    PublisherName = "Питер",
                    PublishingYear = 2015,
                };
                context.Authors.Add(new Author { Name = "Стивен", Surname = "Кинг", Book = book1 });

                var book2 = new Book
                {
                    Name = "Про с#",
                    PadgesCount = 4000,
                    PublisherName = "Orelly",
                    PublishingYear = 2017,
                };
                context.Authors.AddRange(new Author { Name = "Эндрю", Surname = "Стиллмен", Book = book2 },
                    new Author { Name = "Дж", Surname = "Грин", Book = book2 });

                var book3 =
                     new Book
                     {
                         Name = "Лабиринты Ехо",
                         PadgesCount = 500,
                         PublisherName = "Фрам",
                         PublishingYear = 2015,
                       
                     };
                context.Authors.Add(new Author { Name = "Макс", Surname = "Фрай", Book = book3 });
                var book4 = new Book
                {
                    Name = "Кухня народов мира",
                    PadgesCount = 4600,
                    PublisherName = "Эксмо",
                    PublishingYear = 2018,
                    
                };
                context.Authors.AddRange(new Author { Name = "Эндрю", Surname = "Иванов", Book = book4 },
                    new Author { Name = "Ирина", Surname = "Родионова", Book = book4 });
                context.Books.AddRange(book1, book2, book3, book4);
                context.SaveChanges();
            }
        }
    }
}

