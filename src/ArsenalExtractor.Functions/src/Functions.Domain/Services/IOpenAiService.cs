namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface IOpenAiService
    {
        public Task<string> SendQuery(IEnumerable<Message> messages);

        public enum ChatMessageType
        {
            System,
            User,
            Assistant
        }
        public class Message
        {
            public ChatMessageType Type { get; set; }
            public string Content { get; set; } = string.Empty;
        }
    }
}