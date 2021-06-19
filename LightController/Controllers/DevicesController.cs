using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LightController.Data;
using LightController.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightController.Controllers
{
    public class DevicesController : Controller
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public DevicesController(AppDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        
        
        
 
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);


            var devices = await _dbContext.Devices.Include(x=>x.Status).Where(x => x.Owner == user && x.Registered == true).ToListAsync();
            
            
            var statuses = await _dbContext.Statuses.ToListAsync();


            var model = new List<DeviceListModel>();

            foreach (var device in devices)
            {
                model.Add(new DeviceListModel()
                {
                    Id = device.Id,
                    Name = device.Name,
                    Status = device.Status.Name
                });
            }
            

            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Enable(int id)
        {
            
            await ChangeDeviceStatus(id, 2);
            
            /* Tutaj coś powinno wysyłać informacje do urządzenia */

            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Disable(int id)
        {

            await ChangeDeviceStatus(id, 1);
            
            /* Tutaj coś powinno wysyłać informacje do urządzenia */

            return RedirectToAction("Index");
        }


        private async Task ChangeDeviceStatus(int id, int status)
        {
            ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);

            var device = await _dbContext.Devices.FirstAsync(x => x.Id == id && x.Owner == user);
            device.StatusId = status;

            await _dbContext.SaveChangesAsync();
        }

        
        [Authorize]
        public async Task<IActionResult> Info(int id)
        {
            var user = await _userManager.GetUserAsync(this.User);
            
            var device = await _dbContext.Devices.FirstAsync(x => x.Id == id && x.Owner == user);

            ViewBag.Title = device.Name;
            
            
            
            return View();
        }
        
    }
}