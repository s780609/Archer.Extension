# Archer.Extension
NUGET package | UITC每個專案都會用的方法獨立出來放這

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

## JwtHelper
| function name | paramter type | return type | description |
| --- | --- | --- | --- |
| GenerateToken | object | Exception? | Generate token by token model
| ValidateToken | string | bool | validate token |

## DatabaseHelper
| function name | paramter type | return type | description |
| --- | --- | --- | --- |
| CreateConnectionBy | string, DatabaseTypeEnum | IDbConnection | create IDbConnection by connection string |

## Extension method  

Some useful extension method  

| function name | paramter type | return type | 
| --- | --- | --- | 
| ThrowIfNull | object | Exception? |

example: 
```C#
users.ThrowIfNull(nameof(users));
```