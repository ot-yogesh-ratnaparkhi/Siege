﻿namespace Siege.Container.UnitTests.ContextualTests.Classes
{
    public class DefaultTestService : IBaseService
    {
        public DefaultTestService(ITestRepository repository)
        {
            Repository = repository;
        }

        public ITestRepository Repository { get; set; }
    }
}
