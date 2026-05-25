// Ficheiro: Sessao.cs — colocar na raiz do projeto (namespace Projeto_DA_MDS)
// Responsabilidade: guardar o utilizador logado para ser acedido em toda a aplicação
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS
{
    // Classe estática: não precisa de ser instanciada, existe uma só cópia partilhada
    public static class Sessao
    {
        // Guarda o Utilizador que fez login — preenchido pelo FormLogin após validação
        public static Utilizador UtilizadorAtual { get; set; }
    }
}
