using EFCoreDockerMySQL;
using WebAppForGame.ViewModels;

namespace WebAppForGame.Repository;

public class PaymentsRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<PaymentsViewModel> Get()
    {
        // TODO:
        return new List<PaymentsViewModel>();
    }

    public PaymentsViewModel GetById(long id)
    {
        // TODO:
        return null;
    }
}