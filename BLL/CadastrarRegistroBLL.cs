using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DTO;
using SAL;
using BLL.Utils;
using BLL.Interface;
using SAL.Interface;

namespace BLL
{
    public class CadastrarRegistroBLL : ICadastrarRegistroBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLCadastrarRegistro { get; set; }
        public CadastrarRegistroBLL()
        {
            URLCadastrarRegistro = Apontamentos.URLApi + "/Registro/Cadastrar";
            SAL = new Requisicao();
        }
        public string CadastrarRegistro(RegistroDTO registro)
        {
            ValidarRegistro(registro);
            return SAL.ExecutarPost<RegistroDTO, string>(URLCadastrarRegistro, registro);
        }
        private void ValidarRegistro(RegistroDTO registroDTO)
        {
            if (string.IsNullOrEmpty(registroDTO.Nome))
                throw new Exception("Por favor preencha um nome para o registro.");
            if (string.IsNullOrEmpty(registroDTO.Conteudo))
                throw new Exception("Por favor carrega ou preencha um conteúdo para o registro.");
        }

        public void Dispose()
        {
            URLCadastrarRegistro = null;
            SAL.Dispose();
        }
    }
}