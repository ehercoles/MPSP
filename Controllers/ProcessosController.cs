using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPSP.Data;
using MPSP.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MPSP.Controllers
{
    public class ProcessosController : Controller
    {
        private readonly ProcessoContext _context;

        public ProcessosController(ProcessoContext context)
        {
            _context = context;
        }

        // GET: Processos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Processo.ToListAsync());
        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            List<int> processoParteIds = _context.ProcessoParte.Where(x => x.ProcessoId == processo.Id).Select(x => x.ParteId).ToList();
            processo.Partes = _context.Parte.Where(x => processoParteIds.Contains(x.Id)).ToList();

            return View(processo);
        }

        // GET: Processos/Create
        public IActionResult Create()
        {
            Processo processo = new Processo();
            processo.Partes = new List<Parte>();
            processo.Partes.Add(new Parte());

            return View(processo);
        }

        // POST: Processos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroPrefixo,Numero,Data,TipoProcesso,FormFile,Observacao,Sigiloso,Partes")] Processo processo)
        {
            if (ModelState.IsValid)
            {
                await SalvarDocumento(processo);

                _context.Add(processo);
                await _context.SaveChangesAsync();

                HistoricoProcesso hp = new HistoricoProcesso(processo);
                _context.Add(hp);
                await _context.SaveChangesAsync();

                await SalvarPartes(processo, processo.Partes);

                return RedirectToAction(nameof(Index));
            }

            return View(processo);
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            List<int> processoParteIds = _context.ProcessoParte.Where(x => x.ProcessoId == processo.Id).Select(x => x.ParteId).ToList();
            processo.Partes = new List<Parte>();
            processo.Partes.Add(new Parte());
            processo.Partes.AddRange(_context.Parte.Where(x => processoParteIds.Contains(x.Id)).ToList());

            return View(processo);
        }

        // POST: Processos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroPrefixo,Numero,Data,TipoProcesso,FormFile,Documento,ExtensaoDocumento,Observacao,Sigiloso,Partes")] Processo processo)
        {
            if (id != processo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await SalvarDocumento(processo);
                    await SalvarPartes(processo, processo.Partes);

                    HistoricoProcesso hp = new HistoricoProcesso(processo);
                    _context.Add(hp);
                    _context.Update(processo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExists(processo.Id))
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
            return View(processo);
        }

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var processo = await _context.Processo.FindAsync(id);
            _context.Processo.Remove(processo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoExists(int id)
        {
            return _context.Processo.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SalvarDocumento(Processo processo)
        {
            if (processo.FormFile == null)
            {
                return Ok();
            }

            string nome = Path.GetFileName(processo.FormFile.FileName);
            processo.ExtensaoDocumento = Path.GetExtension(nome).Trim('.').ToLowerInvariant();

            using (var stream = new MemoryStream())
            {
                await processo.FormFile.CopyToAsync(stream);
                processo.Documento = stream.ToArray();
            }

            return Ok();
        }

        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processo = await _context.Processo.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }

            return File(processo.Documento, "application/octet-stream", processo.NumeroProcesso + "." + processo.ExtensaoDocumento);
        }

        public async Task<IActionResult> SalvarPartes(Processo processo, List<Parte> partes)
        {
            var processoPartes = _context.ProcessoParte.Where(x => x.ProcessoId == processo.Id);
            _context.ProcessoParte.RemoveRange(processoPartes);

            if (processo.Sigiloso)
            {
                partes.RemoveAt(0);

                foreach (Parte parte in partes)
                {
                    parte.Cpf = Util.RemoverMascaraCpf(parte.Cpf);

                    if (_context.Parte.Any(x => x.Cpf == parte.Cpf))
                    {
                        var entidade = _context.Parte.Attach(parte);
                        entidade.State = EntityState.Modified;
                    }
                    else
                    {
                        _context.Add(parte);
                    }
                }
                await _context.SaveChangesAsync();

                foreach (Parte parte in partes)
                {
                    ProcessoParte processoParte = new ProcessoParte();
                    processoParte.ProcessoId = processo.Id;
                    processoParte.ParteId = parte.Id;
                    _context.Add(processoParte);
                }
            }
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
