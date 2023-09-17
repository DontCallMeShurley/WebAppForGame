using DevExpress.Xpo;
using EFCoreDockerMySQL;
using WebAppForGame.Data;
using WebAppForGame.ViewModels;

namespace WebAppForGame.Repository;

public class ProductsRepository
{
    private readonly ApplicationDbContext _context;

    public ProductsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Products> Get()
    {
        return _context.Products.ToList();
    }

    public async Task<Products> GetById(long id)
    {
        return await _context.Products.FirstAsync(x => x.Id == id);
    }
}