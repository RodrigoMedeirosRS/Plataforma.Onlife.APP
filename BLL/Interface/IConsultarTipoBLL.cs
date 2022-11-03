using Godot;
using System;
using System.Collections.Generic;

using DTO;

namespace BLL.Interface
{
    public interface IConsultarTipoBLL
    {
        List<TipoDTO> ConsultarTipos();
        List<TipoRelacaoDTO> ConsultarTiposRelacao();
        List<IdiomaDTO> ConsultarIdiomas();
        List<TipoDTO> PopularDropDownTipo(OptionButton dropdown);
        List<TipoRelacaoDTO> PopularDropDownTipoRelacao(OptionButton dropdown);
        List<IdiomaDTO> PopularDropDownIdioma(OptionButton dropdown);
        void PopularDropDownGenero(OptionButton dropdown);

        void Dispose();
    }
}