namespace Assets.Scripts.InputSystem
{
    public abstract class InputMiddleWare
    {
        public InputMiddleWare Next;

        public abstract InputState Process(InputState input);
    }
}
