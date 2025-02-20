using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriaController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("config")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];
            var sessao1 = _configuration["sessao1:chave1"];

            return $"Chave: {valor1}\n{valor2}\n{sessao1}";
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias?.AsNoTracking().ToList();

            if (categorias is null) return NotFound("Nenhuma categoria encontrada");

            return categorias;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _context.Categorias?.AsNoTracking().FirstOrDefault<Categoria>(c => c.Id == id);

            if (categoria is null) return NotFound("Categoria não encontrada");

            return categoria;
        }

        [HttpGet("{id:int:min(1)}/Produtos")]
        public ActionResult<IEnumerable<Categoria>> GetProdutosByCategorias(int id)
        {
            try
            {
                return _context.Categorias!.Include(p => p.Produtos).Where(c => c.Id == id).AsNoTracking().ToList();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao tratar a sua requisição.");
            }

        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null) return BadRequest("Entidade inválida. Verifique e tente novamente.");

            _context.Categorias!.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest("Id nao encontrado. Verifique e tente novamente...");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias?.FirstOrDefault(c => c.Id == id);

            if (categoria is null) return BadRequest("Categoria não encontrada. Verifique e tente novamente");

            _context.Categorias!.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
