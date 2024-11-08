using HtmlAgilityPack; // For parsing HTML
using System.Net.Http; // For making HTTP requests
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StockExchangeAnalysisApp.Models; // Namespace for your models
using StockExchangeAnalysisApp.Data; // Namespace for ApplicationDbContext

namespace StockExchangeAnalysisApp.Services
{
    public class IssuerScraperService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ApplicationDbContext _context;

        public IssuerScraperService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ScrapeAndStoreIssuerCodesAsync()
        {
            // Step 1: Fetch the webpage content
            var url = "https://www.mse.mk/en/stats/symbolhistory/kmb";
            var response = await client.GetStringAsync(url); // Sends GET request to fetch the HTML content

            // Step 2: Parse the HTML content
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            // Step 3: Extract the dropdown element
            var dropdown = htmlDoc.DocumentNode.SelectSingleNode("//select[@id='Code']"); // XPath for your dropdown

            if (dropdown != null)
            {
                // Step 4: Iterate through the options and filter them
                foreach (var option in dropdown.SelectNodes("option"))
                {
                    var code = option.Attributes["value"]?.Value.Trim();
                    if (!string.IsNullOrEmpty(code) && !code.Any(char.IsDigit)) // Exclude codes containing numbers
                    {
                        // Check if the issuer already exists in the database
                        if (!_context.Issuers.All(i => i.Code != code))
                        {
                            continue; // Skip if issuer already exists
                        }

                        // Add new issuer to the DbContext
                        _context.Issuers.Add(new Issuer { Code = code });
                    }
                }

                // Save all the new issuers to the database in a single batch
                await _context.SaveChangesAsync(); // Save the changes to the database
            }
        }
    }
}
