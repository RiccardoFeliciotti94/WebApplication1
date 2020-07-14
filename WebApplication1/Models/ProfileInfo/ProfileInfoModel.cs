using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApplication1.Models.ProfileInfo
{
    public class ProfileInfoModel
    {

        public string Name { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string NewPasswordAgain { get; set; }  
        
        public string Info { get; set; }

        public string Img { get; set; }

    }
}
