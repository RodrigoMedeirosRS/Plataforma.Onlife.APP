using Godot;

namespace BLL.Interface
{
    public interface IBuscarBLL
    {
        void InstanciarColuna(string caminho);
        void RemoverColuna(Node linha);
        bool ValidarColuna(int coluna);
        void Dispose();
    }
}