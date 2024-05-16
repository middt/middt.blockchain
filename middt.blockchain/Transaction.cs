using System.Security.Cryptography;
using System.Text;

public class Transaction
{
    // Transaction properties
    public string Sender { get; }
    public string Receiver { get; }
    public double Amount { get; }
    public DateTime Timestamp { get; }
    public byte[] Signature { get; } // Digital signature field

    // Constructor for creating a transaction with a digital signature
    public Transaction(string sender, string receiver, double amount, RSAParameters senderPrivateKey)
    {
        // Initialize transaction properties
        Sender = sender;
        Receiver = receiver;
        Amount = amount;
        Timestamp = DateTime.Now;
        // Sign the transaction data using the sender's private key
        Signature = SignData($"{Sender}{Receiver}{Amount}{Timestamp}", senderPrivateKey);
    }

    // Method for signing transaction data using the sender's private key
    private byte[] SignData(string data, RSAParameters privateKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(privateKey);
            // Sign the data using the SHA256 hashing algorithm
            return rsa.SignData(Encoding.UTF8.GetBytes(data), new SHA256CryptoServiceProvider());
        }
    }

    // Method for verifying the digital signature using the sender's public key
    public bool VerifySignature(RSAParameters senderPublicKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(senderPublicKey);
            // Verify the data against the digital signature
            return rsa.VerifyData(Encoding.UTF8.GetBytes($"{Sender}{Receiver}{Amount}{Timestamp}"), new SHA256CryptoServiceProvider(), Signature);
        }
    }

    // Override ToString method to provide a string representation of the transaction
    public override string ToString()
    {
        return $"{Sender} sent {Amount} to {Receiver} at {Timestamp}";
    }
}
