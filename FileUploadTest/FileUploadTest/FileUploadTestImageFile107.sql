create Procedure [dbo].[USP_State_Practice]
@Country_Id int,
@CountryName nvarchar(50),
@StateName nvarchar(50)

as 
IF NOT Exists ( select State_Id From tblstate where StateName = @StateName)
begin

	insert into tblstate

		(
			Country_Id, StateName
		)

			values

				(
					@Country_Id , @StateName
				)

end

ELSE
begin
          
		  SELECT 'Duplicate Message'
    -- SET @Message = @CountryName SELECT 'Duplicate CountryName...!!!'
end