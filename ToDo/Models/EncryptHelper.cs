using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ToDo.Models
{
    public class EncryptHelper
    {

        public static String encryptPassword(String password)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8Encoding = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8Encoding.GetBytes(password));
                return Convert.ToBase64String(data);
                
            }
        }


    }
}