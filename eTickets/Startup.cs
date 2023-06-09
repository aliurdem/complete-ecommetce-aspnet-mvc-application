using eTickets.Data;
using eTickets.Data.Card;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace eTickets
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//DbContext Config 
			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConectionString")));

			//Stripe Config
			services.Configure<PaymentSettings>(Configuration.GetSection("Stripe"));

			//Services Config 
			services.AddScoped<IActorsService, ActorsService>();
			services.AddScoped<IProducersService, ProducersService>();
			services.AddScoped<ICinemasService, CinemasService>();
            services.AddScoped<IMoviesService, MoviesService>();
			services.AddScoped<IOrdersService, OrdersService>();
			services.AddScoped(sc => ShoppingCard.GetShoppingCard(sc));
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//Authentication and authoziation
			services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
			services.AddMemoryCache();
			services.AddSession();
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;	
			});

            services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			StripeConfiguration.ApiKey = Configuration.GetSection("SecretKey").ToString(); 

			app.UseSession();

			//Authentication & Authorization 
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			//Seed Database 

			AppDbInitiliazer.Seed(app);
			AppDbInitiliazer.SeedUsersAndRolesAsync(app).Wait();
		}
	}
}
