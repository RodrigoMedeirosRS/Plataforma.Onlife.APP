using Godot;

namespace BibliotecaViva.DTO
{
    public class PessoaObject : Object
    {
        public PessoaObject(PessoaDTO pessoa)
        {
            Pessoa = pessoa;
        }
        public PessoaDTO Pessoa { get; set; }
    }
}