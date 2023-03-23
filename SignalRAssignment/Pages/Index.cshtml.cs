using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Models.Product> Product { get; set; } = default!;
        public IList<Models.Category> Categories { get; set; } = default!;

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public decimal? priceFrom { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public decimal? priceTo { get; set; }
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public int? cateId { get; set; }

        public async Task OnGetAsync()
        {
            ////Xu ly category
            if(_context.Categories != null)
            {
                Categories = await _context.Categories.ToListAsync();
            }

            if(cateId != null)
            {
                Product = await _context.Products
                       .Include(p => p.Category)
                       .Include(p => p.Supplier)
                       .Where(p => p.CategoryId == cateId)
                       .ToListAsync();
                return;
            }


            if (_context.Products != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var valueSearch = decimal.Parse(SearchString);

                        //Search theo price + proId
                        if (priceFrom != null && priceTo != null)
                        {
                            //Search theo ProID
                            Product = await _context.Products
                                  .Include(p => p.Category)
                                  .Include(p => p.Supplier)
                                  .Where(p => p.UnitPrice == valueSearch || (p.ProductId == (int)valueSearch
                                    && p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo))
                                  .ToListAsync();
                        }
                        else
                        {
                            //Search theo ProID
                            Product = await _context.Products
                                  .Include(p => p.Category)
                                  .Include(p => p.Supplier)
                                  .Where(p => p.UnitPrice == valueSearch || p.ProductId == (int)valueSearch)
                                  .ToListAsync();
                        }

                    }
                    catch (Exception)
                    {
                        if (priceFrom != null && priceTo != null)
                        {
                            //Search theo Product Name + Price
                            Product = await _context.Products
                                   .Include(p => p.Category)
                                   .Include(p => p.Supplier)
                                   .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim())
                                        && p.UnitPrice >= priceFrom && p.UnitPrice <= priceTo)
                                   .ToListAsync();
                        }
                        else
                        {
                            //Search theo Product Name
                            Product = await _context.Products
                                   .Include(p => p.Category)
                                   .Include(p => p.Supplier)
                                   .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim()))
                                   .ToListAsync();
                        }
                        
                    }
                }
                else
                {
                    Product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier)
                        .ToListAsync();
                }
            }
        }

    }
}
