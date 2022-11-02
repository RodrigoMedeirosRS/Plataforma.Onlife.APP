using System.Collections.Generic;

using BibliotecaViva.DTO;

namespace BibliotecaViva.BLL.Interface
{
    public interface ICadastrarPessoaBLL
    {
        PessoaDTO PopularPessoa(string nome, string sobrenome, string genero, string apelido, int codigoPessoa, List<RelacaoDTO> relacoes);
        string CadastrarPessoa(PessoaDTO pessoa);
        
        void Dispose();
    }
}