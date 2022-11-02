using Godot;
using System;

public class AbrirPopup : WindowDialog
{
    public override void _Ready()
    {
        GD.Print("Popup!");
        Show();

        //Godot.WindowDialog.popup();
        //GD.WindowDialog.popup();
    }

}
