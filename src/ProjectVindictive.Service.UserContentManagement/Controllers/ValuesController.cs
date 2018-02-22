using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloLive.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectVindictive
{
	[Route("api/[controller]")]
	public class ValuesController : AuthorizationReadyController
	{
		// GET api/values
		/// <inheritdoc />
		public ValuesController(IClaimsPrincipalReader haloLiveUserManager) 
			: base(haloLiveUserManager)
		{

		}

		[Authorize]
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2", this.HaloLiveUserManager.GetUserName(this.User) };
		}

		// GET api/values/5

		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/values

		[HttpPost]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5

		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
