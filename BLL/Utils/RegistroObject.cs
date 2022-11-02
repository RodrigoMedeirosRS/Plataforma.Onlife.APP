using Godot;
using BibliotecaViva.DTO;

namespace BibliotecaViva.DTO
{
    public class RegistroObject : Object
    {
        public RegistroObject(RegistroDTO registro, RelacaoDTO relacao)
        {
            Registro = registro;
            Relacao = relacao;
        }
        public RegistroDTO Registro { get; set; }
        public RelacaoDTO Relacao { get; set; }
    }
}