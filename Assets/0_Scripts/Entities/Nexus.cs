
public class Nexus : Selectable
{
    public override void Destroyed(bool _fromNet)
    {
        base.Destroyed(_fromNet);
        GlobalManager.Instance.NexusDestroyed((int)Team);
    }
}
