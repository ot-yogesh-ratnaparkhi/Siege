using System.Reflection.Emit;

namespace Siege.DynamicTypeGeneration
{
    public class BuilderBundle
    {
        public TypeBuilder TypeBuilder { get; set; }
        public ModuleBuilder ModuleBuilder { get; set; }
    }

    public class MethodBuilderBundle : BuilderBundle
    {
        public MethodBuilder MethodBuilder { get; set; }

        public MethodBuilderBundle(BuilderBundle bundle)
        {
            this.TypeBuilder = bundle.TypeBuilder;
            this.ModuleBuilder = bundle.ModuleBuilder;
        }
    }
}