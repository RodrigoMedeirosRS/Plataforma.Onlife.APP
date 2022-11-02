using Godot;
using System.Collections.Generic;

using BibliotecaViva.DTO;

namespace BibliotecaViva.BLL.Interface
{
    public interface IRastrosBLL
    {
        List<LocalizacaoGeograficaDTO> ObterListaDePontos();
        
        void Dispose();
    }
}