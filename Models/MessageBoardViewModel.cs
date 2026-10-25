using System.Collections.Generic;

namespace SSD_LAB1.Models
{
    public class MessageBoardViewModel
    {
        public List<Message> Messages { get; set; } = new();

        public CreateMessageViewModel NewMessage { get; set; } = new();
    }
}