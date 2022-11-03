using Godot;

using CTRL.Interface;

namespace CTRL
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
