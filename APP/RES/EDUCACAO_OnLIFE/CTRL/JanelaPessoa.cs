using Godot;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
//using BibliotecaViva.DTO.Utils;
//using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.BLL.Interface;
//using BibliotecaViva.CTRL.Interface;

public class JanelaPessoa : MarginContainer
{
    // "HBoxContainer/LabelInput/Nome/LineEdit"

    public int CodigoPessoa { get; set; }
    private ICadastrarPessoaBLL CadastroPessoaBLL { get; set; }
    private IConsultarPessoaBLL ConsultarPessoaBLL { get; set; }
    private IConsultarTipoBLL TipoBLL { get; set; }
    private IConsultarRegistroBLL RegistroBLL { get; set; }
    private LineEdit Nome { get; set; }

    public override void _Ready()
    {
        PopularNodes();
        Nome.Text = "Nome Completo da Pessoa Vindo do Banco";
    }
    private void RealizarInjecaoDeDependencias()
    {
        CadastroPessoaBLL = new CadastrarPessoaBLL();
        ConsultarPessoaBLL = new ConsultarPessoaBLL();
        TipoBLL = new ConsultarTipoBLL();
        RegistroBLL = new ConsultarRegistroBLL();
    }
    private void PopularNodes()
    {
        Nome = GetNode<LineEdit>("HBoxContainer/LabelInput/Nome/LineEdit");
        CodigoPessoa = 2;
    }

}
