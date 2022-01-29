using MB.DBST.Attributes;

using MB.DBST.Interfaces;

namespace DBST_Test.Models
{
    public class TestModel : IDBST
    {
        [DBSTKey("id")]
        [DBSTPrimaryKey]
        [DBSTAutoIncrement]
        public int Id { get; set; }

        [DBSTKey("test1")]
        public string Test1 { get; set; }
    }
}
