using System;
using Siege.QueryAbstractions.Queries;

namespace Siege.QueryAbstractions.Statements
{
    public class JoinStatement<TLeftType, TRightType>
    {
        public SelectQuery<TLeftType> On(Func<TLeftType, TRightType, bool> evaluation)
        {
            throw new NotImplementedException();
        }
    }
}