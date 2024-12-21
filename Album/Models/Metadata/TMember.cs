
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Album.Models
{
	[ModelMetadataType(typeof(TMemberMetaData))]
    public partial class TMember
    {

        public class TMemberMetaData
        {
            [Display(Name = "帳號")]
            [Required(ErrorMessage = "必填")]
            public string FUid { get; set; } = null!;

            [Display(Name = "密碼")]
            [Required(ErrorMessage = "必填")]
            public string? FPwd { get; set; }

            [Display(Name = "姓名")]
            [Required(ErrorMessage = "必填")]
            public string? FName { get; set; }

            [Display(Name = "信箱")]
            [Required(ErrorMessage = "必填")]
            [EmailAddress(ErrorMessage = "必須符合信箱格式")]
            public string? FMail { get; set; }

            [Display(Name = "角色")]
            public string? FRole { get; set; }
        }
    }
}
