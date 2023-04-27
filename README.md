# Archer.Extension

一些常用的東西

```C#
using Archer.Extension
```

## Security helper

a encrypting and decryting helper

```C#
string _keyConn = "keyConn";
string _ivConn = "ivConn";
string _keyData = "keyData";
string _ivData = "ivData";

SecurityHelper helper = new SecurityHelper(_keyConn, _ivConn, _keyData, _ivData);

string connectionString = "connection string";

string encryptedConnectionString = helper.EncryptConn(connectionString);
string decryptedConnectionString = helper.DecryptConn(encryptedConnectionString);
```

## extension method  
some useful static extension method
