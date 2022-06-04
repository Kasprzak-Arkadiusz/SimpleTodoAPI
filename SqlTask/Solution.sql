USE Dataedo
GO

DECLARE @report_date date = '2021-04-01'

DECLARE @maxDaysOfDelayForSegmentA int = 90
DECLARE @maxDaysOfDelayForSegmentB int = 60
DECLARE @maxDaysOfDelayForSegmentC int = 30

DECLARE @penaltyForDayOfDelaySegmentA decimal(3,2) = 0.05
DECLARE @penaltyForDayOfDelaySegmentB decimal(3,2) = 0.1
DECLARE @penaltyForDayOfDelaySegmentC decimal(3,2) = 0.15

SELECT * FROM (
    SELECT cus.customer_name as [Customer name],
    i.order_date as [Order date],
    i.segment as Segment,
    i.cost as Cost,
    i.installation_date as [Installation date],
    cit.name as City,
    [Days of delay] = DATEDIFF(day, i.installation_date, @report_date),
    Penalty =
        (CASE
            WHEN i.segment = 'A' AND DATEDIFF(day, i.installation_date, @report_date) > @maxDaysOfDelayForSegmentA
            THEN DATEDIFF(day, i.installation_date, @report_date) * @penaltyForDayOfDelaySegmentA * i.cost
            WHEN i.segment = 'B' AND DATEDIFF(day, i.installation_date, @report_date) > @maxDaysOfDelayForSegmentB
            THEN DATEDIFF(day, i.installation_date, @report_date) * @penaltyForDayOfDelaySegmentB * i.cost
            WHEN i.segment = 'C' AND DATEDIFF(day, i.installation_date, @report_date) > @maxDaysOfDelayForSegmentC
            THEN DATEDIFF(day, i.installation_date, @report_date) * @penaltyForDayOfDelaySegmentC * i.cost
            ELSE 0
        END)
    FROM installations i
    JOIN customers cus on i.customer_id = cus.id
    JOIN cities cit on i.city_id = cit.id
    WHERE i.installation_date IS NOT NULL)
AS ins WHERE ins.Penalty > 0
ORDER BY Penalty DESC