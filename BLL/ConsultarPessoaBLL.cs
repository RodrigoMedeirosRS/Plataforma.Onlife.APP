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
    public class ConsultarPessoaBLL : IConsultarPessoaBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLConsultarPessoa { get; set; }
        private string URLConsultarRelacao { get; set; }
        public ConsultarPessoaBLL()
        {
            SAL = new Requisicao();
            URLConsultarPessoa = Apontamentos.URLApi + "/Pessoa/Consultar";
            URLConsultarRelacao = Apontamentos.URLApi + "/Pessoa/ObterRelacoes";
        }
        public void ValidarPreenchimento(PessoaConsulta pessoaConsulta)
        {
            if (string.IsNullOrEmpty(pessoaConsulta.Nome) && string.IsNullOrEmpty(pessoaConsulta.Sobrenome) && string.IsNullOrEmpty(pessoaConsulta.Apelido))
                throw new Exception("Favor inserir nome, sobrenome ou apelido para realizar a consulta");
        }
        public List<PessoaDTO> ValidarConsulta(List<PessoaDTO> retorno)
        {
            if (retorno.Count == 0)
                throw new Exception("Consulta não Retornou Dados");
            return retorno;
        }
        public List<RegistroDTO> ValidarConsulta(List<RegistroDTO> retorno)
        {
            if (retorno.Count == 0)
                throw new Exception("Consulta não Retornou Dados");
            return retorno;
        }
        public List<PessoaDTO> RealizarConsulta(PessoaConsulta pessoaConsulta)
        {
            ValidarPreenchimento(pessoaConsulta);
            var retorno = SAL.ExecutarPost<PessoaConsulta, List<PessoaDTO>>(URLConsultarPessoa, pessoaConsulta);
            return ValidarConsulta(retorno);
        }
        public List<RegistroDTO> RealizarConsultaDeRegistrosRelacionados(RelacaoConsulta relacaoConsulta)
        {
            var retorno = SAL.ExecutarPost<RelacaoConsulta, List<RegistroDTO>>(URLConsultarRelacao, relacaoConsulta);
            return ValidarConsulta(retorno);
        }
        public Node InstanciarPessoaBox(Node Container, Vector2? posicao)
        {
            var cena = InstanciadorUtil.CarregarCena("res://RES/CENAS/PessoaBox.tscn");
            return InstanciadorUtil.InstanciarObjeto(Container, cena, posicao);
        }

        public void Dispose()
        {
            
        }
    }
}