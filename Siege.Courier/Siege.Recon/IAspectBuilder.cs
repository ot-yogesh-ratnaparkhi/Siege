namespace Siege.Recon
{
    public interface IAspectBuilder<out TAspect, in TSource> where TAspect : IAspect
    {
        TAspect Build(TSource source);
    }
}