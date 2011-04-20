namespace Siege.Requisitions.UnitTests.TestClasses
{
    public class SpecialTestCase
    {
        public string StringInput { get; set; }
        public int IntInput { get; set; }
        public bool BoolInput { get; set; }
        public TestEnum EnumInput { get; set; }
        public TestStruct StructInput { get; set; }

        public SpecialTestCase(string stringArg, int intArg, bool boolArg, TestEnum enumArg, TestStruct structArg)
        {
            StringInput = stringArg;
            IntInput = intArg;
            BoolInput = boolArg;
            EnumInput = enumArg;
            StructInput = structArg;
        }
    }
}
