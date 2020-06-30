using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicApi.Models;

namespace MagicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardItemsController : ControllerBase
    {
        private readonly CardContext _context;

        public CardItemsController(CardContext context)
        {
            _context = context;
        }

        // GET: api/CardItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardItem>>> GetCardItems()
        {
            return await _context.CardItems.ToListAsync();
        }

        // GET: api/CardItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardItem>> GetCardItem(long id)
        {
            var cardItem = await _context.CardItems.FindAsync(id);

            if (cardItem == null)
            {
                return NotFound();
            }

            return cardItem;
        }

        // PUT: api/CardItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardItem(long id, CardItem cardItem)
        {
            if (id != cardItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(cardItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardItemExists(id))
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

        // POST: api/CardItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CardItem>> PostCardItem(CardItem cardItem)
        {
            _context.CardItems.Add(cardItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetCardItem", new { id = cardItem.Id }, cardItem);
            return CreatedAtAction(nameof(GetCardItem), new { id = cardItem.Id }, cardItem);
        }

        // DELETE: api/CardItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CardItem>> DeleteCardItem(long id)
        {
            var cardItem = await _context.CardItems.FindAsync(id);
            if (cardItem == null)
            {
                return NotFound();
            }

            _context.CardItems.Remove(cardItem);
            await _context.SaveChangesAsync();

            return cardItem;
        }

        private bool CardItemExists(long id)
        {
            return _context.CardItems.Any(e => e.Id == id);
        }
    }
}
