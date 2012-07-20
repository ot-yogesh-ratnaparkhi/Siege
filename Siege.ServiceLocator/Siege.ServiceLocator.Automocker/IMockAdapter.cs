using System;

namespace Siege.ServiceLocator.AutoMocker
{
    public interface IMockAdapter
    {
        object Mock(Type type, params object[] parameters);
        object Stub(Type type, params object[] parameters);
    }
}
