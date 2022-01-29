using System;

namespace MB.DBST.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DBSTKey : Attribute
    {
        readonly string positionalString;
        /// <summary>
        /// constructor for setting the column key
        /// </summary>
        /// <param name="key">key as string</param>
        public DBSTKey(string key)
        {
            positionalString = key;
        }
        internal string Key
        {
            get { return positionalString; }
        }
    }
}
