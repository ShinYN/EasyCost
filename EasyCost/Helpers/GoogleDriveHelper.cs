using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
//using Windows.Web.Http;

namespace EasyCost.Helpers
{
    public static class GoogleDriveHelper
    {
        private static string mBaseUri = "https://ggcostwebapi.azurewebsites.net/";
        //private static string mBaseUri = "http://localhost:8546/";
        private static string mRequestUri = "api/googledrive";

        public static async Task<byte[]> LoadFileAsync()
        {
            byte[] fileData = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(mBaseUri);

                var response = await client.GetAsync(mRequestUri);
                if (response.IsSuccessStatusCode)
                {
                    fileData = await response.Content.ReadAsByteArrayAsync();
                }
            }

            return fileData;
        }

        public static async Task<HttpStatusCode> SaveFileAsync()
        {
            var filePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            byte[] fileData = null;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int fileLength = (int)fs.Length;
                fileData = new byte[fileLength];
                fs.Read(fileData, 0, fileLength);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mBaseUri);
                    var byteData = new ByteArrayContent(fileData);
                    var multiContent = new MultipartFormDataContent();
                    multiContent.Add(byteData, "file", "db.sqlite");

                    var result = await client.PostAsync(mRequestUri, multiContent);

                    return result.StatusCode;
                }
            }
        }
    }
}
