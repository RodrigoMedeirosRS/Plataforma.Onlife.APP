using Godot;

namespace BibliotecaViva.DTO
{
    public class LocalizacaoGeograficaObject : Object
    {
        public LocalizacaoGeograficaObject(LocalizacaoGeograficaDTO localizacaoGeografica)
        {
            Localizacao = localizacaoGeografica;
        }
        public LocalizacaoGeograficaDTO Localizacao { get; set; }
    }
}