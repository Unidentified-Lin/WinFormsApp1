using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Utils
{
    public static class APIUtil
    {
        public enum PaginationDirection
        {
            previous,
            next
        }
        public static HttpClient GetHttpClient(string baseUrl, string key)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", key);
            return client;
        }
        public static string GetAuthorizationString(string apikey, string password)
        {
            byte[] loginKey = Encoding.Default.GetBytes(apikey + ":" + password);
            return Convert.ToBase64String(loginKey);
        }

        /// <summary>
        /// 取得request資料，回傳內容放在ResponseModel.JsonData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<ResponseModel<T>> GetResponseAsync<T>(HttpClient client, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            ResponseModel<T> model = new();

            if (response.Headers.Contains("Link"))
            {
                var linkContents = response.Headers.GetValues("Link").First().Split(",");
                model.HasNext = linkContents.Any(c => c.Contains(@"rel=""next"""));
                model.HasPrevious = linkContents.Any(c => c.Contains(@"rel=""previous"""));
                model.NextUrl = GetUrlFromLinkContents(linkContents, PaginationDirection.next);
                model.PreviousUrl = GetUrlFromLinkContents(linkContents, PaginationDirection.previous);
            }
            model.JsonString = await response.Content.ReadAsStringAsync();
            model.JsonData = await response.Content.ReadFromJsonAsync<T>();

            return model;
        }

        /// <summary>
        /// 查詢所有的分頁內容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<ResponseModel<T>> GetAllResponseAsync<T>(HttpClient client, string url)
        {
            ResponseModel<T> model = new();

            bool hasNext;
            do
            {
                var responseModel = await GetResponseAsync<T>(client, url);
                hasNext = responseModel.HasNext;
                model.JsonDataList.Add(responseModel.JsonData);
                model.UpdateModelState(responseModel);
            } while (hasNext);
            return model;
        }

        private static string GetUrlFromLinkContents(string[] linkContents, PaginationDirection dir)
        {
            if (!linkContents.Any(c => c.Contains(@$"rel=""{dir}""")))
            {
                return "";
            }
            var urlSection = linkContents.First(c => c.Contains(@$"rel=""{dir}""")).Split(";")[0].Trim();
            return urlSection.Substring(urlSection.IndexOf("<") + 1, urlSection.IndexOf(">") - 1);
        }
    }
}
