using System;

namespace WebApplication1apiLoginTracker.Models
{
    public class Login
    {
        public Guid Id { get; set; }
        public string LoginType { get; set; }
        public string Username { get; set; }
        public string Machine { get; set; }
        public DateTime Timestamp { get; set; }
        public string DomainController { get; set; }
        public string Gateway { get; set; }
    }
}
