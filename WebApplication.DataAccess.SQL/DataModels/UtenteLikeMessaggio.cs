using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class UtenteLikeMessaggio
    {
        
        public string Email { get; set; }
        
        public string IDMessaggio { get; set; }
        public int SetLike { get; set; }
    }
}
