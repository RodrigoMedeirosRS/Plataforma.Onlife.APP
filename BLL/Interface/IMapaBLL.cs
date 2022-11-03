using Godot;

namespace BLL.Interface
{
    public interface IMapaBLL
    {
        void VerificarMouseParado(Vector2 mouseMovementAtual, Vector2 mouseMovementAnterior);
        void ControlarJanela(Sprite mapa, Vector2 mouseMovement, float delta);
        void Dispose();
    }
}