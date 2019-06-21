using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<Guid?> GetImageByChecksum(String data)
        {
            var checksum = CalculateChecksum(data);
            var imageId = await _dbContext.Images.Where(x => x.Checksum == checksum).Select(x => x.Id).SingleOrDefaultAsync();
            return imageId == Guid.Empty ? (Guid?)null : imageId;
        }

        public async Task<Guid> StoreImage(Byte[] binaryImage, String data)
        {
            var checksum = CalculateChecksum(data);

            var image = new Image
            {
                Id = Guid.NewGuid(),
                Data = binaryImage,
                Checksum = checksum
            };

            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image.Id;
        }

        public async Task<Image> LoadImage(Guid imageId)
        {
            return await _dbContext.Images.SingleOrDefaultAsync(x => x.Id == imageId);
        }

        private String CalculateChecksum(String data)
        {
            MD5 md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}