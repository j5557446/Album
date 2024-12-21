using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Album.Models
{
	[ModelMetadataType(typeof(TAlbumMetaData))]
    public partial class TAlbum
    {

        public class TAlbumMetaData
        {
            [Display(Name = "編號")]
            public int FAlbumId { get; set; }

            [Display(Name = "分類名稱")]
            public int? FCid { get; set; }

            [Display(Name = "主題名稱")]
            [Required(ErrorMessage = "必填")]
            public string? FTitle { get; set; }

            [Display(Name = "描述說明")]
            [Required(ErrorMessage = "必填")]
            public string? FDescription { get; set; }

            [Display(Name = "圖檔")]
            public string? FAlbum { get; set; }

            [Display(Name = "發佈時間")]
            public DateTime? FReleaseTime { get; set; }
        }
    }
}
