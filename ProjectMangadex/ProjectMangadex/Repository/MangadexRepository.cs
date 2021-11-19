using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectMangadex.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ProjectMangadex.Repository
{
    public class MangadexRepository
    {
        private const string _BASEURI = "https://api.mangadex.org";
        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        private async static Task<HttpClient> GetClientWithAuth()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("accept", "application/json");

            var bearerToken = await SecureStorage.GetAsync("bearer_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            return client;
        }

        public static async Task<List<Manga>> GetMangasAsync()
        {
            using (var client = GetClient())
            {
                string url = $"{_BASEURI}/manga";
                try
                {
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        JObject newJson = JObject.Parse(json);
                        JArray data = (JArray)newJson["data"];

                        List<Manga> mangas = data.ToObject<List<Manga>>();


                        foreach (var manga in mangas)
                        {
                            var coverId = manga.Relationships.Find(r => r.Type == "cover_art");
                            string cover = await GetMangaCoverByMangaIdAsync(coverId.Id);
                            manga.Cover = $"https://uploads.mangadex.org/covers/{manga.Id}/{cover}";
                        }

                        return mangas;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<string> GetMangaCoverByMangaIdAsync(Guid mangaId)
        {
            using (var client = GetClient())
            {

                try
                {
                    string url = $"{_BASEURI}/cover/{mangaId}";
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        JObject newJson = JObject.Parse(json);
                        string cover = (string)newJson["data"]["attributes"]["fileName"];

                        return cover;
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<bool> GetUserAsync(User user)
        {
            using (var client = GetClient())
            {
                try
                {
                    string url = $"{_BASEURI}/auth/login";

                    string json = JsonConvert.SerializeObject(user);

                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseJson = await response.Content.ReadAsStringAsync();

                        if (responseJson != null)
                        {
                            JObject newJson = JObject.Parse(responseJson);
                            await SecureStorage.SetAsync("bearer_token", (string)newJson["token"]["session"]);

                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception($"Er ging iets mis met de post method ({response.StatusCode} : {response.ReasonPhrase})");
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task LogToken()
        {
            var bearerToken = await SecureStorage.GetAsync("bearer_token");
            Debug.WriteLine(bearerToken);
        }

        public static async Task<List<Manga>> GetFollowedMangasAsync()
        {
            using (var client = await GetClientWithAuth())
            {
                try
                {
                    string url = $"{_BASEURI}/user/follows/manga";

                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        JObject newJson = JObject.Parse(json);
                        JArray data = (JArray)newJson["data"];

                        List<Manga> mangas = data.ToObject<List<Manga>>();

                        return mangas;
                    }

                    return null;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        //END CLASS !!!!!!!
    }
}
