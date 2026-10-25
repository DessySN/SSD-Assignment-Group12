using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_LAB1.Data;
using SSD_LAB1.Models;

namespace SSD_LAB1.Controllers
{
    public class MessageBoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageBoardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Anyone can view the message board.
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = new MessageBoardViewModel
            {
                Messages = await LoadMessages()
            };

            return View(model);
        }

        // Only authenticated users can post messages.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageBoardViewModel model)
        {
            if (model.NewMessage == null)
            {
                model.NewMessage = new CreateMessageViewModel();
            }

            if (!ModelState.IsValid)
            {
                model.Messages = await LoadMessages();
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var content = model.NewMessage.Content.Trim();

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError(
                    "NewMessage.Content",
                    "Please enter a message.");

                model.Messages = await LoadMessages();
                return View("Index", model);
            }

            var message = new Message
            {
                Content = content,
                PostedAt = DateTime.UtcNow,
                ApplicationUserId = user.Id
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<Message>> LoadMessages()
        {
            return await _context.Messages
                .Include(m => m.ApplicationUser)
                .OrderByDescending(m => m.PostedAt)
                .ToListAsync();
        }
    }
}