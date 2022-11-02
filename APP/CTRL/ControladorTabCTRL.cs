using Godot;

using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
    public class ControladorTabCTRL : Control
    {
        public override void _Ready()
        {
            DesativarFuncoesDeAltoProcessamento();
        }
        private void DesativarFuncoesDeAltoProcessamento()
        {
            SetPhysicsProcess(false);
            SetProcess(false);
        }
        private void _on_Button_button_up()
        {
            (GetParent() as IDisposableCTRL).FecharCTRL();
            QueueFree();
        }
    }
}
