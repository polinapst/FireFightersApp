using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FireFightersApp.Data;
using FireFightersApp.Models;
using Microsoft.AspNetCore.Authorization;
using FireFightersApp.Controllers;
using Microsoft.AspNetCore.Identity;

namespace FireFightersApp.Views.Calls
{
    public class CallsController : DI_BaseController
    {
        public CallsController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
            
        }

        // GET: Calls
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
              return Context.Call != null ? 
                          View(await Context.Call.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Call'  is null.");
        }

        // GET: Calls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || Context.Call == null)
            {
                return NotFound();
            }

            var call = await Context.Call
                .FirstOrDefaultAsync(m => m.CallId == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // GET: Calls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Calls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CallId,CallerId,Address,CallDatetime,IsCompleted")] Call call)
        {
            if (ModelState.IsValid)
            {
                Context.Add(call);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: Calls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || Context.Call == null)
            {
                return NotFound();
            }

            var call = await Context.Call.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }
            return View(call);
        }

        // POST: Calls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CallId,CallerId,Address,CallDatetime,IsCompleted")] Call call)
        {
            if (id != call.CallId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(call);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.CallId))
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
            return View(call);
        }

        // GET: Calls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || Context.Call == null)
            {
                return NotFound();
            }

            var call = await Context.Call
                .FirstOrDefaultAsync(m => m.CallId == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: Calls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Context.Call == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Call'  is null.");
            }
            var call = await Context.Call.FindAsync(id);
            if (call != null)
            {
                Context.Call.Remove(call);
            }
            
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallExists(int id)
        {
          return (Context.Call?.Any(e => e.CallId == id)).GetValueOrDefault();
        }
    }
}
