using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using WebAPI.Models;
using WebAPI.services;

namespace WebAPI.Controllers{
    [ApiController]
    [Route("repositories")]
    public class RepositoryController : ControllerBase {
        private IMemoryCache cache;
        private IFeatureManager featureManager;
        private readonly IConfiguration configuration;
        private readonly IRepositoryServices repositoryServices;
        public RepositoryController(IMemoryCache Cache, IFeatureManager featureManager, IConfiguration configuration, IRepositoryServices RepositoryServices)
        {
            cache = Cache;
            this.featureManager = featureManager;
            this.configuration = configuration;
            repositoryServices = RepositoryServices;

        }
        
        public static class FeatureFlags
{
    public const string MemoryCache = "MemoryCache";
}
        
        [HttpGet]
        [Route("{company}")]
        
        public async Task<List<Repository>> ProcessRepositories(string company)
        {
            var isEnabled = featureManager.IsEnabledAsync(FeatureFlags.MemoryCache);
        if(await isEnabled)
            {
            var cacheEntry = await
            cache.GetOrCreateAsync(company,async RepositoryList =>
            {
                RepositoryList.SlidingExpiration = TimeSpan.FromSeconds(configuration.GetSection("CacheManagement").GetValue<double>("Duration"));
                return await repositoryServices.ProcessRepositories(new HttpClient(), company);
            });

            return cacheEntry;
        }
        else{return await repositoryServices.ProcessRepositories(new HttpClient(), company);
        }
        }
}
}
