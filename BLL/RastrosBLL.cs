using Godot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BibliotecaViva.SAL;
using BibliotecaViva.DTO;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.SAL.Interface;
using BibliotecaViva.BLL.Interface;

namespace BibliotecaViva.BLL
{
    public class RastrosBLL : IRastrosBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLRastros { get; set; }
        public RastrosBLL()
        {
            SAL = new Requisicao();
            URLRastros = Apontamentos.URLApi + "/Rastros/Consultar";
        }
        public List<LocalizacaoGeograficaDTO> ObterListaDePontos()
        {
            return SAL.ExecutarPost<string, List<LocalizacaoGeograficaDTO>>(URLRastros, "ObterListaDePontos");
        }
        public void Dispose()
        {
            URLRastros = null;
            SAL.Dispose();
        }
    }
}