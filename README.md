# SpecificaThor
.NET Standard Fluent Specification Structure combined with a Notification Pattern

## Nuget
> Install-Package SpecificaThor -Version 2.0.0

## Usage

### Interfaces 

The concrete Specification class needs to implement this interface, which will represent the domain rule that will be validated or filtered.

```
public interface ISpecification<TCandidate>
{
    bool Validate(TCandidate candidate);
}
```
If you want to have an error message if the specification rule is expecting *true*, when using the specification chain methods: "Is()", "AndIs()", "OrIs()". Then implement this Interface to build your error message.

```
public interface IHasErrorMessageWhenExpectingTrue<TCandidate>
{
    string GetErrorMessageWhenExpectingTrue(TCandidate candidate);
}
```

If you want to have an error message if the specification rule is expecting *false*, when using the specification chain methods: "IsNot()", "AndIsNot()", "OrIsNot()". Then implement this Interface to build your error message.

```
public interface IHasErrorMessageWhenExpectingFalse<TCandidate>
{
    string GetErrorMessageWhenExpectingFalse(TCandidate candidate);
}
```

### Sample

#### Dummy entity/poco class:
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

#### Concrete Specification Classes: 
```
public class Expired : ISpecification<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
{
    public string GetErrorMessageWhenExpectingFalse(Lot candidate)
        => $"Lot {candidate.LotNumber} is expired and cannot be used";

    public bool Validate(Lot candidate)
        => candidate.ExpirationDate.Date <= DateTime.Now.Date;
}
  
public class AvailableOnStock : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
{
    public string GetErrorMessageWhenExpectingFalse(Lot candidate)
        => $"Lot {candidate.LotNumber} is available on stock";

    public string GetErrorMessageWhenExpectingTrue(Lot candidate)
        => $"Lot {candidate.LotNumber} is not available on stock. Current Quantity: {candidate.AvailableQuantity}";

    public bool Validate(Lot candidate)
        => candidate.AvailableQuantity > 0;
  }
```

#### Validating:
##### Single Validation
```
...
Lot lot = ...;

ISpecificationResult specificationResult = Specification.Create(lot)
                                                        .IsNot<Expired>()
                                                        .AndIsNot<Interdicted>()
                                                        .OrIs<AvailableOnStock>()
                                                        .AndIs<Expired>()
                                                        .GetResult();
```
It should work like that:                                                       
*if ((!lot.Expired && !lot.Interdicted) || (lot.AvailableOnStock && lot.Expired))*

You can set a custom message on the specification chain like this:
```
... Specification.Create(lot)
                 .IsNot<Expired>()
                 .UseThisErrorMessageIfFails("This is a custom error message") 
		 //If the lot is expired the message above will be used
                 .AndIsNot<Interdicted>()
                 .GetResult();
```

The method GetResult() will return an ISpecificationResult, which contains:
 - Properties:
    - IsValid: bool 
    	- True if the validation sequence is succeeded;
    - ErrorMessage: string
    	- All error messages concatenated;
    - TotalOfErrors: int
    	- As the name says: Total number of Errors;
 - Method:
    - HasError\<TSpecification\>(): bool 
    	- Returns true if the result contains an error on a specific validation;
        - Sample: result.HasError\<Expired\>()

##### Enumerable Validation
```
ISpecificationResults<Lot> result = Specification.Create<Lot>(lots)
					         .ThatAre<Expired>()
					         .AndAre<Interdicted>()
					         .AndAreNot<AvailableOnStock>()
					         .GetResults();
```
The method GetResults() will return an ISpecificationResults, which contains:
 - Properties:
    - AreAllCandidatesValid: bool 
    	- True if all candidates passed the validation;
    - ErrorMessages: string
    	- All error messages concatenated;
    - ValidCandidates: IEnumerable\<TCandidate\>
    	- All valid candidates;
    - InvalidCandidates: IEnumerable\<TCandidate\>
    	- All invalid candidates;
     - AllCandidates: IEnumerable\<TCandidate\>
    	- All candidates;
 - Method:
    - HasError\<TSpecification\>(): bool 
    	- Returns true if the result contains an error on a specific validation;
        - Sample: result.HasError\<Expired\>()
    - HasError\<TSpecification\>(TCandidate candidate): bool 
    	- Returns true if the result contains an error on a specific validation and candidate;
        - Sample: result.HasError\<Expired\>(lot)
 
#### Filtering:
```
IEnumerable<Lot> = ...
...
IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                       .ThatAre<Expired>()
                                       .AndAre<Interdicted>()
                                       .OrAre<AvailableOnStock>()
                                       .GetMatched();
```
It should work like that:         
*lots.Where(lot => (lot.Expired && lot.Interdicted) || (lot.AvailableOnStock))*

It also contains an IEnumerable extension method GetCandidates() that creates a specification chain, useful to fluently filter a Linq Query.

Like this sample using Entity Framework:

```
...
var result = await _dbContext.Products
			     .Include(product => product.Lots)
			     .GetCandidates() //This is the same as Specification.Create<Lot>(lots)
			     .ThatAre<Expired>()
			     .AndAre<Interdicted>()
			     .GetMatched()
			     .ToListAsync();
```

