using System.Threading.Tasks;
using back.Controller;
using back.Models.DTO;
using back.Service.Implementation;
using back.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace TestBack
{
    public class BackTesting
    {
        private readonly PiezometerController _controller;
        private readonly IPiezometer _piezometerService;

        public BackTesting()
        {
            _piezometerService = new PiezometerImpl();
            _controller = new PiezometerController(_piezometerService);
        }

        [Fact]
        public async Task Get_Ok()
        {
            var result = await _controller.GetAllPiezometers();

            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Get_Measurements_Ok()
        {
            var piezometerId = 1; // Example piezometer ID
            var result = await _controller.GetAllMeasurementsPiezometersById(piezometerId);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Get_Measurements_Piezometer_By_Id_Ok()
        {
            var ids = new GetMeasurementsPiezometersByIds
            {
                PiezometersIds = new System.Collections.Generic.List<int> { 1, 2, 3 } // Example IDs
            };
            var result = await _controller.GetMeasurementsPiezometerByIds(ids);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Get_Converted_Piezometers_Ok()
        {
            var result = await _controller.GetConvertedPiezometers();
            Assert.IsType<ObjectResult>(result);
        }
    }
}
