using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Cellenza.Pinpon.BackgroundApp
{
    class PinService
    {
        private GpioPin _pinponPin;
        private readonly ILogger<PinService> _logger;

        public PinService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PinService>();
        }

        public async Task Initialize(int pin)
        {
            _logger.LogDebug("Pin : initialization");
            var controller = await GpioController.GetDefaultAsync();
            _pinponPin = controller.OpenPin(pin);
            _pinponPin.SetDriveMode(GpioPinDriveMode.Output);
            TurnOff();
            _logger.LogInformation("Pin : ready to pinpon !");
        }

        public void TurnOn()
        {
            _pinponPin.Write(GpioPinValue.High);
            _logger.LogDebug("Pin : turn on");
        }

        public void TurnOff()
        {
            _pinponPin.Write(GpioPinValue.Low);
            _logger.LogDebug("Pin : turn off");
        }
    }
}
