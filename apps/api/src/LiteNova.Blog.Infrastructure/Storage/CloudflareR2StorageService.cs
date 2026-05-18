using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace LiteNova.Blog.Infrastructure.Storage;

public sealed class CloudflareR2StorageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;
    private readonly string _publicUrl;

    public CloudflareR2StorageService(IAmazonS3 s3, IConfiguration configuration)
    {
        _s3 = s3;
        _bucket = configuration["CLOUDFLARE_R2_BUCKET_NAME"] ?? string.Empty;
        _publicUrl = configuration["CLOUDFLARE_R2_PUBLIC_URL"] ?? string.Empty;
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var key = $"uploads/{Guid.NewGuid()}-{fileName}";
        await _s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType
        }, cancellationToken);
        return $"{_publicUrl.TrimEnd('/')}/{key}";
    }
}
