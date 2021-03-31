using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AppMvc.Models;

namespace AppMvc.Controllers
{
    public class AlunosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		[HttpGet]
        [AllowAnonymous]
		[Route(template: "listar-alunos")]
		public async Task<ActionResult> Index()
		{
			return View(await db.Alunos.ToListAsync());
		}

		[HttpGet]
        [Route(template:"aluno-detalhe/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            var aluno = await db.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return HttpNotFound();
            }

            return View(aluno);
        }

        [HttpGet]
        [Route(template:"novo-aluno")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route(template: "novo-aluno")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Email,Descricao,CPF,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                aluno.DataMatricula = DateTime.Now;
                db.Alunos.Add(aluno);
                await db.SaveChangesAsync();

                TempData["Mensagem"] = "Aluno cadastrado com sucesso!";

                return RedirectToAction("Index");
            }

            return View(aluno);
        }

        [HttpGet]
        [Route(template:"editar-aluno/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            var aluno = await db.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return HttpNotFound();
            }

            return View(aluno);
        }

        [HttpPost]
        [Route(template: "editar-aluno/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nome,Email,Descricao,CPF,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aluno).State = EntityState.Modified;
                db.Entry(aluno).Property(a => a.DataMatricula).IsModified = false;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Mensagem = "Não esqueça que esta ação é irrervesível";

            return View(aluno);
        }

        [HttpPost]
        [Route(template:"excluir-aluno/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var aluno = await db.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return HttpNotFound();
            }

            return View(aluno);
        }

        [HttpPost]
        [Route(template:"excluir-aluno/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var aluno = await db.Alunos.FindAsync(id);
            db.Alunos.Remove(aluno);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
