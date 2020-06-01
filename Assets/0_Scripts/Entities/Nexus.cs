
public class Nexus : Selectable
{
    protected override void Awake()
    {
        maxHP = 100;

        base.Awake();
    }

    public override void Destroyed(bool _fromNet)
    {
        base.Destroyed(_fromNet);

        GlobalManager.Instance.NexusDestroyed((int)Team);
    }
}
