using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.services
{
    public class RepositoryServices : IRepositoryServices
    {
        public async Task<List<Repository>> ProcessRepositories(HttpClient client, string company)
    {
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync($"https://api.github.com/orgs/{company}/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
    }
    }

    public interface IRepositoryServices
    {
        Task<List<Repository>> ProcessRepositories(HttpClient client, string company);
    }
}