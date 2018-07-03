using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDo.Models
{
    [MetadataType(typeof(userlogin.userMetaData))]
    public partial class userlogin
    {
        internal sealed class userMetaData
        {
            [Key]
            public int iduser { get; set; }
            [Required(ErrorMessage = "Login is required")]
            public string login { get; set; }
            [Required(ErrorMessage = "Password is required")]
            public string password { get; set; }
        }
    }
}