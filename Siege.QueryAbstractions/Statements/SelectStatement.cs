using System;

namespace Siege.QueryAbstractions.Statements
{
    public class SelectStatement<TSelectType>
    {
        private Type selectedType;

        public SelectStatement()
        {
            this.selectedType = typeof (TSelectType);
        }

        public Type SelectedType
        {
            get { return selectedType; }
        }
    }
}