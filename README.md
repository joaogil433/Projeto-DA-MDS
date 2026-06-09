================================================================================

&#x20; iShopping — Aplicação de Gestão de Compras

&#x20; Curso TeSP PSI — Desenvolvimento de Aplicações

&#x20; Instituto Politécnico de Leiria — ESTG

&#x20; 2025/2026 — 2 Semestre

================================================================================



\------------------------------------------------------------------------

1\. ELEMENTOS DO GRUPO

\------------------------------------------------------------------------



&#x20; Nº de Aluno: 2025187956   Nome: João Filipe Freitas Gil      

&#x20; Nº de Aluno: 2025181816   Nome: Duarte Miguel Simão Neto     

&#x20; Nº de Aluno: 2025156135   Nome: Rafael Luís Lampreia Vieira  



\------------------------------------------------------------------------

2\. DESCRIÇÃO DA APLICAÇÃO

\------------------------------------------------------------------------



O iShopping é uma aplicação Windows Forms desenvolvida em C# com arquitetura

MVC e Entity Framework 6 (Code First). Permite gerir utilizadores, artigos,

orçamentos mensais e listas de compras planeadas, incluindo um modo de compra

em tempo real com controlo de orçamento e exportação de dados para CSV.



\------------------------------------------------------------------------

3\. REQUISITOS DE INSTALAÇÃO

\------------------------------------------------------------------------



&#x20; - Sistema Operativo: Windows 10 ou superior

&#x20; - Visual Studio 2019 ou superior (com suporte a .NET Framework 4.8)

&#x20; - .NET Framework 4.8 (incluído no Windows 10/11 por omissão)

&#x20; - SQL Server Express com LocalDB (instalado com o Visual Studio)

&#x20;   - Versão necessária: MSSQLLocalDB



&#x20; Pacotes NuGet (restaurados automaticamente ao compilar):

&#x20;   - EntityFramework 6.5.2



\------------------------------------------------------------------------

4\. CONFIGURAÇÃO DA BASE DE DADOS

\------------------------------------------------------------------------



A aplicação utiliza Entity Framework Code First com criação automática

da base de dados. Não é necessário executar scripts SQL manualmente.



&#x20; Connection String (App.config):

&#x20;   Data Source=(LocalDb)\\MSSQLLocalDB;

&#x20;   Initial Catalog=IshoppingDB;

&#x20;   Integrated Security=True



&#x20; A base de dados "IshoppingDB" é criada automaticamente na primeira

&#x20; execução, no servidor LocalDB do SQL Server.



&#x20; O sistema faz seed automático com os seguintes utilizadores de teste:



&#x20;   Username: admin1   Password: 1234   (João)

&#x20;   Username: admin2   Password: 1234   (Duarte)

&#x20;   Username: admin3   Password: 1234   (Rafa)



\------------------------------------------------------------------------

5\. COMPILAÇÃO E EXECUÇÃO

\------------------------------------------------------------------------



&#x20; Passo 1 — Abrir o projeto

&#x20;   Abrir o ficheiro "Projeto DA MDS.sln" no Visual Studio.



&#x20; Passo 2 — Restaurar pacotes NuGet

&#x20;   Menu: Tools > NuGet Package Manager > Restore NuGet Packages



&#x20; Passo 3 — Compilar

&#x20;   Menu: Build > Build Solution  (ou Ctrl+Shift+B)



&#x20; Passo 4 — Executar

&#x20;   Pressionar F5 (com depuração) ou Ctrl+F5 (sem depuração).

&#x20;   A aplicação inicia no FormLogin.



\------------------------------------------------------------------------

6\. UTILIZAÇÃO DA APLICAÇÃO

\------------------------------------------------------------------------



&#x20; 6.1 LOGIN / REGISTO

&#x20;   - Ao iniciar, é apresentado o formulário de autenticação.

&#x20;   - Tab "Login": introduzir username e password para entrar.

&#x20;   - Tab "Registo": criar uma nova conta de utilizador.

&#x20;   - As passwords são armazenadas encriptadas (SHA-256).



&#x20; 6.2 FORMULÁRIO PRINCIPAL

&#x20;   - Após login, é apresentada a lista de compras em aberto.

&#x20;   - O menu superior dá acesso a todas as funcionalidades.

&#x20;   - Duplo clique numa compra abre o Modo Compra.



&#x20; 6.3 GESTÃO DE UTILIZADORES

&#x20;   - CRUD completo de utilizadores.

&#x20;   - Não é possível eliminar o utilizador atualmente autenticado.

&#x20;   - Não é possível eliminar utilizadores com compras ou orçamentos.



&#x20; 6.4 GESTÃO DE TIPOS DE ARTIGO

&#x20;   - CRUD completo de tipos de artigo.

&#x20;   - Não é possível eliminar um tipo que tenha artigos associados.



&#x20; 6.5 GESTÃO DE ARTIGOS

&#x20;   - CRUD completo de artigos.

&#x20;   - Filtro por tipo de artigo.

&#x20;   - Não é possível eliminar artigos associados a compras.



&#x20; 6.6 GESTÃO DE ORÇAMENTOS

&#x20;   - CRUD de orçamentos mensais (um por mês/ano).

&#x20;   - Registo automático do criador e do último editor.



&#x20; 6.7 PLANEAMENTO DE COMPRAS

&#x20;   - Lista de todas as compras com filtro por estado (Aberta/Fechada).

&#x20;   - Criar, editar ou eliminar compras planeadas.

&#x20;   - Compras fechadas não podem ser editadas.



&#x20; 6.8 DETALHES DA COMPRA (CRIAÇÃO / EDIÇÃO)

&#x20;   - Definir nome da compra e adicionar itens previstos.

&#x20;   - Selecionar tipo de artigo para filtrar o artigo pretendido.

&#x20;   - Definir quantidade prevista por item.



&#x20; 6.9 MODO COMPRA

&#x20;   - Visualizar os itens previstos de uma compra em aberto.

&#x20;   - Marcar itens como adquiridos, indicando quantidade e preço unitário.

&#x20;   - Adicionar itens não previstos durante a compra.

&#x20;   - Alerta visual caso o total ultrapasse o orçamento do mês.

&#x20;   - Fechar a compra (fica registada a data e o utilizador).



&#x20; 6.10 ESTATÍSTICAS E APOIO À DECISÃO

&#x20;   - Tab 1: orçamento mensal vs total gasto; listagem de compras fechadas

&#x20;     com percentagem de itens previstos adquiridos.

&#x20;   - Tab 2: sugestão de orçamento para o próximo mês; sugestão de lista

&#x20;     de compras por semana do mês.

&#x20;   - Exportação das compras fechadas para ficheiro CSV.



\------------------------------------------------------------------------

7\. NOTAS ADICIONAIS

\------------------------------------------------------------------------



&#x20; - Se ao iniciar aparecer um aviso do depurador sobre "NonComVisibleBaseClass",

&#x20;   pode ser ignorado — trata-se de uma incompatibilidade conhecida entre

&#x20;   .NET Framework 4.8 e o Windows 11 UI Automation, sem impacto no

&#x20;   funcionamento da aplicação.



&#x20; - O projeto foi desenvolvido com metodologia Scrum, utilizando Jira

&#x20;   para gestão do backlog e GitHub para controlo de versões, com uma

&#x20;   branch por elemento do grupo.



================================================================================



