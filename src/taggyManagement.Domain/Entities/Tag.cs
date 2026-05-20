namespace taggyManagement.Domain.Entities;

public class Tag
{
    public bool Status { get; set; }
    public decimal Balance { get; set; }

    public static Tag Create(bool status, decimal balance)
    {
        return new Tag
        {
            Status = status,
            Balance = balance
        };
    }

    public static void Debitar(Tag tag, decimal valor)
    {
        if (tag is null)
        {
            throw new ArgumentNullException(nameof(tag));
        }

        if (valor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor de débito não pode ser negativo.");
        }

        tag.Balance -= valor;
    }

    public static bool VerificarStatus(Tag tag)
    {
        if (tag is null)
        {
            throw new ArgumentNullException(nameof(tag));
        }

        return tag.Status;
    }
}
