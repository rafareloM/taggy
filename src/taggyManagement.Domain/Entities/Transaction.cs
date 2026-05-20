namespace taggyManagement.Domain.Entities;

public class Transaction
{
    public DateTime DateHour { get; set; }
    public decimal Value { get; set; }

    public static Transaction Create(DateTime dateHour, decimal value)
    {
        return new Transaction
        {
            DateHour = dateHour,
            Value = value
        };
    }

    public static void ValidateValue(Transaction transaction)
    {
        if (transaction is null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(transaction.Value), "O valor da transação não pode ser negativo.");
        }
    }
}
