using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Author
    {
        /// <summary>
        /// ID 
        /// </summary>
        [Required,Key]
        public int AuthourId { get; set; }

        /// <summary>
        /// Имя автора
        /// </summary>
        [StringLength(20, ErrorMessage = "Недопустимая длина строки")]
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия автора
        /// </summary>
        [StringLength(20, ErrorMessage = "Недопустимая длина строки")]
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string Surname { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}
