using CacheManager.Core;
using EFSecondLevelCache.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebFramework.Configuration.Caching_configuraion_Extention
{
    public static class CacheService
    {
        public static void AddCachingServiceExtention(this IServiceCollection services)
        {
            services.AddEFSecondLevelCache();

            // Add an in-memory cache service provider
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
            services.AddSingleton(typeof(ICacheManagerConfiguration),
                new ConfigurationBuilder()
                        .WithJsonSerializer()
                        .WithMicrosoftMemoryCacheHandle(instanceName: "MemoryCache1")
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(5))
                        .Build());

            // Add Redis cache service provider
            //var jss = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};

            //const string redisConfigurationKey = "redis";
            //services.AddSingleton(typeof(ICacheManagerConfiguration),
            //    new CacheManager.Core.ConfigurationBuilder()
            //        .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
            //        .WithUpdateMode(CacheUpdateMode.Up)
            //        .WithRedisConfiguration(redisConfigurationKey, config =>
            //        {
            //            config.WithAllowAdmin()
            //                .WithDatabase(0)
            //                .WithEndpoint("localhost", 6379);
            //        })
            //        .WithMaxRetries(100)
            //        .WithRetryTimeout(50)
            //        .WithRedisCacheHandle(redisConfigurationKey)
            //        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            //        .Build());
            //services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

        }
    }
}
