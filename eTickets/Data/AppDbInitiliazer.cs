﻿using System.Collections.Generic;
using System;
using System.Linq;
using eTickets.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using eTickets.Data.Static;
using Stripe;

namespace eTickets.Data
{
	public class AppDbInitiliazer
	{
		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

				context.Database.EnsureCreated();

				// if there is no Data in the related table, this code block adds some data to the tables

				//Cinema
				if (!context.Cinemas.Any())
				{
					context.Cinemas.AddRange(new List<Cinema>()
					{
						new Cinema()
						{
							Name = "Cinema 1",
							LogoURL = "/images/cinema-1.jpeg",
							Description = "This is the description of the first cinema"
						},
						new Cinema()
						{
							Name = "Cinema 2",
							LogoURL ="/images/cinema-2.jpeg",
                            Description = "This is the description of the  cinema"
						},
						new Cinema()
						{
							Name = "Cinema 3",
							LogoURL = "/images/cinema-3.jpeg",
                            Description = "This is the description of the first cinema"
						},
						new Cinema()
						{
							Name = "Cinema 4",
							LogoURL = "/images/cinema-4.jpeg",
                            Description = "This is the description of the first cinema"
						},
						new Cinema()
						{
							Name = "Cinema 5",
							LogoURL = "/images/cinema-5.jpeg",
                            Description = "This is the description of the first cinema"
						},
					});
					context.SaveChanges();
				}
				//Actors
				if (!context.Actors.Any())
				{
					context.Actors.AddRange(new List<Actor>()
					{
						new Actor()
						{
							FullName = "Actor 1",
							Bio = "This is the Bio of the first actor",
							ProfilePictureURL = "/images/actor-1.jpeg"

                        },
						new Actor()
						{
							FullName = "Actor 2",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL ="/images/actor-2.jpeg"
                        },
						new Actor()
						{
							FullName = "Actor 3",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/actor-3.jpeg"
                        },
						new Actor()
						{
							FullName = "Actor 4",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/actor-4.jpeg"
						},
						new Actor()
						{
							FullName = "Actor 5",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/actor-5.jpeg"
						}
					});
					context.SaveChanges();
				}
				//Producers
				if (!context.Producers.Any())
				{
					context.Producers.AddRange(new List<Producer>()
					{
						new Producer()
						{
							FullName = "Producer 1",
							Bio = "This is the Bio of the first actor",
							ProfilePictureURL = "/images/producer-1.jpeg"

						},
						new Producer()
						{
							FullName = "Producer 2",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/producer-2.jpeg"
                        },
						new Producer()
						{
							FullName = "Producer 3",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/producer-3.jpeg"
                        },
						new Producer()
						{
							FullName = "Producer 4",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/producer-4.jpeg"
                        },
						new Producer()
						{
							FullName = "Producer 5",
							Bio = "This is the Bio of the second actor",
							ProfilePictureURL = "/images/producer-5.jpeg"
                        }
					});
					context.SaveChanges();
				}
				//Movies
				if (!context.Movies.Any())
				{
					context.Movies.AddRange(new List<Movie>()
					{
						new Movie()
						{
							Name = "Life",
							Description = "This is the Life movie description",
							Price = 39.50,
							ImageURL = "/images/movie-3.jpeg",
							StartDate = DateTime.Now.AddDays(-10),
							EndDate = DateTime.Now.AddDays(10),
							CinemaId = 3,
							ProducerId = 3,
							MovieCategory = MovieCategory.Documentary
						},
						new Movie()
						{
							Name = "The Shawshank Redemption",
							Description = "This is the Shawshank Redemption description",
							Price = 29.50,
							ImageURL =  "/images/movie-1.jpeg",
                            StartDate = DateTime.Now,
							EndDate = DateTime.Now.AddDays(3),
							CinemaId = 1,
							ProducerId = 1,
							MovieCategory = MovieCategory.Action
						},
						new Movie()
						{
							Name = "Ghost",
							Description = "This is the Ghost movie description",
							Price = 39.50,
							ImageURL =  "/images/movie-4.jpeg",
                            StartDate = DateTime.Now,
							EndDate = DateTime.Now.AddDays(7),
							CinemaId = 4,
							ProducerId = 4,
							MovieCategory = MovieCategory.Horror
						},
						new Movie()
						{
							Name = "Race",
							Description = "This is the Race movie description",
							Price = 39.50,
							ImageURL = "/images/movie-6.jpeg",
                            StartDate = DateTime.Now.AddDays(-10),
							EndDate = DateTime.Now.AddDays(-5),
							CinemaId = 1,
							ProducerId = 2,
							MovieCategory = MovieCategory.Documentary
						},
						new Movie()
						{
							Name = "Scoob",
							Description = "This is the Scoob movie description",
							Price = 39.50,
							ImageURL = "/images/movie-7.jpeg",
                            StartDate = DateTime.Now.AddDays(-10),
							EndDate = DateTime.Now.AddDays(-2),
							CinemaId = 1,
							ProducerId = 3,
							MovieCategory = MovieCategory.Cartoon
						},
						new Movie()
						{
							Name = "Cold Soles",
							Description = "This is the Cold Soles movie description",
							Price = 39.50,
							ImageURL = "/images/movie-8.jpeg",
                            StartDate = DateTime.Now.AddDays(3),
							EndDate = DateTime.Now.AddDays(20),
							CinemaId = 1,
							ProducerId = 5,
							MovieCategory = MovieCategory.Drama
						}
					});
					context.SaveChanges();
				}
				//Actors & Movies
				if (!context.Actors_Movies.Any())
				{
					context.Actors_Movies.AddRange(new List<Actor_Movie>()
					{
						new Actor_Movie()
						{
							ActorId = 1,
							MovieId = 1
						},
						new Actor_Movie()
						{
							ActorId = 3,
							MovieId = 1
						},

						 new Actor_Movie()
						{
							ActorId = 1,
							MovieId = 2
						},
						 new Actor_Movie()
						{
							ActorId = 4,
							MovieId = 2
						},

						new Actor_Movie()
						{
							ActorId = 1,
							MovieId = 3
						},
						new Actor_Movie()
						{
							ActorId = 2,
							MovieId = 3
						},
						new Actor_Movie()
						{
							ActorId = 5,
							MovieId = 3
						},


						new Actor_Movie()
						{
							ActorId = 2,
							MovieId = 4
						},
						new Actor_Movie()
						{
							ActorId = 3,
							MovieId = 4
						},
						new Actor_Movie()
						{
							ActorId = 4,
							MovieId = 4
						},


						new Actor_Movie()
						{
							ActorId = 2,
							MovieId = 5
						},
						new Actor_Movie()
						{
							ActorId = 3,
							MovieId = 5
						},
						new Actor_Movie()
						{
							ActorId = 4,
							MovieId = 5
						},
						new Actor_Movie()
						{
							ActorId = 5,
							MovieId = 5
						},


						new Actor_Movie()
						{
							ActorId = 3,
							MovieId = 6
						},
						new Actor_Movie()
						{
							ActorId = 4,
							MovieId = 6
						},
						new Actor_Movie()
						{
							ActorId = 5,
							MovieId = 6
						},
					});
					context.SaveChanges();
				}
			}

		}
		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviseScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviseScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

				if (!await roleManager.RoleExistsAsync(UserRoles.User))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

				//Users
				var userManager = serviseScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				string adminUserEmail = "admin@etickets.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

				if (adminUser == null)
				{
					var newAdminUser = new ApplicationUser()
					{
						FullName = "Admin User",
						UserName = "admin-user",
						Email = adminUserEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newAdminUser, "Coding@2350");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}

				string appUserEmail = "appUser@etickets.com";


				var appUser = await userManager.FindByEmailAsync(appUserEmail);
		

				if (appUser == null)
				{
					var newAppUser = new ApplicationUser()
					{
						FullName = "Application User",
						UserName = "application-user",
						Email = appUserEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newAppUser, "Using@2350");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
				}


			}
		}
	}
}



