//using Azure.Storage.Blobs;

public interface IAppFeatures
{
    public string AppName { get; set; }

    public string Key { get; set; }

    public string Audience { get; set; }

    public string Issuer { get; set; }

    public DateTime Expiry { get; set; }

    public byte Hours { get; set; }
}

//public interface IBlobConnection
//{
//    public BlobContainerClient Client { get; }
//}