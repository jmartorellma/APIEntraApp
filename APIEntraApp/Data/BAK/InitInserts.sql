--PAYMENT STATUS
INSERT INTO PaymentStatus
  ( Code, Name, CreationDate)
VALUES
  ('OK', 'Completado', GETDATE()), 
  ('KO', 'Fallido', GETDATE()), 
  ('PENDING','Pendiente', GETDATE());
   
--PROVIDERS

--PRODUCTS

--CATEGORIES

--PRODUCTS_CATEGORIES

--STOCKS

--PURCHASE TYPES
INSERT INTO PurchaseTypes
  ( Code, Name, CreationDate)
VALUES
  ('ONLINE', 'En línea', GETDATE()), 
  ('TOPICKUP','A recoger', GETDATE());

--SHOPS

--SHOP_PURCHASETYPES

--PAYMENT METHODS