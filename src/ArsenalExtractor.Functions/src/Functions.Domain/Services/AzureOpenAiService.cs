using System.ClientModel;
using ArsenalExtractor.Functions.Options;

using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using static ArsenalExtractor.Functions.Domain.Services.IOpenAiService;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class AzureOpenAiService : IOpenAiService
    {
        private readonly ChatClient _client;

        public AzureOpenAiService(IOptions<AzureOpenAI> options)
        {
            string endpoint = options.Value.Endpoint;
            string model = options.Value.Model;
            Console.WriteLine($"AzureOpenAiService: endpoint: {endpoint}, model: {options.Value.Model}");
            AzureOpenAIClient azureClient = new(
                new Uri(endpoint),
                new DefaultAzureCredential()
                );
            _client = azureClient.GetChatClient(model);
        }

        public async Task<string> SendQuery(IEnumerable<Message> messages)
        {
            List<ChatMessage> convertedMessages = ConvertToMessages(messages);
            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "date_range",
                jsonSchema: BinaryData.FromBytes("""
                    {
                        "type": "object",
                        "properties": {
                        "startDate": { "type": "string" },
                        "endDate": { "type": "string" }
                        },
                        "required": ["startDate", "endDate"],
                        "additionalProperties": false
                    }
                    """u8.ToArray()),
            jsonSchemaIsStrict: true),

            };
            try
            {

                ChatCompletion response = await _client.CompleteChatAsync(convertedMessages, options);
                Console.WriteLine(response);
                return response?.Content[0].Text ?? throw new Exception("No response from OpenAI");
            }
            catch (ClientResultException e)
            {
                throw new Exception("Error 400 or 401 while sending query to AzureOpenAI", e);
            }
            catch (Exception e)
            {

                throw new Exception("Error while sending query to AzureOpenAI", e);
            }

        }

        private static List<ChatMessage> ConvertToMessages(IEnumerable<Message> messages)
        {
            List<ChatMessage> convertedMessages = [];
            foreach (var message in messages)
            {
                switch (message.Type)
                {
                    case ChatMessageType.System:
                        convertedMessages.Add(new SystemChatMessage(message.Content));
                        break;
                    case ChatMessageType.User:
                        convertedMessages.Add(new UserChatMessage(message.Content));
                        break;
                    case ChatMessageType.Assistant:
                        convertedMessages.Add(new AssistantChatMessage(message.Content));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return convertedMessages;
        }
    }
}