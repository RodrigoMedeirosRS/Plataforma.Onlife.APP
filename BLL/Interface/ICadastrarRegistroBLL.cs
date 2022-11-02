using Godot;
using System.Collections.Generic;

using BibliotecaViva.DTO;

namespace BibliotecaViva.BLL.Interface
{
    public interface ICadastrarRegistroBLL
    {
        RegistroDTO PopularRegistro(string nome, string apelido, string latlong, string descricao, string conteudo, TipoDTO tipoSelecionado, OptionButton idiomaDropdown, int codigoRegistro, List<RelacaoDTO> relacoes);
        string ValidarPreenchimento(string nome, string latlong, string descricao, string conteudo, TipoDTO tipoSelecionado);
        void ValidarConteudoBinario(string caminhoConteudo, string extensao);
        string CadastrarRegistro(RegistroDTO registro);
        void Dispose();
    }
}