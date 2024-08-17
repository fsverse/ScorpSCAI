using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Security.Policy;
using Ollama;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Client.Events;

namespace ScorpSCAI
{
    public partial class Form1 : Form
    {
        private TwitchClient _client;

        public Form1()
        {
            InitializeComponent();
            InitializeTwitchClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public async Task<string> AskOllama(string prompt)
        {
            using var ollama = new OllamaApiClient();
            var chat = ollama.Chat(
                model: "llama3.1",
                systemMessage: "You are a Twitch chat bot specifically knowledgeable about all things Star Citizen.",
                autoCallTools: true);

            var service = new StarCitizenWikiService(this);
            chat.AddToolService(service.AsTools(), service.AsCalls());

            var response_text = string.Empty;
            try
            {
                _ = await chat.SendAsync(prompt);
            }
            finally
            {
                Debug.WriteLine(chat.PrintMessages());
                response_text = chat.History[4].Content.ToString(); //not sure about the '4' here.. maybe should search for Assistant but it comes up twice.
            }

            return response_text;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var ollama_response_text = await AskOllama(textBox1.Text);
            textBox2.Text = ollama_response_text;
        }

        //////////////////////////////////////////////////////////////////////////
        ///
        ///                 TWITCH STUFF
        ///
        //////////////////////////////////////////////////////////////////////////
        ///


        private void InitializeTwitchClient()
        {
            // Set your Twitch credentials here
            string twitchUsername = "langdonw_ai";
            string accessToken = "3p7rah44ajykv1v95acgbvkcvkqubw";
            //string channelName = "Sc0rp10n66";
            string channelName = "langdonw";


            ConnectionCredentials credentials = new ConnectionCredentials(twitchUsername, accessToken);
            _client = new TwitchClient();
            _client.Initialize(credentials, channelName);

            // Subscribe to the OnMessageReceived event
            _client.OnMessageReceived += Client_OnMessageReceived;

            // Connect to the Twitch chat
            _client.Connect();
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            // Extract sender's username and message content
            string username = e.ChatMessage.Username;
            string message = e.ChatMessage.Message;

            // Optionally, get other information like display name, subscriber status, etc.
            string displayName = e.ChatMessage.DisplayName;
            bool isModerator = e.ChatMessage.IsModerator;
            bool isSubscriber = e.ChatMessage.IsSubscriber;

            var prompt = message;
            var text_response = "";

            if (prompt.Length >= 3 && prompt.Substring(0, 3) == "!sc")  // if we're requesting Ollama look up the Star  Citizen Wiki
            {
                // remove first 4 chars of prompt
                prompt = prompt.Substring(4);
                text_response = await AskOllama(username + "says:" + prompt);

            }
            else         // if we're not requesting Wiki data, just send the message to Ollama
            {
                //text_response = await AskOllama(username + "says:" + prompt);  NOT SENDING TO OLLAMA FOR NOW !!!

            }



            if (!string.IsNullOrWhiteSpace(text_response) && _client.IsConnected)
            {
                string cleanedResponse = StripHtml(text_response);
                SendLongMessage(cleanedResponse);

                //textBoxToTwitch.Clear();
            }

            // Append the message to the TextBox with additional information
            this.Invoke((MethodInvoker)delegate
            {
                textBoxFromTwitch.AppendText($"{displayName} (Mod: {isModerator}, Sub: {isSubscriber}): {message}{Environment.NewLine}");
            });
        }

        private string StripHtml(string text_response)
        {
            // Pattern to match HTML tags
            string pattern = "<.*?>";

            // Replace HTML tags with an empty string
            string cleanedText = Regex.Replace(text_response, pattern, string.Empty);

            return cleanedText;
        }

        private void SendLongMessage(string text_response)
        {
            const int maxMessageLength = 500;

            // Split the text_response into chunks of maxMessageLength
            for (int i = 0; i < text_response.Length; i += maxMessageLength)
            {
                // Get the substring of length maxMessageLength or the remaining characters
                string messagePart = text_response.Substring(i, Math.Min(maxMessageLength, text_response.Length - i));

                // Send the message part
                _client.SendMessage(_client.JoinedChannels[0], messagePart);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Send the message in the textBoxMessageToSend to the Twitch chat
            if (!string.IsNullOrWhiteSpace(textBoxToTwitch.Text) && _client.IsConnected)
            {
                _client.SendMessage(_client.JoinedChannels[0], textBoxToTwitch.Text);
                textBoxToTwitch.Clear();
            }
        }

        private void textBoxToTwitch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxToTwitch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound on Enter
                e.SuppressKeyPress = true;

                // Call the button click method or directly call your desired method
                button2.PerformClick();  // Simulate a button click
            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound on Enter
                e.SuppressKeyPress = true;
                textBox1.Clear();
                // Call the button click method or directly call your desired method
                button1.PerformClick();  // Simulate a button click
            }
        }
    }









    //////////////////////////////////////////////////////////////////////////
    ///                           TOOLS
    ///
    ///    
    /// <summary>
    /// StarCitizenWiki
    /// </summary>



    public class StarCitizenWiki
    {
        public string search_term { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }


    [OllamaTools]
    public interface IStarCitizenWikiFunctions
    {
        [Description("Search the Star Citizen wiki for a page extract")]
        public Task<StarCitizenWiki> GetStarCitizenWikiPageExcerptAsync(
            [Description("One or two words only that summerize the request")] string search_term,
            CancellationToken cancellationToken = default);
    }

    public class StarCitizenWikiService : IStarCitizenWikiFunctions
    {
        private readonly Form1 _form;

        private static readonly HttpClient client = new HttpClient();

        public StarCitizenWikiService(Form1 form)
        {
            _form = form;
        }


        public async Task<StarCitizenWiki> GetStarCitizenWikiPageExcerptAsync(string search_term, CancellationToken cancellationToken = default)
        {
        
  
            var page_extract = await AskWiki(search_term);



            return new StarCitizenWiki
            {
                Description = "" + search_term + page_extract,
            };
        }

        public async Task<string> AskWiki(string prompt)
        {
                // post to Star Citizen wiki
                try
                {
                    var srsearch = prompt;
                    var (firstPageExtract, concatenatedTitles) = await SearchStarCitizenAsync(srsearch);
                    Debug.WriteLine("Request was successful");
                    Debug.WriteLine("First Page Extract: " + firstPageExtract);
                    Debug.WriteLine("Concatenated Titles: " + concatenatedTitles);

                    //textBox4.Text = firstPageExtract;

                    var prompt_temp = "CIG says: " + firstPageExtract + "\n\n Other mentions are " + concatenatedTitles;
                    prompt = prompt_temp;
                    return prompt;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Request failed");
                    Debug.WriteLine("Error: " + ex.Message);
                    var text_response = "Error: " + ex.Message;
                    return text_response;
                }
        }



        public static async Task<(string firstPageExtract, string concatenatedTitles)> SearchStarCitizenAsync(string srsearch)
        {
            var url = $"https://starcitizen.tools/api.php?action=query&format=json&list=search&continue=-%7C%7C&formatversion=1&srsearch={Uri.EscapeDataString(srsearch)}&srnamespace=0";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                var searchResults = jsonResponse["query"]["search"];
                int firstPageId = (int)searchResults[0]["pageid"];
                StringBuilder concatenatedTitles = new StringBuilder();
                string firstPageExtract = await GetStarCitizenPageAsync(firstPageId);

                foreach (var result in searchResults)
                {
                    concatenatedTitles.Append(result["title"].ToString() + ",");
                }

                return (firstPageExtract, concatenatedTitles.ToString().Trim());
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

        public static async Task<string> GetStarCitizenPageAsync(int pageId)
        {
            var url = $"https://starcitizen.tools/api.php?action=query&format=json&prop=extracts&pageids={pageId}&formatversion=1&exchars=1200";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                return jsonResponse["query"]["pages"][pageId.ToString()]["extract"].ToString();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////
    ///
}
