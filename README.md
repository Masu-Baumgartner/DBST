# DBST
A wrapper for a mysql database, which uses models to display data

## Examples ##

###Setup mysql connection###

```csharp
MySQLActions.ConnectionString = $"server=<serverip>;" +
                                            $"port=<serverport>;" +
                                            $"database=<databasename>;" +
                                            $"uid=<username>;" +
                                            $"pwd=<password>";
```

###Initialise table###

```csharp
Table = new DBST<TestModel>("test");
```

###Insert model###

```csharp
Table.Insert(new TestModel()
            {
                Test1 = "foxy"
            });
```

###More Examples###
```csharp
MySQLActions.ConnectionString = $"server=<serverip>;" +
                                                        $"port=<serverport>;" +
                                                        $"database=<databasename>;" +
                                                        $"uid=<username>;" +
                                                        $"pwd=<password>";

            Table = new DBST<TestModel>("test");

            Table.Insert(new TestModel()
            {
                Test1 = "foxy"
            });

            TestModel model = null;

            var m = Table.AsList().Find(x => x.Test1 == "foxy");

            m.Test1 = "fox";

            Table.Modify(m);

            Table.Delete(model);
```
