using Godot;
using System;

public class Link : TextureButton
{
    public override void _Ready()
    {
    }

    public void _on_Link_pressed()
    {
        OS.ShellOpen("https://clauveira.itch.io/");
    }
}
