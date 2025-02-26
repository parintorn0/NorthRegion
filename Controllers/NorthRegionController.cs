using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        private readonly IWebHostEnvironment environment;

        // public NorthRegionController(ILogger<NorthRegionController> logger)
        // {
        //     _logger = logger;
        // }

        public NorthRegionController(NorthRegionDbContext northRegionDbContext,IWebHostEnvironment environment){
            _context = northRegionDbContext;
            this.environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var northRegionList = await _context.NorthRegion.ToListAsync();
            northRegionList = northRegionList.OrderByDescending(x => x.Id).ToList();
            foreach(var northRegion in northRegionList){
                if(northRegion.ImageFileName == ""|northRegion.ImageFileName==null){
                    northRegion.ImageFileName = "nodata.png";
                }
            }
            return View(northRegionList);
        }

        [HttpGet] //show blank form
        public IActionResult Create(){
            return View();
        }
        [HttpPost] //filled form
        public async Task<IActionResult> Create(NorthRegionViewModel addNorthRegionViewModel, IFormFile ImageFile){
            try{
                string? strImageFile = "nodata.png";
                if(ImageFile!=null){
                    string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    strImageFile = strDateTime + "_" + ImageFile.FileName;
                    string? PhotoFullPath = this.environment.WebRootPath + "/images/" + strImageFile;
                    using(var fileStream = new FileStream(PhotoFullPath, FileMode.Create)){
                        await ImageFile.CopyToAsync(fileStream);
                    }
                }
                NorthRegionViewModel northRegionViewModel = new NorthRegionViewModel() {
                    Id = addNorthRegionViewModel.Id,
                    Name = addNorthRegionViewModel.Name,
                    Price = addNorthRegionViewModel.Price,
                    ExpiredDate = DateTime.Now.AddDays(7).ToString("dd-MM-yyyy"),
                    ImageFileName = strImageFile,
                    Source = addNorthRegionViewModel.Source,   
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
                TempData["PhotoFilePath"] = "/images/"+northRegion.ImageFileName;
                if(northRegion.Source != null){
                    TempData["Source"] = northRegion.Source?.ToString();
                }else{
                    TempData["Source"] = "Unknown";
                }
                return View(northRegion);
            }
            catch (Exception ex){
                TempData["errorMessage"] = ex.Message + "<br/>" + ex.StackTrace;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(NorthRegionViewModel northRegionViewModel, IFormFile ImageFile) {
            try {
                var northRegion = await _context.NorthRegion.SingleOrDefaultAsync(n => n.Id == northRegionViewModel.Id);
                if (northRegion == null) {
                    return View("No data");
                }
                else {
                    northRegion.Name = northRegionViewModel.Name;
                    northRegion.Description = northRegionViewModel.Description;
                    northRegion.Price = northRegionViewModel.Price;
                    //for photo
                    if(ImageFile!=null){
                        string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                        string strImageFile = strDateTime + "_" + ImageFile.FileName;
                        string? PhotoFullPath = this.environment.WebRootPath + "/images/" + strImageFile;
                        using(var fileStream = new FileStream(PhotoFullPath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }
                        northRegion.ImageFileName = strImageFile;//use new photo file data
                    }else{
                        northRegion.ImageFileName = northRegionViewModel.ImageFileName;//use existing img file data
                    }
                    northRegion.Source = northRegionViewModel.Source; //Source here for order of column
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
                TempData["PhotoFilePath"] = "/images/"+northRegion.ImageFileName;
                TempData["Source"]=northRegion?.ToString();
                if(northRegion.Source != null){
                    TempData["Source"] = northRegion.Source?.ToString();
                }else{
                    TempData["Source"] = "Unknown";
                }
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