using Godot;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.DTO.Dominio;

namespace BibliotecaViva.BLL.Interface
{
    public interface IConsultarRegistroBLL
    {
        void ValidarPreenchimento(RegistroConsulta registroConsulta);
        List<RegistroDTO> ValidarConsulta(List<RegistroDTO> retorno);
        List<RegistroDTO> RealizarConsulta(RegistroConsulta pessoaConsulta);
        ReferenciaRetorno RealizarConsultaDeRegistrosRelacionados(RelacaoConsulta relacaoConsulta);
        Node InstanciarRegistroBox(Node Container, Vector2? posicao);

        void Dispose();
    }
}