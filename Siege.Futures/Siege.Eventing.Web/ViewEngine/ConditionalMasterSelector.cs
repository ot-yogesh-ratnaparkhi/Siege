﻿using System;

namespace Siege.Eventing.Web.ViewEngine
{
    public class ConditionalMasterSelector<T> : ConditionalTemplateSelector<T>, IMasterTemplateSelector
    {
        public ConditionalMasterSelector(Func<T, bool> condition, string path) : base(condition, path)
        {
            
        }
    }
}