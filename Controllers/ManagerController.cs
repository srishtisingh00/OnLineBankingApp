using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using OnLineBankingApp.Models;

namespace OnLineBankingApp.Controllers
{
    public class ManagerController : Controller
    {
        // GET: ManagerController
               
        private readonly AppDBContext _context;

        public ManagerController(AppDBContext context)
        {
            _context = context;
        }
    

        public async Task<IActionResult> Index()
        {

            int ?id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            //Customer customer =await _context.Customers.FirstOrDefaultAsync(c=>c.CustomerID==id);
            if (HttpContext.Session.GetInt32("CustomerID") == null)
            {
                return RedirectToAction(nameof(Index), "OnLineBankingPortal");

            }


            var customer = await _context.Customers
               .Include(c => c.Gender)
               .Include(c => c.RoleType)
               .Include(c => c.Status)
               .FirstOrDefaultAsync(m => m.CustomerID == id);

            return View(customer);
        }

        // GET: ManagerController/Details/5
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Gender)
                .Include(c => c.RoleType)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> CustomerStatusUpdate()
         
        {
            var appDBContext = _context.Customers.Include(c => c.Gender).Include(c => c.RoleType).Include(c => c.Status);
            return View(await appDBContext.ToListAsync());
        }

        //ADD ACCOUNT
        // GET: Accounts/Create
        public async Task<IActionResult> AddAccount(int? id)
        {

            Account account = await _context.Accounts.FirstOrDefaultAsync(c => c.CustomerID == id);
            if (account!=null)
            {
                ViewBag.Account = account;
                ViewData["AccountStatus"] = $"Account Number Already Generated : {account.AccountId}";


            }
            else
            {
                ViewData["CustomerID"] = new SelectList(_context.Customers.Where(c => c.CustomerID == id), "CustomerID", "CustomerName");

                ViewData["Id"] = new SelectList(_context.AccountType, "Id", "AccountTypeName");
            }

          
           

            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccount([Bind("AccountId,CustomerID,Id,Balance,DateOfOpening")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();

                Account acc = await _context.Accounts.FirstOrDefaultAsync(a => a.CustomerID == account.CustomerID);
                Customer cust = await _context.Customers.FirstOrDefaultAsync(c=>c.CustomerID== account.CustomerID);


                Beneficiary beneficiary = new Beneficiary()
                {
                    BeneficiaryName = "SELF",

                    CustomerID = account.CustomerID,
                    BeneficiaryAccountNumber = acc.AccountId,
                    Phone = cust.Phone,
                    Email = cust.Email,
                    BankName = "SELF",
                    IFSC = "AAAA0123456",
                    Branch = "SELF"
                  };
                _context.Beneficiaries.Add(beneficiary);
                await _context.SaveChangesAsync();
                    

                return RedirectToAction(nameof(CustomerStatusUpdate));


            }






            ViewData["CustomerID"] = new SelectList(_context.Customers.Where(c => c.CustomerID == account.CustomerID), "CustomerID", "CustomerName", account.CustomerID);
            ViewData["Id"] = new SelectList(_context.AccountType, "Id", "AccountTypeName");

            return View(account);
        }


















        // GET: ManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManagerController/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderType", customer.GenderId);
            ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName", customer.RoleTypeId);
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", customer.StatusID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,CustomerName,RoleTypeId,GenderId,DateOfBirth,PAN,Email,Address,City,State,PinCode,Phone,StatusID,Password")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerStatusUpdate));
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderType", customer.GenderId);
            ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName", customer.RoleTypeId);
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", customer.StatusID);
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }

        // GET: ManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
