
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_LAB1.Models;
using SSD_LAB1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Supervisor,Employee")]
public class CompanyContractsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompanyContractsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: COMPANYCONTRACTS
    public async Task<IActionResult> Index()
    {
        return View(await _context.CompanyContracts
            .Include(c => c.Company)
            .ToListAsync());
    }

    // GET: COMPANYCONTRACTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontract = await _context.CompanyContracts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companycontract == null)
        {
            return NotFound();
        }

        return View(companycontract);
    }

    // GET: COMPANYCONTRACTS/Create
    [Authorize(Roles = "Supervisor")]
    public IActionResult Create()
    {
        PopulateCompanies();
        return View();
    }

    // POST: COMPANYCONTRACTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Create([Bind("Id,ContractNumber,StartDate,EndDate,ContractValue,Status,CompanyId,Company")] CompanyContract companycontract)
    {
        if (ModelState.IsValid)
        {
            _context.Add(companycontract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        PopulateCompanies(companycontract.CompanyId);
        return View(companycontract);
    }

    // GET: COMPANYCONTRACTS/Edit/5
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontract = await _context.CompanyContracts.FindAsync(id);
        if (companycontract == null)
        {
            return NotFound();
        }
        PopulateCompanies(companycontract.CompanyId);
        return View(companycontract);

    }

    // POST: COMPANYCONTRACTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize(Roles = "Supervisor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,ContractNumber,StartDate,EndDate,ContractValue,Status,CompanyId,Company")] CompanyContract companycontract)
    {
        if (id != companycontract.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(companycontract);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyContractExists(companycontract.Id))
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
        PopulateCompanies(companycontract.CompanyId);
        return View(companycontract);
    }

    // GET: COMPANYCONTRACTS/Delete/5
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontract = await _context.CompanyContracts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companycontract == null)
        {
            return NotFound();
        }

        return View(companycontract);
    }

    // POST: COMPANYCONTRACTS/Delete/5
    [Authorize(Roles = "Supervisor")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var companycontract = await _context.CompanyContracts.FindAsync(id);
        if (companycontract != null)
        {
            _context.CompanyContracts.Remove(companycontract);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CompanyContractExists(int? id)
    {
        return _context.CompanyContracts.Any(e => e.Id == id);
    }
    private void PopulateCompanies(int? selectedCompanyId = null)
    {
        ViewData["CompanyId"] = new SelectList(
            _context.Companies.OrderBy(c => c.Name).ToList(),
            "Id",
            "Name",
            selectedCompanyId);
    }
}
