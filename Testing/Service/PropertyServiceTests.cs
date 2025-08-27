using Application.Dto;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Interface;
using Infraestructure.Repository;
using MongoDB.Driver.Linq;
using Moq;
using NUnit.Framework;

namespace Testing.Service
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _propertyRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private PropertyService _service;

        [SetUp]
        public void SetUp()
        {
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PropertyService(_propertyRepositoryMock.Object, _mapperMock.Object);
        }

        // -------------------------------
        // GetByIdAsync
        // -------------------------------
        [Test]
        public async Task GetByIdAsync_ReturnsPropertyDetailDto_WhenExists()
        {
            var propertyId = "123";
            var expected = new PropertyDetailDto
            {
                Id = propertyId,
                IdOwner = "owner-001",
                Name = "Casa Bonita",
                Address = "Calle 123",
                Price = 250000m,
                Image = "casa.jpg",
                Year = 2021,
                OwnerName = "Juan Pérez"
            };

            _propertyRepositoryMock
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _service.GetByIdAsync(propertyId, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Id);
            Assert.AreEqual("Casa Bonita", result.Name);
            Assert.AreEqual(250000m, result.Price);

            _propertyRepositoryMock.Verify(
                r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {           
            var propertyId = "999"; // id inexistente

            _propertyRepositoryMock
                .Setup(r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PropertyDetailDto?)null);

            var result = await _service.GetByIdAsync(propertyId, CancellationToken.None);

            Assert.IsNull(result);

            _propertyRepositoryMock.Verify(
                r => r.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }


        // -------------------------------
        // CreateAsync
        // -------------------------------
        [Test]
        public async Task CreateAsync_ReturnsTrue_WhenRepositoryCreatesSuccessfully()
        {
            var createDto = new PropertyItemCreateDto
            {
                Name = "Casa Bonita",
                Address = "Calle 123",
                Price = 150000m,
                CodeInternal = "INT-001",
                Year = 2020,
                OwnerName = "Juan Pérez",
                OwnerAddress = "Avenida Siempre Viva 742",
                OwnerPhoto = "photo.jpg",
                Birthday = new DateTime(1980, 5, 12),
                PropertyFile = "file.jpg",
                DateSale = DateTime.UtcNow,
                TraceName = "Registro1",
                Value = 200000m,
                Tax = 15000m
            };

            _propertyRepositoryMock
                .Setup(r => r.CreateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _service.CreateAsync(createDto, CancellationToken.None);

            Assert.IsTrue(result);
    
            
        }


        [Test]
        public async Task CreateAsync_ReturnsFalse_WhenRepositoryFails_WithFullDto()
        {
            var createDto = new PropertyItemCreateDto
            {
                Name = "Casa Incompleta",
                Address = "Calle Sin Número",
                Price = 0m, 
                CodeInternal = "ERR-001",
                Year = 1900,
                OwnerName = "Propietario Desconocido",
                OwnerAddress = "Sin dirección",
                OwnerPhoto = string.Empty,
                Birthday = null,
                PropertyFile = "archivo.png",
                DateSale = null,
                TraceName = null,
                Value = null,
                Tax = null
            };

            _propertyRepositoryMock
                .Setup(r => r.CreateAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _service.CreateAsync(createDto, CancellationToken.None);

            Assert.IsFalse(result);
        }


        // -------------------------------
        // GetAllPropertiesByFilter
        // -------------------------------
        [Test]
        public async Task GetAllPropertiesByFilter_ShouldReturnSuccess_WhenPropertiesExist()
        {
            var filter = new PropertyFilterDto { PageNumber = 1, PageSize = 10 };

            var properties = new List<Property>
            { new Property { Id = "1", Name = "Casa 1" }  };

            var totalCount = properties.Count;
          
            var propertyItemDtos = new List<PropertyItemDto>
            {
                new PropertyItemDto { Id = "1" }
            };
           
            _propertyRepositoryMock
                .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilterDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((propertyItemDtos.AsEnumerable(), totalCount));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<PropertyItemDto>>(It.IsAny<IEnumerable<Property>>()))
                .Returns(new List<PropertyItemDto>
                {
                new PropertyItemDto { Id = "1", }
                });

            var result = await _service.GetAllPropertiesByFilter(filter, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.TotalCount, Is.EqualTo(totalCount));
            Assert.That(result.PageSize, Is.EqualTo(filter.PageSize));
            Assert.That(result.PageNumber, Is.EqualTo(filter.PageNumber));
        }

        [Test]
        public async Task GetAllPropertiesByFilter_ShouldReturnEmpty_WhenNoPropertiesAreFound()
        {          
            var filter = new PropertyFilterDto { PageNumber = 1, PageSize = 10 };

            var properties = new List<Property>(); 
            var totalCount = 0; 

            var propertyItemDtos = new List<PropertyItemDto>(); 
         
            _propertyRepositoryMock
                 .Setup(r => r.GetFilteredAsync(It.IsAny<PropertyFilterDto>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((propertyItemDtos.AsEnumerable(), totalCount));
           
            _mapperMock
                .Setup(m => m.Map<IEnumerable<PropertyItemDto>>(It.IsAny<IEnumerable<Property>>()))
                .Returns(propertyItemDtos);

            var result = await _service.GetAllPropertiesByFilter(filter, new CancellationToken());
                       
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items.Count, Is.EqualTo(0));
            Assert.That(result.TotalCount, Is.EqualTo(0));        
            Assert.That(result.PageSize, Is.EqualTo(filter.PageSize));
            Assert.That(result.PageNumber, Is.EqualTo(filter.PageNumber));
        }
    }
}
