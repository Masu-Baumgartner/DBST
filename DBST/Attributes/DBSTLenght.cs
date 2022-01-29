using System;

namespace MB.DBST.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DBSTLenght : Attribute
    {
        readonly int lenght = -1;
        public DBSTLenght(int lenght)
        {
            this.lenght = lenght;
        }

        internal int Lenght
        {
            get { return lenght; }
        }
    }
}
