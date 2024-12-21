using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Album.Models
{
    [ModelMetadataType(typeof(TCategoryMetaData))]
    public partial class TCategory
    {

        public class TCategoryMetaData
        {
            [Display(Name = "類別編號")]
            public int FCid { get; set; }

            [Display(Name = "類別名稱")]
            [Required(ErrorMessage = "必填")]
            public string? FCname { get; set; }
        }
    }
}
