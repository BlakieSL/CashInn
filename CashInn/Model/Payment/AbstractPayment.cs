﻿using System.Collections.Immutable;
using System.Text.Json;
using CashInn.Enum;
using CashInn.Helper;
using CashInn.Model.MenuItem;

namespace CashInn.Model.Payment;

public abstract class AbstractPayment
{
    private static readonly List<AbstractPayment> Payments = [];
    private static string _filepath = ClassExtentFiles.PaymentsFile;
    public abstract string PaymentType { get; }
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
            if (value < DateTime.Now)
                throw new ArgumentException("DateOfPayment cannot be before now", nameof(DateOfPayment));
            _dateOfPayment = value;
        }
    }
    private DateTime _dateOfPayment;

    protected AbstractPayment(int id, double amount, DateTime dateOfPayment)
    {
        Id = id;
        Amount = amount;
        DateOfPayment = dateOfPayment;
    }
    
    public abstract object ToSerializableObject();
    public static void SaveExtent()
    {
        Saver.Serialize(Payments.Select(e => e.ToSerializableObject()).ToList(), _filepath);
    }
    
    public static void LoadExtent()
    {
        var deserializedEmployees = Saver.Deserialize<List<dynamic>>(_filepath);
        Payments.Clear();

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
            
            AbstractPayment abstractPayment;
            if (itemType == "Cash")
            {
                abstractPayment = new CashPayment(
                    id,
                    amount,
                    paymentData.GetProperty("CashReceived").GetDouble(),
                    paymentData.GetProperty("ChangeGiven").GetDouble(),
                    dateOfPayment.Value
                );
            }
            else if (itemType == "Card")
            {
                abstractPayment = new CardPayment(
                    id,
                    amount,
                    dateOfPayment.Value,
                    paymentData.GetProperty("CardNumber").GetString()
                );
            } 
            else
            {
                throw new InvalidOperationException("Unknown employee type.");
            }
        }
    }
    
    public static void SavePayment(AbstractPayment abstractMenuItem)
    {
        ArgumentNullException.ThrowIfNull(abstractMenuItem);
        Payments.Add(abstractMenuItem);
    }
    
    public static ICollection<AbstractPayment> GetAll()
    {
        return Payments.ToImmutableList();
    }
}