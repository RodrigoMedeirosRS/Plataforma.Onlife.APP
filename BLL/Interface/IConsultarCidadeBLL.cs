using System.Collections.Generic;

using DTO;
using DTO.Dominio;

namespace BLL.Interface
{
    public interface IConsultarCidadeBLL
    {
        public List<LocalidadeDTO> ListarCidades();
        public LocalidadeDTO ConsultarCidade(LocalidadeConsulta localidadeConsulta);

        void Dispose();
    }
}