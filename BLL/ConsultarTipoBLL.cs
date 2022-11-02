using Godot;
using System;
using System.Collections.Generic;

using BibliotecaViva.BLL.Utils;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.DTO;
using BibliotecaViva.SAL;
using BibliotecaViva.DTO.Interface;
using BibliotecaViva.SAL.Interface;

namespace BibliotecaViva.BLL
{
    public class ConsultarTipoBLL : IConsultarTipoBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLConsultarIdioma { get; set; }
        private string URLConsultarTipo { get; set; }
        private string URLConsultarTipoRelacao { get; set; }
        public ConsultarTipoBLL()
        {
            SAL = new Requisicao();
            URLConsultarIdioma = Apontamentos.URLApi + "/Tipo/ConsultarIdiomas";
            URLConsultarTipo = Apontamentos.URLApi + "/Tipo/ConsultarTipos";
            URLConsultarTipoRelacao = Apontamentos.URLApi + "/Tipo/ConsultarTiposRelacao";
        }
        public List<TipoDTO> ConsultarTipos()
        {
            return SAL.ExecutarPost<string, List<TipoDTO>>(URLConsultarTipo, "Consultar");
        }
        public List<TipoRelacaoDTO> ConsultarTiposRelacao()
        {
            return SAL.ExecutarPost<string, List<TipoRelacaoDTO>>(URLConsultarTipoRelacao, "Consultar");
        }
        public List<IdiomaDTO> ConsultarIdiomas()
        {
            return SAL.ExecutarPost<string, List<IdiomaDTO>>(URLConsultarIdioma, "Consultar");
        }
        public void PopularDropDownGenero(OptionButton dropdown)
        {
            dropdown.AddItem(string.Empty);
            dropdown.AddItem("Feminino");
            dropdown.AddItem("Masculino");
            dropdown.AddItem("Prefiro não declarar");
        }
        public List<TipoDTO> PopularDropDownTipo(OptionButton dropdown)
        {
            var tipos = ConsultarTipos();
            PopularDropdown<TipoDTO>(tipos, dropdown);
            return tipos;
        }
        public List<TipoRelacaoDTO> PopularDropDownTipoRelacao(OptionButton dropdown)
        {
            var tiposRelacao = ConsultarTiposRelacao();
            PopularDropdown<TipoRelacaoDTO>(tiposRelacao, dropdown);
            return tiposRelacao;
        }
        public List<IdiomaDTO> PopularDropDownIdioma(OptionButton dropdown)
        {
            var idiomas = ConsultarIdiomas();
            PopularDropdown<IdiomaDTO>(idiomas, dropdown);
            return idiomas;
        }
        private void PopularDropdown<T>(List<T> itens, OptionButton dropdown) where T : INomeado
        {
            foreach(var item in itens)
                dropdown.AddItem(item.Nome);
            dropdown.Selected = 0;
        }

        public void Dispose()
        {
            SAL.Dispose();
            URLConsultarTipo = null;
            URLConsultarTipoRelacao = null;
            URLConsultarIdioma = null;
        }
    }
}