USE [POS_SERVER_HISTORY]
GO
/****** Object:  StoredProcedure [dbo].[sp_DailyVoucherSelling]    Script Date: 06/05/2017 09:50:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER Procedure [dbo].[sp_DailyVoucherSelling] @Day char(6),@Month char(6),@Year char(6),@Code char(6)
AS
declare @nov as varchar(30)
declare @refv as varchar(30)
declare @Min as integer
declare @Max as integer
declare @MinNo as varchar(30)
declare @MaxNo as varchar(30)
declare @refvNo as varchar(30)

--declare @Day as char(6)
--declare @Month as char(6)
--declare @Year as char(6)
--declare @Code as char(6)
--set @Day = '2' 
--set @Month =  '6' 
--set @Year = '2017'
--set @Code = 'B'
Set @Min = -1
Set @Max = -1
set @refvNo = ''
Create table #DailyVoucherSelling (V_CODE char(1), V_NO Varchar(60), V_AMT Decimal(18,0), V_DESC Varchar(50), 
V_FLAG char(1), V_REC datetime, V_KUNCI char(20), V_SELL datetime, V_REF varchar(10), L_TYPE char(10), L_AMT decimal(18,0)
, L_DISC Decimal(18,0),L_LUPDATE datetime)

DECLARE hitung_selling_cursor CURSOR FOR
select  V_NO,V_REF from NewVoc where day(V_SELL) = @Day and month(V_SELL) = @Month and year(V_SELL) = @Year and V_CODE = @Code order by v_no
OPEN hitung_selling_cursor;
FETCH NEXT FROM hitung_selling_cursor INTO @nov,@refv;
WHILE @@FETCH_STATUS = 0
BEGIN
	if (@Min < 0)
	begin
	set @Min = Convert(Integer,Right(@nov,5))
	set @MinNo = @nov
	set @Max = Convert(Integer,Right(@nov,5))
	set @MaxNo = @nov
	set @refvNo = @refv
	end
	else
	if Convert(Integer,Right(@nov,5)) = @max + 1 And @refvNo = @refv
	begin
	set @Max = Convert(Integer,Right(@nov,5))
	set @MaxNo = @nov
	set @refvNo = @refv
	end
	else
	begin
		insert into #DailyVoucherSelling SELECT     a.V_CODE, @MinNo + ' - ' + @MaxNo AS V_NO, a.V_AMT, SUBSTRING(a.V_DESC + ' ' + c.V_DESC_DTL,1,50), a.V_FLAG, a.V_REC, a.V_KUNCI, a.V_SELL, a.V_REF, b.L_TYPE, 
        b.L_AMT, b.L_DISC, b.L_LUPDATE FROM   dbo.NewVoc AS a inner join NewVocDtl c on a.V_NO = c.v_no LEFT OUTER JOIN dbo.lain AS b ON a.V_REF = b.L_REF where day(V_SELL) = @Day and month(V_SELL) = @Month and year(V_SELL) = @Year and a.V_CODE = @Code
        and a.V_NO = @MinNo
		--set @Min = -1
		--Set @Max = -1  
		--set @refvNo = ''
		set @Min = Convert(Integer,Right(@nov,5))
		set @MinNo = @nov
		set @Max = Convert(Integer,Right(@nov,5))
		set @MaxNo = @nov
		set @refvNo = @refv
	End
	
   FETCH NEXT FROM hitung_selling_cursor INTO @nov,@refv;
END
--select @MinNo
insert into #DailyVoucherSelling SELECT     a.V_CODE, @MinNo + ' - ' + @MaxNo AS V_NO, a.V_AMT, SUBSTRING(a.V_DESC + ' ' + c.V_DESC_DTL,1,50) , a.V_FLAG, a.V_REC, a.V_KUNCI, a.V_SELL, a.V_REF, b.L_TYPE, 
        b.L_AMT, b.L_DISC, b.L_LUPDATE FROM   dbo.NewVoc AS a inner join NewVocDtl c on a.V_NO = c.v_no LEFT OUTER JOIN dbo.lain AS b ON a.V_REF = b.L_REF where day(V_SELL) = @Day and month(V_SELL) = @Month and year(V_SELL) = @Year and a.V_CODE = @Code
        and a.V_NO = @MinNo
CLOSE hitung_selling_cursor;

DEALLOCATE hitung_selling_cursor;
select * from #DailyVoucherSelling order by V_NO 

--exec sp_DailyVoucherSelling  '12' , '5' , '2017' , 'A'
