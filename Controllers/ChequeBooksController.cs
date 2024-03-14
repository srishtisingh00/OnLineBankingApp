using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnLineBankingApp.Models;

namespace OnLineBankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChequeBooksController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ChequeBooksController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/ChequeBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChequeBook>>> GetChequeboooks()
        {
            return await _context.Chequeboooks.ToListAsync();
        }

        // GET: api/ChequeBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChequeBook>> GetChequeBook(int id)
        {
            var chequeBook = await _context.Chequeboooks.FindAsync(id);

            if (chequeBook == null)
            {
                return NotFound();
            }

            return chequeBook;
        }

        // PUT: api/ChequeBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChequeBook(int id, ChequeBook chequeBook)
        {
            if (id != chequeBook.ChqId)
            {
                return BadRequest();
            }

            _context.Entry(chequeBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChequeBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChequeBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChequeBook>> PostChequeBook(ChequeBook chequeBook)
        {
            _context.Chequeboooks.Add(chequeBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChequeBook", new { id = chequeBook.ChqId }, chequeBook);
        }

        // DELETE: api/ChequeBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChequeBook(int id)
        {
            var chequeBook = await _context.Chequeboooks.FindAsync(id);
            if (chequeBook == null)
            {
                return NotFound();
            }

            _context.Chequeboooks.Remove(chequeBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChequeBookExists(int id)
        {
            return _context.Chequeboooks.Any(e => e.ChqId == id);
        }
    }
}
