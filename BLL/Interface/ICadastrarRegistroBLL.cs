using Godot;
using System.Collections.Generic;

using DTO;

namespace BLL.Interface
{
    public interface ICadastrarRegistroBLL
    {
        string CadastrarRegistro(RegistroDTO registro);
        void Dispose();
    }
}