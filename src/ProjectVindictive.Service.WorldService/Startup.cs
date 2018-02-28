using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloLive.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProjectVindictive
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddMvc();
			services.AddHaloLiveAuthorization();

			//Register and load the DB config
			services.RegisterDatabaseConfigOptions(Configuration);
			IOptions<DatabaseConfigModel> dbConfig = services.GetDatabaseConfig();
			services.AddDbContext<WorldDatabaseContext>(options => options.UseMySql(dbConfig.Value.ConnectionString));

			//We only need readonly privileges in the WorldService for the game. At least for entries.
			services.AddTransient<IReadOnlyWorldEntryRepository, WorldDatabaseRepository>();

			//Adds and registers S3 service for URLBuilding and communication/credentials and etc
			services.AddS3Service(Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			//TODO: At some point this will break in netstandard2.0 so we must do something different
			app.UseHaloLiveAuthorization(HaloLive.Hosting.X509Certificate2Loader.Create("Certs/TestCert.pfx").Load());
			app.UseIdentity();
			app.UseMvcWithDefaultRoute();
		}
	}
}
