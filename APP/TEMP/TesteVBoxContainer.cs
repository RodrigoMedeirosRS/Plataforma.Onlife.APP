using Godot;
using System;

namespace Onlife.CTRL
{

    public class TesteVBoxContainer : VBoxContainer
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        [Export]
        public NodePath ponto_a = null;
        [Export]
        public NodePath ponto_b = null;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            /* */
            //var box = this.GetChildren();
            //PackedScene linha = (PackedScene)ResourceLoader.Load("res://RES/EDUCACAO_OnLIFE/CENAS/JANELA_PART/FioVermelho.tscn");


        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("ui_select"))
            {
                PackedScene linha = GD.Load<PackedScene>("res://RES/EDUCACAO_OnLIFE/CENAS/JANELA_PART/FioVermelho.tscn");
                Linha newLinha = (Linha)linha.Instance();
                //GetParent().CallDeferred("add_child", newLinha);
                AddChild(newLinha);
                //ponto_a = newLinha.GetNode(ponto_a).GetPath();
                //ponto_b = newLinha.GetNode(ponto_b).GetPath();
                newLinha.set_pontos("../JanelaGPedU", "../../../Linha2/VBoxContainer/JanelaEquipe5");

            }
        }
    }
}