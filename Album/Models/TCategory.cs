using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Album.Models
{
    public partial class TCategory
    {
       
        public int FCid { get; set; }

        
        public string? FCname { get; set; }
    }
}
