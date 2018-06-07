using System;
using System.Collections.Generic;
using System.Linq;

namespace PortableLibrary.Core.Infrastructure.Templating.Table
{
    public class TableModel
    {
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public void Validate()
        {
            if (Headers == null)
                throw new Exception("Headers collection is empty.");

            if (Rows == null)
                throw new Exception("Rows collection is empty.");

            var areRowsMatchHeaders = Rows.All(r =>
                r.Count == Headers.Count
            );

            if (!areRowsMatchHeaders)
                throw new Exception("Row's items do not match with headers.");
        }
    }
}