# Archer.Extension
NUGET package | UITC每個專案都會用的方法獨立出來放這

## .Net version
.NET Standard 2.0

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

## WatermarkHelper
命名空間: `Archer.Extension.Images`

| function name | description |
| --- | --- | --- | --- |
| AddWatermarkToImages | 給圖片增加浮水印 |
| RemoveWatermark | [實驗性功能] 移除浮水印 |

### 給圖片增加浮水印
```C#
using Archer.Extension.Images;

string base64Image = string.Empty;
using (FileStream fs = new FileStream(@"D:\暫時\IMG_0123.jpg", FileMode.Open, FileAccess.Read))
{
    byte[] buffer = new byte[fs.Length];
    fs.Read(buffer, 0, buffer.Length);

    base64Image = Convert.ToBase64String(buffer);
}

WatermarkHelper watermarkHelper = new WatermarkHelper();

string[] watermarkImages = watermarkHelper.AddWatermarkToImages(new string[] { base64Image },
    "想按摩師傅的小腿肚，嘿嘿嘿",
    new Font(FontFamily.GenericSerif, 50, FontStyle.Bold, GraphicsUnit.Pixel),
    Color.FromArgb(127, 0, 153, 153),
    WatermarkPosition.MiddleCenter);
```

### 移除浮水印
```C#
// 浮水印顏色
Color watermarkColor = Color.FromArgb(127, 0, 153, 153);

// 顏色相似度閾值
int threshold = 100;

string removedResult = watermarkHelper.RemoveWatermark(watermarkImages[0], watermarkColor, threshold);

byte[] buffer2 = Convert.FromBase64String(removedResult);

using (FileStream fs2 = new FileStream($@"D:\暫時\IMG_0123_watermarked_removedResult.jpg", FileMode.Create, FileAccess.Write))
{
    fs2.Write(buffer2, 0, buffer2.Length);
}
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