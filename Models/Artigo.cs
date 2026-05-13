using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_DA_MDS.Models
{
    public class Artigo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public int TipoArtigoId { get; set; }

        [ForeignKey("TipoArtigoId")]
        public virtual TipoArtigo Tipo { get; set; }

        public virtual List<ItemCompra> ItemsCompra { get; set; }

        public Artigo()
        {
            ItemsCompra = new List<ItemCompra>();
        }
    }
}
