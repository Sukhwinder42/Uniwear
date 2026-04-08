using System.ComponentModel.DataAnnotations;

namespace Uniwear.ViewModels
{
    public class ChatViewModel
    {
        [Required]
        public string UserMessage { get; set; }

        public string AiResponse { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
