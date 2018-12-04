# SpecificaThor
.NET Standard Fluent Specification Structure combined with a Notification Pattern

## Nuget
> Install-Package SpecificaThor -Version 1.1.0

## Usage
Dummy class:
```
  public class Lot
  {
      public long Id { get; set; }
      public string LotNumber { get; set; }
      public bool IsInterdicted { get; set; }
      public DateTime ExpirationDate { get; set; }
      public int AvailableQuantity { get; set; }
  }
```

### Sample of Specification Classes: 
```
  public class Expired : ISpecification<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
  {
      public string GetErrorMessageWhenExpectingFalse(Lot contract)
          => $"Lot {contract.LotNumber} is expired and connot be used";

      public bool Validate(Lot contract)
          => contract.ExpirationDate.Date <= DateTime.Now.Date;
  }
  
  public class AvailableOnStock : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
  {
      public string GetErrorMessageWhenExpectingFalse(Lot contract)
          => $"Lot {contract.LotNumber} is available on stock";

      public string GetErrorMessageWhenExpectingTrue(Lot contract)
          => $"Lot {contract.LotNumber} is not available on stock. Current Quantity: {contract.AvailableQuantity}";

      public bool Validate(Lot contract)
          => contract.AvailableQuantity > 0;
  }
```

### Validating:
```
...
Lot lot = ...;

SpecificationResult specificationResult = Specification.Create(lot)
                                                       .IsNot<Expired>()
                                                       .AndIsNot<Interdicted>()
                                                       .OrIs<AvailableOnStock>()
                                                       .AndIs<Expired>()
                                                       .GetResult();
//It should work like that:                                                       
//if ((!lot.Expired && !lot.Interdicted) || (lot.AvailableOnStock && lot.Expired))

//You can set a custom message on the specification chain like this

... Specification.Create(lot)
                 .Is<Expired>()
                 .UseThisErrorMessageIfFails("This is a custom error message") //If the lot is not expired this message will be used
                 .AndIsNot<Interdicted>()
                 .GetResult();

```
```
SpecificationResult:
 - Properties:
    specificationResult.IsValid //bool: True if the validation sequence is succeeded;
    specificationResult.ErrorMessage //String: All error messages concatenated;
    specificationResult.TotalOfErrors //int: As the name says: Total number of Errors;
 - Method:
    result.HasError<T>() //Returns true if the result contains an error on an specific validation. Sample: result.HasError<Expired>()
 ``` 

### Filtering:
```
//lots is an IEnumerable<Lot>
...
IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                       .ThatAre<Expired>()
                                       .AndAre<Interdicted>()
                                       .OrAre<AvailableOnStock>()
                                       .GetMatched();

//It should work like that:         
//lots.Where(lot => (lot.Expired && lot.Interdicted) || (lot.AvailableOnStock))

...

//IEnumerable extension method GetSubjects() creates a specification chain
//Useful to fluently filter a Linq Query
//Like this sample using Entity Framework:

...
var result = await _dbContext.Products
			     .Include(product => product.Lots)
			     .GetSubjects() //This is the same as Specification.Create<Lot>(lots)
			     .ThatAre<Expired>()
			     .AndAre<Interdicted>()
			     .GetMatched()
			     .ToListAsync();
```

