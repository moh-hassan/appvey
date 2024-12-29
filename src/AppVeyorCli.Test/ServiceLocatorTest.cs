using AppVeyor.Api;

namespace AppVeyor.Test
{
    public class ServiceLocatorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RegisterService_ShouldRegisterService()
        {
            // Arrange
            var dummyService = new DummyService();

            // Act
            ServiceLocator.RegisterService<IDummyService>(dummyService);

            // Assert
            var registeredService = ServiceLocator.GetService<IDummyService>();
            Assert.That(registeredService, Is.EqualTo(dummyService));
        }

        [Test]
        public void GetService_ShouldReturnRegisteredService()
        {
            // Arrange
            var dummyService = new DummyService();
            ServiceLocator.RegisterService<IDummyService>(dummyService);

            // Act
            var service = ServiceLocator.GetService<IDummyService>();

            // Assert
            Assert.That(service, Is.EqualTo(dummyService));
        }

        [Test]
        public void GetService_ShouldThrowException_WhenServiceNotRegistered()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => ServiceLocator.GetService<INonRegisteredService>());
        }

        public interface IDummyService { }

        public class DummyService : IDummyService { }

        public interface INonRegisteredService { }
     }
}
