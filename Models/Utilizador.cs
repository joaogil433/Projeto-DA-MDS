using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Models
{
    public class Utilizador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public int? CriadoPorId { get; set; }
        public int? AlteradoPorId { get; set; }

        public virtual List<ListaCompra> ListasCriadas { get; set; }
        public virtual List<Orcamento> Orcamentos { get; set; }

        public Utilizador()
        {
            ListasCriadas = new List<ListaCompra>();
            Orcamentos = new List<Orcamento>();
        }
    }
}
