using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadsRoot;

        public FileStorageService(IConfiguration configuration)
        {
            // Configure uploads folder path (e.g., "uploads/images/books")
            _uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "images", "books");
        }

        public void DeleteBookImages(int bookId)
        {
            string bookPath = Path.Combine(_uploadsRoot, $"book-{bookId}");
            if (Directory.Exists(bookPath))
            {
                foreach (var file in Directory.GetFiles(bookPath))
                    File.Delete(file);

                Directory.Delete(bookPath, true);
            }
        }
    }

}
