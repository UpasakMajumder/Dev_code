using System.Text;

namespace Kadena.Dto.CreditCard.MicroserviceResponses
{
    public class AuthorizeAmountResponseDto
    {
        public string AuthCode { get; set; }
        public object TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
        public string TransactionKey { get; set; }
        public string ResponseStatus { get; set; }
        public bool Succeeded { get; set; }
        public string Warnings { get; set; }
        public string SummeryStatus { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"AuthCode: {AuthCode}");
            sb.AppendLine($"TransactionType: {TransactionType}");
            sb.AppendLine($"TotalAmount: {TotalAmount}");
            sb.AppendLine($"TransactionKey: {TransactionKey}");
            sb.AppendLine($"ResponseStatus: {ResponseStatus}");
            sb.AppendLine($"Succeeded: {Succeeded}");
            sb.AppendLine($"Warnings: {Warnings}");
            sb.AppendLine($"SummeryStatus: {SummeryStatus}");
            return sb.ToString();
        }
    }
}
