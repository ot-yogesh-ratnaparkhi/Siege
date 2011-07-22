using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Siege.Eventing;
using Siege.Eventing.Messages;

namespace Web.Sample.Messages
{
    [HttpMethod("POST")]
    public class LogOnAccountMessage : IMessage
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}