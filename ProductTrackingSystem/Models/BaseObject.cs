using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductTrackingSystem.Models
{
    public class BaseObject
    {
        public static string Session { get; set; }
        public static string Token { get; set; }
    }
}