﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnLineBankingApp.Models;

namespace OnLineBankingApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDBContext _context;

        public TransactionsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Transactions.Include(t => t.Accounts).Include(t => t.Beneficiary).Include(t => t.Status);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Accounts)
                .Include(t => t.Beneficiary)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId");
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries, "BeneficiaryID", "BankName");
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionID,AccountId,BeneficiaryId,OpeningBalance,Amount,ClosingBalance,TraId,StatusID,Description,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries, "BeneficiaryID", "BankName", transaction.BeneficiaryId);
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", transaction.StatusID);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries, "BeneficiaryID", "BankName", transaction.BeneficiaryId);
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", transaction.StatusID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionID,AccountId,BeneficiaryId,OpeningBalance,Amount,ClosingBalance,TraId,StatusID,Description,Date")] Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionID))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", transaction.AccountId);
            ViewData["BeneficiaryId"] = new SelectList(_context.Beneficiaries, "BeneficiaryID", "BankName", transaction.BeneficiaryId);
            ViewData["StatusID"] = new SelectList(_context.Statuss, "StatusID", "StatusType", transaction.StatusID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Accounts)
                .Include(t => t.Beneficiary)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'AppDBContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return _context.Transactions.Any(e => e.TransactionID == id);
        }
    }
}