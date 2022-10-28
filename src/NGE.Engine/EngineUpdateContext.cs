namespace NGE.Engine
{
    public class EngineUpdateContext : UpdateContext
    {
        public readonly LocalSettings localSettings;

        public EngineUpdateContext(LocalSettings localSettings)
        {
            this.localSettings = localSettings;
        }
    }
}
