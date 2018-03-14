USE [NewGalaxyDb]
GO

---去除字符串中重复的值函数 
ALTER FUNCTION dbo.StringRemove(@str NVARCHAR(2000))
RETURNS VARCHAR(2000)
AS
BEGIN 
    DECLARE @result NVARCHAR(2000), @temp NVARCHAR(1000); 
    SET @result = ''; 
    SET @temp = '';
    SET @str = @str + ',';
    WHILE (CHARINDEX(',,', @str) > 0 )
    BEGIN
        SET @str = REPLACE(@str, ',,', ',');
    END;
    WHILE (CHARINDEX(',', @str) <> 0 )
    BEGIN  
        SET @temp = SUBSTRING(@str, 1, CHARINDEX(',', @str)); 
        IF (CHARINDEX(@temp, @result) <= 0 )
		BEGIN
            SET @result = @result + @temp;
		END
        SET @str = STUFF(@str, 1, CHARINDEX(',', @str), '');
    END; 
    RETURN LEFT(@result, LEN(@result)-1); 
END; 