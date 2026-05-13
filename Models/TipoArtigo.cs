using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_DA_MDS.Models
{
    public class TipoArtigo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public virtual List<Artigo> Artigos { get; set; }

        public TipoArtigo()
        {
            Artigos = new List<Artigo>();
        }
    }
}
