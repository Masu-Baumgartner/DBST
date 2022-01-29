using MB.DBST.Attributes;

namespace MB.DBST.Interfaces
{
    /// <summary>
    /// This class is an interface for implementing a table model in DBST
    /// </summary>
    public interface IDBST
    {
        [DBSTKey("id")]
        public int Id { get; set; }
    }
}
