using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;

[Table("Categorias")]
public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>(); // É RESPONSABILIDADE DA CLASSE INICIAR A COLEÇÃO
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(80)]
    public string? ImagemUrl { get; set; }
    
    [Required]
    [StringLength(300)]
    public ICollection<Produto>? Produtos { get; set; }
}
