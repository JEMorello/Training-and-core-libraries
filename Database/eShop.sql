/* Shopping schema
	Objects of focus
		Order History
		Transactions
		Search Results
		Shopping Cart
		Purchaseable Items
		
	Actions of focus
		Search
		Add to Cart
		Buy
		Recover History
		Establish delivery
		
*/
Create Schema eSH AUTHORIZATION dbo

--* Inventory


--* Order

--* Shipping Connector

--* esUser

--* Payment Method

--* Inventory Element

--*quality value

--* Images

Create TABLE eSH.Images
(
	ImageID INT IDENTITY(1,1) PRIMARY KEY,
	Content VARBINARY(MAX),
	FileType VARCHAR(8) NOT NULL,
	OriginalFilename VARCHAR(250) NOT NULL,
	Description VARCHAR(250)
)
--* Links

--* Contact Information

--* Shippable addresses




