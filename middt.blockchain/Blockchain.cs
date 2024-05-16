// Blockchain class
using System.Security.Cryptography;

public class Blockchain : IEnumerable<Block>
{
    private List<Block> chain;
    private int difficulty;
    private Dictionary<string, RSAParameters> wallets; // Store public-private key pairs for each participant

    public Blockchain(int difficulty)
    {
        this.difficulty = difficulty;
        chain = new List<Block> { CreateGenesisBlock() };
        wallets = new Dictionary<string, RSAParameters>();
    }

    public Block CreateGenesisBlock()
    {
        return new Block(0, DateTime.Now, new List<Transaction>(), null);
    }

    public Block GetLatestBlock()
    {
        return chain[^1];
    }

    public void AddBlock(Block newBlock)
    {
        newBlock.PreviousHash = GetLatestBlock().Hash;
        newBlock.MineBlock(difficulty);
        chain.Add(newBlock);
    }

    public bool IsValidChain()
    {
        for (int i = 1; i < chain.Count; i++)
        {
            Block currentBlock = chain[i];
            Block previousBlock = chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }

    public RSAParameters GetOrCreateWallet(string participant)
    {
        if (!wallets.ContainsKey(participant))
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                wallets[participant] = rsa.ExportParameters(true);
            }
        }
        return wallets[participant];
    }

    public IEnumerator<Block> GetEnumerator()
    {
        return chain.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}