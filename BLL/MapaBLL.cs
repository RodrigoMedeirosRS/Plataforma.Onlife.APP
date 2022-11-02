using Godot;
using System;

using BibliotecaViva.BLL.Interface;

namespace BibliotecaViva.BLL
{
    public class MapaBLL : IMapaBLL, IDisposable
    {
        public void VerificarMouseParado(Vector2 mouseMovementAtual, Vector2 mouseMovementAnterior)
        {
			if (mouseMovementAtual == mouseMovementAnterior)
				mouseMovementAtual = new Vector2(0,0);
			else
				mouseMovementAnterior = mouseMovementAtual;
        }
        public void ControlarJanela(Sprite mapa, Vector2 mouseMovement, float delta)
		{
			Zoom(mapa, delta);
			Drag(mapa, mouseMovement, delta);
		}
        private void Zoom(Sprite mapa, float delta)
		{
			if (Input.IsActionJustReleased("ui_page_down"))
				AplicarZoom(mapa, 0.97f);
			if (Input.IsActionJustReleased("ui_page_up"))
				AplicarZoom(mapa, 1.03f);
		}

		private void AplicarZoom(Sprite mapa, float incremento)
		{
			mapa.Scale *= new Vector2(incremento, incremento);
		}
		
		private void Drag(Sprite mapa, Vector2 mouseMovement, float delta)
		{
			if (Input.IsActionPressed("ui_accept"))
			{
				mapa.MoveLocalX(mouseMovement.x);
				mapa.MoveLocalY(mouseMovement.y);
			}
		}
		
		public void Dispose()
        {
            
        }
    }
}