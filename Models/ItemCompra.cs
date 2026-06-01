using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Models
{
    public abstract class ItemCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ListaCompraId { get; set; }

        [ForeignKey("ListaCompraId")]
        public virtual ListaCompra Lista { get; set; }

        [Required]
        public int ArtigoId { get; set; }

        [ForeignKey("ArtigoId")]
        public virtual Artigo Artigo { get; set; }

        public int QuantidadeAdquirida { get; set; }

        public decimal PrecoUnitario { get; set; }

        public int UtilizadorId { get; set; }

        [ForeignKey("UtilizadorId")]
        public virtual Utilizador Utilizador { get; set; }
    }
}
