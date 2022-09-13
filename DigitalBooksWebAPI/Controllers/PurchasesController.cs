﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DigitalBooksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly DigitalBooksContext _context;

        public PurchasesController(DigitalBooksContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        [HttpGet]
        [Route("BookHistory")]
        public List<Purchase> GetPurchaseData(string emailId)
        {
            var purchases =  _context.Purchases.Where(p=> p.EmailId==emailId).ToList();
            return purchases;
        }

        

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
          if (_context.Purchases == null)
          {
              return Problem("Entity set 'DigitalBooksContext.Purchases'  is null.");
          }
            _context.Purchases.Add(purchase);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseExists(purchase.PurchaseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(int id)
        {
            return (_context.Purchases?.Any(e => e.PurchaseId == id)).GetValueOrDefault();
        }
    }
}
