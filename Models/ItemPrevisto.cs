using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS.Models
{
    public class ItemPrevisto: ItemCompra
    {
        [Required]
        public int QuantidadePrevista { get; set; }
    }
}
