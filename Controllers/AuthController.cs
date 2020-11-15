using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamBuilder.Models;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Controllers
{
    public class AuthController : Controller
    {
        string client_id = "7621006";
        string client_secret = "cPVPZ6pZSumZIgkEIvhJ";
        static readonly HttpClient client = new HttpClient();
        private readonly DataBaseContext db;

        public AuthController(DataBaseContext context)
        {
            db =  context;
        }
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("UserData"))
            {
                if (!Request.Query.ContainsKey("code"))
                    return Redirect($"https://oauth.vk.com/" +
                                $"authorize?client_id={client_id}" +
                                $"&display=page" +
                                $"&redirect_uri=https://localhost:44342/auth" +
                                $"&scope=photos" +
                                $"&response_type=code" +
                                $"&v=5.124");
                else
                {
                    
                    string path = $"https://oauth.vk.com/" +
                    $"access_token?client_id={client_id}" +
                    $"&client_secret={client_secret}" +
                    $"&redirect_uri=https://localhost:44342/auth" +
                    $"&code={Request.Query["code"]}";

                    using (var webClient = new System.Net.WebClient())
                    {
                        var jsonForToken = webClient.DownloadString(path);
                        Api data = JsonSerializer.Deserialize<Api>(jsonForToken);

                        string UserInfo = webClient.DownloadString($"https://api.vk.com/method/users.get?" +
                                                                $"user_ids={data.user_id}" +
                                                                $"&fields=photo_50,photo_max,photo_id,first_name,last_name" +
                                                                $"&access_token={data.access_token}" +
                                                                $"&v=5.124");

                        /*"{\"response\":[{\"id\":472248745,\"first_name\":\"Александр\",\
                         * "last_name\":\"Зоткин\",\"is_closed\":true,\"can_access_closed\":true,\
                         * "photo_50\":\"https:\\/\\/sun1-47.userapi.com\\/impf\\/c854024\\/v854024599\\/8f279\\/w7IVcGx88UE.jpg?size=50x0&quality=88&crop=283,16,547,547&sign=52a5c5c8ca366d27e751eb2b4afcaa79&c_uniq_tag=JkDa_F3agfqIOb11eB9I6YUBPU2iqVDFvzGK0eFjV78&ava=1\",
                         * \"photo_max\":\"https:\\/\\/sun1-47.userapi.com\\/impf\\/c854024\\/v854024599\\/8f279\\/w7IVcGx88UE.jpg?size=200x0&quality=88&crop=283,16,547,547&sign=d841fc9884c1d6f2a886a5fda760bf6c&c_uniq_tag=5TVt5eu4mQ2kavzWny0YsYFao3IfGw5L3QVV9RpJkBw&ava=1\",
                         * \"photo_id\":\"472248745_457242603\"}]}"*/

                        string first_name = Regex.Match(UserInfo, "(?<=first_name\\\":\\\")[А-Яа-яA-Za-z]+").Value;
                        string last_name = Regex.Match(UserInfo, "(?<=last_name\\\":\\\")[А-Яа-яA-Za-z]+").Value;
                        string photo_50 = Regex.Match(UserInfo, "(?<=photo_50\\\":\\\").+?(?=\")").Value.Replace("\\", "");
                        string photo_id = Regex.Match(UserInfo, "(?<=photo_id\\\":\\\")\\d+_\\d+(?=\\\")").Value;
                        //string photo_max = Regex.Match(UserInfo, "(?<=photo_max\\\":\\\").+?(?=\")").Value.Replace("\\", "");
                        /*
                         "{\"response\":[{\"album_id\":-6,
                                        \"date\":1562877530,\
                                        "id\":457242603,\"owner_id\":472248745,\"has_tags\":false,\"post_id\":88,\"sizes\":
                        [{\"height\":130,\"url\":\"https:\\/\\/sun9-67.userapi.com\\/c854024\\/v854024599\\/8f276\\/9S2cyW5fqJ8.jpg\",\"type\":\"m\",\"width\":130},{\"height\":130,\"url\":\"https:\\/\\/sun9-54.userapi.com\\/c854024\\/v854024599\\/8f27a\\/PCDkshyqiLA.jpg\",\"type\":\"o\",\"width\":130},{\"height\":200,\"url\":\"https:\\/\\/sun9-41.userapi.com\\/c854024\\/v854024599\\/8f27b\\/TyJ5Ih0E0xg.jpg\",\"type\":\"p\",\"width\":200},{\"height\":320,\"url\":\"https:\\/\\/sun9-13.userapi.com\\/c854024\\/v854024599\\/8f27c\\/4nYf0EjmwOY.jpg\",\"type\":\"q\",\"width\":320},{\"height\":510,\"url\":\"https:\\/\\/sun9-44.userapi.com\\/c854024\\/v854024599\\/8f27d\\/zatQ3dAPHCs.jpg\",\"type\":\"r\",\"width\":510},{\"height\":75,\"url\":\"https:\\/\\/sun9-23.userapi.com\\/c854024\\/v854024599\\/8f275\\/1Dwnt_oz_q0.jpg\",\"type\":\"s\",\"width\":75},
                        {\"height\":604,\"url\":\"https:\\/\\/sun9-59.userapi.com\\/c854024\\/v854024599\\/8f277\\/dIQZKIXEEUI.jpg\",\"type\":\"x\",\"width\":604},{\"height\":807,\"url\":\"https:\\/\\/sun9-10.userapi.com\\/c854024\\/v854024599\\/8f278\\/s2IjxSfRwo0.jpg\",\"type\":\"y\",\"width\":807},
                        {\"height\":1080,\"url\":\"https:\\/\\/sun9-47.userapi.com\\/c854024\\/v854024599\\/8f279\\/w7IVcGx88UE.jpg\",\"type\":\"z\",\"width\":1080}],\"text\":\"\"}]}"
                         */

                        //{\"height\":1080,\"url\":\"https:\\/\\/sun9-47.userapi.com\\/c854024\\/v854024599\\/8f279\\/w7IVcGx88UE.jpg\",\"type\":\"z\",\"wi
                        string photo_max;
                        if (photo_id != "")
                        {
                            photo_max = webClient.DownloadString($"https://api.vk.com/method/photos.getById?" +
                                                                        $"photos={photo_id}" +
                                                                        $"&access_token={data.access_token}" +
                                                                        $"&v=5.124");
                            photo_max = Regex.Match(photo_max, "(?<=url\\\":\\\").+?(?=\\\",\\\"type\\\":\\\"z)").Value;
                            photo_max = photo_max.Substring(photo_max.Length - 74, 74).Replace("\\", "");
                        }
                        else
                        {
                            photo_max = "";
                        }
                    

                    //РЕГИСТРАЦИЯ
                    if (!db.Users.Any(u => u.VkId == data.user_id))
                    {
                        User User = new User();
                        User.FirstName = first_name;
                        User.LastName = last_name;
                        User.Photo50 = photo_50;
                        User.PhotoMax = photo_max;
                        User.VkId = data.user_id;
                        User.AccessToken = data.access_token;
                        db.Users.Add(User);
                    }
                    else
                    {
                        User User = await db.Users.Where(u => u.VkId == data.user_id).FirstAsync();
                        User.FirstName = first_name;
                        User.LastName = last_name;
                        User.Photo50 = photo_50;
                        User.PhotoMax = photo_max;
                        User.AccessToken = data.access_token;
                    }

                    await db.SaveChangesAsync();
                    Response.Cookies.Append("UserData", data.user_id.ToString());
                    Response.Cookies.Append("AccessToken", data.access_token);
                    }
                }
                return Redirect("/");
            }
            else
                return Redirect("/");

        }

        //public string AuthCode()
        
            
            //var httpResponce = new HttpClient().PostAsync(path, new StringContent(JsonConvert.SerializeObject(new { }), Encoding.UTF8, "application/json"));
            //var httpResponce = new HttpClient().GetAsync(path);
            //return httpResponce.Result;
           // HttpResponseMessage response = await client.GetAsync(path);
           // response.EnsureSuccessStatusCode();
           // string responseBody = await response.Content.ReadAsStringAsync();
           // return responseBody;

        
    }
}
