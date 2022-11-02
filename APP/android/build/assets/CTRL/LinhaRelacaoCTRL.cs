using Godot;
using System;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.DTO.Utils;

namespace BibliotecaViva.CTRL
{
	public class LinhaRelacaoCTRL : Control, IDisposable
	{
		public Label Nome { get; set; }
		public int CodigoRelacao { get; set; }
		public TextureButton BotaoRelacionar { get; set; }
		private bool Relacionado { get; set; }
		private List<TipoRelacaoDTO> Tipos { get; set; }
		private OptionButton DropdownTipoRelacao { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
			ExibirDropdownRelacoes();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularNodes()
		{
			Relacionado = false;
			Tipos = new List<TipoRelacaoDTO>();
			Nome = GetNode<Label>("./Nome");
			DropdownTipoRelacao = GetNode<OptionButton>("./TipoRelacao");
			BotaoRelacionar = GetNode<TextureButton>("./TextureButton");
		}
		public bool ObterRelacao()
		{
			return Relacionado;
		}
		public void PopularRelacoes(List<TipoRelacaoDTO> tipos)
		{
			Tipos = tipos;
			if (tipos.Count > 0)
				DropdownTipoRelacao.AddItem("");
			foreach(var tipo in Tipos)
				DropdownTipoRelacao.AddItem(tipo.Nome);
			ExibirDropdownRelacoes();
			PodeRelacionar();
		}
		public TipoRelacaoDTO ObterTipoRelacao()
		{
			return Tipos.Count > 0 ? Tipos[DropdownTipoRelacao.Selected -1] : null;
		}
		public void DefinirRelacao(bool relacionado)
		{
			Relacionado = relacionado && !BotaoRelacionar.Disabled;
		}
		private void _on_TipoRelacao_item_selected(int index)
		{
			PodeRelacionar();
		}
		private void _on_TextureButton_toggled(bool button_pressed)
		{
			Relacionado = button_pressed && !BotaoRelacionar.Disabled;
		}
		private void ExibirDropdownRelacoes()
		{
			DropdownTipoRelacao.Visible = Tipos.Count > 0;
		}
		private void PodeRelacionar()
		{
			if (Tipos.Count > 0)
			{
				BotaoRelacionar.Disabled = DropdownTipoRelacao.Selected == 0;
				DefinirRelacao(Relacionado);
			}
			else
				BotaoRelacionar.Disabled = false;

		}
		public void RemoverLinha()
		{
			DropdownTipoRelacao.QueueFree();
			BotaoRelacionar.QueueFree();
			QueueFree();
		}
		public void SelecionarTipoRelecao(string nomeTipoRelacao)
		{
			if (Tipos.Count > 0)
				DropdownTipoRelacao.Select(BustarIndicePorNome(nomeTipoRelacao));
		}
		private int BustarIndicePorNome(string nomeTipoRelacao)
		{
			for(int i = 0; i < DropdownTipoRelacao.Items.Count; i ++)
			{
				if(DropdownTipoRelacao.GetItemText(i) == nomeTipoRelacao)
					return i;
			}
			throw new Exception("Tipo de relação não encontrado");
		}
	}
}
