using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.DTOs.Products
{
    public record ImageUploadRequestDto
    {
        /// <summary>
        /// Файл изображения для загрузки.
        /// </summary>
        [Required] // Указываем, что файл обязателен
        public IFormFile File { get; set; }

        /// <summary>
        /// Является ли это изображение главным.
        /// </summary>
        public bool IsMain { get; set; }
        public string? AltText { get; set; } // Альтернативный текст для изображения, может быть null
    }
}
