using System;
using System.Collections.Generic;

namespace Album.Models
{
    public partial class TMember
    {
        public string FUid { get; set; } = null!;
        public string? FPwd { get; set; }
        public string? FName { get; set; }
        public string? FMail { get; set; }
        public string? FRole { get; set; }
    }
}
