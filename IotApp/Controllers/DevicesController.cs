using IotApp.Data;
using IotApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IotApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DevicesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve all users asynchronously
            var users = await _context.Users.ToListAsync();

            // Create a SelectList with the users, setting the currently logged-in user as the selected item
            ViewBag.Users = new SelectList(users, "Id", "UserName", userId);
            var devices = await _context.Devices
                .ToListAsync();
            return View(devices);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve all users asynchronously
            var users = await _context.Users.ToListAsync();

            // Create a SelectList with the users, setting the currently logged-in user as the selected item
            ViewBag.Users = new SelectList(users, "Id", "UserName", userId);
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,UserId")] Device device)
        {

            // Verificați dacă modelul este valid
            if (ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Dacă modelul nu este valid, pregătiți din nou datele pentru formular
            var userId = _userManager.GetUserId(User);

            // Retrieve all users asynchronously
            var users = await _context.Users.ToListAsync();

            // Create a SelectList with the users, setting the currently logged-in user as the selected item
            ViewBag.Users = new SelectList(users, "Id", "UserName", userId);
            return View(device);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = _userManager.GetUserId(User);

            // Retrieve all users asynchronously
            var users = await _context.Users.ToListAsync();

            // Create a SelectList with the users, setting the currently logged-in user as the selected item
            ViewBag.Users = new SelectList(users, "Id", "UserName", userId);
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,UserId")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }
        [Authorize]
        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}

