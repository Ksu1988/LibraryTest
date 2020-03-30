using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MyLibrary.Models
{
    public class Book
    {

        /// <summary>
        /// ID книги
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название книги
        /// </summary>
        [Display(Name = "Название книги")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(30, ErrorMessage = "Недопустимая длина строки")]
        public string Name { get; set; }
        /// <summary>
        /// Авторы книги
        /// </summary>
        [Display(Name = "Авторы")]
        public List<Author> Authors { get; set; } = new List<Author>();
        /// <summary>
        /// Количество страниц
        /// </summary>
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Количество страниц")]
        [Range(0, 10000, ErrorMessage = "Недопустимое количество страниц")]
        public int PadgesCount
        {
            get; set;
        }
        /// <summary>
        /// Год издания
        /// </summary>
        [Display(Name = "Год издания")]
        [Range(1800, 2020, ErrorMessage = "Год издания должен быть больше 1800 и меньше текущего")]
        public int PublishingYear
        {
            get; set;
        }
        /// <summary>
        /// Название издательства
        /// </summary>
        [StringLength(30, ErrorMessage = "Недопустимая длина строки")]
        [Display(Name = "Название издательства")]
        public string PublisherName
        {
            get; set;
        }
        /// <summary>
        /// Изображение
        /// </summary>
        [Display(Name = "Обложка")]
        public byte[] Image
        {
            get; set;
        }

        /*
         * заголовок (обязательный параметр, не более 30 символов)
- список авторов (книга должна содержать хотя бы одного автора)
- имя автора (обязательный параметр, не более 20 символов)
- фамилия автора (обязательный параметр, не более 20 символов)
- количество страниц (обязательный параметр, больше 0 и не более 10000)
- название издательства (опциональный параметр, не более 30 символов)
- год публикации (не раньше 1800)
- изображение (опциональный параметр)

         */
    }
}
