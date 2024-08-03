using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for UI Button interaction
using Meta.Voice.Samples.Dictation;

public class Gemini : MonoBehaviour
{
    private MethodInfo onClickMethod = typeof(Button).GetMethod("Press", BindingFlags.NonPublic | BindingFlags.Instance);

    public UnityEngine.UI.InputField textToSpeechInputTextField;
    public Button textToSpeechStartButton; // Reference to the UI Button
    public Button textToSpeechStopButton; // Reference to the UI Button

    public DictationActivation controller; // Assign this in the Inspector
    public UnityEngine.UI.Button clearButton;
    public TextMeshProUGUI transcriptionText; // Reference to the TextMeshPro UI component on the button

    public TextMeshProUGUI statusText; // Reference to the TextMeshPro UI component on the button


    private List<Dictionary<string, object>> conversation = new List<Dictionary<string, object>>();
    private string geminiApiKey = "AIzaSyCLGjQmQawSeNZnm0JwAz1H860WKHR6Clg";
    private string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";
    
    // TODO: Add audio indicator for listening

    private bool isListening = false;

    private void Start()
    {
        InitializeGemini("You are a helpful assistant.");
    }

    public void InitializeGemini(string initialPrompt, string name="Gemini")
    {
        Speak("Hello, I am " + name + "!");
        conversation = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "role", "user" },
                { "parts", new List<object>
                    {
                        new { text = "Answer in first person without any prefix. You are: " + name + " . More information: " + initialPrompt },
                    }
                }
            },
            new Dictionary<string, object>
            {
                { "role", "model" },
                { "parts", new List<object>
                    {
                        new { text = "Ok, I am: " + name },
                    }
                }
            }
        };
    }

    public void Speak(string text)
    {
        Debug.Log("Speak: " + text);
        statusText.text = text;
        onClickMethod?.Invoke(clearButton, null);
        onClickMethod?.Invoke(textToSpeechStopButton, null);
        textToSpeechInputTextField.text = text;
        onClickMethod?.Invoke(textToSpeechStartButton, null);
    }

    public void StartListening()
    {
        if (isListening) return;
        isListening = true;
        // Speak("Listening.");
        Debug.Log("Listen start");
        onClickMethod?.Invoke(clearButton, null);
        onClickMethod?.Invoke(textToSpeechStopButton, null);
        controller.ToggleActivation();
        statusText.text = "Listening...";
    }

    public void StopListening()
    {
        if (!isListening) return;
        isListening = false;
        // Speak("Finished listening.");
        Debug.Log("Listen end");
        string user_input = transcriptionText.text;
        AskGemini(user_input);
        statusText.text = "Asking ... " + user_input;
        controller.ToggleActivation();
    }



    public async void AskGemini(string userQuery, bool resetConversation = false, bool announceQuestion = true)
    {
        if (string.IsNullOrWhiteSpace(userQuery))
        {
            return;
        }
        if (announceQuestion)
        {
            Speak("I heard: " + userQuery);
        }

        if (resetConversation)
        {
            conversation.Clear();
        }

        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(url);
            var requestUri = $"?key={geminiApiKey}";

            conversation.Add(new Dictionary<string, object>
            {
                { "role", "user" },
                { "parts", new List<object>
                    {
                        new { text = userQuery },
                    }
                }
            });

            Debug.Log(JsonConvert.SerializeObject(conversation, Newtonsoft.Json.Formatting.Indented));
            var safetySettings = new List<object>
            {
                new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" }
            };

            var requestBody = new
            {
                contents = conversation,
                safetySettings = safetySettings,
            };
            Debug.Log(JsonConvert.SerializeObject(requestBody, Newtonsoft.Json.Formatting.Indented));

            var conversationJson = JsonConvert.SerializeObject(requestBody, Newtonsoft.Json.Formatting.None);
            HttpContent content = new StringContent(conversationJson, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage httpresponse = await client.PostAsync(requestUri, content);
                httpresponse.EnsureSuccessStatusCode();
                string responseBody = await httpresponse.Content.ReadAsStringAsync();
                Debug.Log(responseBody);

                JObject jsonResponse = JObject.Parse(responseBody);
                Debug.Log(jsonResponse);

                JToken jtokenResponse = jsonResponse["candidates"][0]["content"]["parts"][0]["text"];

                if (jtokenResponse != null)
                {
                    string extractedText = jtokenResponse.ToString();
                    Debug.Log("Extracted Text: " + extractedText);

                    conversation.Add(new Dictionary<string, object>
                    {
                        { "role", "model" },
                        { "parts", new List<object>
                            {
                                new { text = extractedText }
                            }
                        }
                    });

                    Debug.Log(JsonConvert.SerializeObject(conversation, Newtonsoft.Json.Formatting.Indented));

                    if (extractedText != null)
                    {
                        Speak(extractedText);
                    }
                }
                else
                {
                    JToken functionCall = jsonResponse["candidates"][0]["content"]["parts"][0]["functionCall"];
                    if (functionCall != null)
                    {
                        string functionName = (string)functionCall["name"];
                        JObject args = (JObject)functionCall["args"];

                        Debug.Log($"Function Call Detected: {functionName}");
                    }
                    else
                    {
                        Debug.Log("No valid text or function call found in the response.");
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Debug.Log("\nException Caught!");
                Debug.Log("Message :{0} " + e.Message);
            }
        }
    }

}
