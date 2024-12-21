using System;
using System.Collections.Generic;

namespace Album.Models
{
    public partial class TAlbum
    {
        public int FAlbumId { get; set; }
        public int? FCid { get; set; }
        public string? FTitle { get; set; }
        public string? FDescription { get; set; }
        public string? FAlbum { get; set; }
        public DateTime? FReleaseTime { get; set; }
    }
}
