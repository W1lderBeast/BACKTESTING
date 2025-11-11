using Microsoft.Extensions.Configuration;
using Projet_OOS.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projet_OOS.Web.Services
{
    public class AlphaVantageService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public AlphaVantageService(IConfiguration config)
        {
            _apiKey = config["AlphaVantage:ApiKey"] ?? throw new ArgumentNullException("AlphaVantage:ApiKey");
            _httpClient = new HttpClient();
        }

        public async Task<List<FinancialData>> GetDailyDataAsync(string symbol)
        {
            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("Note", out var note))
            {
                throw new Exception($"Quota API dépassé : {note.GetString()}");
            }

            if (root.TryGetProperty("Error Message", out var error))
            {
                throw new Exception($"Erreur API : {error.GetString()}");
            }

            if (!root.TryGetProperty("Time Series (Daily)", out var timeSeries))
            {
                throw new Exception("Réponse API invalide ou données manquantes.");
            }

            var dataList = new List<FinancialData>();

            foreach (var entry in timeSeries.EnumerateObject())
            {
                var date = DateTime.Parse(entry.Name);
                var values = entry.Value;

                dataList.Add(new FinancialData
                {
                    Symbol = symbol,
                    Date = date,
                    Open = decimal.Parse(values.GetProperty("1. open").GetString(), CultureInfo.InvariantCulture),
                    High = decimal.Parse(values.GetProperty("2. high").GetString(), CultureInfo.InvariantCulture),
                    Low = decimal.Parse(values.GetProperty("3. low").GetString(), CultureInfo.InvariantCulture),
                    Close = decimal.Parse(values.GetProperty("4. close").GetString(), CultureInfo.InvariantCulture),
                    AdjustedClose = decimal.Parse(values.GetProperty("4. close").GetString(), CultureInfo.InvariantCulture),
                    Volume = long.Parse(values.GetProperty("5. volume").GetString(), CultureInfo.InvariantCulture)
                });
            }

            return dataList.OrderBy(d => d.Date).ToList();
        }
    }
}
