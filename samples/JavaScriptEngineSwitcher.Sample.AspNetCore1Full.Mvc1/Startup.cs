﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.Jurassic;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.Sample.Logic.Services;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Vroom;

namespace JavaScriptEngineSwitcher.Sample.AspNetCore1Full.Mvc1
{
	public class Startup
	{
		/// <summary>
		/// Gets or sets a instance of hosting environment
		/// </summary>
		public IHostingEnvironment HostingEnvironment
		{
			get;
			set;
		}

		public IConfigurationRoot Configuration
		{
			get;
			set;
		}


		public Startup(IHostingEnvironment env)
		{
			HostingEnvironment = env;

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(Configuration);

			// Add JavaScriptEngineSwitcher services to the services container.
			services.AddJsEngineSwitcher(options =>
			{
				options.AllowCurrentProperty = false;
				options.DefaultEngineName = ChakraCoreJsEngine.EngineName;
			})
				.AddChakraCore()
				.AddJurassic()
				.AddMsie(options =>
				{
					options.EngineMode = JsEngineMode.ChakraIeJsRt;
				})
				.AddV8()
				.AddVroom()
				;

			// Add framework services.
			var manager = new ApplicationPartManager();
			manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));

			services.AddSingleton(manager);

			services.AddMvc(options =>
			{
				options.CacheProfiles.Add("CacheCompressedContent5Minutes",
					new CacheProfile
					{
						NoStore = HostingEnvironment.IsDevelopment(),
						Duration = 300,
						Location = ResponseCacheLocation.Client,
						VaryByHeader = "Accept-Encoding"
					}
				);
			});

			// Add JavaScriptEngineSwitcher sample services to the services container.
			services.AddSingleton<JsEvaluationService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}