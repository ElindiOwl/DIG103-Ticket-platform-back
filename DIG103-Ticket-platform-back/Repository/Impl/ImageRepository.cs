using DIG103_Ticket_platform_back.Data;
using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Repository.Impl;

public class ImageRepository(AppliactionDbContext context) : IImageRepository
{
    public async Task<Image> CreateAsync(Image image)
    {
        context.Images.Add(image);

        await context.SaveChangesAsync();
        return image;
    }

    public async Task<Image?> GetByIdAsync(int id)
    {
        return await context.Images.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var image = await GetByIdAsync(id);

        if (image == null)
        {
            return false;
        }

        context.Images.Remove(image);
        
        await context.SaveChangesAsync();
        return true;
    }
}