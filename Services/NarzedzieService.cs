using Warsztat.Data;
using Warsztat.Models;
using Microsoft.EntityFrameworkCore;

namespace Warsztat.Services
{
    public class NarzedzieService
    {
        private readonly WarsztatContext _context;

        public NarzedzieService(WarsztatContext context)
        {
            _context = context;
        }

        public async Task<List<Narzedzie>> GetAllAsync()
        {
            return await _context.Narzedzia.ToListAsync();
        }
    }
}
