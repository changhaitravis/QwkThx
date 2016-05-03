using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using System.Net.Http;

namespace QwkThx.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

			return View ();
		}


		[HttpPost]
		public ActionResult Index (string handler, string comment)
		{	
			var payload = new SlackAttack();
			payload.username = "QwkThx";
			payload.text = handler + " received a thank you with the note: \n" + comment;
			var json = JsonConvert.SerializeObject(payload);

			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://hooks.slack.com/");
			//your slack "services/#########/#########/#############"webhook endpoint below
			var response = client.PostAsync ("services/_______/_______/______________", new StringContent(json,
				Encoding.UTF8, 
				"application/json"));
			return  !response.IsCompleted? View ("Success") :  View ("Error");
		}


	}

	public class SlackAttack
	{
		public string icon;
		public string username;
		public string text;
	}
}

