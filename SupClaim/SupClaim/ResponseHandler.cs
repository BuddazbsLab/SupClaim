

using Nancy.Json;

namespace SupClaim
{
    internal class ResponseHandler
    {
        private readonly string? _answerSupport;
        private readonly string? _answerSupportClient;
        private readonly string? _answerGroupYT;
        private readonly string? _answerChekYT;


        public ResponseHandler((string?, string?, string?, string?) responseApi)
        {
            this._answerSupport = responseApi.Item2;
            this._answerSupportClient = responseApi.Item3;
            this._answerGroupYT = responseApi.Item4;
            this._answerChekYT = responseApi.Item1;
        }


        public string? AnswerSupport => this._answerSupport;
        public string? AnswerSupportClient => this._answerSupportClient;
        public string? AnswerGroupYT => this._answerGroupYT;
        public string? AnswerChekYT => this._answerChekYT;


        public string ConvertAnswersIntoOne()
        {
            //Десириализуем полученные ответы
            var javaScriptSerializer = new JavaScriptSerializer();
            var newYTDetail = javaScriptSerializer.DeserializeObject(AnswerChekYT);
            var newSupportDetail = javaScriptSerializer.DeserializeObject(AnswerSupport);
            var newSupportClientDetail = javaScriptSerializer.DeserializeObject(AnswerSupportClient);
            var newYTGroupDetail = javaScriptSerializer.DeserializeObject(AnswerGroupYT);

            //Пихаем все в один объект
            var result = javaScriptSerializer.Serialize(new { ChekYT = newYTDetail, Support = newSupportDetail, SupportClient = newSupportClientDetail, GroupYT = newYTGroupDetail });
            return result;
        }
    }
}
