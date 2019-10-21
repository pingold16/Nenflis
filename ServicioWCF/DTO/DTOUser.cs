using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOUser
    {
        [DataMember]
        public string user { get; set; }
        public string pass { get; set; }
        public string pass2 { get; set; }
    }
}
