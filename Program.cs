using System;
using System.Linq;
using Yubico.Core.Buffers;
using Yubico.YubiKey;
using Yubico.YubiKey.Piv;

namespace YubiKeyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var yubiKey = YubiKeyDevice.FindAll().First();
            using (var piv = new PivSession(yubiKey))
            {
                piv.KeyCollector = KeyCollectorPrompt;
                var publicKey = piv.GenerateKeyPair(PivSlot.Authentication, PivAlgorithm.EccP256);
                // piv.Decrypt(PivSlot.Authentication,)
                Console.WriteLine();
                Console.WriteLine($"Public Key: {Hex.BytesToHex(publicKey.PivEncodedPublicKey.ToArray())}");
            }
        }

         static bool KeyCollectorPrompt(KeyEntryData entryData)
        {
            switch (entryData.Request)
            {
                case KeyEntryRequest.AuthenticatePivManagementKey:
                    // Console.Write("Enter the PIV management Key:");
                    // string strKey = Console.ReadLine();
                    string strKey = "010203040506070801020304050607080102030405060708";
                    entryData.SubmitValue(Hex.HexToBytes(strKey).ToArray());
                    return true;
            }

            return false;
        }
    }
}
