# SpecificaThor
Fluent Specification Structure merged with a Notification Pattern

# Usage

Sample of an Entity:

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

Sample of a Specification Class: 

```
  public class Expired : ISpecification<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
  {
      public string GetErrorMessageWhenExpectingFalse(Lot contract)
          => $"Lot {contract.LotNumber} is expired and connot be used";

      public bool Validate(Lot contract)
          => contract.ExpirationDate.Date <= DateTime.Now.Date;
  }
```
  
Validating:

```
SpecificationResult specificationResult = Specification.Create(lot)
                                                       .IsNot<Expired>()
                                                       .AndIsNot<Interdicted>()
                                                       .OrIs<AvailableOnStock>()
                                                       .IsSatisfied();

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
Filtering:

```
IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                       .ThatAre<Expired>()
                                       .AndAre<Interdicted>()
                                       .OrAre<AvailableOnStock>()
                                       .GetMatched();
```
