using Warsztat.Data;
using Warsztat.Models;
using Microsoft.EntityFrameworkCore;

namespace Warsztat.Services
{
    public class MebelService
    {
        private readonly WarsztatContext _context;

        public MebelService(WarsztatContext context)
        {
            _context = context;
        }

        public async Task<List<Mebel>> GetAllAsync()
        {
            return await _context.Meble.Include(m => m.Narzedzia).ToListAsync();
        }
    }
}
