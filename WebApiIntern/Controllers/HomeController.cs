using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApiIntern.Models;

namespace WebApiIntern.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ApiGet()
        {
            ApiModel key = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://interns.bcgdvsydney.com/api/v1/key");

                
                var responseTask = client.GetAsync("key");
                responseTask.Wait();

              
                var result = responseTask.Result;
                var status = responseTask.Status;

              
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ApiModel>();
                    readTask.Wait();

                    key = readTask.Result;
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }


               
                    client.BaseAddress = new Uri("https://interns.bcgdvsydney.com/api/v1/submit?apiKey="+key.Key);

                PersonalInfoModel myInfo = new PersonalInfoModel();

                myInfo.Email = ""; // Your email here
                myInfo.Name = ""; //Your name here
                var content = new StringContent(myInfo.ToString(), Encoding.UTF8, "application/json");
                var postTask = client.PostAsync("https://interns.bcgdvsydney.com/api/v1/submit?apiKey=" + key.Key, content);
                    postTask.Wait();

                    var resultPost = postTask.Result;
                    if (resultPost.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                

                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                //return View(student);

                return View(key);

           // return View();
        }
    }
}