using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Data;
using Vets.Models;

namespace Vets.Controllers
{
    public class VeterinariosController : Controller
    {
        /// <summary>
        /// cria uma instacia de acesso à Base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public VeterinariosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Veterinarios
        public async Task<IActionResult> Index()
        {
            /*acesso à base de dados
             * SELECT *
             * FROM Veterinarios
             * e depois estamos a enviar os dados para a View
             */
            return View(await _context.Veterinarios.ToListAsync());
        }

        // GET: Veterinarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // GET: Veterinarios/Create
        /// <summary>
        /// usado para o primeiro acesso à View 'Create' em modo HTTP GET
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// método usado para recuperar os dados enviados pelos utilizadores
        /// do Browser para o servidor
        /// </summary>
        /// <param name="veterinarios"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios, IFormFile fotoVet)
        {
            /*
             *Algoritmo para processar ao ficheiro com a imagem
             *
             *Se ficheiro imagem nulo 
             *   atribuir uma imagem genérica ao veterinário
             * else
             *  será que o ficheiro é uma imagem?
             *  se não for 
             *      criar mensagem de erro
             *      devolver o controlo da app à view
             *  else
             *      definir o nome a atribuir à imagem
             *      atribuir aos dados do novo vet, o nome do ficheiro da imagem
             *      guardar a imagem no disco rigido do servidor
            */

            if (fotoVet == null){
                veterinarios.Fotografia = "noVet.png";
            }
            else {
                if(fotoVet.ContentType == "image/png" || fotoVet.ContentType == "image/jpeg") {
                    //criar mensagem de erro
                    ModelState.AddModelError("", "Por favor, adicione um ficheiro .png ou .jpg");
                    //devolver o controlo da app à View
                    //fornecendo-lhe os dados que o utilizador já tinha preenchido no formulário
                    return View(veterinarios);
                }
                else {
                    //temos ficheiro e é uma imagem
                    //definir o nome da foto
                    Guid g = Guid.NewGuid();
                    string nomeFoto = veterinarios.NumCedulaProf+g.ToString();
                    string extensaoFoto = Path.GetExtension(fotoVet.FileName);
                    nomeFoto += extensaoFoto;
                    //atribuir ao vet o nome da sua foto
                    veterinarios.Fotografia = nomeFoto;
                }
            }
            //avaliar se os dados fornecidos pleo utilizador 
            //estão de acordo com as regras do Model
            if (ModelState.IsValid){
                try
                {
                    //adicionar os dados à BD
                    _context.Add(veterinarios);
                    //consolidar esses dados (commit)
                    await _context.SaveChangesAsync();
                }
                catch (Exception) {
                    //registar do disco rígido do servidor todos os dados da operação
                    //data e hora
                    //nome do utilizador
                    //nome do controller + método
                    //dados do erro
                    //outros dados
                    ModelState.AddModelError("", "Ocorrei um erro com a aoperação de guardar ");
                    return View(veterinarios);



                }
                //concretizar a açáo de guardar o ficheiro da foto
                if (fotoVet != null) {
                    //onde o ficheiro vai ser guardado
                    string nomeLocalizacaoFicheiro = _webHostEnvironment.WebRootPath;
                    //avaliar se a pasta 'Fotos' existe
                    if (!Directory.Exists(nomeLocalizacaoFicheiro)) {
                        Directory.CreateDirectory(nomeLocalizacaoFicheiro);
                    }
                    nomeLocalizacaoFicheiro = Path.Combine(nomeLocalizacaoFicheiro, "Fotos");
                    //nome do documento a guardar
                    string nomeDaFoto = Path.Combine(nomeLocalizacaoFicheiro, veterinarios.Fotografia);
                    //criar o objeto que vai manipular o ficheiro
                    using var stream = new FileStream(nomeDaFoto, FileMode.Create);
                    //guardar no disco rigido
                    await fotoVet.CopyToAsync(stream);

                    //devolver o controlo da app à View
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(veterinarios);
        }

        // GET: Veterinarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios.FindAsync(id);
            if (veterinarios == null)
            {
                return NotFound();
            }
            return View(veterinarios);
        }

        // POST: Veterinarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios)
        {
            if (id != veterinarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinariosExists(veterinarios.Id))
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
            return View(veterinarios);
        }

        // GET: Veterinarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // POST: Veterinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        

        private bool VeterinariosExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
