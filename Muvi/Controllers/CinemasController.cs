using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Muvi.Data;
using Muvi.Data.Base;
using Muvi.Data.Interfaces;
using Muvi.Data.Static;
using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Controllers
{
    [Authorize(Roles =UserRoles.Admin)]
    public class CinemasController : Controller
    {
        private readonly ICinemaInterface _service;

        public CinemasController(ICinemaInterface service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allCinemas =await _service.GetAll();
            return View(allCinemas);
        }

        //GET: Cinema/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await _service.Get(id);

            if (cinemaDetails == null)
            {
                return View("Not Found!");
            }

            return View(cinemaDetails);
        }

        //Get cinema/create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //POST: add Cinema
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, Logo, Description")] Cinema newCinema)
        {
            if (!ModelState.IsValid)
            {
                return View(newCinema);
            }

            await _service.Add(newCinema);

            return RedirectToAction(nameof(Index));
        }

        //Get Cinemas/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id);
        }

        //POST: Edit Cinema
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name, Logo, Description")] Cinema newCinema)
        {
            if (!ModelState.IsValid)
            {
                return View(newCinema);
            }

            await _service.Update(newCinema);

            return RedirectToAction(nameof(Index));
        }

        //Get Cinemas/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //POST: Delete Cinema
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var CinemaDetails = _service.Get(id);

            if (CinemaDetails == null)
            {
                return View("Not Found!");
            }

            await _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
