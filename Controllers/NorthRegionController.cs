using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthRegion.Models;

namespace NorthRegion.Controllers
{
    // [Route("[controller]")]
    public class NorthRegionController : Controller
    {
        private readonly NorthRegionDbContext _context;

        // public NorthRegionController(ILogger<NorthRegionController> logger)
        // {
        //     _logger = logger;
        // }

        public NorthRegionController(NorthRegionDbContext northRegionDbContext){
            _context = northRegionDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var northRegionList = await _context.NorthRegion.ToListAsync();
            return View(northRegionList);
        }

        [HttpGet]
        public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NorthRegionViewModel addNorthRegionViewModel){
            try{
                NorthRegionViewModel northRegionViewModel = new NorthRegionViewModel() {
                    Id = addNorthRegionViewModel.Id,
                    Name = addNorthRegionViewModel.Name,
                    Price = addNorthRegionViewModel.Price
                };
                await _context.AddAsync(northRegionViewModel);
                await _context.SaveChangesAsync();
                TempData["successMessage"] = $"New Product Created ({addNorthRegionViewModel.Name})";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex){
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id){
            try{
                var northRegion = await _context.NorthRegion.SingleOrDefaultAsync(n => n.Id == id);
                return View(northRegion);
            }
            catch (Exception ex){
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NorthRegionViewModel northRegionViewModel) {
            try {
                var northRegion = await _context.NorthRegion.SingleOrDefaultAsync(n => n.Id == northRegionViewModel.Id);
                if (northRegion == null) {
                    return View("No data");
                }
                else {
                    northRegion.Name = northRegionViewModel.Name;
                    northRegion.Description = northRegionViewModel.Description;
                    northRegion.Price = northRegionViewModel.Price;
                    await _context.SaveChangesAsync();
                    TempData["successMessage"] = $"{northRegionViewModel.Name} was Edited";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex) {
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id) {
            try {
                var northRegion = await _context.NorthRegion.SingleOrDefaultAsync(n => n.Id == id);
                return View(northRegion);
            }
            catch(Exception ex) {
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(NorthRegionViewModel northRegionViewModel) {
            try {
                var northRegion = await _context.NorthRegion.SingleOrDefaultAsync(n => n.Id == northRegionViewModel.Id);
                if (northRegion == null) {
                    TempData["errorMessage"] = $"Product Not Found with Id {northRegionViewModel.Id}";
                    return View("No data");
                }
                else {
                    var name = northRegionViewModel.Name;
                    _context.NorthRegion.Remove(northRegion);
                    await _context.SaveChangesAsync();
                    TempData["successMessage"] = $"{name} was Deleted";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception ex) {
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}