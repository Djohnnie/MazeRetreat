using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MazeRetreat.Api.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace MazeRetreat.Api.Logic
{
    public class ImageLogic
    {
        private readonly DatabaseContext _dbContext;

        public ImageLogic(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Image> StoreImage(Byte[] binaryImage)
        {
            var checksum = CalculateChecksum(binaryImage);
            var existingImage = await _dbContext.Images.SingleOrDefaultAsync(x => x.Checksum == checksum);
            if (existingImage != null)
            {
                return existingImage;
            }

            var image = new Image
            {
                Id = Guid.NewGuid(),
                Data = binaryImage,
                Checksum = checksum
            };
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<Image> LoadImage(Guid imageId)
        {
            return await _dbContext.Images.SingleOrDefaultAsync(x => x.Id == imageId);
        }

        private String CalculateChecksum(Byte[] bytes)
        {
            MD5 md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(bytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}