using System;

namespace Siege.Requisitions.AutoMocker
{
    public interface IMockAdapter
    {
        object Mock(Type type, params object[] parameters);
        object Stub(Type type, params object[] parameters);
    }
}
