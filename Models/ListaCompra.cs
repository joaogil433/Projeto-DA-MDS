using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Models
{
    public class ListaCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public DateTime? DataFecho { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public int UtilizadorCriouId { get; set; }

        [ForeignKey("UtilizadorCriouId")]
        public virtual Utilizador UtilizadorCriou { get; set; }

        public int? UtilizadorAlterouId { get; set; }

        [ForeignKey("UtilizadorAlterouId")]
        public virtual Utilizador UtilizadorAlterou { get; set; }

        public virtual List<ItemCompra> Itens { get; set; }

        public ListaCompra()
        {
            Itens = new List<ItemCompra>();
            DataCriacao = DateTime.Now;
        }
    }
}
