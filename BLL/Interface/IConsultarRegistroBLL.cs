using Godot;
using System.Collections.Generic;

using DTO;
using DTO.Dominio;

namespace BLL.Interface
{
    public interface IConsultarRegistroBLL
    {
        void ValidarPreenchimento(RegistroConsulta registroConsulta);
        List<RegistroDTO> ValidarConsulta(List<RegistroDTO> retorno);
        List<RegistroDTO> RealizarConsulta(RegistroConsulta pessoaConsulta);
        ReferenciaRetorno RealizarConsultaDeRegistrosRelacionados(RelacaoConsulta relacaoConsulta);
        List<RegistroDTO> ListarRegistroPorLocalidade(LocalidadeConsulta localidade);
        Node InstanciarRegistroBox(Node Container, Vector2? posicao);

        void Dispose();
    }
}