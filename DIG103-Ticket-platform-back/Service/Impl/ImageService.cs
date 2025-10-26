using DIG103_Ticket_platform_back.Model;
using DIG103_Ticket_platform_back.Repository;

namespace DIG103_Ticket_platform_back.Service.Impl;

public class ImageService(IImageRepository imageRepository, IMinioService minioService): IImageService
{
    public async Task<Image> UploadAsync(IFormFile file, string folder)
    {
        var imagePath = await minioService.UploadImageAsync(file, folder);

        var image = new Image()
        {
            ImagePath = imagePath,
            ContentType = file.ContentType,
            UploadedAt = DateTime.Now
        };

        return await imageRepository.CreateAsync(image);
    }

    public async Task DeleteAsync(int id)
    {
        var image = await imageRepository.GetByIdAsync(id);

        if (image == null)
        {
            throw new KeyNotFoundException("no image found for deletion");
        }
        
        await minioService.DeleteImageAsync(image.ImagePath);
        await imageRepository.DeleteAsync(id);
            
    }
}