using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace QwkThx.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index (string handler, string ticket, string customer)
		{
			if (handler != null && ticket != null && customer != null) {
				//If all parameters are in, then PostToSlack without resorting to post\
				var payload = new SlackAttack();
				payload.username = "QwkThx";
				payload.text = handler + " received a thank you from " + customer;
				return  !postToSlack(payload).IsCompleted? View ("Success") :  View ("Error");
			} 

			ViewData ["handler"] = new SelectList(new Team().Staff, handler);
			ViewData ["Ticket"] = ticket;
			ViewData ["Customer"] = customer;
	

			return View ();
		}


		[HttpPost]
		public ActionResult Index (string handler, string comment, string ticket, string customer)
		{	
			var payload = new SlackAttack();
			payload.username = "QwkThx";
			payload.text = handler + " received a thank you with the note: \n" + comment;
			return  !postToSlack(payload).IsCompleted? View ("Success") :  View ("Error");
		}

		private Task<HttpResponseMessage> postToSlack(SlackAttack payload){
			var json = JsonConvert.SerializeObject(payload);
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("https://hooks.slack.com/");
			//your slack "services/#########/#########/#############"webhook endpoint below
			return client.PostAsync ("services/_______/_______/______________", new StringContent(json,
				Encoding.UTF8, 
				"application/json"));
			
		}

	}

	public class SlackAttack
	{
		public string icon;
		public string username;
		public string text;
	}

 
}

