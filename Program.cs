using System.Reflection;
using JSONKeyYouTubeID.Models;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        // YouTube API Key which you can retrieve here: https://console.cloud.google.com/apis/api/youtube.googleapis.com
        string apiKey = "britneyjeanspears";

        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        string? executingAssemblyLocation = null;
        string dataPath = string.Empty;

        if (executingAssembly == null)
        {
            Console.WriteLine("Filed to get executing assembly");
            Environment.Exit(1);
        }

        executingAssemblyLocation = Path.GetDirectoryName(executingAssembly.Location);
        if (executingAssemblyLocation == null)
        {
            Console.WriteLine("Filed to get executing assembly location");
            Environment.Exit(1);
        }

        //Path to JSON file that contains objects of Exercise
        dataPath = Path.Combine(executingAssemblyLocation, @"Data/raw_exercises.json");
        string jsonString = await File.ReadAllTextAsync(dataPath);

        // Deserialize JSON string to list of Exercise objects 
        var exerciseData = JsonConvert.DeserializeObject<List<Exercise>>(jsonString);

        if(exerciseData == null)
        {
            Console.WriteLine("No exercise found in the JSON");
            Environment.Exit(1);
        }

        // Update YouTubeID of an Exercise object using the first result of YouTube search using Exercise name as keyword
        foreach (var exercise in exerciseData)
        {
            string exerciseName = exercise.Name;
            if (!string.IsNullOrEmpty(exerciseName))
            {
                try
                {
                    string youtubeId = await GetFirstYouTubeVideoIdByName(exerciseName);
                    if (!string.IsNullOrEmpty(youtubeId))
                    {
                        exercise.YouTubeId = youtubeId;
                        Console.WriteLine($"Exercise {exerciseData.IndexOf(exercise) + 1} of {exerciseData.Count} written");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
            }
        }

        string modifiedPath = Path.Combine(executingAssemblyLocation, @"Data/editted_exercises.json");
        await File.WriteAllTextAsync(modifiedPath, JsonConvert.SerializeObject(exerciseData, Formatting.Indented));

        Console.WriteLine("Exercises successfully updated");
        Environment.Exit(0);

        // Function to get YouTube video ID based on exercise name
        async Task<string> GetFirstYouTubeVideoIdByName(string exerciseName)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://www.googleapis.com/youtube/v3/search?q={exerciseName} exercise&part=id&type=video&maxResults=1&key={apiKey}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(json)) return null;

                    var result = JsonConvert.DeserializeObject(json);
                    if (result == null)
                    {
                        return null;
                    }

                    dynamic dynamicResult = result;
                    if (dynamicResult.items != null && dynamicResult.items.Count > 0)
                    {
                        return dynamicResult.items[0].id.videoId;
                    }
                }
                else
                {
                    throw new Exception("The YouTube API returned a non-successful response: " + response.StatusCode);
                }
                return null;
            }
        }
    }
}
