# Konrad Bartecki

## Comments

1. Multiple return points in `ProductApplicaitonService`, but this file is small enough for now to ignore that.
2. `return (result.Success) ? result.ApplicationId ?? -1 : -1;`
    
    This basically means:
    ```
    if true
        if app id exists
            return result app id
        else
            return -1
    else
        return -1     
    ``` 

    Readbility is poor, I recommend refactoring to always return a clearly defined object   
  
 3. Using `if type is XYZ` logic is not really extensible if we would want to add more microservices in the future. Now in real life I wouldn't refactor this because this file is simple enough.
 4. `IConfidentialInvoiceService` has too many parameters
 5. Lack of defensive coding or incoming data validation.
 
    This code could potentially throw NullReferenceExceptions if `CompanyData` in `application.CompanyData.Number.ToString()` is `null`.
    Now depending on the code style you may or may not want to pass that exception to the API consumer, but it is good to be aware of that.  

