using System;
using System.Collections.Generic;

using BLL.Utils;
using BLL.Interface;
using DTO;
using SAL;
using SAL.Interface;

namespace BLL
{
    public class CadastrarPessoaBLL : ICadastrarPessoaBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLCadastroPessoa { get; set; }
        public CadastrarPessoaBLL()
        {
            URLCadastroPessoa = Apontamentos.URLApi + "/Pessoa/Cadastrar";
            SAL = new Requisicao();
        }
        public void CadastrarPessoa(PessoaDTO pessoa)
        {
            ValidarPreenchimento(pessoa.Nome, pessoa.Foto);
            var resposta = SAL.ExecutarPost<PessoaDTO, string>(URLCadastroPessoa, pessoa);
            if (!resposta.ToLower().Contains("sucesso"))
                throw new Exception("Erro ao cadastrar pista viva.");
        }
        private void ValidarPreenchimento(string nome, string foto)
        {
            if (string.IsNullOrEmpty(nome))
            	throw new Exception("Por favor preencha o nome completo da pista viva.");
            if (string.IsNullOrEmpty(foto))
            	throw new Exception("Por favor escolha uma foto de perfil para a pista viva.");
        }
        public void Dispose()
        {
            URLCadastroPessoa = null;
            SAL.Dispose();
        }
    }
}