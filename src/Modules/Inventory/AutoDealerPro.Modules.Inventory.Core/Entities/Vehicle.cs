using AutoDealerPro.Shared.Kernel.Types;

namespace AutoDealerPro.Modules.Inventory.Core.Entities;

public class Vehicle : EntityBase
{
    // Basic Info
    public string Make { get; private set; }          // Toyota
    public string Model { get; private set; }         // Camry
    public int Year { get; private set; }
    public string PlateNumber { get; private set; }
    public string Trim { get; private set; }          // LE, XLE, etc.

    // Specs
    public int Mileage { get; private set; }
    public string ExteriorColor { get; private set; }
    public string InteriorColor { get; private set; }
    public string Transmission { get; private set; }   // Automatic, Manual
    public string FuelType { get; private set; }       // Gasoline, Diesel, Hybrid
    public string BodyType { get; private set; }       // Sedan, SUV, Truck

    // Business Data
    public decimal PurchasePrice { get; private set; }     // What we paid
    public decimal AskingPrice { get; private set; }       // What we're selling for
    public decimal? SellingPrice { get; private set; }     // What we actually sold for
    public VehicleStatus Status { get; private set; }
    public string? Notes { get; private set; }             // Staff notes

    // Photos
    public List<string> PhotoUrls { get; private set; } = new();

    // Tracking
    public int ViewCount { get; private set; }
    public DateTime? SoldAt { get; private set; }

    private Vehicle() { } // EF Core

    public static Vehicle Create(
        string make, string model, int year, string plateNumber, string trim,
        int mileage, string exteriorColor, string interiorColor,
        string transmission, string fuelType, string bodyType,
        decimal purchasePrice, decimal askingPrice, string? notes = null)
    {
        ValidateYear(year);
        ValidateMileage(mileage);
        ValidatePurchasePrice(purchasePrice);
        ValidateAskingPrice(askingPrice);

        return new Vehicle
        {
            Make = make,
            Model = model,
            Year = year,
            PlateNumber = plateNumber.ToUpper(),
            Trim = trim,
            Mileage = mileage,
            ExteriorColor = exteriorColor,
            InteriorColor = interiorColor,
            Transmission = transmission,
            FuelType = fuelType,
            BodyType = bodyType,
            PurchasePrice = purchasePrice,
            AskingPrice = askingPrice,
            Status = VehicleStatus.Available,
            Notes = notes
        };
    }

    public void UpdatePrice(decimal newAskingPrice)
    {
        ValidateAskingPrice(newAskingPrice);
        AskingPrice = newAskingPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMileage(int newMileage)
    {
        ValidateMileage(newMileage);

        if (newMileage < Mileage)
            throw new InvalidOperationException("Cannot decrease mileage");

        Mileage = newMileage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsSold(decimal sellingPrice)
    {
        ValidateSellingPrice(sellingPrice);

        if (Status == VehicleStatus.Sold)
            throw new InvalidOperationException("Vehicle already sold");

        Status = VehicleStatus.Sold;
        SellingPrice = sellingPrice;
        SoldAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddPhoto(string photoUrl)
    {
        ValidatePhotoUrl(photoUrl);

        if (PhotoUrls.Count >= 15)
            throw new InvalidOperationException("Maximum 15 photos allowed");

        PhotoUrls.Add(photoUrl);
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementViewCount()
    {
        ViewCount++;
    }

    #region ::: validations :::
    private static void ValidateYear(int year)
    {
        if (year < 1900 || year > DateTime.Now.Year + 1)
            throw new ArgumentException($"Year must be between 1900 and {DateTime.Now.Year + 1}", nameof(year));
    }

    private static void ValidateMileage(int mileage)
    {
        if (mileage < 0)
            throw new ArgumentException("Mileage must be greater than or equal to 0", nameof(mileage));
    }

    private static void ValidatePurchasePrice(decimal purchasePrice)
    {
        if (purchasePrice <= 0)
            throw new ArgumentException("Purchase price must be greater than 0", nameof(purchasePrice));
    }

    private static void ValidateAskingPrice(decimal askingPrice)
    {
        if (askingPrice <= 0)
            throw new ArgumentException("Asking price must be greater than 0", nameof(askingPrice));
    }

    private static void ValidateSellingPrice(decimal sellingPrice)
    {
        if (sellingPrice <= 0)
            throw new ArgumentException("Selling price must be greater than 0", nameof(sellingPrice));
    }

    private static void ValidatePhotoUrl(string photoUrl)
    {
        if (!Uri.TryCreate(photoUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Photo URL must be a valid absolute URI", nameof(photoUrl));
    }

    #endregion
}

public enum VehicleStatus
{
    Available,      // Ready to sell
    Pending,        // Deposit received
    Sold,           // Completed sale
    InRepair        // Being serviced
}