using System.Net.Http.Json;
using BookTracker2.Models;

namespace BookTracker2.Data
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public class GoogleBooksResponse
        {
            public List<GoogleBooksItem>? Items { get; set; }
        }

        public class GoogleBooksItem
        {
            public VolumeInfo? VolumeInfo { get; set; }
        }

        public class VolumeInfo
        {
            public string? Title { get; set; }
            public List<string>? Authors { get; set; }
            public List<string>? Categories { get; set; }
            public string? Description { get; set; }
        }

        public async Task<VolumeInfo?> LookupByISBNAsync(string isbn)
        {
            isbn = isbn.Replace("-", "").Replace(" ", "").Trim();

            try
            {
                var url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}&key={Constants.GoogleBooksApiKey}";
                var response = await _httpClient.GetFromJsonAsync<GoogleBooksResponse>(url);
                return response?.Items?.FirstOrDefault()?.VolumeInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
