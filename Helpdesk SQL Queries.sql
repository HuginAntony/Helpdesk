USE Helpdesk

--ALTER TABLE dbo.Request
--ADD CONSTRAINT CK_Request CHECK ([Status] IN('New','In Progress', 'In Development', 'Awaiting Deployment','Resolved'))

SELECT B.Name, U.Name, U.Surname, U.DateCreated
FROM dbo.[User] U
INNER JOIN dbo.UserBrand UB ON UB.UserId = U.Id
INNER JOIN dbo.Brand B ON B.Id = UB.BrandId

SELECT * FROM dbo.Brand

SELECT * FROM dbo.Application
SELECT * FROM dbo.UserRole UR INNER JOIN dbo.[User] U ON U.Id = UR.UserId WHERE RoleId=2
SELECT * FROM dbo.UserDeveloper
SELECT * FROM dbo.Developer
SELECT * FROM dbo.Request

SELECT * FROM dbo.[User] WHERE Name='julia'


SELECT Status AS 'Request Status', COUNT(Status) AS 'Total'
FROM Request
GROUP BY Status

SELECT D.Name, R.Status, COUNT(R.Status) AS 'Total'
FROM Request R
INNER JOIN Developer D ON D.Id = R.DeveloperId
WHERE Status IN('In Development')
GROUP BY D.Name, R.Status


SELECT A.Name, R.RequestType, COUNT(*) AS 'Total'
FROM Request R
INNER JOIN Application A ON A.Id = R.ApplicationId
GROUP BY A.Name, R.RequestType
ORDER BY A.Name