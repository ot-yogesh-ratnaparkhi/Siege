declare @Customers_DateCreated as int
declare @Customers_CustomerFirstName as nvarchar(max)
declare @Customers_CustomerLastName as nvarchar(max)
declare @Customers_CustomerID int


INSERT INTO siege.Customers 
		([DateCreated], [CustomerFirstName], [CustomerLastName]) 
	VALUES 
		(@Customers_DateCreated, @Customers_CustomerFirstName, @Customers_CustomerLastName)

SELECT @Customers_CustomerID = Scope_Identity()

SELECT @Customers_CustomerID