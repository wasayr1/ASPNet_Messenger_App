using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace SignalRMessenger.Hubs
{
    public class ChatHub : Hub
    {
        private readonly OpenAIClient _aiClient;

        // Update constructor to get the IConfiguration service
        public ChatHub(IConfiguration configuration)
        {
            // Create the AI client using the secrets we configured
            var endpoint = new Uri(configuration["AzureAI:Endpoint"]);
            var credential = new AzureKeyCredential(configuration["AzureAI:ApiKey"]);
            _aiClient = new OpenAIClient(endpoint, credential);
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            // 1. Send the message to both users immediately
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, message);

            // 2. Asynchronously get AI suggestions and send them ONLY to the receiver
            var suggestions = await GetAiSuggestions(message);
            await Clients.User(receiverId).SendAsync("ReceiveSuggestions", suggestions);
        }

        private async Task<List<string>> GetAiSuggestions(string message)
        {
            try
            {
                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    DeploymentName = "DeepSeek-R1-0528", 
                    Messages =
                    {
                        new ChatRequestSystemMessage("You are an AI assistant that provides three short, relevant, one-to-three word reply suggestions for a chat message. Your replies should be in a JSON array format, like [\"suggestion1\", \"suggestion2\", \"suggestion3\"]."),
                        new ChatRequestUserMessage($"Generate replies for this message: \"{message}\"")
                    },
                    MaxTokens = 80
                };

                Response<ChatCompletions> response = await _aiClient.GetChatCompletionsAsync(chatCompletionsOptions);
                string rawJson = response.Value.Choices[0].Message.Content;

                // Parse the JSON array string into a List<string>
                var suggestions = JsonSerializer.Deserialize<List<string>>(rawJson);
                return suggestions ?? new List<string>();
            }
            catch (Exception ex)
            {
                // If AI fails, just return an empty list.
                Console.WriteLine($"AI Suggestion Error: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
