using DTO;

namespace BLL.Interface
{
    public interface ICadastrarPessoaBLL
    {
        void CadastrarPessoa(PessoaDTO pessoa);
        
        void Dispose();
    }
}