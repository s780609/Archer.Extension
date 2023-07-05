# Archer.Extension
一些常用的東西

## Using
```C#
using Archer.Extension
```

## Security helper
an encrypting and decryting helper

| function name | paramter type | return type | 
| --- | --- | --- | 
| EncryptConn | string | string |
| DecryptConn | string | string |
| GetMd5Hash | string | string |
| GetSha256Hash | string | byte[] |

example:
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

## Extension method  

Some useful extension method  

| function name | paramter type | return type | 
| --- | --- | --- | 
| ThrowIfNull | object | Exception? |

example: 
```C#
users.ThrowIfNull(nameof(users));
```