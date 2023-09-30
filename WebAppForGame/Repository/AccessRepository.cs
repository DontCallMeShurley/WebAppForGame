using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using WebAppForGame.Data;

namespace WebAppForGame.Repository;

public class AccessRepository
{
    private readonly ApplicationDbContext _context;

    public AccessRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GetPassword()
    {
        var setting = await _context.Settings.FirstAsync();
        return setting.AdminPassword ?? ""; 
    }
}