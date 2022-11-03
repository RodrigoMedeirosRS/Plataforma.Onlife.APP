using Godot;
using System;

namespace Onlife.CTRL
{
	public class Linha : Line2D
	{
		[Export]
		public NodePath ponto_a = null;
		[Export]
		public NodePath ponto_b = null;

		//public Godot.Object ponto_a_obj;
		//public Godot.Object ponto_b_obj;

		private Control OffsetNode;
		private Vector2 PontaLinha = new Vector2(0.35f, -0.5f);

		public override void _Ready()
		{
			//SetPointPosition(0, new Vector2(0, 0));
			//SetPointPosition(1, new Vector2(-200, 150));
			/*
			if (ponto_a != null)
			{
				//ponto_a_obj = GetNode(ponto_a);
			}
			if (ponto_b != null)
			{
				//ponto_b_obj = GetNode(ponto_b);
			}*/
			Position = new Vector2(0, 359);
			OffsetNode = GetParent<Control>();
			Atualizar_posicao();
		}

		public void set_ponto_a(NodePath a)
		{
			ponto_a = a;
			//ponto_a_obj = GetNode(ponto_a);
			OffsetNode = GetParent<Control>();
			Atualizar_posicao();
		}
		public void set_ponto_b(NodePath b)
		{
			ponto_b = b;
			//ponto_b_obj = GetNode(ponto_b);
		}
		public void set_pontos(NodePath a, NodePath b)
		{
			ponto_a = a;
			//ponto_a_obj = GetNode(ponto_a);
			OffsetNode = GetParent<Control>();
			ponto_b = b;
			//ponto_b_obj = GetNode(ponto_b);
			Atualizar_posicao();
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
			Atualizar_posicao();
		}

		public void Atualizar_posicao()
		{
			if (ponto_a != null && ponto_b != null)
			{
				//GD.Print("Reposiciona Linha!");

				//SetPointPosition(0, (GetNode<Control>(ponto_a).RectPosition + GetNode<Control>(ponto_a).RectSize * PontaLinha));// * OffsetNode.RectScale - OffsetNode.RectPosition));
				//SetPointPosition(1, (GetNode<Control>(ponto_b).RectPosition + GetNode<Control>(ponto_b).RectSize * PontaLinha)); // - OffsetNode.RectPosition* OffsetNode.RectScale
				//GD.Print(GetNode<Godot.Control>(ponto_b).RectGlobalPosition);
				SetPointPosition(0, (GetNode<Control>(ponto_a).RectGlobalPosition + GetNode<Control>(ponto_a).RectSize * PontaLinha));//  * OffsetNode.RectScale - OffsetNode.RectPosition));
				SetPointPosition(1, (GetNode<Control>(ponto_b).RectGlobalPosition + GetNode<Control>(ponto_b).RectSize * PontaLinha)); // - OffsetNode.RectPosition* OffsetNode.RectScale

			}
		}
	}
}
