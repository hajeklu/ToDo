using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDo
{
    [MetadataType(typeof(userlogin.MetaData))]
    public partial class userlogin
    {

        sealed class MetaData
        {
            [Key]
            public int iduser { get; set; }

            
            [Required(ErrorMessage = "Login is required")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must have min 3 characters and max 30 characters")]
            public string login { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string password { get; set; }
        }
    }
}