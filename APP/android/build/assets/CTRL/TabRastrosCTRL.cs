using Godot;
using System;
using System.Threading.Tasks;

using BibliotecaViva.BLL;
using BibliotecaViva.DTO;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class TabRastrosCTRL : Tabs, IDisposableCTRL
	{
		private Node2D Container { get; set; }
		private Sprite Ponto { get; set; }
		private Sprite Mapa { get ; set; }
		private IMapaBLL MapaBLL { get; set; }
		private IRastrosBLL RastrosBLL { get; set; }
		private Vector2 MouseMovementAtual { get; set; }
		private Vector2 MouseMovementAnterior { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			AtualizarPontosNoMapa();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		public void PopularNodes()
		{
			MouseMovementAtual = new Vector2(0,0);
			MouseMovementAnterior = new Vector2(0,0);
			Ponto = GetNode<Sprite>("./Controladores/Ponto");
			Mapa = GetNode<Sprite>("./AreaDoMapa/Mapa");
			Container = GetNode<Node2D>("./AreaDoMapa/Mapa/Container");
		}
		public void RealizarInjecaoDeDependencias()
		{
			MapaBLL = new MapaBLL();
			RastrosBLL = new RastrosBLL();
		}
		public override void _Process(float delta)
		{
			try
			{
				MapaBLL.VerificarMouseParado(MouseMovementAtual, MouseMovementAnterior);
				MapaBLL.ControlarJanela(Mapa, MouseMovementAtual, delta);
			}
			catch(Exception ex)
			{
				GD.Print(ex.Message);
			}
		}
		public override void _Input(InputEvent @event)
		{
			if (@event.GetType().ToString() == "Godot.InputEventMouseMotion")
				MouseMovementAtual = (@event as InputEventMouseMotion).Relative;
		}
		private void _on_Atualizar_button_up()
		{
			AtualizarPontosNoMapa();
		}
		private void AtualizarPontosNoMapa()
		{
			RemoverPontos();
			Task.Run(async () => await BuscarPontos());
		}
		private async Task BuscarPontos()
		{
			var pontos = RastrosBLL.ObterListaDePontos();
			if(pontos != null)
				foreach (var ponto in pontos)
					CallDeferred("DesenharPonto", new LocalizacaoGeograficaObject(ponto));

		}
		private void DesenharPonto(LocalizacaoGeograficaObject posicao)
		{
			var ponto = Ponto.Duplicate();
			Container.AddChild(ponto);
			(ponto as Sprite).Position = new Vector2(Convert.ToSingle(posicao.Localizacao.Longitude), Convert.ToSingle(posicao.Localizacao.Latitude));
		}
		private void RemoverPontos()
		{
			foreach(var ponto in Container.GetChildren())
				(ponto as Sprite).QueueFree();
		}
		public void FecharCTRL()
		{
			MapaBLL.Dispose();
			RastrosBLL.Dispose();
			Mapa.QueueFree();
			Ponto.QueueFree();
			RemoverPontos();
			Container.QueueFree();
			QueueFree();
		}
	}
}
