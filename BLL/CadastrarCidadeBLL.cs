using System;
using Godot;

using BLL.Utils;
using BLL.Interface;
using DTO;
using SAL;
using SAL.Interface;

namespace BLL
{
    public class CadastrarCidadeBLL : ICadastrarCidadeBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLCadastroCidade { get; set; }
        public CadastrarCidadeBLL()
        {
            URLCadastroCidade = Apontamentos.URLApi + "/Localidade/Cadastrar";
            SAL = new SAL.Requisicao();
        }
        public void CadastrarCidade(LocalidadeDTO localidade)
        {
            ValidarPreenchimento(localidade.Nome, localidade.Mapa, new Vector3(localidade.X, localidade.Y, localidade.Z));
            var resposta = SAL.ExecutarPost<LocalidadeDTO, string>(URLCadastroCidade, localidade);
            if (!resposta.ToLower().Contains("sucesso"))
                throw new Exception("Erro interno no sistema.");
        }
        private void ValidarPreenchimento(string nome, string mapa, Vector3 vector3)
        {
            if (string.IsNullOrEmpty(nome))
            	throw new Exception("Por favor preencha o nome completo da cidade.");
            if (string.IsNullOrEmpty(mapa))
            	throw new Exception("Por favor carrege o mapa da cidade.");
            if (vector3 == new Vector3(0,0,0))
            	throw new Exception("Erro de interno no sistema.");
        }
        public void Dispose()
        {
            SAL.Dispose();
        }
    }
}