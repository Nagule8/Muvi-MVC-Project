using Microsoft.AspNetCore.Mvc;
using Muvi.Data;
using Muvi.Data.Base;
using Muvi.Data.Interfaces;
using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorInterface _service;

        public ActorsController(IActorInterface service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allActors = await _service.GetAll();

            return View(allActors);
        }

        //GET: Actors/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _service.Get(id);

            if(actorDetails == null){
                return View("Not Found!");
            }

            return View(actorDetails);
        }

        //Get actors/create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //POST: add actor
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Actor newActor)
        {
            if (!ModelState.IsValid)
            {   
                return View(newActor);
            }

            await _service.Add(newActor);

            return RedirectToAction(nameof(Index));
        }

        //Get actors/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id);
        }

        //POST: Edit actor
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Actor newActor)
        {
            if (!ModelState.IsValid)
            {
                return View(newActor);
            }

            await _service.Update(newActor);

            return RedirectToAction(nameof(Index));
        }

        //Get actors/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //POST: Delete actor
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = _service.Get(id);

            if(actorDetails == null)
            {
                return View("Not Found!");
            }

            await _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
