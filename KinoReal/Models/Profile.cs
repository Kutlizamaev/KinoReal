using System;

namespace KinoReal.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string City { get; set; }
        public string Birthday { get; set; }
    }
}
