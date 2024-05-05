using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter the game directory where you want to download BepInEx:");
        string directoryPath = Console.ReadLine();

        await DownloadAndExtractZip(directoryPath);
    }

    static async Task DownloadAndExtractZip(string directoryPath)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip");

                if (response.IsSuccessStatusCode)
                {
                    string zipFilePath = Path.Combine(directoryPath, "temp.zip");

                    using (var fileStream = File.Create(zipFilePath))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }

                    ZipFile.ExtractToDirectory(zipFilePath, directoryPath);

                    if (File.Exists(zipFilePath))
                    {
                        File.Delete(zipFilePath);
                        Console.WriteLine("temp.zip file removed.");
                    }
                    else
                    {
                        Console.WriteLine("An error occured.");
                    }

                    Console.WriteLine("BepInEx downloaded successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to download BepInEx. Status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
