using Godot;

namespace BibliotecaViva.BLL.Interface
{
    public interface IMainBLL
    {
        Node IntanciarTab(string nomeTab, string caminhoTab);
        
        void Dispose();
    }
}