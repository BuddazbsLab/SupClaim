using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using SupClaim;

var configSettingsApp = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configSettingsApp.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();


Settings settings = new(configSettingsApp);
var botSettings = settings.GetSettingsBotConfig();

InitBot initBot = new();
var botInit = await Task.Run(() => initBot.StartBot(settings));



var startTimeSpan = TimeSpan.Zero;
var periodTimeSpan = TimeSpan.FromMinutes(10);
var timer = new Timer(async e =>
{
    var timesleep = DateTime.Now.Hour;
    var datesleep = DateTime.Now.DayOfWeek;
    if (timesleep >= 08 && timesleep < 18 && datesleep != DayOfWeek.Saturday &&
        datesleep != DayOfWeek.Sunday)
    {
        logger.Info("[*]Начат процесс сбора данных");
        RequestApiClaim requestApiClaim = new(botSettings);
        var responseApi = requestApiClaim.MakeRequestToApiClaim();

        Message message = new(responseApi, botInit, botSettings, logger);
        await message.SendMessage();

    }
}, null, startTimeSpan, periodTimeSpan);

Console.ReadLine();

