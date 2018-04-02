﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortableLibrary.Core.Database.Entities.Book
{
    public class LibraryBook
    {
        public LibraryBook()
        {
            Genres = new List<LibraryBookGenre>();
            Categories = new List<LibraryBookCategory>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LibraryBookId { get; set; }
        public int? BookId { get; set; }
        public int BooksLibraryId { get; set; }

        public int LanguageId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        public string Comments { get; set; }
        public string CoverImage { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsFavourite { get; set; }
        public bool IsReading { get; set; }
        public bool IsRead { get; set; }
        public bool IsReadingPlanned { get; set; }
        public bool IsWaitingToBecomeGlobal { get; set; }

        public virtual Book Book { get; set; }
        public virtual BooksLibrary BooksLibrary { get; set; }

        public virtual ICollection<LibraryBookGenre> Genres { get; set; }
        public virtual ICollection<LibraryBookCategory> Categories { get; set; }
    }
}
