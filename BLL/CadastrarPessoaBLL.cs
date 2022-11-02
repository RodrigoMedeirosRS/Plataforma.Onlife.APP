using System;
using System.Collections.Generic;

using BibliotecaViva.BLL.Utils;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.DTO;
using BibliotecaViva.SAL;
using BibliotecaViva.SAL.Interface;

namespace BibliotecaViva.BLL
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
        private void ValidarPreenchimento(string nome, string sobrenome, string genero)
        {
            if (string.IsNullOrEmpty(nome))
            	throw new Exception("Por favor preencher o Nome.");
            if (string.IsNullOrEmpty(sobrenome))
            	throw new Exception("Por favor preencher o Sobrenome.");
            if (string.IsNullOrEmpty(genero))
            	throw new Exception("Por favor escolha uma opção de Gênero.");
        }
        public PessoaDTO PopularPessoa(string nome, string sobrenome, string genero, string apelido, int codigoPessoa, List<RelacaoDTO> relacoes)
        {
            ValidarPreenchimento(nome, sobrenome, genero);

            var pessoa = new PessoaDTO()
            {
                Nome = nome,
                Sobrenome = sobrenome,
                Apelido = apelido,
                Genero = genero,
                Relacoes = relacoes
            };

            if (codigoPessoa != 0)
                pessoa.Codigo = codigoPessoa;

            return pessoa;
        }
        public string CadastrarPessoa(PessoaDTO pessoa)
        {    
            return SAL.ExecutarPost<PessoaDTO, string>(URLCadastroPessoa, pessoa);
        }
        public void Dispose()
        {
            URLCadastroPessoa = null;
            SAL.Dispose();
        }
    }
}