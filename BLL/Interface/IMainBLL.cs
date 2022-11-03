using Godot;

namespace BLL.Interface
{
    public interface IMainBLL
    {
        Node IntanciarTab(string nomeTab, string caminhoTab);
        
        void Dispose();
    }
}