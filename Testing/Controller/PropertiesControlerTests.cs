using Application.Common;
using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MillionApi.Controllers;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Testing.Controller
{ 
[TestFixture]
public class PropertiesControllerTests: IDisposable
{
    private Mock<IPropertyService> _propertyServiceMock;
    private PropertiesController _controller;

    [SetUp]
    public void Setup()
    {
        // Se inicializan los mocks y el controlador antes de cada test
        _propertyServiceMock = new Mock<IPropertyService>();
        _controller = new PropertiesController(_propertyServiceMock.Object);
    }

    [Test]
    public async Task GetById_WhenIdIsInvalid_ReturnsNotFound()
    {
        var invalidId = "invalid-id-format";
        var result = await _controller.GetById(invalidId, CancellationToken.None) as NotFoundResult;
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetById_WhenPropertyIsNotFound_ReturnsNotFound()
    {        
        var id = "507f1f77bcf86cd799439011"; // Un ObjectId válido

        _propertyServiceMock
            .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PropertyDetailDto)null);

        var result = await _controller.GetById(id, CancellationToken.None) as NotFoundResult;

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetById_WhenPropertyIsFound_ReturnsOkWithProperty()
    {        
        var id = "507f1f77bcf86cd799439011";
        var property = new PropertyDetailDto { Id = id, Name = "Test Property" };
                   
        _propertyServiceMock
            .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        var result = await _controller.GetById(id, CancellationToken.None) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.InstanceOf<PropertyDetailDto>());
        var returnedProperty = result.Value as PropertyDetailDto;
        Assert.That(returnedProperty.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetById_WhenServiceThrowsException_ReturnsInternalServerError()
    {       
        var id = "507f1f77bcf86cd799439011";
        var errorMessage = "Simulated service error";
                   
        _propertyServiceMock
            .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new System.Exception(errorMessage));
                    
        var result = await _controller.GetById(id, CancellationToken.None) as ObjectResult;
                    
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(500));
        Assert.That(result.Value.ToString(), Does.Contain(errorMessage));
    }


        [Test]
        public async Task GetAllPropertiesByFilter_ShouldReturnOkWithData()
        {
            // Arrange
            var filter = new PropertyFilterDto { PageNumber = 1, PageSize = 10 };
                    
            var expectedPaginationResult = new PaginationResult<PropertyItemDto>();
            expectedPaginationResult.Items = new List<PropertyItemDto>
            {
                new PropertyItemDto { Id = "1", Name = "Casa 1" }
            };
            expectedPaginationResult.TotalCount = 1;
            expectedPaginationResult.PageNumber = 1;
            expectedPaginationResult.PageSize = 10;

            _propertyServiceMock
                .Setup(s => s.GetAllPropertiesByFilter(It.IsAny<PropertyFilterDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPaginationResult);

            var result = await _controller.GetAllPropertiesByFilter(filter, CancellationToken.None) as OkObjectResult;
                       
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.InstanceOf<PaginationResult<PropertyItemDto>>());

            var returnedData = result.Value as PaginationResult<PropertyItemDto>;
            Assert.That(returnedData.TotalCount, Is.EqualTo(1));
            Assert.That(returnedData.Items.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_ValidDto_ReturnsOkWithTrue()
        {
            var dto = new PropertyItemCreateDto
            {
                Name = "Casa de prueba",
                Address = "Calle Falsa 123",
                Price = 250000.00M,
                CodeInternal = "A1B2C3D4",
                Year = 2023,
                OwnerName = "Juan Pérez",
                OwnerAddress = "Calle Larga 456",
                OwnerPhoto = "foto.jpg",
                Birthday = new DateTime(1990, 5, 15),
                PropertyFile = "documento.png"
            };
            var cancellationToken = new CancellationToken();

            _propertyServiceMock
                .Setup(s => s.CreateAsync(It.IsAny<PropertyItemCreateDto>(), cancellationToken))
                .ReturnsAsync(true); 

            var actionResult = await _controller.Create(dto, cancellationToken);
            var result = actionResult as OkObjectResult;
                       
            Assert.IsNotNull(result);

            Assert.That(result.StatusCode, Is.EqualTo(200));

            Assert.That(result.Value, Is.True);

        }

            [Test]
            public async Task Create_NullDto_ReturnsBadRequest()
            {
                PropertyItemCreateDto dto = null;
                var cancellationToken = new CancellationToken();
                           
                var actionResult = await _controller.Create(dto, cancellationToken);
                var result = actionResult as BadRequestObjectResult;
                Assert.IsNotNull(result);
                Assert.That(result.StatusCode, Is.EqualTo(400));
                Assert.That(result.Value, Is.EqualTo(400));
        }

            [Test]
            public async Task Create_ServiceThrowsException_ReturnsStatusCode500()
            {
                var dto = new PropertyItemCreateDto
                {
                    Name = "Casa de prueba",
                    PropertyFile = "documento.png"
                };
                var cancellationToken = new CancellationToken();

                _propertyServiceMock
                    .Setup(s => s.CreateAsync(It.IsAny<PropertyItemCreateDto>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new System.Exception("Error de prueba"));
               
                var actionResult = await _controller.Create(dto, cancellationToken);
                var result = actionResult as ObjectResult;

                Assert.IsNotNull(result);
                Assert.That(result.StatusCode, Is.EqualTo(500));
            }

         [TearDown]
        public void TearDown()
        {       
            _controller.Dispose();
        }
              
        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}