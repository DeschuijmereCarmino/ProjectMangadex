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

        //public static async Task<List<Manga>> GetMangasAsync(int offset)
        public static async Task<List<Manga>> GetMangasAsync()
        {
            using (var client = GetClient())
            {
                //string url = $"{_BASEURI}/manga?offset={offset}";
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
                            await SecureStorage.SetAsync("username", user.Username);

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

        public static async Task FollowMangaAsync(Guid mangaId)
        {
            using (var client = await GetClientWithAuth())
            {
                try
                {
                    string url = $"{_BASEURI}/manga/{mangaId}/follow";
                    var response = await client.PostAsync(url, null);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Er ging iets mis bij het volgen van de manga.({response.StatusCode} : {response.ReasonPhrase})");
                    }
                    else
                    {
                        Debug.WriteLine("Volgen succesvol");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task UnfollowMangaAsync(Guid mangaId)
        {
            using (var client = await GetClientWithAuth())
            {
                try
                {
                    string url = $"{_BASEURI}/manga/{mangaId}/follow";
                    var response = await client.DeleteAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Er ging iets mis bij het ontvolgen van de manga. ({response.StatusCode} : {response.ReasonPhrase})");
                    }
                    else
                    {
                        Debug.WriteLine("Ontvolgen succesvol");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task GetUserLoggedOutAsync()
        {
            using (var client = await GetClientWithAuth())
            {
                try
                {
                    string url = $"{_BASEURI}/auth/logout";
                    var response = await client.PostAsync(url, null);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Er ging iets mis bij het uitloggen.({response.StatusCode} : {response.ReasonPhrase})");
                    }

                    SecureStorage.Remove("bearer_token");
                    SecureStorage.Remove("username");
                    Debug.WriteLine("Uitloggen succesvol");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<List<string>> GetAuthorsForMangaAsync(Manga manga)
        {
            List<Guid> creatorIds = new List<Guid>();

            foreach (var relationship in manga.Relationships)
            {
                switch (relationship.Type)
                {
                    case "author":
                        creatorIds.Add(relationship.Id);
                    break;

                    case "artist":
                        if (!creatorIds.Contains(relationship.Id))
                        {
                            creatorIds.Add(relationship.Id);
                        }
                    break;
                }
            }

            using (var client = GetClient())
            {
                try
                {
                    List<string> authors = new List<string>();

                    foreach(var id in creatorIds)
                    {
                        string url = $"{_BASEURI}/author/{id}";
                        string json = await client.GetStringAsync(url);


                        if (json != null)
                        {
                            JObject newJson = JObject.Parse(json);
                            string author = newJson["data"]["attributes"]["name"].ToObject<string>();

                            authors.Add(author);
                        }
                    }
                    return authors;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<Boolean> CheckMangaFollowed(Guid mangaId)
        {
            using (var client = await GetClientWithAuth())
            {
                try
                {
                    await LogToken();
                    string url = $"{_BASEURI}/user/follows/manga/{mangaId}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task CreateAccount(User user)
        {
            using (var client = GetClient())
            {
                try
                { 
                    string url = $"{_BASEURI}/account/create";
                    string json = JsonConvert.SerializeObject(user);

                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Er ging iets mis bij het creeren van het account.({response.StatusCode} : {response.ReasonPhrase})");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //!!!!!!! END CLASS !!!!!!!
    }
}
