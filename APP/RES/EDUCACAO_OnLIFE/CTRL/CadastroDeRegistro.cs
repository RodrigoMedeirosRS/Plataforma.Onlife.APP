using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BLL.Interface;
using DTO;
using DTO.Utils;
using BLL.Utils;
using DTO.Dominio;


public class CadastroDeRegistro : ConfirmationDialog
{
	private LineEdit Nome { get; set; }
	private LineEdit Apelido { get; set; }
	private OptionButton Idioma { get; set; }
	private OptionButton Tipo { get; set; }
	private TextEdit Descricao { get; set; }
	private IConsultarTipoBLL BLLTipo { get; set; }
	private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
	private List<TipoDTO> Tipos { get; set; }

	private Control TextoInput { get; set; }
	private Control ImagemInput { get; set; }
	private Control AudioInput { get; set; }
	private Control ArquivoInput { get; set; }
	private Control URLInput { get; set; }


	private RegistroDTO Registro { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDependencias();
		PopularNodes();
		ExibirInterfaceTextual();
	}
	public void RealizarInjecaoDependencias()
	{
		BLLTipo = new BLL.ConsultarTipoBLL();
		CadastrarRegistroBLL = new BLL.CadastrarRegistroBLL();
	}
	public void PopularNodes()
	{
		Nome = GetNode<LineEdit>("./Control/Nome");
		Apelido = GetNode<LineEdit>("./Control/Apelido");
		Idioma = GetNode<OptionButton>("./Control/Idioma");
		Tipo = GetNode<OptionButton>("./Control/Tipo");
		Descricao = GetNode<TextEdit>("./Control/Label/Descricao");
		TextoInput = GetNode<Control>("./Control/TextoInput");
		ImagemInput = GetNode<Control>("./Control/ImagemInput");
		AudioInput = GetNode<Control>("./Control/AudioInput");
		ArquivoInput = GetNode<Control>("./Control/ArquivoInput");
		URLInput = GetNode<Control>("./Control/URLInput");
		Registro = new RegistroDTO();

		Tipos = BLLTipo.PopularDropDownTipo(Tipo);
		BLLTipo.PopularDropDownIdioma(Idioma);
	}
	public void CarregarEdicao(RegistroDTO registroDTO)
	{
		this.Popup_();
	}
	private void _on_Tipo_item_selected(int index)
	{
		switch(Tipos[index].TipoExecucao)
		{
			case (TipoExecucao.Texto):
				ExibirInterfaceTextual();
				break;
			case (TipoExecucao.Imagem):
				ExibirInterfaceImagem();
				break;
			case (TipoExecucao.Audio):
				ExibirInterfaceAudio();
				break;
			case (TipoExecucao.Arquivo):
				ExibirInterfaceArquivo();
				break;
			case (TipoExecucao.URL):
				ExibirInterfaceURL();
				break;
		}
	}
	private void ExibirInterfaceTextual()
	{
		TextoInput.Visible = true;
		ImagemInput.Visible = false;
		AudioInput.Visible = false;
		ArquivoInput.Visible = false;
		URLInput.Visible = false;
	}
	private void ExibirInterfaceImagem()
	{
		TextoInput.Visible = false;
		ImagemInput.Visible = true;
		AudioInput.Visible = false;
		ArquivoInput.Visible = false;
		URLInput.Visible = false;
	}
	private void ExibirInterfaceAudio()
	{
		TextoInput.Visible = false;
		ImagemInput.Visible = false;
		AudioInput.Visible = true;
		ArquivoInput.Visible = false;
		URLInput.Visible = false;
	}
	private void ExibirInterfaceArquivo()
	{
		TextoInput.Visible = false;
		ImagemInput.Visible = false;
		AudioInput.Visible = false;
		ArquivoInput.Visible = true;
		URLInput.Visible = false;
	}
	private void ExibirInterfaceURL()
	{
		TextoInput.Visible = false;
		ImagemInput.Visible = false;
		AudioInput.Visible = false;
		ArquivoInput.Visible = false;
		URLInput.Visible = true;
	}

}
