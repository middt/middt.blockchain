using System.Security.Cryptography;


Blockchain blockchain = new Blockchain(2);

// Adding transactions and blocks
RSAParameters alicePrivateKey = blockchain.GetOrCreateWallet("Alice");
RSAParameters bobPrivateKey = blockchain.GetOrCreateWallet("Bob");

blockchain.AddBlock(new Block(1, DateTime.Now, new List<Transaction>
        {
            new Transaction("Alice", "Bob", 10, alicePrivateKey),
            new Transaction("Bob", "Charlie", 5, bobPrivateKey)
        }, blockchain.GetLatestBlock().Hash));

blockchain.AddBlock(new Block(2, DateTime.Now, new List<Transaction>
        {
            new Transaction("Charlie", "Alice", 2, blockchain.GetOrCreateWallet("Charlie"))
        }, blockchain.GetLatestBlock().Hash));

// Output the blockchain
foreach (var block in blockchain)
{
    Console.WriteLine($"Block: {block.Index}, Hash: {block.Hash}");
    foreach (var transaction in block.Transactions)
    {
        Console.WriteLine(transaction);
    }
    Console.WriteLine();
}

// Check if the blockchain is valid
Console.WriteLine($"Is Blockchain Valid: {blockchain.IsValidChain()}");
Console.ReadLine();



