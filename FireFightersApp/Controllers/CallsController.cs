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
using FireFightersApp.Authorization;

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
            var calls = from i in Context.Call select i;
            
            var isDispatcher = User.IsInRole(Constants.CallDispatcherRole);
            var isAdmin = User.IsInRole(Constants.CallAdminRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isDispatcher && !isAdmin)
            {
                return Context.Call != null ?
                          View(await Context.Call.Where(i => i.CallerId == currentUserId).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Call'  is null.");
            }

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

            var isCreator = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Read);

            var isDispatcher = User.IsInRole(Constants.CallDispatcherRole);

            if (!isCreator.Succeeded && !isDispatcher)
            {
                return Forbid();
            }

            return View(call);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, CallStatus status)
        {
            Call call = await Context.Call.FindAsync(id);

            if (call == null)
            {
                return NotFound();
            }

            var callOperation = status == CallStatus.Assigned ? CallOperations.Assigned : CallOperations.Completed;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, callOperation);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            call.Status = status;
            Context.Call.Update(call);

            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
            call.CallerId = UserManager.GetUserId(User);
            call.CallDatetime = DateTime.Now;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Create);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            
            Context.Add(call);
            await Context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
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

            var callTemp = await Context.Call.AsNoTracking().SingleOrDefaultAsync(m => m.CallId == id);

            if (callTemp == null)
            {
                return NotFound();
            }

            call.CallerId = callTemp.CallerId;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
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

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
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

            if (call == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, call, CallOperations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Call.Remove(call);            
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallExists(int id)
        {
          return (Context.Call?.Any(e => e.CallId == id)).GetValueOrDefault();
        }
    }
}
