using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using DTO;
using DTO.Dominio;
using BLL.Interface;
using SAL.Interface;
using BLL.Utils;

namespace BLL
{
    public class ConsultarCidadeBLL : IConsultarCidadeBLL, IDisposable
    {
        public IRequisicao SAL { get; set; }
        private string URLListarCidade { get; set; }
        private string URLConsultarCidade { get; set; }

        public ConsultarCidadeBLL()
        {
            SAL = new SAL.Requisicao();
            URLListarCidade = Apontamentos.URLApi + "/Localidade/Listar";
            URLConsultarCidade = Apontamentos.URLApi + "/Localidade/Consultar";
        }
        public List<LocalidadeDTO> ListarCidades()
        {
            return SAL.ExecutarPost<string, List<LocalidadeDTO>>(URLListarCidade, "Consultar") ?? new List<LocalidadeDTO>();
        }
        public LocalidadeDTO ConsultarCidade(LocalidadeConsulta localidadeConsulta)
        {
            return SAL.ExecutarPost<LocalidadeConsulta, LocalidadeDTO>(URLConsultarCidade, localidadeConsulta);
        }

        public void Dispose()
        {
            SAL.Dispose();
        }
    }
}