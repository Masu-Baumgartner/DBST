using System;
using System.Collections;
using System.Collections.Generic;

using MB.DBST.Attributes;
using MB.DBST.Interfaces;

namespace MB.DBST
{
    public class DBST<T> : IEnumerable, IDBSTBase
    {
        private string Table { get; set; }
        private List<T> Data { get; set; }
        private string InsertStatement { get; set; }
        private string UpdateStatement { get; set; }
        private string CreateStatement { get; set; }
        private string KeyField { get; set; } = null;

        public DBST(string table)
        {
            Table = table;
            Data = new List<T>();

            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            List<string> types = new List<string>();
            List<int> lenght = new List<int>();
            List<bool> autoIncrement = new List<bool>();
            List<bool> primaryKey = new List<bool>();

            Type type = typeof(T);

            foreach (var prop in type.GetProperties())
            {
                int cl = -1;
                bool ai = false;
                bool pk = false;

                var res = prop.GetCustomAttributes(true);
                if (res != null)
                {
                    foreach (var obj in res)
                    {
                        if (obj is DBSTKey key)
                        {
                            keys.Add(key.Key);
                            values.Add(prop.Name);
                        }

                        if (obj is DBSTLenght dbstl)
                        {
                            cl = dbstl.Lenght;
                        }

                        if(obj is DBSTAutoIncrement)
                        {
                            ai = true;
                        }

                        if(obj is DBSTPrimaryKey)
                        {
                            pk = true;
                            KeyField = prop.Name;
                        }
                    }
                }

                if (cl == -1)
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "Int32":
                            cl = 255;
                            break;
                        case "String":
                            cl = 1024;
                            break;
                        default:
                            cl = 255;
                            break;
                    }
                }

                switch(prop.PropertyType.Name)
                {
                    case "Int32":
                        types.Add("INT");
                        break;
                    case "String":
                        types.Add("VARCHAR");
                        break;
                    default:
                        types.Add("VARCHAR");
                        break;
                }

                lenght.Add(cl);
                autoIncrement.Add(ai);
                primaryKey.Add(pk);
            }

            InsertStatement = $"INSERT INTO {Table} (";

            for (int i = 0; i < keys.Count; i++)
            {
                if (i != keys.Count - 1)
                    InsertStatement += keys[i] + ", ";
                else
                    InsertStatement += keys[i];
            }

            InsertStatement += ") VALUES (";

            for (int i = 0; i < values.Count; i++)
            {
                if (i != keys.Count - 1)
                    InsertStatement += "@" + values[i] + ", ";
                else
                    InsertStatement += "@" + values[i];
            }

            InsertStatement += "); SELECT LAST_INSERT_ID();";

            UpdateStatement = $"UPDATE {Table} SET ";

            for (int i = 0; i < keys.Count; i++)
            {
                if (i != keys.Count - 1)
                    UpdateStatement += keys[i] + " = @" + values[i] + ", ";
                else
                    UpdateStatement += keys[i] + " = @" + values[i];
            }

            string CreateTable = $"CREATE TABLE IF NOT EXISTS {table} ( ";

            for(int i = 0; i < keys.Count; i++)
            {
                CreateTable += $"`{keys[i]}` {types[i]}({lenght[i]}) NOT NULL {(autoIncrement[i] ? "AUTO_INCREMENT" : "")}";

                if (i != keys.Count - 1)
                    CreateTable += " , ";
            }

            for(int i = 0; i < keys.Count; i++)
            {
                if(primaryKey[i])
                {
                    CreateTable += $" , PRIMARY KEY (`{keys[i]}`)";
                }
            }

            CreateTable += " );";


            CreateStatement = CreateTable;

            MySQLActions.SaveData(CreateTable, new object { }).Wait();
            ReloadData();
        }

        public void ReloadData()
        {
            lock(Data)
            {
                Data = MySQLActions.LoadData<T, dynamic>($"SELECT * FROM {Table}", new object { }).Result;
            }
        }

        public void Delete(T model)
        {
            lock(Data)
            {
                MySQLActions.RemoveData($"DELETE FROM {Table} WHERE id=@Id", model).Wait();
                Data.Remove(model);
            }
        }

        public void Modify(T model)
        {
            MySQLActions.SaveData(UpdateStatement + $" WHERE id=@Id", model).Wait();

            int index = -1;

            lock (Data)
            {
                foreach (var cm in Data)
                {
                    if (((IDBST)cm).Id == ((IDBST)model).Id)
                    {
                        index = Data.IndexOf(cm);

                        break;
                    }
                }

                if (index != -1)
                {
                    Data.RemoveAt(index);
                    Data.Insert(index, model);
                }
            }
        }

        public int Insert(T model)
        {
            int i = MySQLActions.SaveDataAndReturnKey(InsertStatement, model).Result;

            model.GetType().GetProperty(KeyField).SetValue(model, i, null);

            lock(Data)
            {
                Data.Add(model);
            }

            return i;
        }

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        public List<T> AsList()
        {
            List<T> list = new List<T>();

            lock(Data)
            {
                foreach (var m in Data)
                    list.Add(m);
            }

            return list;
        }
    }
}
