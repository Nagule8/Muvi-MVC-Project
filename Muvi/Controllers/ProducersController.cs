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
    public class ProducersController : Controller
    {
        private readonly IProducerInterface _service;

        public ProducersController(IProducerInterface service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allProducers = await _service.GetAll();
            return View(allProducers);
        }

        //GET: producers/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _service.Get(id);

            if (producerDetails == null)
            {
                return View("Not Found!");
            }

            return View(producerDetails);
        }

        //Get producers/create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //POST: add producer
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProfilePictureURL,FullName,Bio")] Producer newProducer)
        {
            if (!ModelState.IsValid)
            {
                return View(newProducer);
            }

            await _service.Add(newProducer);

            return RedirectToAction(nameof(Index));
        }

        //Get producers/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id);
        }

        //POST: Edit producer
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProfilePictureURL,FullName,Bio")] Producer newProducer)
        {
            if (!ModelState.IsValid)
            {
                return View(newProducer);
            }

            await _service.Update(newProducer);

            return RedirectToAction(nameof(Index));
        }

        //Get producers/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //POST: Delete producer
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = _service.Get(id);

            if (producerDetails == null)
            {
                return View("Not Found!");
            }

            await _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
