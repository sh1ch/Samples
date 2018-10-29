using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPVisionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = ImageAnnotatorClient.Create();
            var fileName = Console.ReadLine();

            var image = Image.FromFile(fileName);

            var response = client.DetectLabels(image);
            var info = client.DetectWebInformation(image);

            Console.WriteLine($"About file: {fileName} is ...");

            foreach (var label in response) Console.WriteLine(label.Description);

            var name = info.BestGuessLabels;

            Console.WriteLine($"Best guess is {name[0]?.Label ?? "error"}");

            Console.ReadKey();
        }
    }
}
