using System.Collections.Immutable;
using System.Text.Json;
using CashInn.Enum;
using CashInn.Helper;
using CashInn.Model.MenuItem;

namespace CashInn.Model.Payment;

public class AbstractPayment
{
    private static readonly List<AbstractPayment> Instances = [];
    private static string _filepath = ClassExtentFiles.PaymentsFile;
    public Order Order { get; private set; }

    public int Id 
    {
        get => _id;
        set
        {
            if (value < 0)
                throw new ArgumentException("Id cannot be less than 0", nameof(Id));
            _id = value;
        }
    }
    private int _id;

    public double Amount
    {
        get => _amount;
        set
        {
            if (value < 0)
                throw new ArgumentException("Amount cannot be less than 0", nameof(Amount));
            _amount = value;
        }
    }
    private double _amount;

    public DateTime DateOfPayment
    {
        get => _dateOfPayment;
        set
        {
            if (value < DateTime.Today)
                throw new ArgumentException("DateOfPayment cannot be before now", nameof(DateOfPayment));
            _dateOfPayment = value;
        }
    }
    private DateTime _dateOfPayment;

    private IPaymentRole _role;

    public AbstractPayment(int id, double amount, DateTime dateOfPayment, IPaymentRole role, Order? order = null)
    {
        Id = id;
        Amount = amount;
        DateOfPayment = dateOfPayment;
        _role = role;
        if (order != null) 
            SetOrder(order);
        
        AddInstance();
    }

    public void ChangeRole(IPaymentRole newRole)
    {
        _role = newRole;
    }

    public object RoleDetails => _role.GetDetails();

/*
    public abstract object ToSerializableObject();

    public static void SaveExtent()
    {
        Saver.Serialize(Instances.Select(e => e.ToSerializableObject()).ToList(), _filepath);
    }
    
    public static void LoadExtent()
    {
        var deserializedEmployees = Saver.Deserialize<List<dynamic>>(_filepath);
        Instances.Clear();

        if (deserializedEmployees == null) return;

        foreach (var paymentData in deserializedEmployees)
        {
            int id = paymentData.GetProperty("Id").GetInt32();
            double amount = paymentData.GetProperty("Amount").GetDouble();
            string itemType = paymentData.GetProperty("PaymentType").GetString();
            
            DateTime? dateOfPayment = null;
            if (paymentData.TryGetProperty("DateOfPayment", out JsonElement dateOfPayJson) && dateOfPayJson.ValueKind != JsonValueKind.Null)
            {
                dateOfPayment = dateOfPayJson.GetDateTime();
            }
            
            Order deserOrder = JsonSerializer.Deserialize<Order>(paymentData.GetProperty("Order").GetRawText());
            if (deserOrder == null)
            {
                throw new InvalidOperationException("Order cannot be null.");
            }
            
            AbstractPayment abstractPayment;
            if (itemType == "Cash")
            {
                // abstractPayment = new CashPayment(
                //     id,
                //     amount,
                //     paymentData.GetProperty("CashReceived").GetDouble(),
                //     paymentData.GetProperty("ChangeGiven").GetDouble(),
                //     dateOfPayment.Value,
                //     deserOrder
                // );
            }
            else if (itemType == "Card")
            {
                // abstractPayment = new CardPayment(
                //     id,
                //     amount,
                //     dateOfPayment.Value,
                //     paymentData.GetProperty("CardNumber").GetString(),
                //     deserOrder
                // );
            } 
            else
            {
                throw new InvalidOperationException("Unknown employee type.");
            }
        }
    }
*/

    public static void SavePayment(AbstractPayment abstractMenuItem)
    {
        ArgumentNullException.ThrowIfNull(abstractMenuItem);
        Instances.Add(abstractMenuItem);
    }
    
    public static ICollection<AbstractPayment> GetAll()
    {
        return Instances.ToImmutableList();
    }

    public void SetOrder(Order order)
    { 
        ArgumentNullException.ThrowIfNull(order);
        
        if (Order == order) return;

        if (Order != null)
        {
            throw new InvalidOperationException("Payment is already assigned to another Order");
        }

        Order = order;
        order.AddPaymentInternal(this);
    }
    
    internal void AddInstance()
    {
        Instances.Add(this);
    }
    
    internal void RemoveInstance()
    {
        Instances.Remove(this);
    }
}