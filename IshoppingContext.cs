using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS
{
    public class IshoppingContext: DbContext
    {
        public IshoppingContext() : base("name=IshoppingDB")
        {
        }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<TipoArtigo> TiposArtigo { get; set; }
        public DbSet<Artigo> Artigos { get; set; }
        public DbSet<ItemCompra> ItensCompra { get; set; }
        public DbSet<ListaCompra> ListasCompras { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListaCompra>()
                .HasRequired(l => l.UtilizadorCriou)
                .WithMany(u => u.ListasCriadas)
                .HasForeignKey(l => l.UtilizadorCriouId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ListaCompra>()
                .HasOptional(l => l.UtilizadorAlterou)
                .WithMany()
                .HasForeignKey(l => l.UtilizadorAlterouId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orcamento>()
                .HasRequired(o => o.UtilizadorCriou)
                .WithMany(u => u.Orcamentos)
                .HasForeignKey(o => o.UtilizadorCriouId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orcamento>()
                .HasOptional(o => o.UtilizadorAlterou)
                .WithMany()
                .HasForeignKey(o => o.UtilizadorAlterouId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
