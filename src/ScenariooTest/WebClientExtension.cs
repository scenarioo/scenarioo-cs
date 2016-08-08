using System;
using System.Net;
using System.Net.Cache;

namespace ScenariooTest
{
    public static class WebClientExtension
    {
        public static string GetStringFromUrl(this string url)
        {
            using (WebClient client = new WebClient())
            {
                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                return client.DownloadString(url + "?no-cache=" + Guid.NewGuid()); // because this NoCacheNoStore doesn't really work, we have to bypass the caching in a more creative way
            }
        }
    }
}