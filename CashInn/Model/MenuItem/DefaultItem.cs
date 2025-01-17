﻿using CashInn.Enum;

namespace CashInn.Model.MenuItem;

public class DefaultItem : AbstractMenuItem
{
    public ServingSize ServingSize { get; set; }
    public DefaultItem(int id, string name, double price, string description, string dietaryInformation, bool available,
        ServingSize servingSize, Category? category = null)
        : base(id, name, price, description, dietaryInformation, available, category)
    {
        ServingSize = servingSize;
        
        SaveItem(this);
    }

    public override string ItemType => "Default";
    
    public override object ToSerializableObject()
    {
        return new
        {
            Id,
            Name,
            Price,
            Description,
            DietaryInformation,
            Available,
            ServingSize,
            // Category,
            ItemType
        };
    }
}