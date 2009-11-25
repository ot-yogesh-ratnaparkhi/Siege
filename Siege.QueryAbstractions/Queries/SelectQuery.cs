using System;
using Siege.QueryAbstractions.Statements;

namespace Siege.QueryAbstractions.Queries
{
    public class SelectQuery<TSelectType> : IQuery
    {
        public SelectQuery<TSelectType> Select()
        {
            return this;
        }

        public SelectQuery<TSelectType> Join<TJoinType>()
        {
            var join = new JoinStatement<TSelectType, TJoinType>();

            return this;
        }

        public SelectQuery<TSelectType> Join<TLeftType, TRightType>()
        {
            var join = new JoinStatement<TLeftType, TRightType>();

            return this;
        }

        public SelectQuery<TSelectType> Join<TLeftType, TRightType>(Func<TLeftType, TRightType, bool> condition)
        {
            var join = new JoinStatement<TLeftType, TRightType>();

            join.On(condition);

            return this;
        }

        public SelectQuery<TSelectType> Where<TCondition>(Predicate<TCondition> condition)
        {
            return this;
        }
    }
}