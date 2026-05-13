using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_DA_MDS.Models
{
    public class Orcamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Mes { get; set; }

        [Required]
        public int Ano { get; set; }

        public decimal ValorMaximo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAlteracao { get; set; }

        [Required]
        public int UtilizadorCriouId { get; set; }

        [ForeignKey("UtilizadorCriouId")]
        public virtual Utilizador UtilizadorCriou { get; set; }

        public int? UtilizadorAlterouId { get; set; }

        [ForeignKey("UtilizadorAlterouId")]
        public virtual Utilizador UtilizadorAlterou { get; set; }
    }
}

