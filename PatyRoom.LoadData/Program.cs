using PatyRoom.LoadData;
using System.Security.Cryptography;

byte[] keyBytes = new byte[32]; // 256 бит
using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(keyBytes);
}

string jwtSecretKey = Convert.ToBase64String(keyBytes);
Console.WriteLine(jwtSecretKey);
//LoadUsers loader = new LoadUsers();
//await loader.PrintUser();
//Console.WriteLine("Загружено");