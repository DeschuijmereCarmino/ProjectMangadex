using Newtonsoft.Json.Linq;
using ProjectMangadex.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

    }
}
