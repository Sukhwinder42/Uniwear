using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
using Uniwear.Data;
using Uniwear.Models;
using static System.Net.WebRequestMethods;

namespace Uniwear.Services
{
    public class OutfitService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OutfitService(ApplicationDbContext context, HttpClient httpClient, IConfiguration config)
        {
            _context = context;
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"];
        }

        public async Task<(List<Product>, bool)> GenerateOutfit(Product selectedProduct)
        {
            var allProducts = await _context.Products.ToListAsync();
            var outfit = new List<Product> { selectedProduct };

            bool usedAI = false;

            List<Product> aiProducts = new List<Product>();

            try
            {
                //  AI CALL
                var prompt = BuildPrompt(selectedProduct, allProducts);
                var aiResponse = await CallGemini(prompt);
                //test
                Console.WriteLine("AI TEXT RESPONSE:");
                Console.WriteLine(aiResponse);


                var ids = ExtractIds(aiResponse);
                //test
                Console.WriteLine("Extracted IDs:");
                foreach (var id in ids)
                {
                    Console.WriteLine(id);
                }



                //  RELAXED FILTER
                aiProducts = allProducts
                    .Where(p => ids.Contains(p.Id))
                    .Where(p => p.GenderGroup == selectedProduct.GenderGroup)
                    .ToList();

                if (aiProducts.Any())
                {
                    usedAI = true;
                }

                // FULL BODY FILTER
                if (selectedProduct.Type == "Full Body")
                {
                    aiProducts = aiProducts
                        .Where(p => p.Type == "Shoes" || p.Type == "Accessory")
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI ERROR: " + ex.Message);
            }

            outfit = EnforceOutfitStructure(aiProducts, selectedProduct, allProducts);

            // if still only 1 item
            if (outfit.Count <= 1)
            {
                var fallback = allProducts
                    .Where(p => p.Id != selectedProduct.Id)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToList();

                outfit.AddRange(fallback);
            }

            return (outfit
            .Where(p => p != null)
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .ToList(), usedAI);
        }

        //  PROMPT
        private string BuildPrompt(Product selected, List<Product> products)
        {
            var productList = string.Join("\n",
                products.Select(p =>
                    $"{p.Id} | {p.Name} | {p.Type} | {p.Color} | {p.GenderGroup} | {p.Occasion}"));

            return $@"
            Return ONLY product IDs (numbers).
            No text. No explanation.

            Example:
            2,5,8

            Selected:
            Type: {selected.Type}
            Gender: {selected.GenderGroup}

            Products:
            {productList}

            Rules:
            Top → Bottom + Shoes
            Bottom → Top + Shoes
            Shoes → Top + Bottom
            Full Body → Shoes + Accessory
            Accessory -> Top + Bottom + Shoes

            Return only numbers.
            ";
        }

        //  GEMINI API
        private async Task<string> CallGemini(string prompt)
        {

            var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=" + _apiKey;

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            HttpResponseMessage response = null;

            int retries = 3;

            for (int i = 0; i < retries; i++)
            {
                response = await _httpClient.PostAsJsonAsync(url, body);

                if (response.IsSuccessStatusCode)
                    break;

                Console.WriteLine($"Retry {i + 1}: {response.StatusCode}");

                await Task.Delay(2000 * (i + 1));
            }
            //test
            //Console.WriteLine("STATUS: " + response.StatusCode);
            Console.WriteLine("API Key Loaded: " + !string.IsNullOrEmpty(_apiKey));

            //if (!response.IsSuccessStatusCode)
            //    return "";
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("FULL ERROR RESPONSE:");
                Console.WriteLine(error);
                return "";
            }

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("candidates", out var candidates))
                return "";

            var first = candidates.EnumerateArray().FirstOrDefault();

            if (first.ValueKind == JsonValueKind.Undefined)
                return "";

            Console.WriteLine("RAW AI JSON:");
            Console.WriteLine(json);

            return first
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
        }

        //  EXTRACT IDS
        private List<int> ExtractIds(string response)
        {
            var ids = new List<int>();

            if (string.IsNullOrEmpty(response))
                return ids;

            var matches = Regex.Matches(response, @"\d+");

            foreach (Match m in matches)
            {
                ids.Add(int.Parse(m.Value));
            }

            return ids;
        }


        // GET MATCH SCORE
        private int GetMatchScore(Product p, Product selected)
        {
            int score = 0;

            // Strong match
            if (p.GenderGroup == selected.GenderGroup)
                score += 5;

            if (p.Occasion == selected.Occasion)
                score += 3;

            // Color matching
            if (!string.IsNullOrEmpty(p.Color) && !string.IsNullOrEmpty(selected.Color))
            {
                var pColor = p.Color.ToLower();
                var sColor = selected.Color.ToLower();

                if (pColor == sColor)
                    score += 3;

                // neutral colors match everything
                if (pColor.Contains("black") || pColor.Contains("white") || pColor.Contains("grey"))
                    score += 2;
            }

            return score;
        }

        private Product GetBestMatch(List<Product> allProducts, string type, Product selected)
        {
            return allProducts
                .Where(p => p.Type == type && p.Id != selected.Id)
                .OrderByDescending(p => GetMatchScore(p, selected))
                .FirstOrDefault();
        }

        // STRUCTURE LOGIC
        private List<Product> EnforceOutfitStructure(List<Product> aiProducts, Product selected, List<Product> allProducts)
        {
            var result = new List<Product> { selected };

            var top = aiProducts.FirstOrDefault(p => p.Type == "Top");
            var bottom = aiProducts.FirstOrDefault(p => p.Type == "Bottom");
            var shoes = aiProducts.FirstOrDefault(p => p.Type == "Shoes");
            var accessory = aiProducts.FirstOrDefault(p => p.Type == "Accessory");

            if (selected.Type == "Top")
            {
                result.Add(bottom ?? GetBestMatch(allProducts, "Bottom", selected));
                result.Add(shoes ?? GetBestMatch(allProducts, "Shoes", selected));
            }
            else if (selected.Type == "Bottom")
            {
                result.Add(top ?? GetBestMatch(allProducts, "Top", selected));
                result.Add(shoes ?? GetBestMatch(allProducts, "Shoes", selected));
            }
            else if (selected.Type == "Shoes")
            {
                result.Add(top ?? GetBestMatch(allProducts, "Top", selected));
                result.Add(bottom ?? GetBestMatch(allProducts, "Bottom", selected));
            }
            else if (selected.Type == "Full Body")
            {
                result.Add(shoes ?? GetBestMatch(allProducts, "Shoes", selected));
                result.Add(accessory ?? GetBestMatch(allProducts, "Accessory", selected));
            }
            else  if (selected.Type == "Accessory")
            {
                result.Add(top ?? GetBestMatch(allProducts, "Top", selected));
                result.Add(bottom ?? GetBestMatch(allProducts, "Bottom", selected));
                result.Add(shoes ?? GetBestMatch(allProducts, "Shoes", selected));
            }

            return result
                    .Where(p => p != null)
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .ToList();
        }

        
    }
}
