using Godot;

namespace DTO
{
    public class LocalizacaoGeograficaObject : Object
    {
        public LocalizacaoGeograficaObject(LocalidadeDTO localizacaoGeografica)
        {
            Localizacao = localizacaoGeografica;
        }
        public LocalidadeDTO Localizacao { get; set; }
    }
}