using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> saldacaoFromServices([FromServices] IMeuServico service, string nome)
        {
            return service.saudacao(nome);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos?.AsNoTracking().ToListAsync(); // Requisições de apenas consultas não precisam ser rastreadas

            if(produtos is null) return NotFound("Produtos não encontrados...");

            return produtos;
        }

        [HttpGet("{valor:alpha:minlength(5)}")]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
            var produtos = await _context.Produtos?.AsNoTracking().ToListAsync();

            if (produtos is null) return NotFound("Produtos não encontrados...");

            return produtos;
        }

        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _context.Produtos?.AsNoTracking().FirstOrDefault<Produto>(p => p.Id == id);

            if (produto is null) return NotFound("Produto não encontrado...");

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {

            if (produto is null) return BadRequest("A forma que o produto é enviado está inválida... Verifique e tente novamente.");

            _context.Produtos!.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody]Produto produto)
        {
            if (id != produto.Id) return BadRequest("O Id passado é diferente do Id do produto");

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault<Produto>(p => p.Id == id);

            if (produto is null) return NotFound("Produto não encontrado...");

            _context.Produtos!.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
