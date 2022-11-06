using Godot;
using System;
using System.Collections.Generic;

using SAL;
using DTO;
using BLL.Utils;
using DTO.Dominio;
using SAL.Interface;
using BLL.Interface;

namespace BLL
{
    public class ConsultarRegistroBLL : IConsultarRegistroBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLConsultarRegistro { get; set; }
        private string URLConsultarRelacao { get; set; }
        private string URLListarRegistroPorLocalidade { get; set; }
        public ConsultarRegistroBLL()
        {
            URLConsultarRegistro = Apontamentos.URLApi + "/Registro/Consultar";
            URLConsultarRelacao = Apontamentos.URLApi + "/Registro/ObterReferencias";
            URLListarRegistroPorLocalidade = Apontamentos.URLApi + "/Registro/ListarPorLocalidade";
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
        public List<RegistroDTO> ListarRegistroPorLocalidade(LocalidadeConsulta localidade)
        {
            var retorno = SAL.ExecutarPost<LocalidadeConsulta, List<RegistroDTO>>(URLListarRegistroPorLocalidade, localidade);
            return retorno;
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