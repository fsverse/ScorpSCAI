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
        private int[] savedContext = new int[] { };

        public Form1()
        {
            InitializeComponent();
            InitializeTwitchClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //////////////////////////////////////////////////////////////////////////
        ///
        ///                 LLAMA STUFF
        ///
        //////////////////////////////////////////////////////////////////////////
        ///

        public class ApiClient
        {
            private static readonly HttpClient client = new HttpClient();

            public static async Task<string> PostToApiAsync(string prompt, int[] context, string model = "llama3.1", int numCtx = 22000, bool stream = false)
            {
                var url = "http://127.0.0.1:11434/api/generate";  // Replace with the actual URL of your API endpoint

                var data = new
                {
                    model = model,
                    options = new
                    {
                        num_ctx = numCtx
                    },
                    prompt = prompt,
                    stream = stream,
                    context = context  // Ensure this is an array of integers
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Request failed with status code {response.StatusCode}, content: {responseContent}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine("Request failed");
                    Debug.WriteLine("Error: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                    throw;  // Rethrow the exception after logging
                }
            }



        }

        // Define a class to deserialize the response
        public class ResponseData
        {
            public string model { get; set; }
            public string created_at { get; set; }
            public string response { get; set; }
            public bool done { get; set; }
            public string done_reason { get; set; }
            public int[] context { get; set; }
            public long total_duration { get; set; }
            public long load_duration { get; set; }
            public int prompt_eval_count { get; set; }
            public long prompt_eval_duration { get; set; }
            public int eval_count { get; set; }
            public long eval_duration { get; set; }
        }

        public async Task<string> AskOllama(string prompt)
        {
            try
            {
                var response = await ApiClient.PostToApiAsync(prompt, savedContext);

                // Parse the response JSON
                var jsonResponse = JsonConvert.DeserializeObject<ResponseData>(response);

                // Append the new response to the top of textBox2
                var response_text = jsonResponse.response;
                // Save the new context
                savedContext = jsonResponse.context;
                return response_text;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Request failed\nError: " + ex.Message);
                var response_text = "Error: " + ex.Message;
                return response_text;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var prompt = textBox3.Text;
            var text_response = "";
            text_response = await AskOllama(prompt);
            textBox4.Text = text_response;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound on Enter
                e.SuppressKeyPress = true;
                // Call the button click method or directly call your desired method
                button3.PerformClick();  // Simulate a button click
            }
        }
        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            textBox3.Clear();
        }

        //////////////////////////////////////////////////////////////////////////
        ///
        ///                 OLLAMA and TOOLS
        ///
        //////////////////////////////////////////////////////////////////////////
        ///
        public async Task<string> AskOllamaAboutStarCitizen(string prompt)
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
            var ollama_response_text = await AskOllamaAboutStarCitizen(textBox1.Text);
            textBox2.Text = ollama_response_text;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound on Enter
                e.SuppressKeyPress = true;

                // Call the button click method or directly call your desired method
                button1.PerformClick();  // Simulate a button click
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Clear();
        }

        //////////////////////////////////////////////////////////////////////////
        ///
        ///                 TWITCH STUFF
        ///
        //////////////////////////////////////////////////////////////////////////
        ///


        private void InitializeTwitchClient()
        {
            /*
            // Set your Twitch credentials here
            string twitchUsername = "langdonw_ai";  //the bots twitch username
            string accessToken = "redacted";
            string channelName = "langdonw";    // the channel you're hijacking
            */


            // Set your Twitch credentials here
            string twitchUsername = "langdonw_ai";  //this can be whatever you set up.  It's the bot's twitch username
            string accessToken = "";  // see https://twitchtokengenerator.com/
            string channelName = "Sc0rp10n66";




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
                text_response = await AskOllamaAboutStarCitizen(username + "says:" + prompt);

            }
            else         // if we're not requesting Wiki data, just send the message to Ollama
            {
                //text_response = await AskOllama(username + "says: " + prompt);  NOT SENDING TO OLLAMA FOR NOW !!!

            }



            if (!string.IsNullOrWhiteSpace(text_response) && _client.IsConnected)
            {
                string cleanedResponse = StripHtml(text_response);
                SendLongMessage(cleanedResponse);


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
                textBoxToTwitch.Clear();
                // Call the button click method or directly call your desired method
                button2.PerformClick();  // Simulate a button click
            }

        }
        private void textBoxToTwitch_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxToTwitch.Clear();
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
                search_term = search_term,
                Description = "" + page_extract,
            };
        }

        public async Task<string> AskWiki(string prompt)
        {
                // post to Star Citizen wiki
                try
                {
                    var srsearch = prompt;
                    var (firstPageExtract, concatenatedTitles) = await SearchStarCitizenWikiAsync(srsearch);
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



        public static async Task<(string firstPageExtract, string concatenatedTitles)> SearchStarCitizenWikiAsync(string srsearch)
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
                string firstPageExtract = await GetStarCitizenWikiPageAsync(firstPageId);

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

        public static async Task<string> GetStarCitizenWikiPageAsync(int pageId)
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
