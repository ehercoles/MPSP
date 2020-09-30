using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MPSP.Data;
using MPSP.Models;

namespace MPSP.Controllers
{
    public class PartesController : Controller
    {
        private readonly ProcessoContext _context;

        public PartesController(ProcessoContext context)
        {
            _context = context;
        }

        // GET: Partes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parte.ToListAsync());
        }

        // GET: Partes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Parte = await _context.Parte
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Parte == null)
            {
                return NotFound();
            }

            return View(Parte);
        }

        // GET: Partes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Partes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cpf,Nome,Sexo")] Parte Parte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Parte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Parte);
        }

        // GET: Partes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Parte = await _context.Parte.FindAsync(id);
            if (Parte == null)
            {
                return NotFound();
            }
            return View(Parte);
        }

        // POST: Partes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cpf,Nome,Sexo")] Parte Parte)
        {
            if (id != Parte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Parte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParteExists(Parte.Id))
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
            return View(Parte);
        }

        // GET: Partes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Parte = await _context.Parte
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Parte == null)
            {
                return NotFound();
            }

            return View(Parte);
        }

        // POST: Partes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Parte = await _context.Parte.FindAsync(id);
            _context.Parte.Remove(Parte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParteExists(int id)
        {
            return _context.Parte.Any(e => e.Id == id);
        }
    }
}
