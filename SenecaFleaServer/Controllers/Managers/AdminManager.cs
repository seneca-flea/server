using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SenecaFleaServer.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SenecaFleaServer.Controllers
{
    public class AdminManager
    {
        private DataContext ds;

        public AdminManager()
        {
            ds = new DataContext();
        }

        // Get items; only used for Item Administration 
        public IEnumerable<ItemBase> ItemGet()
        {
            var items = ds.Items.OrderBy(i => i.Title);
            return Mapper.Map<IEnumerable<ItemBase>>(items);
        }

        public ItemWithMedia ItemGetByIdWithMedia(int id)
        {
            var result = ds.Items.Include("Images")
                .SingleOrDefault(i => i.ItemId == id);

            return Mapper.Map<ItemWithMedia>(result);
        }

        public Image ItemImageGetById(int id)
        {
            var o = ds.Images.Find(id);
            return (o == null) ? null : Mapper.Map<Image>(o);
        }

        // Delete item
        public void ItemDelete(int id)
        {
            var storedItem = ds.Items.Find(id);

            if (storedItem != null)
            {
                ds.Items.Remove(storedItem);
                ds.SaveChanges();
            }
        }

        private HttpClient CreateRequest(string acceptValue = "application/json")
        {
            var request = new HttpClient();

            // Could also fetch the base address string from the app's global configuration
            // Base URI of the web service we are interacting with
            request.BaseAddress = new Uri("http://senecafleamarket.azurewebsites.net/api/");

            // Accept header configuration
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptValue));

            // Attempt to get the token from session state memory
            // Info: https://msdn.microsoft.com/en-us/library/system.web.httpcontext.session(v=vs.110).aspx

            var token = HttpContext.Current.Session["token"] as string;

            if (string.IsNullOrEmpty(token)) { token = "empty"; }

            // Authorization header configuration
            request.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer", token);

            return request;
        }


        public async Task<bool> GetAccessToken(LoginItems loginItems)
        {
            // Continue?
            if (loginItems == null) { return false; }

            // Clean the incoming data
            // Packaging alternatives... dictionary or list of key-value pairs

            // Create a package for the request - dictionary
            var rd = new Dictionary<string, string>();
            rd.Add("grant_type", "password");
            rd.Add("username", loginItems.Email);
            rd.Add("password", loginItems.Password);

            // Create a package for the request - list of key-value pairs
            var requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            // etc.

            // Create an HttpContent object
            var content = new FormUrlEncodedContent(rd);

            // Create a request
            using (var request = CreateRequestToken())
            {
                // Send the request... POST, to token endpoint, with the form URL encoded content
                // Make it complete by adding the Result property
                var response = request.PostAsync("http://senecafleaia.azurewebsites.net/token", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Extract the token from the response
                    var tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();

                    // Save the token in session state
                    HttpContext.Current.Session["token"] = tokenResponse.access_token;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///  To easily create and use an HTTP client object.
        /// </summary>
        /// <param name="acceptValue"></param>
        /// <returns></returns>
        private HttpClient CreateRequestToken(string acceptValue = "application/json")
        {
            var request = new HttpClient();

            // Could also fetch the base address string from the app's global configuration
            // Base URI of the web service we are interacting with
            request.BaseAddress = new Uri("http://senecafleaia.azurewebsites.net/api/");

            // Accept header configuration
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(acceptValue));

            // Attempt to get the token from session state memory
            // Info: https://msdn.microsoft.com/en-us/library/system.web.httpcontext.session(v=vs.110).aspx

            var token = HttpContext.Current.Session["token"] as string;

            if (string.IsNullOrEmpty(token)) { token = "empty"; }

            // Authorization header configuration
            request.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue
                ("Bearer", token);

            return request;
        }

        //public ItemPhotos VehiclePhotoGetById(int id)
        //{
        //    var o = ds.Items.Find(id);

        //    return (o == null) ? null : Mapper.Map<ItemPhotos>(o);
        //}

    }
}