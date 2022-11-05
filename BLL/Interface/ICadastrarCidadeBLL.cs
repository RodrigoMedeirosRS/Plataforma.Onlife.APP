using DTO;

namespace BLL.Interface
{
    public interface ICadastrarCidadeBLL
    {
        void CadastrarCidade(LocalidadeDTO localidade);
        
        void Dispose();
    }
}