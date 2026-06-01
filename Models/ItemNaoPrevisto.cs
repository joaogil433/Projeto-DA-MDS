using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Models
{
    public class ItemNaoPrevisto: ItemCompra
    {
        [Required]
        public string Observacoes { get; set; }
    }
}
