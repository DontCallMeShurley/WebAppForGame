using DevExpress.Xpo;
using EFCoreDockerMySQL;
using WebAppForGame.Data;

namespace WebAppForGame.Repository;

public class PaymentsRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Payments> Get()
    {
        return _context.Payments.ToList();
    }

    public async Task<Payments> GetById(Guid id)
    {
        return await _context.Payments.FirstAsync(x => x.Id == id);
    }
}