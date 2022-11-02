using Godot;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.DTO.Dominio;

namespace BibliotecaViva.BLL.Interface
{
    public interface IConsultarPessoaBLL
    {
        void ValidarPreenchimento(PessoaConsulta pessoaConsulta);
        List<PessoaDTO> ValidarConsulta(List<PessoaDTO> retorno);
        List<PessoaDTO> RealizarConsulta(PessoaConsulta pessoaConsulta);
        List<RegistroDTO> RealizarConsultaDeRegistrosRelacionados(RelacaoConsulta relacaoConsulta);
        Node InstanciarPessoaBox(Node Container, Vector2? posicao);

        void Dispose();
    }
}