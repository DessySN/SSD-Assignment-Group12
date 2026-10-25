
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD_LAB1.Models;
using SSD_LAB1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles ="Supervisor,Employee")]
public class CompanyContactsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompanyContactsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: COMPANYCONTACTS
    public async Task<IActionResult> Index()
    {
        return View(await _context.CompanyContacts
            .Include(c => c.Company)
            .ToListAsync());
    }

    // GET: COMPANYCONTACTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontact = await _context.CompanyContacts
             .Include(m => m.Company)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companycontact == null)
        {
            return NotFound();
        }

        return View(companycontact);
    }

    // GET: COMPANYCONTACTS/Create
    [Authorize(Roles = "Supervisor")]
    public IActionResult Create()
    {
        PopulateCompanies();
        return View();
    }

    // POST: COMPANYCONTACTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,JobTitle,Email,PhoneNumber,CompanyId,Company")] CompanyContact companycontact)
    {
        if (ModelState.IsValid)
        {
            _context.Add(companycontact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        PopulateCompanies(companycontact.CompanyId);
        return View(companycontact);
    }

    // GET: COMPANYCONTACTS/Edit/5
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontact = await _context.CompanyContacts.FindAsync(id);
        if (companycontact == null)
        {
            return NotFound();
        }
        PopulateCompanies(companycontact.CompanyId);
        return View(companycontact);
    }

    // POST: COMPANYCONTACTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize(Roles = "Supervisor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FirstName,LastName,JobTitle,Email,PhoneNumber,CompanyId,Company")] CompanyContact companycontact)
    {
        if (id != companycontact.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(companycontact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyContactExists(companycontact.Id))
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
        PopulateCompanies(companycontact.CompanyId);
        return View(companycontact);
    }

    // GET: COMPANYCONTACTS/Delete/5
    [Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var companycontact = await _context.CompanyContacts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (companycontact == null)
        {
            return NotFound();
        }

        return View(companycontact);
    }

    // POST: COMPANYCONTACTS/Delete/5
    [Authorize(Roles = "Supervisor")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var companycontact = await _context.CompanyContacts.FindAsync(id);
        if (companycontact != null)
        {
            _context.CompanyContacts.Remove(companycontact);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CompanyContactExists(int? id)
    {
        return _context.CompanyContacts.Any(e => e.Id == id);
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
