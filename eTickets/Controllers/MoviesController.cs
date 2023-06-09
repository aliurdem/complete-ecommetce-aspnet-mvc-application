﻿using eTickets.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using eTickets.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using eTickets.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace eTickets.Controllers
{
	public class MoviesController : Controller
	{
		private readonly IMoviesService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(IMoviesService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
		{
			var allMovies = await _service.GetAllAsync(n => n.Cinema);
			return View(allMovies);
		}
        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

			if (!string.IsNullOrEmpty(searchString))
			{
				var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) || n.Description.Contains(searchString));

				return View("Index", filteredResult);
			}

            return View(allMovies);
        }

        public async Task<IActionResult> Details(int id) 
		{
			var movieDetails = await _service.GetMovieByIdAsync(id);
			return View(movieDetails);
		}

		//GET : Movies/Create
		public async Task<IActionResult> Create()
		{
			var movieDropDownData = await _service.GetNewMovieDropDownsValues();

			ViewBag.Cinemas = new SelectList(movieDropDownData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropDownData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropDownData.Actors, "Id", "FullName");

            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(NewMovieVM movie) 
		{
			var movieDropDownData = await _service.GetNewMovieDropDownsValues();

			if (!ModelState.IsValid)
			{
				ViewBag.Cinemas = new SelectList(movieDropDownData.Cinemas, "Id", "Name");
				ViewBag.Producers = new SelectList(movieDropDownData.Producers, "Id", "FullName");
				ViewBag.Actors = new SelectList(movieDropDownData.Actors, "Id", "FullName");
				return View(movie);
			}
            if (movie.Image != null)
            {
                string folder = "images/ActorImages";
                folder += Guid.NewGuid().ToString() + movie.Image.FileName;
                movie.ImageURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await movie.Image.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }

            await _service.AddNewMovieAsync(movie);

			return RedirectToAction("Index"); 
		}


		//Get: Movies/Edit/1
		public async Task<IActionResult> Edit(int id) {

			var movieDetails = await _service.GetMovieByIdAsync(id);
			if(movieDetails == null) return View("NotFound");
			
			var response = new NewMovieVM()
			{
				Id = movieDetails.Id,
				Name = movieDetails.Name,
				Description = movieDetails.Description,
				Price = movieDetails.Price,
				ImageURL = movieDetails.ImageURL,
				StartDate = movieDetails.StartDate,
				EndDate = movieDetails.EndDate,
				CinemaId = movieDetails.CinemaId,
				ProducerId = movieDetails.ProducerId,
				ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId ).ToList()
			};

			var movieDropDownData = await _service.GetNewMovieDropDownsValues();

			ViewBag.Cinemas = new SelectList(movieDropDownData.Cinemas, "Id", "Name");
			ViewBag.Producers = new SelectList(movieDropDownData.Producers, "Id", "FullName");
			ViewBag.Actors = new SelectList(movieDropDownData.Actors, "Id", "FullName");

			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, NewMovieVM movie) 
		{
			var movieDropDownData = await _service.GetNewMovieDropDownsValues();

			if (id != movie.Id) return View("NotFound"); 

			if (!ModelState.IsValid)
			{
				ViewBag.Cinemas = new SelectList(movieDropDownData.Cinemas, "Id", "Name");
				ViewBag.Producers = new SelectList(movieDropDownData.Producers, "Id", "FullName");
				ViewBag.Actors = new SelectList(movieDropDownData.Actors, "Id", "FullName");
				return View(movie);
			}

            if (movie.Image != null)
            {
                string folder = "images/ActorImages";
                folder += Guid.NewGuid().ToString() + movie.Image.FileName;
                movie.ImageURL = "/" + folder;

                string serveFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                await movie.Image.CopyToAsync(new FileStream(serveFolder, FileMode.Create));
            }

            await _service.UpdateMovieAsync(movie);

			return RedirectToAction("Index");
		}


		public async Task<IActionResult> Delete(int id) 
		{
			var movie = await _service.GetMovieByIdAsync(id);
			if(movie == null) return View("NotFound");

			return View(movie);
		}

		[HttpPost ,ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var movie = await _service.GetMovieByIdAsync(id);
			if (movie == null)
				return View("NotFound"); 

			await _service.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
