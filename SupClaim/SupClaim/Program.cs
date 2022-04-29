using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using SupClaim;
using Telegram.Bot;

#region Конфиги
var configSettingsApp = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configSettingsApp.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();


Settings settings = new(configSettingsApp);
var botSettings = settings.GetSettingsBotConfig();
#endregion


var startTimeSpan = TimeSpan.Zero;
var periodTimeSpan = TimeSpan.FromMinutes(10);
var timer = new Timer(async e =>
{
    var timesleep = DateTime.Now.Hour;
    var datesleep = DateTime.Now.DayOfWeek;
    if (timesleep >= 08 && timesleep < 18 && datesleep != DayOfWeek.Saturday &&
        datesleep != DayOfWeek.Sunday)
    {
        var botInit = new TelegramBotClient(settings.GetSettingsBotConfig().Token);
        logger.Info("[*]Начат процесс сбора данных");
        RequestApiClaim requestApiClaim = new(botSettings);
        var responseApi = requestApiClaim.MakeRequestToApiClaim();

        ResponseHandler responseHandler = new(responseApi);
        var resultHendler = responseHandler.ConvertAnswersIntoOne();

        #region Сообщеньки
        SupportMessage supportMessage = new(resultHendler, botInit, botSettings, logger);
        var supmessage = supportMessage.SendSupportMessage();

        SupportClientMessage supportClientMessage = new(resultHendler, botInit, botSettings, logger);
        var supClient = supportClientMessage.SendSupportClientMessage();

        YTGroupMessage yTGroupMessage = new(resultHendler, botInit, botSettings, logger);
        var ytGr = yTGroupMessage.SendYTGroupMessage();

        ChekYTMessage chekYTMessage = new(resultHendler, botInit, botSettings, logger);
        var chekYt = chekYTMessage.SendChekYTMessage();
        #endregion

        AnswerComparator answerComparator = new(logger, resultHendler);
        var comparisonResult = await answerComparator.CompareDataObtained();

        if(comparisonResult == true)
        {
            Message message = new(resultHendler, botInit, botSettings, logger) { SendSomeMessage = supmessage + supClient + ytGr + chekYt };
            await message.SendMessage();
        }
        else
        {

            logger.Info(@"[*]Данные совпадают. Слать нечего
=======================================================================");
        }
    }
    else
    {
        logger.Info("Я отдыхаю.");
    }
}, null, startTimeSpan, periodTimeSpan);

Console.ReadLine();

