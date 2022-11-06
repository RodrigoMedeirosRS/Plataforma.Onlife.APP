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

	private RegistroDTO Registro { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDependencias();
		PopularNodes();
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
		Registro = new RegistroDTO();

		Tipos = BLLTipo.PopularDropDownTipo(Tipo);
		BLLTipo.PopularDropDownIdioma(Idioma);
	}
	public void CarregarEdicao(RegistroDTO registroDTO)
	{
		this.Popup_();
	}
}
