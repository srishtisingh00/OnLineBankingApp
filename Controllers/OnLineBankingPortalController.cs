using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnLineBankingApp.Models;

namespace OnLineBankingApp.Controllers
{
    public class OnLineBankingPortalController : Controller
    {
        // GET: OnLineBankingPortalController
        private readonly AppDBContext _context;

        public OnLineBankingPortalController(AppDBContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("CustomerID,CustomerName,RoleTypeId,GenderId,DateOfBirth,PAN,Email,Address,City,State,PinCode,Phone,StatusID,Password")] Customer customer)
        {
            Customer cust =await _context.Customers.FirstOrDefaultAsync(c=>c.Email==customer.Email && c.Password == customer.Password);

            HttpContext.Session.SetInt32("CustomerID", cust.CustomerID);

            if (cust!=null)
            {
                if (cust.RoleTypeId==1 && cust.StatusID==2)
                {
                    return RedirectToAction(nameof(Index), "Manager");

                }
                if (cust.RoleTypeId == 2 && cust.StatusID == 2)
                {

                    return RedirectToAction(nameof(DashBoard));
                }


            }


            ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName", customer.RoleTypeId);
            ViewData["Message"] = "Invalid User name or password or Account has not been approved";

            return View(customer);
        }


        public async Task<IActionResult> DashBoard()
        {
            
          int ?id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            //Customer customer =await _context.Customers.FirstOrDefaultAsync(c=>c.CustomerID==id);
            if (HttpContext.Session.GetInt32("CustomerID") == null)
            {
                return RedirectToAction(nameof(Index));

            }

            var customer = await _context.Customers
               .Include(c => c.Gender)
               .Include(c => c.RoleType)
               .Include(c => c.Status)
               .FirstOrDefaultAsync(m => m.CustomerID == id);

            return View(customer);
        }


        // GET: Accounts/Details/5
        public async Task<IActionResult> AccountDetails()
        {
            int ?id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));

            var appDBContext = _context.Accounts.Include(a => a.Customer).Where(c=>c.CustomerID==id);
            return View(await appDBContext.ToListAsync());

            
        }


        // GET: Accounts/Edit/5
        public async Task<IActionResult> AddAmount(int? id)
        {
            int? cuid = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers.Where(c=>c.CustomerID==cuid), "CustomerID", "CustomerName", account.CustomerID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind("AccountId,CustomerID,Id,Balance,DateOfOpening")] Account account
        public async Task<IActionResult> AddAmount(int id,AccountTopup accounttu)
        {
            if (id != accounttu.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int? cuid = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
                    Account account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accounttu.AccountId);

                    account.Balance = account.Balance + accounttu.AddAmount;
                    _context.Update(account);

                    Beneficiary beneficiary = await _context.Beneficiaries.FirstOrDefaultAsync(b => b.CustomerID == cuid && b.BeneficiaryName == "SELF"); 


                    Transaction transaction = new Transaction()
                    {
                        AccountId = accounttu.AccountId,
                        StatusID = 6,
                        BeneficiaryId = beneficiary.BeneficiaryID,
                        OpeningBalance = accounttu.Balance,
                        Amount = accounttu.AddAmount,

                        ClosingBalance = accounttu.Balance + accounttu.AddAmount,
                        TraId = 2,
                        Description = "Amount Added in Account",
                        Date = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))


                    };
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(accounttu.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AccountDetails));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerName", accounttu.CustomerID);
            return View(accounttu);
        }


        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }












        public async Task<IActionResult> BeneficiariesDetails()
        {
            int? id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            var appDBContext = _context.Beneficiaries.Include(b => b.Customer).Where(c => c.CustomerID == id); 
            return View(await appDBContext.ToListAsync());
           
          

        }
        public async Task<IActionResult> TransactionsDetails()
        {
            int? id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));

            Account acc =await _context.Accounts.FirstOrDefaultAsync(a => a.CustomerID == id);
            var appDBContext = _context.Transactions.Include(t => t.Accounts).Include(t => t.Beneficiary).Include(t => t.Status).Include(t=>t.TransactionType).Where(t=>t.AccountId==acc.AccountId);
            return View(await appDBContext.ToListAsync());



        }


        // GET: Transactions/Create
        public async Task<IActionResult> DoTransactions()
        {
            int? id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            Account account = await _context.Accounts.FirstOrDefaultAsync(a => a.CustomerID == id);     

            ViewData["AccountId"] = new SelectList(_context.Accounts.Where(a=>a.AccountId== account.AccountId), "AccountId", "AccountId");
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries.Where(a => a.CustomerID == id), "BeneficiaryID", "BeneficiaryName");
            ViewData["Balance"] = account.Balance;

            //ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoTransactions([Bind("TransactionID,AccountId,BeneficiaryId,OpeningBalance,Amount,ClosingBalance,TraId,StatusID,Description,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                int? id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
                Account account = await _context.Accounts.FirstOrDefaultAsync(a => a.CustomerID == id);

                if (account.Balance >= transaction.Amount)
                {
                    account.Balance = account.Balance - transaction.Amount;
                    transaction.StatusID = 6;
                    transaction.ClosingBalance = account.Balance;
                }
                else
                {
                    transaction.StatusID = 7;
                    transaction.ClosingBalance = account.Balance;

                }

                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TransactionsDetails));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries, "BeneficiaryID", "BankName", transaction.BeneficiaryId);
            //ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", transaction.StatusID);
            return View(transaction);
        }






        // GET: OnLineBankingPortalController/AddBeneficiaries
        public IActionResult AddBeneficiaries()
        {
            int? id = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            ViewData["CustomerID"] = new SelectList(_context.Customers.Where(b=>b.CustomerID== id), "CustomerID", "CustomerName");
            return View();
        }

        // POST: Beneficiaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBeneficiaries([Bind("BeneficiaryID,BeneficiaryName,CustomerID,BeneficiaryAccountNumber,Phone,Email,BankName,IFSC,Branch")] Beneficiary beneficiary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beneficiary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BeneficiariesDetails));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerName", beneficiary.CustomerID);
            return View(beneficiary);
        }

        // GET: Beneficiaries/Delete/5
        public async Task<IActionResult> BeneficiariesDelete(int? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiary = await _context.Beneficiaries
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(m => m.BeneficiaryID == id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return View(beneficiary);
        }

        // POST: Beneficiaries/BeneficiariesDelete/5
        [HttpPost, ActionName("BeneficiariesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BeneficiariesDeleteConfirmed(int id)
        {
            if (_context.Beneficiaries == null)
            {
                return Problem("Entity set 'AppDBContext.Beneficiaries'  is null.");
            }
            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary != null)
            {
                _context.Beneficiaries.Remove(beneficiary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: OnLineBankingPortalController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            int? cuid = Convert.ToInt32(HttpContext.Session.GetInt32("CustomerID"));
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Customer)
                 .Include(a => a.AccountType)
                .FirstOrDefaultAsync(m => m.AccountId == id);
                 
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        public ActionResult SignUp()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderType");
            //ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName");
            //ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType");
            return View();
        }
        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("CustomerID,CustomerName,RoleTypeId,GenderId,DateOfBirth,PAN,Email,Address,City,State,PinCode,Phone,StatusID,Password")] Customer customer)
        {
           
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderType", customer.GenderId);
            //ViewData["RoleTypeId"] = new SelectList(_context.RoleTypes, "RoleTypeId", "RoleTypeName", customer.RoleTypeId);
            //ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", customer.StatusID);
            return View(customer);
        }




       
      
        // GET: OnLineBankingPortalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnLineBankingPortalController/Create
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

        // GET: OnLineBankingPortalController/Edit/5

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
                return RedirectToAction(nameof(DashBoard));
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




        // GET: OnLineBankingPortalController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OnLineBankingPortalController/Delete/5
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
