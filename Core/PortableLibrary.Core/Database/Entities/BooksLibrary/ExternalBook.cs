using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PortableLibrary.Core.Database.Entities.Base;

namespace PortableLibrary.Core.Database.Entities.BooksLibrary
{
    public class ExternalBook : BaseEntity
    {
        public ExternalBook()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExternalBookId { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}