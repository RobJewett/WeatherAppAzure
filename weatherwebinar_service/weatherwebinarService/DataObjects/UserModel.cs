using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace weatherwebinarService.DataObjects
{
    public class User : EntityData
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string defaultLocation { get; set; }

    }
}