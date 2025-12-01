using Minio;
using Minio.DataModel.Args;

namespace DIG103_Ticket_platform_back.Service.Impl;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _publicUrl;
    private bool _bucketChecked = false;
    private readonly SemaphoreSlim _bucketCheckLock = new(1, 1);

    public MinioService(IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("MinIO");
        var endpoint = minioConfig["Endpoint"];
        var accessKey = minioConfig["AccessKey"];
        var secretKey = minioConfig["SecretKey"];
        _bucketName = minioConfig["BucketName"];
        _publicUrl = minioConfig["PublicUrl"] ?? $"http://{endpoint}/{_bucketName}";

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            // remove then https is implemented
            .WithSSL(false)
            .Build();
        
    }

    private async Task EnsureBucketExistsAsync()
    {
        if (_bucketChecked)
            return;

        await _bucketCheckLock.WaitAsync();
        try
        {
            if (_bucketChecked)
                return;

            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            bool isFound = await _minioClient.BucketExistsAsync(bucketExistsArgs);

            if (!isFound)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(_bucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs);
            }

            _bucketChecked = true;
        }
        finally
        {
            _bucketCheckLock.Release();
        }
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        await EnsureBucketExistsAsync();
        
        string actualContentType = await DetectContentType(file);

        if (!actualContentType.StartsWith("image/"))
        {
            throw new InvalidOperationException("file is not a valid image");
        }

        var fileExtension = actualContentType switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/gif" => ".gif",
            "image/webp" => ".webp",
            _ => throw new InvalidOperationException("Wrong image type")
        };

        var objectName = $"{folder}/{Guid.NewGuid()}{fileExtension}";

        using var stream = file.OpenReadStream();

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(actualContentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        return objectName;
    }

    public string GetPublicUrl(string imagePath)
    {
        return $"{_publicUrl}/{imagePath}";
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if(string.IsNullOrEmpty(imagePath))
        {
            return;
        }

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(imagePath);

        await _minioClient.RemoveObjectAsync(removeObjectArgs);
    }

    private async Task<string> DetectContentType(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var buffer = new byte[12];
        await stream.ReadAsync(buffer, 0, buffer.Length);

        if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF)
            return "image/jpeg";

        if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
            return "image/png";

        if (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46)
            return "image/gif";

        if (buffer[8] == 0x57 && buffer[9] == 0x45 && buffer[10] == 0x42 && buffer[11] == 0x50)
            return "image/webp";

        throw new InvalidOperationException("Unsupported file type");
    }
}