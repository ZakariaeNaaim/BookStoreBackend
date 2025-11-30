using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.Interfaces
{
    public interface IFileStorageService
    {
        void DeleteBookImages(int bookId);
    }

}
