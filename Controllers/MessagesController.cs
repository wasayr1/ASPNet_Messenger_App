using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRMessenger.Data;
using SignalRMessenger.Models;
using SignalRMessenger.ViewModels;
using System.Security.Claims;

namespace SignalRMessenger.Controllers
{
    [Authorize] // Ensures only logged-in users can access this controller
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor to get the database context and user manager via dependency injection
        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Messages
        // This action shows a list of all users you can chat with.
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _context.Users
                                    .Where(u => u.Id != currentUserId) // Don't show the current user in the list
                                    .ToListAsync();
            return View(users);
        }

        // GET: /Messages/Chat/{receiverId}
        // This action shows the chat history with a specific user.
        public async Task<IActionResult> Chat(string receiverId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null)
            {
                return NotFound();
            }

            // Fetch messages between the current user and the receiver
            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var viewModel = new ChatViewModel
            {
                Receiver = receiver,
                Messages = messages
            };

            return View(viewModel);
        }

        
    }
}