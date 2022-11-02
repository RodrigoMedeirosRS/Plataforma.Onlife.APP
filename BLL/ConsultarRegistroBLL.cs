using Godot;
using System;
using System.Collections.Generic;

using BibliotecaViva.SAL;
using BibliotecaViva.DTO;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.SAL.Interface;
using BibliotecaViva.BLL.Interface;

namespace BibliotecaViva.BLL
{
    public class ConsultarRegistroBLL : IConsultarRegistroBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLConsultarRegistro { get; set; }
        private string URLConsultarRelacao { get; set; }
        public ConsultarRegistroBLL()
        {
            URLConsultarRegistro = Apontamentos.URLApi + "/Registro/Consultar";
            URLConsultarRelacao = Apontamentos.URLApi + "/Registro/ObterReferencias";
            SAL = new Requisicao();
        }
        public void ValidarPreenchimento(RegistroConsulta registroConsulta)
        {
            if (string.IsNullOrEmpty(registroConsulta.Nome) && string.IsNullOrEmpty(registroConsulta.Apelido))
                throw new Exception("Favor inserir nome ou apelido para realizar a consulta");
        }
        public List<RegistroDTO> ValidarConsulta(List<RegistroDTO> retorno)
        {
            if (retorno.Count == 0)
                throw new Exception("Consulta não Retornou Dados");
            return retorno;
        }
        public ReferenciaRetorno ValidarConsulta(ReferenciaRetorno retorno)
        {
            if (retorno.Pessoas.Count == 0 && retorno.Registros.Count == 0)
                throw new Exception("Consulta não Retornou Dados");
            return retorno;
        }
        public List<RegistroDTO> RealizarConsulta(RegistroConsulta registroCosnulta)
        {
            ValidarPreenchimento(registroCosnulta);
            var retorno = SAL.ExecutarPost<RegistroConsulta, List<RegistroDTO>>(URLConsultarRegistro, registroCosnulta);
            return ValidarConsulta(retorno);
        }
        public ReferenciaRetorno RealizarConsultaDeRegistrosRelacionados(RelacaoConsulta relacaoConsulta)
        {
            var retorno = SAL.ExecutarPost<RelacaoConsulta, ReferenciaRetorno>(URLConsultarRelacao, relacaoConsulta);
            return ValidarConsulta(retorno);
        }
        public Node InstanciarRegistroBox(Node Container, Vector2? posicao)
        {
            var cena = InstanciadorUtil.CarregarCena("res://RES/CENAS/RegistroBox.tscn");
            return InstanciadorUtil.InstanciarObjeto(Container, cena, posicao);
        }

        public void Dispose()
        {
            URLConsultarRegistro = null;
            SAL.Dispose();
        }
    }
}